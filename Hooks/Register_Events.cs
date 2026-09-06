using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.ValveConstants.Protobuf;

namespace Auto_Restart_Server_GoldKingZ;

public partial class MainPlugin
{
    public void Register_Events()
    {
        RegisterEventHandler<EventRoundStart>(OnEventRoundStart);
        RegisterEventHandler<EventPlayerDisconnect>(OnEventPlayerDisconnect);
    }

    public void Remove_Events()
    {
        DeregisterEventHandler<EventRoundStart>(OnEventRoundStart);
        DeregisterEventHandler<EventPlayerDisconnect>(OnEventPlayerDisconnect);
    }

    public HookResult OnEventRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;

        Helper.CheckTimer();

        return HookResult.Continue;
    }

    public HookResult OnEventPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;

        var player = @event.Userid;
        var PlayerOnLoop = @event.Reason == (int)NetworkDisconnectionReason.NETWORK_DISCONNECT_LOOPDEACTIVATE || @event.Reason == (int)NetworkDisconnectionReason.NETWORK_DISCONNECT_LOOPSHUTDOWN;

        if (!player.IsValid() || PlayerOnLoop) return HookResult.Continue;

        Server.NextFrame(() => OnDisconnect.Check(true));

        

        return HookResult.Continue;
    }
}
