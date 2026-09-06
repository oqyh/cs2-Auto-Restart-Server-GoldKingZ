using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Timers;

namespace Auto_Restart_Server_GoldKingZ;

public static class OnDisconnect
{
    public static void Check(bool FromPlayerDisconnect = false)
    {
        if(!FromPlayerDisconnect)
        {
            Globals.Restarting = false;
        }

        var players = Helper.GetPlayersController().Count;
        var limit = Configs.Instance.OnDisconnect_WhenXPlayersInServerORLess;

        if (limit < 0 || players > limit)
        {
            Helper.ResetTimer(Globals.EventTimer.OnDisconnect, $"players {players} above limit {limit}");
            return;
        }

        if (Globals.Restarting || !FromPlayerDisconnect || Globals.Event_Calling) return;

        Helper.CheckTimerInGlobals(Globals.EventTimer.OnDisconnect,
            MainPlugin.Instance.AddTimer(1.0f, CallBack, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE),
            Configs.Instance.OnDisconnect_RestartMethod_Wait.ToSeconds(),
            Configs.Instance.OnDisconnect_RestartMethod,
            limit,
            Configs.Instance.OnDisconnect_Commands,
            Configs.Instance.OnDisconnect_RestartMethod_Mode4);
    }

    private static void CallBack()
    {
        if (!Globals.TimerCheck_Data.TryGetValue(Globals.EventTimer.OnDisconnect, out var data) || data.Event_Delay == null) return;

        var players = Helper.GetPlayersController().Count;
        var limit = Configs.Instance.OnDisconnect_WhenXPlayersInServerORLess;

        if (players > limit)
        {
            Helper.ResetTimer(Globals.EventTimer.OnDisconnect, $"player joined during the wait, {players} above limit {limit}");
            return;
        }

        if (!Helper.Countdown(Globals.EventTimer.OnDisconnect, data)) return;

        Helper.ExecuteMethod(Globals.EventTimer.OnDisconnect, data.Method, data.Map);
    }
}