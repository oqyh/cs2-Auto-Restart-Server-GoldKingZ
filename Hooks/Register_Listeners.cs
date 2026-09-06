using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System.Text.RegularExpressions;

namespace Auto_Restart_Server_GoldKingZ;

public partial class MainPlugin
{
    public void Register_Listeners()
    {
        RegisterListener<Listeners.OnMapStart>(OnMapStart);
        RegisterListener<Listeners.OnTick>(OnTick);
        RegisterListener<Listeners.OnClientConnected>(OnClientConnected);
        RegisterListener<Listeners.OnServerHibernationUpdate>(OnServerHibernationUpdate);
        RegisterListener<Listeners.OnMapEnd>(OnMapEnd);
    }

    public void Remove_Listeners()
    {
        RemoveListener<Listeners.OnMapStart>(OnMapStart);
        RemoveListener<Listeners.OnTick>(OnTick);
        RemoveListener<Listeners.OnClientConnected>(OnClientConnected);
        RemoveListener<Listeners.OnServerHibernationUpdate>(OnServerHibernationUpdate);
        RemoveListener<Listeners.OnMapEnd>(OnMapEnd);
    }

    public void OnMapStart(string mapname)
    {
        Helper.CheckTimer();
    }

    public void OnMapEnd()
    {
        try
        {
            Globals.Clear();
        }
        catch (Exception ex)
        {
            Helper.Debug($"OnMapEnd Error: {ex.Message}", true);
        }
    }

    public void OnClientConnected(int playerSlot)
    {
        var player = Utilities.GetPlayerFromSlot(playerSlot);
        if (player == null || !player.IsValid) return;

        Helper.CheckPlayerInGlobals(player);

        OnDisconnect.Check();
    }

    public void OnServerHibernationUpdate(bool isHibernating)
    {
        if (!isHibernating) return;

        Helper.Debug("============================================", true);
        Helper.Debug($"{Con.Yellow}WARNING: Found (sv_hibernate_when_empty true)", true);
        Helper.Debug("Sleeping Server Freezes Timers And Breaks This Plugin", true);
        Helper.Debug($"{Con.Green}Forcing (sv_hibernate_when_empty false) For Now", true);
        Helper.Debug($"Please Add {Con.Green}sv_hibernate_when_empty false {Con.Red}To Your server.cfg", true);
        Helper.Debug("============================================", true);

        Server.ExecuteCommand("sv_hibernate_when_empty false");
    }

    public void OnTick()
    {
        if (Globals.CenterHtml.Length == 0 && Globals.CenterBottom.Length == 0) return;

        var html = Globals.CenterHtml.Length > 0
            ? Regex.Replace(Globals.CenterHtml, "{nextline}", "<br>", RegexOptions.IgnoreCase)
            : "";

        var bottom = Globals.CenterBottom.Length > 0
            ? Regex.Replace(Globals.CenterBottom, "{nextline}", "\n", RegexOptions.IgnoreCase)
            : "";

        foreach (var playerData in Globals.Player_Data.Values)
        {
            if (playerData == null) continue;

            var player = playerData.Player;
            if (!player.IsValid()) continue;

            if (html.Length > 0) player.PrintToCenterHtml(html);
            if (bottom.Length > 0) player.PrintToCenter(bottom);
        }
    }
}
