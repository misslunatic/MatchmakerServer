namespace Matchmaker.Server;

public static class Config
{
    /// <summary>
    /// This allows you to name the server so your game cannot connect to other ones.
    /// Use the game name here and on the client as well.
    /// </summary>
    public const string GameId = "My Game Name";

    /// <summary>
    /// The version of the matchmaker API. This must match here and on the client for a connection.
    /// </summary>
    public const string MatchmakerAPIVersion = "1.1.6";

    /// <summary>
    /// What version the game is. This must match here and on the client for a connection.
    /// </summary>
    public const string GameVersion = "0.0.1";

    /// <summary>
    /// How many people can be connected to the List server instance at the same time.
    /// </summary>
    public const ushort ListMaxClients = 512;

    /// <summary>
    /// The maximum amount of Lobbies.
    /// </summary>
    public const ushort MaxLobbies = 1024;

    /// <summary>
    /// What port the server will be on (0-65535). The lobby server will use a port two (2) above this.
    /// </summary>
    public const ushort Port = 26950;

    /// <summary>
    /// The display name for the List server instance, if you want to change it.
    /// </summary>
    public const string ListServerDisplayName = "List Server";
    
    /// <summary>
    /// The display name for the Lobby server instance, if you want to change it.
    /// </summary>
    public const string LobbyServerDisplayName = "Lobby Server";

    /// <summary>
    /// How long generated UUIDs are
    /// </summary>
    public const int UuidLength = 7;

    /// <summary>
    /// Sends a response to the Client whenever a message is received. Slow, for debugging.
    /// </summary>
    public const bool Sync = false;

    /// <summary>
    /// Whether or not to show LogDebug() text.
    /// </summary>
    public const bool ShowTerminalDebug = true;

    /// <summary>
    /// Text that is prepended to a Log() text.
    /// </summary>
    public const string LogText = "  LOG   ";

    /// <summary>
    /// Text that is prepended to a LogDebug() text.
    /// </summary>
    public const string LogDebugText = " DEBUG  ";

    /// <summary>
    /// Text that is prepended to a LogInfo() text.
    /// </summary>
    public const string LogInfoText =  " CHECK  ";

    /// <summary>
    /// Text that is prepended to a LogWarn() text.
    /// </summary>
    public const string LogWarningText = "WARNING ";

    /// <summary>
    /// Text that is prepended to a LogError() text.
    /// </summary>
    public const string LogErrorText = " ERROR  ";

    /// <summary>
    /// Text that is prepended to a LogSuccess() text.
    /// </summary>
    public const string LogSuccessText = "SUCCESS ";
}