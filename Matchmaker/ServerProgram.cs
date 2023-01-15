using Matchmaker.Server.BaseServer;

namespace Matchmaker.Server;

public static class Program
{
    // ReSharper disable once FieldCanBeMadeReadOnly.Global
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once ConvertToConstant.Global
    public static bool RunLoop = true;

    
        private static BaseServer.Server? _mainServ = new();
    public static BaseServer.Server? LobbyServ = new();
    private static bool _serverStarted;

    public static void Main()
    {
        Console.WriteLine(
            $"Techiesplash's Matchmaking API - Server (Ver. {Config.MatchmakerAPIVersion})");
        StartServer();

        while (RunLoop)
        {
        }
    }

    public static void StartServer()
    {
        if (!_serverStarted)
        {
            Console.Title = "Matchmaking Server";

            ThreadManager.Start();

            var ClientPackets = new BaseServer.Server.ServerPackets(
                new Dictionary<int, BaseServer.Server.ServerPackets.PacketHandler>
                {
                    { 10, Packets.PacketHandlers.RequestAllLobbyIDs },
                    { 11, Packets.PacketHandlers.RequestLobbyIdsWithMatchingAttribute },
                    { 12, Packets.PacketHandlers.GetLobbyAttribute }
                });

            var LobbyPackets = new BaseServer.Server.ServerPackets(
                new Dictionary<int, BaseServer.Server.ServerPackets.PacketHandler>
                {
                });
            _mainServ = new BaseServer.Server();
            LobbyServ = new BaseServer.Server();
            _mainServ.Start(ClientPackets, Config.ListMaxClients, 26950, "List Server");
            LobbyServ.Start(LobbyPackets, Config.MaxLobbies, 26952, "Lobby Server");
            _serverStarted = true;
        }
        else
        {
            Terminal.LogWarn("Server already started.");
        }
    }

    public static void StopServer()
    {
        _serverStarted = false;
        _mainServ = null;
        LobbyServ = null;
        ThreadManager.Stop();
    }
}