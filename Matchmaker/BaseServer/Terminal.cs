/*
 * MIT License
 *
 * Copyright (c) 2022 Vincent Dowling
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

namespace Matchmaker.Server.BaseServer;

public static class Terminal
{
    // ReSharper disable once UnusedMember.Global
    public static void Log(string str)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(Config.LogText);
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write("({0:t}) " + str + "\n", DateTime.Now);
        Console.ForegroundColor = ConsoleColor.White;
    }
    
    // ReSharper disable once UnusedMember.Global
    public static void LogError(string str)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(Config.LogErrorText);
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write("({0:t}) " + str + "\n", DateTime.Now);
        Console.ForegroundColor = ConsoleColor.White;
    }

    // ReSharper disable once UnusedMember.Global
    public static void LogDebug(string str)
    {

        if (Config.ShowTerminalDebug)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(Config.LogDebugText);
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("({0:t}) " + str + "\n", DateTime.Now);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    
    // ReSharper disable once UnusedMember.Global
    public static void LogWarn(string str)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(Config.LogWarningText);
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write("({0:t}) " + str + "\n", DateTime.Now);
        Console.ForegroundColor = ConsoleColor.White;
    }
    
    // ReSharper disable once UnusedMember.Global
    public static void LogSuccess(string str)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(Config.LogSuccessText);
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write("({0:t}) " + str + "\n", DateTime.Now);
        Console.ForegroundColor = ConsoleColor.White;
    }
    
    // ReSharper disable once UnusedMember.Global
    public static void LogInfo(string str)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(Config.LogInfoText);
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write("({0:t}) " + str + "\n", DateTime.Now);
        Console.ForegroundColor = ConsoleColor.White;
    }
}