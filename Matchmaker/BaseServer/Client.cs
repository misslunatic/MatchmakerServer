using System.Net;
using System.Net.Sockets;

namespace Matchmaker.Server.BaseServer;

public class Client
{
    public ServerSend.StatusType LastStatus;

    public readonly ClientAttributes Attributes = new();

    public readonly TCP? Tcp;
    public readonly UDP Udp;
    public readonly Server ServerParent;

    public Client(int clientId, Server server)
    {
        Tcp = new TCP(clientId, server);
        Udp = new UDP(clientId, server);
        ServerParent = server;
    }

    public bool IsConnected
    {
        get
        {
            try
            {
                if (Tcp?.Socket != null && Tcp.Socket.Client != null && Tcp.Socket.Client.Connected)
                {
                    /* pear to the documentation on Poll:
                     * When passing SelectMode.SelectRead as a parameter to the Poll method it will return 
                     * -either- true if Socket.Listen(Int32) has been called and a connection is pending;
                     * -or- true if data is available for reading; 
                     * -or- true if the connection has been closed, reset, or terminated; 
                     * otherwise, returns false
                     */

                    // Detect if client disconnected
                    if (Tcp.Socket.Client.Poll(0, SelectMode.SelectRead))
                    {
                        byte[] buff = new byte[1];
                        if (Tcp.Socket.Client.Receive(buff, SocketFlags.Peek) == 0)
                        {
                            // Client disconnected
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }

    // ReSharper disable once InconsistentNaming
    public class TCP
    {
        private readonly Server _serv;
        public TcpClient? Socket;

        private readonly int _id;
        private NetworkStream? _stream;
        private Packet? _receivedData;
        private byte[]? _receiveBuffer;

        public TCP(int id, Server server)
        {
            _serv = server;
            _id = id;
        }


        public void Connect(TcpClient? socket)
        {
            Terminal.LogDebug("Connecting TCP...");
            Socket = socket;
            if (Socket != null)
            {
                Socket.ReceiveBufferSize = Constants.DataBufferSize;
                Socket.SendBufferSize = Constants.DataBufferSize;

                _stream = Socket.GetStream();
            }

            _receivedData = new Packet();
            _receiveBuffer = new byte[Constants.DataBufferSize];

            _stream?.BeginRead(_receiveBuffer, 0, Constants.DataBufferSize, ReceiveCallback, null);

            ServerSend.Welcome(_serv, _id, "Welcome to the server!");
        }

        public void SendData(Packet packet)
        {
            try
            {
                if (Socket != null)
                {
                    _stream?.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                }
            }
            catch (Exception ex)
            {
                Terminal.LogError($"Error sending data to player {_id} via TCP: {ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                if (Socket?.Client.RemoteEndPoint is not IPEndPoint remoteIpEndPoint)
                {
                    Terminal.LogDebug($"[{_serv.DisplayName}]: RemoteIpEndPoint is NULL!");
                    _serv.Clients[_id].Attributes.Clear();
                    _serv.Clients[_id].Disconnect();
                    return;
                }

                if (_stream != null)
                {
                    var byteLength = _stream.EndRead(result);
                    if (byteLength <= 0)
                    {
                        Terminal.LogDebug("Client has left. Disconnecting...");
                        _serv.DisconnectClient(_id);
                        return;
                    }

                    var data = new byte[byteLength];
                    if (_receiveBuffer != null) Array.Copy(_receiveBuffer, data, byteLength);

                    _receivedData?.Reset(HandleData(data));
                }

                if (_receiveBuffer != null)
                {
                    _stream?.BeginRead(_receiveBuffer, 0, Constants.DataBufferSize, ReceiveCallback, null);
                }
            }
            catch (Exception ex)
            {
                Terminal.LogError($"[{_serv.DisplayName}] Error receiving TCP data: {ex}");
                _serv.DisconnectClient(_id);
            }
        }

        private bool HandleData(IEnumerable<byte> data)
        {
            var packetLength = 0;

            _receivedData?.SetBytes(data);

            if (_receivedData?.UnreadLength() >= 4)
            {
                packetLength = _receivedData.ReadInt();
                if (packetLength <= 0)
                {
                    return true;
                }
            }

            while (packetLength > 0 && packetLength <= _receivedData?.UnreadLength())
            {
                var packetBytes = _receivedData.ReadBytes(packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    try
                    {
                        var packet = new Packet(packetBytes);

                        var packetId = packet.ReadInt();
                        if ((packetId != int.MaxValue - 3) && Config.Sync)
                        {
                            ServerSend.Status(_serv, _id, ServerSend.StatusType.RECEIVED);
                        }

                        Terminal.LogDebug($"[{_serv.DisplayName}] Received TCP Packet with ID: " + packetId);
                        var x = _serv.Packets?.PacketHandlers.ContainsKey(packetId);
                        if (x != null && x != false)
                        {
                            _serv.Packets?.PacketHandlers[packetId].DynamicInvoke(_serv, _id, packet);
                        }
                        else
                        {
                            Terminal.LogError(
                                $"[{_serv.DisplayName}] Received unregistered TCP packet with ID: {packetId}");
                        }
                    }
                    catch (Exception e)
                    {
                        Terminal.LogError($"[{_serv.DisplayName}] Error processing packet: {e}");
                        throw;
                    }
                });

                packetLength = 0;
                if (_receivedData.UnreadLength() < 4) continue;
                packetLength = _receivedData.ReadInt();
                if (packetLength <= 0)
                {
                    return true;
                }
            }

            return packetLength <= 1;
        }

        public void Disconnect()
        {
            Socket?.Close();
            _stream = null;
            _receivedData = null;
            _receiveBuffer = null;
            Socket = null;
        }
    }

    // ReSharper disable once InconsistentNaming
    public class UDP
    {
        public IPEndPoint? EndPoint;
        private readonly Server _serv;
        private readonly int _id;

        public UDP(int id, Server server)
        {
            _serv = server;
            _id = id;
        }

        public void Connect(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
        }

        public void SendData(Packet packet)
        {
            if (EndPoint != null) _serv.SendUdpData(EndPoint, packet);
        }

        public void HandleData(Packet packetData)
        {
            var packetLength = packetData.ReadInt();
            var packetBytes = packetData.ReadBytes(packetLength);

            ThreadManager.ExecuteOnMainThread(() =>
            {
                try
                {
                    using var packet = new Packet(packetBytes);
                    var packetId = packet.ReadInt();
                    if ((packetId != int.MaxValue - 3) && Config.Sync)
                    {
                        ServerSend.Status(_serv, _id, ServerSend.StatusType.RECEIVED);
                    }

                    Terminal.LogDebug($"[{_serv.DisplayName}] Received UDP Packet with ID: " + packetId);
                    var x = _serv.Packets?.PacketHandlers.ContainsKey(packetId);
                    if (x != null && x != false)
                    {
                        _serv.Packets?.PacketHandlers[packetId].DynamicInvoke(_serv, _id, packet);
                    }
                    else
                    {
                        Terminal.LogError(
                            $"[{_serv.DisplayName}] Received unregistered UDP packet with ID: {packetId}");
                    }
                }
                catch (Exception e)
                {
                    Terminal.LogError($"[{_serv.DisplayName}] Error processing packet: {e}");
                    throw;
                }
            });
        }

        public void Disconnect()
        {
            EndPoint = null;
        }
    }

    public void Disconnect()
    {
        Terminal.LogInfo(
            $"[{ServerParent.DisplayName}] Disconnecting Client ({Tcp?.Socket?.Client.RemoteEndPoint})...");

        Tcp?.Disconnect();
        Udp.Disconnect();
        Attributes.Clear();
    }
}