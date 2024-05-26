using System.Drawing;
using Newtonsoft.Json;
using Auto_Restart_Server_GoldKingZ.Config;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Localization;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Core.Attributes;

namespace Auto_Restart_Server_GoldKingZ;

[MinimumApiVersion(234)]
public class AutoRestartServerGoldKingZ : BasePlugin
{
    public override string ModuleName => "Auto Restart Server (Auto Restart Server On Last Player Disconnect)";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "Gold KingZ";
    public override string ModuleDescription => "https://github.com/oqyh";

    public override void Load(bool hotReload)
    {
        Configs.Load(ModuleDirectory);
        Configs.Shared.CookiesModule = ModuleDirectory;
        RegisterListener<Listeners.OnClientConnected>(OnClientConnected);
        RegisterListener<Listeners.OnClientDisconnectPost>(OnClientDisconnectPost);
        RegisterListener<Listeners.OnMapStart>(OnMapStart);
        RegisterListener<Listeners.OnMapEnd>(OnMapEnd);
    }
    private void OnMapStart(string Map)
    {
        if(Configs.GetConfigData().TextLog_AutoDeleteLogsMoreThanXdaysOld > 0)
        {
            string Fpath = Path.Combine(ModuleDirectory,"../../plugins/Auto-Restart-Server-GoldKingZ/logs");
            Helper.DeleteOldFiles(Fpath, "*" + ".txt", TimeSpan.FromDays(Configs.GetConfigData().TextLog_AutoDeleteLogsMoreThanXdaysOld));
        }

        if(Configs.GetConfigData().RestartMode == 1 || Configs.GetConfigData().RestartMode == 2 || Configs.GetConfigData().RestartMode == 3)
        {            
            if (ConVar.Find("sv_hibernate_when_empty")!.GetPrimitiveValue<bool>())
            {
                Console.WriteLine("|||||||||||||||||||||||||||||| E R R O R ||||||||||||||||||||||||||||||");
                Console.WriteLine("[Auto-Restart-Server-GoldKingZ] Found (sv_hibernate_when_empty = true)");
                Console.WriteLine("[Auto-Restart-Server-GoldKingZ] Plugin Will Not Work Properly");
                Console.WriteLine("[Auto-Restart-Server-GoldKingZ] Please Make (((sv_hibernate_when_empty = false)))");
                Console.WriteLine("|||||||||||||||||||||||||||||| E R R O R ||||||||||||||||||||||||||||||");
            }
        }
        Globals._restartTimer?.Kill();
        Globals._restartTimer = null;
        Globals._restartTimer2?.Kill();
        Globals._restartTimer2 = null;
    }
    private void OnClientConnected(int playerSlot)
    {
        var playersCount = Helper.GetAllCount();

        if(Configs.GetConfigData().RestartMode == 1 || Configs.GetConfigData().RestartMode == 2 || Configs.GetConfigData().RestartMode == 3)
        {
            if(playersCount <= Configs.GetConfigData().RestartWhenXPlayersInServerORLess)
            {
                Globals.onetime2 = false;
                Globals._restartTimer?.Kill();
                Globals._restartTimer = null;
                Globals._restartTimer2?.Kill();
                Globals._restartTimer2 = null;
                Globals._restartTimer = AddTimer(0.1f, RestartTimer_Callback, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
            }else if(playersCount > Configs.GetConfigData().RestartWhenXPlayersInServerORLess)
            {
                Globals.onetime2 = false;
                Globals._restartTimer?.Kill();
                Globals._restartTimer = null;
                Globals._restartTimer2?.Kill();
                Globals._restartTimer2 = null;
            }
        }

    }
    private void OnClientDisconnectPost(int playerSlot)
    {
        var playersCount = Helper.GetAllCount();

        if(Configs.GetConfigData().RestartMode == 1 || Configs.GetConfigData().RestartMode == 2 || Configs.GetConfigData().RestartMode == 3)
        {
            if(playersCount <= Configs.GetConfigData().RestartWhenXPlayersInServerORLess)
            {
                Globals.onetime2 = false;
                Globals._restartTimer?.Kill();
                Globals._restartTimer = null;
                Globals._restartTimer2?.Kill();
                Globals._restartTimer2 = null;
                Globals._restartTimer = AddTimer(0.1f, RestartTimer_Callback, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
            }else if(playersCount > Configs.GetConfigData().RestartWhenXPlayersInServerORLess)
            {
                Globals.onetime2 = false;
                Globals._restartTimer?.Kill();
                Globals._restartTimer = null;
                Globals._restartTimer2?.Kill();
                Globals._restartTimer2 = null;
            }
        }

    }

    private void RestartTimer_Callback()
    {
        var playersCount = Helper.GetAllCount();

        if(playersCount <= Configs.GetConfigData().RestartWhenXPlayersInServerORLess && Globals.onetime == false)
        {
            Globals._restartTimer2 = AddTimer(Configs.GetConfigData().RestartXTimerInMins * 60, RestartTimer_Callback2, TimerFlags.STOP_ON_MAPCHANGE);
            Globals.onetime = true;
        }else if(playersCount > Configs.GetConfigData().RestartWhenXPlayersInServerORLess)
        {
            Globals.onetime = false;
            Globals._restartTimer?.Kill();
            Globals._restartTimer = null;
            Globals._restartTimer2?.Kill();
            Globals._restartTimer2 = null;
        }
    }
    private void RestartTimer_Callback2()
    {
        var playersCount = Helper.GetAllCount();
        if(playersCount <= Configs.GetConfigData().RestartWhenXPlayersInServerORLess)
        {
            if(Configs.GetConfigData().TextLog_Enable && Configs.GetConfigData().RestartMode != 0)
            {
                string modename = "";
                if(Configs.GetConfigData().RestartMode == 1)
                {
                    modename = "Restart";
                }else if(Configs.GetConfigData().RestartMode == 2)
                {
                    modename = "Quit";
                }else if(Configs.GetConfigData().RestartMode == 3)
                {
                    modename = "Crash";
                }
                string Time = DateTime.Now.ToString(Configs.GetConfigData().TextLog_TimeFormat);
                string Date = DateTime.Now.ToString(Configs.GetConfigData().TextLog_DateFormat);
                var replacerlog = !string.IsNullOrEmpty(Configs.GetConfigData().TextLog_MessageFormat)
                ? Helper.ReplaceMessages(
                    Configs.GetConfigData().TextLog_MessageFormat,  
                    Time,  
                    Date,  
                    modename
                ): string.Empty;

                if(!string.IsNullOrEmpty(Configs.GetConfigData().TextLog_MessageFormat))
                {
                    string Fpath = Path.Combine(ModuleDirectory,"../../plugins/Auto-Restart-Server-GoldKingZ/logs/");
                    string fileName = DateTime.Now.ToString(Configs.GetConfigData().TextLog_DateFormat) + ".txt";
                    string Tpath = Path.Combine(ModuleDirectory,"../../plugins/Auto-Restart-Server-GoldKingZ/logs/") + $"{fileName}";
                    if(!Directory.Exists(Fpath))
                    {
                        Directory.CreateDirectory(Fpath);
                    }

                    if(!File.Exists(Tpath))
                    {
                        using (File.Create(Tpath)) { }
                    }
                    try
                    {
                        File.AppendAllLines(Tpath, new[]{replacerlog});
                    }catch
                    {

                    }
                }
            }

            if(Configs.GetConfigData().DiscordLog_EnableMode != 0 && Configs.GetConfigData().RestartMode != 0)
            {
                string modename = "";
                if(Configs.GetConfigData().RestartMode == 1)
                {
                    modename = "Restart";
                }else if(Configs.GetConfigData().RestartMode == 2)
                {
                    modename = "Quit";
                }else if(Configs.GetConfigData().RestartMode == 3)
                {
                    modename = "Crash";
                }
                string Time = DateTime.Now.ToString(Configs.GetConfigData().DiscordLog_TimeFormat);
                string Date = DateTime.Now.ToString(Configs.GetConfigData().DiscordLog_DateFormat);
                int hostPort = ConVar.Find("hostport")!.GetPrimitiveValue<int>();
                var replacerDiscord = !string.IsNullOrEmpty(Configs.GetConfigData().DiscordLog_MessageFormat)
                ? Helper.ReplaceMessages(
                    Configs.GetConfigData().DiscordLog_MessageFormat,  
                    Time,  
                    Date,  
                    modename
                ): string.Empty;
                
                if(!string.IsNullOrEmpty(replacerDiscord))
                {
                    if(Configs.GetConfigData().DiscordLog_EnableMode == 1)
                    {
                        Task.Run(() =>
                        {
                            _ = Helper.SendToDiscordWebhookNormal(Configs.GetConfigData().DiscordLog_WebHookURL, replacerDiscord);
                        });
                    }else if(Configs.GetConfigData().DiscordLog_EnableMode == 2)
                    {
                        Task.Run(() =>
                        {
                            _ = Helper.SendToDiscordWebhookNameLinkWithPicture2(Configs.GetConfigData().DiscordLog_WebHookURL, replacerDiscord);
                        });
                    }else if(Configs.GetConfigData().DiscordLog_EnableMode == 3)
                    {
                        Task.Run(() =>
                        {
                            string serverIp = Helper.GetServerPublicIPAsync().Result;
                            _ = Helper.SendToDiscordWebhookNameLinkWithPicture3(Configs.GetConfigData().DiscordLog_WebHookURL, replacerDiscord, $"{serverIp}:{hostPort}");
                        });
                    }
                }
                
            }
            
            if(Configs.GetConfigData().RestartMode == 1)
            {
                Server.ExecuteCommand("sv_cheats 1; restart");
                Globals.Defaultmap?.Kill();
                Globals.Defaultmap = null;
                Globals.Defaultmap = AddTimer(10.1f, DefaultmapTimer_Callback);
                Globals._restartTimer?.Kill();
                Globals._restartTimer = null;
                Globals._restartTimer2?.Kill();
                Globals._restartTimer2 = null;
            }else if(Configs.GetConfigData().RestartMode == 2)
            {
                Server.NextFrame(() =>
                {
                    AddTimer(2.00f, () =>
                    {
                        Server.ExecuteCommand("sv_cheats 1; quit");
                    }, TimerFlags.STOP_ON_MAPCHANGE);
                });
                
            }else if(Configs.GetConfigData().RestartMode == 3)
            {
                Server.NextFrame(() =>
                {
                    AddTimer(2.00f, () =>
                    {
                        Server.ExecuteCommand("sv_cheats 1; crash");
                    }, TimerFlags.STOP_ON_MAPCHANGE);
                });
            }  

        }else if(playersCount > Configs.GetConfigData().RestartWhenXPlayersInServerORLess)
        {
            Globals.onetime2 = false;
            Globals._restartTimer?.Kill();
            Globals._restartTimer = null;
            Globals._restartTimer2?.Kill();
            Globals._restartTimer2 = null;
        }
    }
    private void DefaultmapTimer_Callback()
    {
        string DEFAULTMAP = Configs.GetConfigData().Mode1_RestartServerDefaultMap;

        if (DEFAULTMAP.StartsWith("ds:") )
        {
            string dsworkshop = DEFAULTMAP.TrimStart().Substring("ds:".Length).Trim();
            Server.ExecuteCommand($"ds_workshop_changelevel {dsworkshop}");
        }else if (DEFAULTMAP.StartsWith("host:"))
        {
            string hostworkshop = DEFAULTMAP.TrimStart().Substring("host:".Length).Trim();
            Server.ExecuteCommand($"host_workshop_map {hostworkshop}");
        }else if (!(DEFAULTMAP.StartsWith("ds:") || DEFAULTMAP.StartsWith("host:")))
        {
            Server.ExecuteCommand($"changelevel {DEFAULTMAP}");
        }

        Globals.Defaultmap?.Kill();
        Globals.Defaultmap = null;
    }
    private void OnMapEnd()
    {
        Helper.ClearVariables();
    }
    public override void Unload(bool hotReload)
    {
        Helper.ClearVariables();
    }
}