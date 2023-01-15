
namespace Matchmaker.Server.BaseServer;

internal static class ThreadManager
{
    private static readonly List<Action> ExecuteOnMainThreadList = new();
    private static readonly List<Action> ExecuteCopiedOnMainThread = new();
    private static bool _actionToExecuteOnMainThread;

    /// <summary>
    /// Controls whether or not the Main Thread should run. Set to FALSE to shut down Main Thread.
    /// </summary>
    private static bool _isRunning;
    private static Thread? _thread;
    
    
    /// <summary>Sets an action to be executed on the main thread.</summary>
    /// <param name="action">The action to be executed on the main thread.</param>
    public static void ExecuteOnMainThread(Action action)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (action == null)
        {
            Terminal.LogWarn("No action to execute on main thread!");
            return;
        }

        lock (ExecuteOnMainThreadList)
        {
            ExecuteOnMainThreadList.Add(action);
            _actionToExecuteOnMainThread = true;
        }
    }

    /// <summary>
    /// Start the Main Thread so Packets can be acted upon
    /// </summary>
    public static void Start()
    {
        _isRunning = true;
        _thread = new Thread(MainThread);
        _thread.Start();
    }

    /// <summary>
    /// Stop the Main Thread
    /// </summary>
    public static void Stop()
    {
        _isRunning = false;
    }

    /// <summary>
    /// Executes all code meant to run on the main thread. NOTE: Call this ONLY from the main thread.
    /// </summary>
    public static void UpdateMain()
    {
        if (!_actionToExecuteOnMainThread) return;
        ExecuteCopiedOnMainThread.Clear();
        lock (ExecuteOnMainThreadList)
        {
            ExecuteCopiedOnMainThread.AddRange(ExecuteOnMainThreadList);
            ExecuteOnMainThreadList.Clear();
            _actionToExecuteOnMainThread = false;
        }

        foreach (var t in ExecuteCopiedOnMainThread)
        {
            t();
        }
    }
    
    private static void MainThread()
    {
        
        var nextLoop = DateTime.Now;

        while (_isRunning)
        {
            while (nextLoop < DateTime.Now)
            {
                GameLogic.Update();

                nextLoop = nextLoop.AddMilliseconds(Constants.MsPerTick);

                if (nextLoop > DateTime.Now)
                {
                    Thread.Sleep(nextLoop - DateTime.Now);
                }
            }
        }
    }
}