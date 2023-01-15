using System.Net;

// ReSharper disable once CheckNamespace
namespace Matchmaker.Server.BaseServer;

internal static class ServerHandle
{
    /// <summary>
    /// Gets the message received in response to the Welcome message
    /// </summary>
    /// <param name="server"></param>
    /// <param name="fromClient"></param>
    /// <param name="packet"></param>
    public static void WelcomeReceived(Server server, int fromClient, Packet packet)
    {
        try
        {
            var clientIdCheck = packet.ReadInt();
            var clientAPIVersion = packet.ReadString();
            var clientGameVersion = packet.ReadString();
            var clientGameId = packet.ReadString();

            if (clientAPIVersion != Config.MatchmakerAPIVersion)
            {
                Terminal.LogError(
                    $"[{server.DisplayName}] Client API is not the same version as the Server! \n Server API version: {Config.MatchmakerAPIVersion}. \n Client API version: {clientAPIVersion}");
                server.Clients[fromClient].Disconnect();
                return;
            }
   
            if (clientGameId != Config.GameId)
            {
                Terminal.LogError($"Client Game ID and Server Game ID do not match: \n Server Game ID: {Config.GameId}. \n Client Game ID: {clientGameId}.");
                server.Clients[fromClient].Disconnect();
                return;
            }

            if (clientGameVersion != Config.GameVersion)
            {
                Terminal.LogError($"Client Game Version and Server Game Version do not match: \n Server Game Ver: {Config.GameVersion}. \n Client Game Ver: {clientGameVersion}.");
                server.Clients[fromClient].Disconnect();
                return;
            }

            var tcp = server.Clients[fromClient].Tcp;
            if (tcp == null)
            {
                Terminal.LogWarn("TCP is null!");
            }

            if (tcp is { Socket: { } })
                Terminal.LogSuccess(
                    $"[{server.DisplayName}] {tcp.Socket.Client.RemoteEndPoint} connected successfully and is now player {fromClient}.");
            if (fromClient != clientIdCheck)
            {
                Terminal.LogWarn(
                    $"[{server.DisplayName}] Player (ID: {fromClient}) has assumed the wrong client ID ({clientIdCheck})!");
            }


            var remoteIpEndPoint = server.Clients[fromClient].Tcp?.Socket?.Client.RemoteEndPoint as IPEndPoint;
            if (remoteIpEndPoint == null)
            {
                Terminal.LogDebug($"[{server.DisplayName}] WelcomeReceived: RemoteIpEndPoint is NULL!");
                server.Clients[fromClient].Attributes.Clear();
                server.Clients[fromClient].Disconnect();
                return;
            }

            var uuidList = new List<string>();
            for (var i = 1; i < server.Clients.Count; i++)
            {
                if (server.Clients[i].IsConnected)
                {
                    var x = server.Clients[i].Attributes.GetAttribute("_auto_uuid");
                    if (x != null)
                    {
                        uuidList.Add(x);
                    }
                }
            }


            server.Clients[fromClient].Attributes.SetAttribute("_auto_ip", remoteIpEndPoint.Address.ToString());
            server.Clients[fromClient].Attributes.SetAttribute("_auto_port", remoteIpEndPoint.Port.ToString());
            server.Clients[fromClient].Attributes
                .SetAttribute("_auto_uuid", new UUID(Config.UuidLength, uuidList).GetValue());
            server.Clients[fromClient].Attributes
                .SetAttribute("_auto_game_version", clientGameVersion);

            Terminal.LogInfo(
                $"[{server.DisplayName}] Automatically assigned _auto_ip for client {fromClient}. _auto_ip = {server.Clients[fromClient].Attributes.GetAttribute("_auto_ip")}");
            Terminal.LogInfo(
                $"[{server.DisplayName}] Automatically assigned _auto_port for client {fromClient}. _auto_port = {server.Clients[fromClient].Attributes.GetAttribute("_auto_port")}");
            Terminal.LogInfo(
                $"[{server.DisplayName}] Automatically assigned _auto_uuid for client {fromClient}. _auto_uuid = {server.Clients[fromClient].Attributes.GetAttribute("_auto_uuid")}");
            Terminal.LogInfo(
                $"[{server.DisplayName}] Automatically assigned _auto_game_version for client {fromClient} based on received Client data. _auto_game_version = {server.Clients[fromClient].Attributes.GetAttribute("_auto_game_version")}");
        }
        catch (Exception ex)
        {
            Terminal.LogError($"[{server.DisplayName}] Error in WelcomeReceived packet handler: {ex}");
            ServerSend.Status(server, fromClient, ServerSend.StatusType.FAIL);
            server.Clients[fromClient].Disconnect();
        }
    }

    public static void SetClientAttribute(Server server, int fromClient, Packet packet)
    {
        var clientIdCheck = packet.ReadInt();
        var name = packet.ReadString();
        var value = packet.ReadString();

        var tcp = server.Clients[fromClient].Tcp;
        if (tcp is { Socket: { } })
            Terminal.LogInfo(
                $"[{server.DisplayName}] {tcp.Socket.Client.RemoteEndPoint} is changing Client Attribute {name} to {value}.");
        if (fromClient != clientIdCheck)
        {
            Terminal.LogWarn(
                $"[{server.DisplayName}] Player (ID: {fromClient}) has assumed the wrong client ID ({clientIdCheck})!");

            return;
        }

        server.Clients[fromClient].Attributes.SetAttribute(name, value);
    }

    public static void GetClientAttribute(Server server, int fromClient, Packet packet)
    {
        var clientIdCheck = packet.ReadInt();
        var requestedId = packet.ReadInt();
        var name = packet.ReadString();

        try
        {
            var tcp = server.Clients[fromClient].Tcp;
            if (tcp is { Socket: { } })
                Terminal.LogInfo(
                    $"[{server.DisplayName}] {tcp.Socket.Client.RemoteEndPoint} requests Client Attribute {name}.");
            if (fromClient != clientIdCheck)
            {
                Terminal.LogWarn(
                    $"[{server.DisplayName}] Player (ID: {fromClient}) has assumed the wrong client ID ({clientIdCheck})!");
            }

            var value = server.Clients[requestedId].Attributes.GetAttribute(name);
            ServerSend.GetClientAttributesReceived(server, fromClient, requestedId, name, value ?? "");
        }
        catch (Exception ex)
        {
            Terminal.LogError($"[{server.DisplayName}] Error in GetClientAttribute: {ex}");
            ServerSend.GetClientAttributesReceived(server, fromClient, requestedId, name, "ERR_HANDLED_EXCEPTION");
        }
    }
}