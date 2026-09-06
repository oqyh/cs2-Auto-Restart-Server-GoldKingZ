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
    public static void CheckTimerInGlobals(Globals.EventTimer Event, CounterStrikeSharp.API.Modules.Timers.Timer timer, float wait, int method, int limit, string commands = "", string map = "")
    {
        ResetTimer(Event);

        if (!Globals.TimerCheck_Data.ContainsKey(Event))
        {
            var initialData = new Globals.TimerCheckClass
            {
                Event_Delay = timer,
                Remaining = wait,
                Method = method,
                Limit = limit,
                Commands = commands,
                Map = map
            };
            Globals.TimerCheck_Data.TryAdd(Event, initialData);
        }else
        {
            var data = Globals.TimerCheck_Data[Event];
            data.Event_Delay = timer;
            data.Remaining = wait;
            data.Method = method;
            data.Limit = limit;
            data.Commands = commands;
            data.Map = map;
            data.CommandsSent = false;
            data.Grace = 0f;
        }

        Debug($"[{Event}] countdown {wait:0}s - method {method}, limit {limit}");
    }
    
    public static void ResetTimer(Globals.EventTimer Event, string reason = "")
    {
        if (!Globals.TimerCheck_Data.TryGetValue(Event, out var data) || data.Event_Delay == null) return;

        data.Event_Delay.Kill();
        data.Event_Delay = null;

        Globals.CenterHtml = "";
        Globals.CenterBottom = "";

        if (string.IsNullOrEmpty(reason)) return;

        var text = Lang("PrintToChatToAll.Restart.Cancelled", Event, data, (int)Math.Round(data.Remaining));
        if (text.Length > 0) AdvancedServerPrintToChatAll(text);
        Debug($"[{Event}] countdown cancelled - {reason}");
    }

    public static void CheckTimer()
    {
        if (Globals.TimerCheck != null) return;

        Globals.TimerCheck = MainPlugin.Instance.AddTimer(3.0f, CallBack_CheckTimer, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
    }

    private static void CallBack_CheckTimer()
    {
        var GetPlayers = GetPlayersController().Count;

        UpRunningServer.Check(GetPlayers);
        Schedule.Check(GetPlayers);
    }
    
    public static void ExecuteMethod(Globals.EventTimer Event, int Method, string RestartMethod_Mode4 = "")
    {
        Debug($"[{Event}] restarting now - method {Method}");
        Schedule.MarkIfInWindow();
        
        if (Configs.Instance.TextLog_Enable || !string.IsNullOrWhiteSpace(Configs.Instance.Discord_WebHook))
        {
            var reps = GetPlaceholders(Event, Method);
            WriteTextLog(reps);
            SendDiscord(reps);
        }

        string Command = "";
        if (Method == 1)
        {
            Command = "sv_cheats true; quit";
        }
        else if (Method == 2)
        {
            Command = "sv_cheats true; crash";
        }
        else if (Method == 3)
        {
            RelaunchServer();
        }
        else if (Method == 4)
        {
            var map = RestartMethod_Mode4.Trim();

            if (string.IsNullOrWhiteSpace(map))
            {
                Globals.Restarting = true;
                RestartMap();
                return;
            }

            if (map.StartsWith("host:", StringComparison.OrdinalIgnoreCase))
                Command = $"host_workshop_map {map.Substring(5)}";
            else if (map.StartsWith("ds:", StringComparison.OrdinalIgnoreCase))
                Command = $"ds_workshop_changelevel {map.Substring(3)}";
            else
                Command = $"changelevel {map}";
        }

        if (!string.IsNullOrWhiteSpace(Command))
        {
            Globals.Restarting = true;
            Server.ExecuteCommand(Command);
        }
    }

    public static void RestartMap()
    {
        var MapName = Server.MapName;
        var MapWorkShop_ID = Server_Utils.GetAddonID();

        if (!string.IsNullOrWhiteSpace(MapWorkShop_ID))
        {
            Server.ExecuteCommand("host_workshop_map " + MapWorkShop_ID);
        }
        else
        {
            Server.ExecuteCommand("changelevel " + MapName);
        }
    }

    private static void RelaunchServer()
    {
        try
        {
            var exe = Environment.ProcessPath;
            if (string.IsNullOrWhiteSpace(exe))
            {
                Debug("Cant Find Server Exe Path, Relaunch Failed", true);
                return;
            }

            var dir = Path.GetDirectoryName(exe) ?? "";
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            var psi = new ProcessStartInfo
            {
                FileName = exe,
                WorkingDirectory = dir,
                UseShellExecute = isWindows
            };

            foreach (var a in Environment.GetCommandLineArgs().Skip(1))
            {
                psi.ArgumentList.Add(a);
            }

            Process.Start(psi);
        }
        catch (Exception ex)
        {
            Debug($"Relaunch Failed : {ex.Message}", true);
            return;
        }

        Server.NextFrame(() => Server.ExecuteCommand("sv_cheats true; quit"));
    }
}
