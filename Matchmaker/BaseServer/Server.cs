

using System.Net;
using System.Net.Sockets;

namespace Matchmaker.Server.BaseServer;

public class Server
{
    public int MaxPlayers;
    private int _port;
    public readonly Dictionary<int, Client> Clients = new();

    public string DisplayName = "Unnamed Server";

    public class ServerPackets
    {
        public delegate void PacketHandler(Server server, int fromClient, Packet packet);

        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public Dictionary<int, PacketHandler> PacketHandlers;

        public ServerPackets(Dictionary<int, PacketHandler> packetHandlers)
        {
            PacketHandlers = packetHandlers;
            PacketHandlers.Add(int.MaxValue, ServerHandle.WelcomeReceived);
            PacketHandlers.Add(int.MaxValue - 1, ServerHandle.SetClientAttribute);
            PacketHandlers.Add(int.MaxValue - 2, ServerHandle.GetClientAttribute);
        }
    }

    public ServerPackets? Packets;
    /*
            public Dictionary<int, Delegate> packetHandlers = new Dictionary<int, Delegate>()
            {
                { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.setClientAttributes, ServerHandle.SetClientAttribute },
                { (int)ClientPackets.getClientAttributes, ServerHandle.GetClientAttribute }
            };
    */
    private TcpListener _tcpListener = null!;

    private UdpClient _udpListener = null!;

    // ReSharper disable once UnusedMethodReturnValue.Global
    public bool Start(ServerPackets packets, int maxPlayers, int port, string displayName)
    {
        try
        {
            DisplayName = displayName;

            Packets = packets;

            MaxPlayers = maxPlayers;
            _port = port;

            InitializeServerData();

            _tcpListener = new TcpListener(IPAddress.Any, _port);
            _tcpListener.Start();
            _tcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);

            _udpListener = new UdpClient(_port);
            _udpListener.BeginReceive(UdpReceiveCallback, null);

            Terminal.LogSuccess($"[{DisplayName}] Server started on port {_port}.");
            return true;
        }
        catch (Exception ex)
        {
            Terminal.LogError($"[{DisplayName}] Failed to start server: {ex}");
            return false;
        }
    }

    public void DisconnectClient(int id)
    {
        Clients[id].Disconnect();
    }


    private void TcpConnectCallback(IAsyncResult result)
    {
        var client = _tcpListener.EndAcceptTcpClient(result);
        _tcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);
        Terminal.LogInfo($"[{DisplayName}] Incoming connection from {client.Client.RemoteEndPoint}...");

        for (var i = 1; i <= MaxPlayers; i++)
        {
            if (Clients[i].Tcp!.Socket != null) continue;
            Clients[i].Tcp!.Connect(client);
            return;
        }

        Terminal.LogWarn($"[{DisplayName}] {client.Client.RemoteEndPoint} failed to connect: Server full!");
    }

    private void UdpReceiveCallback(IAsyncResult result)
    {
        try
        {
            var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            var data = _udpListener.EndReceive(result, ref clientEndPoint!);
            _udpListener.BeginReceive(UdpReceiveCallback, null);

            if (data.Length < 4)
            {
                return;
            }

            using var packet = new Packet(data);
            var clientId = packet.ReadInt();

            if (clientId == 0)
            {
                return;
            }

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (Clients[clientId].Udp.EndPoint == null)
            {
                Clients[clientId].Udp.Connect(clientEndPoint);

                return;
            }

            if (Clients[clientId].Udp.EndPoint?.ToString() == clientEndPoint.ToString())
            {
                Clients[clientId].Udp.HandleData(packet);
            }
        }
        catch (Exception ex)
        {
            Terminal.LogError($"[{DisplayName}] Error receiving UDP data: {ex}");
        }
    }

    public void SendUdpData(IPEndPoint clientEndPoint, Packet packet)
    {
        try
        {
            _udpListener.BeginSend(packet.ToArray(), packet.Length(), clientEndPoint, null, null);
        }
        catch (Exception ex)
        {
            Terminal.LogError($"[{DisplayName}] Error sending data to {clientEndPoint} via UDP: {ex}");
        }
    }

    private void InitializeServerData()
    {
        for (var i = 1; i <= MaxPlayers; i++)
        {
            Clients.Add(i, new Client(i, this));
        }

        Terminal.LogSuccess($"[{DisplayName}] Server data is now initialized.");
    }
}