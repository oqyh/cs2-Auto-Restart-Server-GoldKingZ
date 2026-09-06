using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace Auto_Restart_Server_GoldKingZ;

public partial class MainPlugin : BasePlugin
{
    public override string ModuleName => "[Auto Restart Server] Auto Restart Your Server On 3 Triggers (Last Player Disconnect | Uptime | Schedule)";
    public override string ModuleVersion => "1.0.2";
    public override string ModuleAuthor => "Gold KingZ";
    public override string ModuleDescription => "https://github.com/oqyh";

    public static MainPlugin Instance { get; private set; } = null!;

    public override void Load(bool hotReload)
    {
        Instance = this;

        Configs.Load(hotReload);

        Helper.RemoveRegisterCommandsAndHooks();
        Globals.Clear();
        Helper.RegisterCommandsAndHooks();
        Helper.ReloadPlayersGlobals();
    }

    public override void Unload(bool hotReload)
    {
        try
        {
            Helper.RemoveRegisterCommandsAndHooks();
            Globals.Clear();
        }
        catch (Exception ex)
        {
            Helper.Debug($"Unload Error: {ex.Message}", true);
        }
    }

    /*
    [ConsoleCommand("css_test", "Show what is inside the plugin timer state")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    public void test(CCSPlayerController? player, CommandInfo commandInfo)
    {
        
    } 
    */
}
