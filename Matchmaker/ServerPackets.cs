using Matchmaker.Server.BaseServer;

namespace Matchmaker.Server;

public static class Packets
{
    public static class PacketHandlers
    {
        /// <summary>
        /// Handle for if a client requests all IDs in the Lobby server database
        /// </summary>
        /// <param name="server">What server the Client is in</param>
        /// <param name="fromClient">What Client the request is from</param>
        /// <param name="packet">The data</param>
        public static void RequestAllLobbyIDs(BaseServer.Server server, int fromClient, Packet packet)
        {
            var clientIdCheck = packet.ReadInt();

            var tcp = server.Clients[fromClient].Tcp;
            if (tcp is { Socket: { } })
                Terminal.LogInfo(
                    $"[{server.DisplayName}] Client {fromClient} has requested to cycle through all available lobbies.");

            if (fromClient != clientIdCheck)
            {
                Terminal.LogWarn(
                    $"[{server.DisplayName}] [RequestAllLobbyIDs] Player (ID: {fromClient}) has assumed the wrong client ID ({clientIdCheck})!");
            }

            for (var i = 1; i <= Program.LobbyServ!.Clients.Count; i++)
            {
                if (Program.LobbyServ.Clients[i].IsConnected)
                {
                    ServerPackets.SendLobbyId(server, fromClient, i, 0);
                }
            }

            ServerPackets.FinishedSendingLobbyIDs(server, fromClient);
        }

        /// <summary>
        /// Handle for if a client requests an ID from the Lobby server database matching a UUID
        /// </summary>
        /// <param name="server">What server the Client is in</param>
        /// <param name="fromClient">What Client the request is from</param>
        /// <param name="packet">The data</param>
        public static void RequestLobbyIdsWithMatchingAttribute(BaseServer.Server server, int fromClient, Packet packet)
        {
            var clientIdCheck = packet.ReadInt();
            var clientAttribName = packet.ReadString();
            var clientAttribValue = packet.ReadString();
            var tcp = server.Clients[fromClient].Tcp;
            if (tcp is { Socket: { } })
                Terminal.LogInfo(
                    $"[{server.DisplayName}] {tcp.Socket.Client.RemoteEndPoint} requests all Lobby IDs with a matching Attribute ({clientAttribName}={clientAttribValue})");

            if (fromClient != clientIdCheck)
            {
                Terminal.LogWarn(
                    $"[{server.DisplayName}] [RequestLobbyIDMatchingUUID] Player (ID: {fromClient}) has assumed the wrong client ID ({clientIdCheck})!");
            }

            var i = 1;
            foreach (var lobby in Program.LobbyServ!.Clients)
            {
                if (lobby.Value.Attributes.GetAttribute(clientAttribName) == clientAttribValue)
                {
                    Terminal.LogSuccess(
                        $"[{server.DisplayName}] Found Lobby with matching Attribute value ({clientAttribName}={clientAttribValue}). LobbyId: {i}");
                    ServerPackets.SendLobbyId(server, fromClient, i, 0);
                }


                i++;
            }

            ServerPackets.FinishedSendingLobbyIDs(server, fromClient);
        }


        public static void GetLobbyAttribute(BaseServer.Server server, int fromClient, Packet packet)
        {
            var clientIdCheck = packet.ReadInt();
            var requestedId = packet.ReadInt();
            var name = packet.ReadString();

            var tcp = server.Clients[fromClient].Tcp;
            if (tcp is { Socket: { } })
                Terminal.LogInfo(
                    $"[{server.DisplayName}] Client {fromClient} requests Lobby Attribute {name}.");
            if (fromClient != clientIdCheck)
            {
                Terminal.LogWarn(
                    $"[{server.DisplayName}] GetLobbyAttribute] Player (ID: {fromClient}) has assumed the wrong client ID ({clientIdCheck})!");
            }

            if (!Program.LobbyServ!.Clients[requestedId].IsConnected)
            {
                Program.LobbyServ.Clients[requestedId].Disconnect();
                Terminal.LogDebug($"[{server.DisplayName}] Attempted to get Attribute of disconnected lobby!");
                ServerPackets.GetLobbyAttributesReceived(server, fromClient, requestedId, name,
                    "ERR_DISCONNECTED_LOBBY");
                return;
            }

            var value = Program.LobbyServ.Clients[requestedId].Attributes.GetAttribute(name);
            ServerPackets.GetLobbyAttributesReceived(server, fromClient, requestedId, name, value ?? "");
        }
    }

    private static class ServerPackets
    {
        /// <summary>
        /// Send a Server's ID through a Callback
        /// </summary>
        /// <param name="server">What server the Client is in</param>
        /// <param name="toClient">ID for the recipient</param>
        /// <param name="serverId">What ID is being passed from the Server database</param>
        public static void SendLobbyId(BaseServer.Server server, int toClient, int serverId, int mod)
        {
            Terminal.LogDebug($"[{server.DisplayName}] Sending lobby ID {serverId} to Client {toClient}...");
            using var packet = new Packet(10);
            packet.Write(serverId);
            packet.Write(mod);

            ServerSend.SendTcpData(server, toClient, packet);
        }

        /// <summary>
        /// Send a Server's Attribute to the Client
        /// </summary>
        /// <param name="server">What server the Client is in</param>
        /// <param name="toClient">ID for the recipient</param>
        /// <param name="requestedId">What Server the Attribute is from</param>
        /// <param name="name">The Attribute name</param>
        /// <param name="value">The Attribute value</param>
        public static void GetLobbyAttributesReceived(BaseServer.Server server, int toClient, int requestedId,
            string name, string value)
        {
            Terminal.LogDebug(
                $"[{server.DisplayName}] Sending Lobby Attributes. toClient={toClient} requestedId={requestedId} name={name} value={value}");
            using var packet = new Packet(11);
            packet.Write(requestedId);
            packet.Write(name);
            packet.Write(value);

            ServerSend.SendUdpData(server, toClient, packet);
        }

        public static void FinishedSendingLobbyIDs(BaseServer.Server server, int toClient)
        {
            Terminal.LogDebug(
                $"[{server.DisplayName}] Informing client that we have finished sending Lobby IDs...");
            using var packet = new Packet(12);

            ServerSend.SendTcpData(server, toClient, packet);
        }
    }
}