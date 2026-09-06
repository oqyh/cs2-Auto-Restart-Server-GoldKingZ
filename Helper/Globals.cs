using CounterStrikeSharp.API.Core;
using Timer = CounterStrikeSharp.API.Modules.Timers.Timer;

namespace Auto_Restart_Server_GoldKingZ;

public static class Globals
{
    public enum EventTimer { OnDisconnect = 0, UpRunningServer = 1, Schedule = 2 }
    public class TimerCheckClass
    {
        public Timer? Event_Delay { get; set; }
        public float Remaining { get; set; }
        public int Method { get; set; }
        public int Limit { get; set; }
        public string Commands { get; set; } = "";
        public string Map { get; set; } = "";
        public bool CommandsSent { get; set; }
        public float Grace { get; set; }
    }
    public static readonly Dictionary<EventTimer, TimerCheckClass> TimerCheck_Data = new();

    public class PlayerDataClass
    {
        public CCSPlayerController? Player { get; set; }
        public DateTime EventPlayerChat { get; set; } = DateTime.MinValue;
    }
    public static readonly Dictionary<int, PlayerDataClass> Player_Data = new();

    public static Timer? TimerCheck;
    public static bool Event_Calling => TimerCheck_Data.Values.Any(x => x.Event_Delay != null);
    public static bool Schedule_Loaded = false;
    public static bool Restarting = false;
    public static string Schedule_Saved = "";
    public static string CenterHtml = "";
    public static string CenterBottom = "";
    public static DateTime Schedule_Ocscurrence = DateTime.MinValue;

    public static void Clear()
    {
        Player_Data?.Clear();

        TimerCheck?.Kill();
        TimerCheck = null!;

        foreach (var data in TimerCheck_Data.Values)
        {
            data.Event_Delay?.Kill();
            data.Event_Delay = null!;
        }

        TimerCheck_Data?.Clear();

        Schedule_Loaded = false;
        Schedule_Saved = "";
        CenterHtml = "";
        CenterBottom = "";
        Schedule_Ocscurrence = DateTime.MinValue;
    }
}
