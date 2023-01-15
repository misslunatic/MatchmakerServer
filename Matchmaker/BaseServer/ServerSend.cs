


namespace Matchmaker.Server.BaseServer;

public static class ServerSend
{

    public enum StatusType
    {
        OK,
        RECEIVED,
        FAIL
    }


    /// <summary>
    /// Sends TCP data to a Client
    /// </summary>
    /// <param name="server">Server the Client is in</param>
    /// <param name="toClient">ID for the recipient</param>
    /// <param name="packet">Data to send</param>
    public static void SendTcpData(Server server, int toClient, Packet packet)
    {
        packet.WriteLength();
        var tcp = server.Clients[toClient].Tcp;
        tcp?.SendData(packet);
    }

    /// <summary>
    /// Sends UDP data to a Client
    /// </summary>
    /// <param name="server">Server the Client is in</param>
    /// <param name="toClient">ID for the recipient</param>
    /// <param name="packet">Data to send</param>
    // ReSharper disable once UnusedMember.Global
    public static void SendUdpData(Server server, int toClient, Packet packet)
    {
        packet.WriteLength();
        var udp = server.Clients[toClient].Udp;
        udp.SendData(packet);
    }
    
    /// <summary>
    /// Sends TCP data to all Clients
    /// </summary>
    /// <param name="server">Server the Client is in</param>
    /// <param name="packet">Data to send</param>
    // ReSharper disable once UnusedMember.Global
    public static void SendTcpDataToAll(Server server, Packet packet)
    {
        packet.WriteLength();
        for (var i = 1; i <= server.MaxPlayers; i++)
        {
            var tcp = server.Clients[i].Tcp;
            tcp?.SendData(packet);
        }
    }
    
    /// <summary>
    /// Sends TCP data to all Clients except one
    /// </summary>
    /// <param name="server">Server the Client is in</param>
    /// /// <param name="exceptClient">What Client to exclude</param>
    /// <param name="packet">Data to send</param>
    // ReSharper disable once UnusedMember.Global
    public static void SendTcpDataToAll(Server server, int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (var i = 1; i <= server.MaxPlayers; i++)
        {
            if (i == exceptClient) continue;
            var tcp = server.Clients[i].Tcp;
            tcp?.SendData(packet);
        }
    }

    /// <summary>
    /// Sends UDP data to all Clients
    /// </summary>
    /// <param name="server">Server the Client is in</param>
    /// <param name="packet">Data to send</param>
    // ReSharper disable once UnusedMember.Global
    public static void SendUdpDataToAll(Server server, Packet packet)
    {
        packet.WriteLength();
        for (var i = 1; i <= server.MaxPlayers; i++)
        {
            var udp = server.Clients[i].Udp;
            udp.SendData(packet);
        }
    }
    
    
    /// <summary>
    /// Sends UDP data to all Clients except one
    /// </summary>
    /// <param name="server">Server the Client is in</param>
    /// <param name="exceptClient">What Client to exclude</param>
    /// <param name="packet">Data to send</param>
    // ReSharper disable once UnusedMember.Global
    public static void SendUdpDataToAll(Server server, int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (var i = 1; i <= server.MaxPlayers; i++)
        {
            if (i == exceptClient) continue;
            var udp = server.Clients[i].Udp;
            udp.SendData(packet);
        }
    }
    
    /// <summary>
    /// Sends TCP data to a Client
    /// </summary>
    /// <param name="server">Server the Client is in</param>
    /// <param name="toClient">ID for the recipient</param>
    /// <param name="packet">Data to send</param>
    public static void SendTcpDataNoSync(Server server, int toClient, Packet packet)
    {
        packet.WriteLength();
        var tcp = server.Clients[toClient].Tcp;
        tcp?.SendData(packet);
    }

    /// <summary>
    /// Sends UDP data to a Client
    /// </summary>
    /// <param name="server">Server the Client is in</param>
    /// <param name="toClient">ID for the recipient</param>
    /// <param name="packet">Data to send</param>
    // ReSharper disable once UnusedMember.Global
    public static void SendUdpDataNoSync(Server server, int toClient, Packet packet)
    {
        packet.WriteLength();
        var udp = server.Clients[toClient].Udp;
        udp.SendData(packet);
    }
    
    /// <summary>
    /// Sends TCP data to all Clients
    /// </summary>
    /// <param name="server">Server the Client is in</param>
    /// <param name="packet">Data to send</param>
    // ReSharper disable once UnusedMember.Global
    public static void SendTcpDataToAllNoSync(Server server, Packet packet)
    {
        packet.WriteLength();
        for (var i = 1; i <= server.MaxPlayers; i++)
        {
            var tcp = server.Clients[i].Tcp;
            tcp?.SendData(packet);
        }
    }
    
    /// <summary>
    /// Sends TCP data to all Clients except one
    /// </summary>
    /// <param name="server">Server the Client is in</param>
    /// /// <param name="exceptClient">What Client to exclude</param>
    /// <param name="packet">Data to send</param>
    // ReSharper disable once UnusedMember.Global
    public static void SendTcpDataToAllNoSync(Server server, int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (var i = 1; i <= server.MaxPlayers; i++)
        {
            if (i == exceptClient) continue;
            var tcp = server.Clients[i].Tcp;
            tcp?.SendData(packet);
        }
    }

    /// <summary>
    /// Sends UDP data to all Clients
    /// </summary>
    /// <param name="server">Server the Client is in</param>
    /// <param name="packet">Data to send</param>
    // ReSharper disable once UnusedMember.Global
    public static void SendUdpDataToAllNoSync(Server server, Packet packet)
    {
        packet.WriteLength();
        for (var i = 1; i <= server.MaxPlayers; i++)
        {
            var udp = server.Clients[i].Udp;
            udp.SendData(packet);
        }
    }
    
    
    /// <summary>
    /// Sends UDP data to all Clients except one
    /// </summary>
    /// <param name="server">Server the Client is in</param>
    /// <param name="exceptClient">What Client to exclude</param>
    /// <param name="packet">Data to send</param>
    // ReSharper disable once UnusedMember.Global
    public static void SendUdpDataToAllNoSync(Server server, int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (var i = 1; i <= server.MaxPlayers; i++)
        {
            if (i == exceptClient) continue;
            var udp = server.Clients[i].Udp;
            udp.SendData(packet);
        }
    }

    #region Built-in Packets

    /// <summary>
    /// Welcome message for Client
    /// </summary>
    /// <param name="server">Server the Client is in</param>
    /// <param name="toClient">ID for the recipient</param>
    /// <param name="msg">Message to send</param>
    public static void Welcome(Server server, int toClient, string msg)
    {
        Terminal.LogDebug($"[{server.DisplayName}] Sending 'Welcome' message.");
        using var packet = new Packet(int.MaxValue);
        packet.Write(msg);
        packet.Write(toClient);
        packet.Write(Config.MatchmakerAPIVersion);
        packet.Write(Config.GameId);
        packet.Write(Config.GameVersion);

        SendTcpData(server, toClient, packet);
    }
        
    /// <summary>
    /// Disconnect a Client
    /// </summary>
    /// <param name="server">What server the Client is in</param>
    /// <param name="toClient">What Client to kick</param>
    public static void DisconnectClient(Server server, int toClient)
    {
        using var packet = new Packet(int.MaxValue-1);
        SendTcpData(server, toClient, packet);
    }
        
    /// <summary>
    /// Send an Attribute to the Client
    /// </summary>
    /// <param name="server">What server the Client is in</param>
    /// <param name="toClient">ID for the recipient</param>
    /// <param name="requestedId">What Client the Attribute is from</param>
    /// <param name="name">The Attribute name</param>
    /// <param name="value">The Attribute value</param>
    public static void GetClientAttributesReceived(BaseServer.Server server, int toClient, int requestedId, string name, string value)
    {
        using var packet = new Packet(int.MaxValue-2);
        packet.Write(requestedId);
        packet.Write(name);
        packet.Write(value);

        ServerSend.SendTcpData(server, toClient, packet);
    }
    
    /// <summary>
    /// Respond to the client with a status
    /// </summary>
    /// <param name="server">What server the Client is in</param>
    /// <param name="toClient">ID for the recipient</param>
    /// <param name="status">What status</param>
    public static void Status(BaseServer.Server server, int toClient, StatusType status)
    {
        Terminal.LogDebug($"[{server.DisplayName}] Sending status {status}...");
        using var packet = new Packet(int.MaxValue-3);
        packet.Write((int)status);

        ServerSend.SendUdpDataNoSync(server, toClient, packet);
    }


       
    #endregion
}