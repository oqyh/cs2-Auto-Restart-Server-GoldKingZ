using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.UserMessages;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core.Attributes.Registration;

namespace Auto_Restart_Server_GoldKingZ;

public partial class Helper
{
    public static bool Countdown(Globals.EventTimer Event, Globals.TimerCheckClass data)
    {
        var left = (int)Math.Round(data.Remaining);

        if (data.Remaining > 0)
        {
            Warning.SendChat(Event, data, left);
            Warning.SendCenter(Event, data, left);

            data.Remaining -= 1f;
            return false;
        }

        if (!data.CommandsSent)
        {
            data.CommandsSent = true;

            Warning.SendChat(Event, data, 0, true);
            Warning.SendCenter(Event, data, 0);

            data.Grace = 3f;

            if (!string.IsNullOrWhiteSpace(data.Commands))
            {
                Server.ExecuteCommand(data.Commands);
                data.Grace = 6f;
                Debug($"[{Event}] commands sent \"{data.Commands}\", restarting in {data.Grace:0}s");
            }

            return false;
        }

        if (data.Grace > 0)
        {
            data.Grace -= 1f;
            return false;
        }

        return true;
    }

    public static string Lang(string key, Globals.EventTimer Event, Globals.TimerCheckClass data, int left)
    {
        string raw = MainPlugin.Instance.Localizer[key];
        if (string.IsNullOrWhiteSpace(raw) || raw == key) return "";

        object[] args = { TimeLeft(left), EventName(Event), MethodName(data.Method), GetPlayersController().Count, data.Limit };

        for (int i = 0; i < args.Length; i++)
        {
            raw = raw.Replace($"{{{i}}}", args[i]?.ToString() ?? "");
        }

        return raw;
    }

    public static string TimeLeft(int secs)
    {
        if (secs >= 86400) { var d = secs / 86400; return $"{d} day{(d == 1 ? "" : "s")}"; }
        if (secs >= 3600)  { var h = secs / 3600;  return $"{h} hour{(h == 1 ? "" : "s")}"; }
        if (secs >= 60)    { var m = secs / 60;    return $"{m} minute{(m == 1 ? "" : "s")}"; }
        return $"{secs} second{(secs == 1 ? "" : "s")}";
    }

    public static string EventName(Globals.EventTimer Event) => Event switch
    {
        Globals.EventTimer.OnDisconnect => "Last Player Disconnect",
        Globals.EventTimer.UpRunningServer => "Up Running Server",
        Globals.EventTimer.Schedule => "Schedule",
        _ => Event.ToString()
    };

    public static string MethodName(int method) => method switch
    {
        1 => "Quit",
        2 => "Crash",
        3 => "Relaunch",
        4 => "Normal Restart",
        _ => "Unknown"
    };
}
