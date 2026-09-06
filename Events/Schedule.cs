using System.Globalization;
using System.Text.RegularExpressions;
using CounterStrikeSharp.API.Modules.Timers;

namespace Auto_Restart_Server_GoldKingZ;

public static class Schedule
{
    private static string TrackFile => Path.Combine(MainPlugin.Instance.ModuleDirectory, "last_restart.dat");

    public static void Check(int GetPlayers)
    {
        var Schedule = Configs.Instance.Schedule.Trim();
        if (string.IsNullOrWhiteSpace(Schedule))
        {
            Helper.ResetTimer(Globals.EventTimer.Schedule);
            return;
        }

        if (!Globals.Schedule_Loaded)
        {
            Globals.Schedule_Loaded = true;
            LoadMark();
        }

        if (!string.Equals(Globals.Schedule_Saved, Schedule, StringComparison.OrdinalIgnoreCase))
        {
            Helper.Debug($"[Schedule] setting changed to \"{Schedule}\", current window marked so it does not fire instantly");
            SaveMark(GetOccurrence(Schedule));
        }

        var occurrence = GetOccurrence(Schedule);

        if (occurrence == DateTime.MinValue || Globals.Schedule_Ocscurrence == occurrence || GetPlayers > Configs.Instance.Schedule_WhenXPlayersInServerORLess)
        {
            Helper.ResetTimer(Globals.EventTimer.Schedule,
                occurrence == DateTime.MinValue ? "outside the window"
                : Globals.Schedule_Ocscurrence == occurrence ? "window already used"
                : $"players {GetPlayers} above limit {Configs.Instance.Schedule_WhenXPlayersInServerORLess}");
            return;
        }

        if (Globals.Event_Calling) return;

        Helper.CheckTimerInGlobals(Globals.EventTimer.Schedule,
            MainPlugin.Instance.AddTimer(1.0f, CallBack, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE),
            Configs.Instance.Schedule_RestartMethod_Wait.ToSeconds(),
            Configs.Instance.Schedule_RestartMethod,
            Configs.Instance.Schedule_WhenXPlayersInServerORLess,
            Configs.Instance.Schedule_Commands,
            Configs.Instance.Schedule_RestartMethod_Mode4);
    }

    private static void CallBack()
    {
        if (!Globals.TimerCheck_Data.TryGetValue(Globals.EventTimer.Schedule, out var data) || data.Event_Delay == null) return;

        var occurrence = GetOccurrence(Configs.Instance.Schedule);
        var players = Helper.GetPlayersController().Count;

        if (occurrence == DateTime.MinValue || Globals.Schedule_Ocscurrence == occurrence || players > data.Limit)
        {
            Helper.ResetTimer(Globals.EventTimer.Schedule,
                occurrence == DateTime.MinValue ? "window closed during the wait"
                : Globals.Schedule_Ocscurrence == occurrence ? "another trigger restarted first"
                : $"player joined during the wait, {players} above limit {data.Limit}");
            return;
        }

        if (!Helper.Countdown(Globals.EventTimer.Schedule, data)) return;

        SaveMark(occurrence);
        Helper.ExecuteMethod(Globals.EventTimer.Schedule, data.Method, data.Map);
    }

    private static void LoadMark()
    {
        try
        {
            var lines = File.ReadAllLines(TrackFile);
            Globals.Schedule_Saved = lines.Length > 0 ? lines[0].Trim() : "";

            if (lines.Length < 2 || !DateTime.TryParse(lines[1].Trim(), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var parsed))
            {
                parsed = DateTime.MinValue;
            }

            Globals.Schedule_Ocscurrence = parsed;
        }
        catch
        {
            Helper.Debug("[Schedule] no readable last_restart.dat, marking the current window so it does not fire on load");
            SaveMark(GetOccurrence(Configs.Instance.Schedule));
        }
    }

    private static void SaveMark(DateTime occurrence)
    {
        Globals.Schedule_Saved = Configs.Instance.Schedule.Trim();
        Globals.Schedule_Ocscurrence = occurrence;

        try
        {
            File.WriteAllLines(TrackFile, new[] { Globals.Schedule_Saved, Globals.Schedule_Ocscurrence.ToString("o") });
        }
        catch (Exception ex)
        {
            Helper.Debug($"Cant Save last_restart.dat : {ex.Message}", true);
        }
    }

    private static DateTime GetOccurrence(string schedule)
    {
        if (string.IsNullOrWhiteSpace(schedule)) return DateTime.MinValue;

        var parts = Regex.Split(schedule.Trim(), @"\s+to\s+", RegexOptions.IgnoreCase);
        var now = DateTime.UtcNow;

        if (!DateTime.TryParse(parts[0].Trim(), CultureInfo.InvariantCulture, DateTimeStyles.None, out var start)) return DateTime.MinValue;
        var startT = start.TimeOfDay;

        if (parts.Length == 1)
        {
            var todays = now.Date + startT;
            return now.TimeOfDay >= startT ? todays : todays.AddDays(-1);
        }

        if (!DateTime.TryParse(parts[1].Trim(), CultureInfo.InvariantCulture, DateTimeStyles.None, out var end)) return DateTime.MinValue;
        var endT = end.TimeOfDay;

        if (startT <= endT) return now.TimeOfDay >= startT && now.TimeOfDay <= endT ? now.Date + startT : DateTime.MinValue;

        if (now.TimeOfDay >= startT) return now.Date + startT;
        if (now.TimeOfDay <= endT) return now.Date.AddDays(-1) + startT;

        return DateTime.MinValue;
    }

    public static void MarkIfInWindow()
    {
        var schedule = Configs.Instance.Schedule.Trim();
        if (string.IsNullOrWhiteSpace(schedule)) return;

        var occurrence = GetOccurrence(schedule);
        if (occurrence == DateTime.MinValue) return;
        if (Globals.Schedule_Ocscurrence == occurrence) return;

        SaveMark(occurrence);
        Helper.Debug("Restart happened inside the Schedule window, marked so Schedule will not fire again");
    }
}