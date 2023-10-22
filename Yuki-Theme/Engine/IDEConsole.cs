using System;

namespace YukiTheme.Engine;

// ReSharper disable once InconsistentNaming
public static class IDEConsole
{
    internal static void Log(string text)
    {
        IDEAlterer.Instance.Form1.AddTextToCompilerMessages($"Yuki Theme: {text}{Environment.NewLine}");
    }
}