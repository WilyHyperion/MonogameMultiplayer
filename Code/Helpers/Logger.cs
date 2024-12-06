

using Microsoft.Xna.Framework;
using Game.Abstract.UI;
using System;
namespace Game.Helpers;
public static class Logger {
    public static void Flush()
    {
        LogText.Text = "";
    }
    public static UILogText    LogText;
    public static void Log(string message)
    {
        LogText.Text += message + "\n";
        int numLines = LogText.Text.Split('\n').Length;
        if (numLines > 10)
        {
            LogText.Text = LogText.Text.Substring(LogText.Text.IndexOf('\n') + 1);
        }
    }
    public static void Log(string message, Color color)
    {
        LogText.Text += message;
        LogText.color = color;
    }
}