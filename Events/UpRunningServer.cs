using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Timers;

namespace Auto_Restart_Server_GoldKingZ;

public static class UpRunningServer
{
    public static void Check(int GetPlayers)
    {
        if (string.IsNullOrWhiteSpace(Configs.Instance.UpRunningServer) || !IsUp(GetPlayers))
        {
            Helper.ResetTimer(Globals.EventTimer.UpRunningServer);
            return;
        }

        if (Globals.Event_Calling) return;

        Helper.CheckTimerInGlobals(Globals.EventTimer.UpRunningServer,
            MainPlugin.Instance.AddTimer(1.0f, CallBack, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE),
            Configs.Instance.UpRunningServer_RestartMethod_Wait.ToSeconds(),
            Configs.Instance.UpRunningServer_RestartMethod,
            Configs.Instance.UpRunningServer_WhenXPlayersInServerORLess,
            Configs.Instance.UpRunningServer_Commands);
    }

    private static void CallBack()
    {
        if (!Globals.TimerCheck_Data.TryGetValue(Globals.EventTimer.UpRunningServer, out var data) || data.Event_Delay == null) return;

        var players = Helper.GetPlayersController().Count;

        if (players > data.Limit)
        {
            Helper.ResetTimer(Globals.EventTimer.UpRunningServer, $"player joined during the wait, {players} above limit {data.Limit}");
            return;
        }

        if (!Helper.Countdown(Globals.EventTimer.UpRunningServer, data)) return;

        Helper.ExecuteMethod(Globals.EventTimer.UpRunningServer, data.Method);
    }

    private static bool IsUp(int GetPlayers)
    {
        var upSecs = Configs.Instance.UpRunningServer.ToSeconds();
        return upSecs > 0f && Server.EngineTime >= upSecs && GetPlayers <= Configs.Instance.UpRunningServer_WhenXPlayersInServerORLess;
    }
}