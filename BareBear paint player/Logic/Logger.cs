using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BareBear_paint_player.Abstractions.Delegates;

namespace BareBear_paint_player.Logic;

public static class Logger
{
    private static DisplayLogDelegate? displayLogDelegate;

    // TODO: Implement file logging & Interface segregation
    public static void Log(string message)
    {
        Console.WriteLine(message);
        displayLogDelegate?.Invoke(message);
    }

    public static void SetDisplayLogDelegate(DisplayLogDelegate displayLogDelegate)
    {
        Logger.displayLogDelegate = displayLogDelegate;
    }

    public static string CreateTimestamp() => DateTime.Now.ToString("HH:mm:ss") + ": ";
}
