namespace Auto_Restart_Server_GoldKingZ;

public class Reload_Plugin
{
    [Comment("Note: Console_Commands Can Be Execute Via Both Console And Chat By (! or css_)")]
    [Comment("Making Both Console_Commands And Chat_Commands Empty = Disable")]
    [String("Console_Commands", "Chat_Commands")]
    public string Reload_Plugin_CommandsInGame { get; set; } = "Console_Commands: css_reloadautorestart,css_reloadars,css_reloadar | Chat_Commands: ";

    [Comment("If [Reload_Plugin_CommandsInGame] Pass, Is There Any Specified Restricted Flags, Groups, SteamIDs")]
    [Comment("Example:")]
    [Comment("\"SteamIDs: 76561198206086993,STEAM_0:1:507335558 | Flags: @css/root,@css/admin | Groups: #css/root,#css/admin\"")]
    [Comment("\"SteamIDs:  | Flags:  | Groups: \" = To Allow Everyone")]
    [String("SteamIDs", "Flags", "Groups")]
    public string Reload_Plugin_Flags { get; set; } = "SteamIDs: 76561198206086993,STEAM_0:1:507335558 | Flags: @css/root,@css/admin | Groups: #css/root,#css/admin";

    [Comment("If [Reload_Plugin_Flags] Pass, Hide Chat After Execute Reload_Plugin_CommandsInGame?:")]
    [Comment("0 = No")]
    [Comment("1 = Yes, But Only After Toggle Successfully")]
    [Comment("2 = Yes, Hide All The Time")]
    [Range(0, 2)]
    public int Reload_Plugin_Hide { get; set; } = 0;
}

public class Discord_Style
{
    [Comment("Plain Text Above The Embed")]
    [Comment("Use <@&ROLE_ID> To Ping A Role, Example: <@&123456789012345678>")]
    [Comment("Empty = Send Embed Only")]
    public string Content { get; set; } = "";

    [Comment("Embed Title")]
    [Comment("Empty = No Title")]
    public string Title { get; set; } = "🔄 Server Restarting 🔄";

    [Comment("Embed Main Text")]
    [Comment("Empty = No Description")]
    public string Description { get; set; } = "";

    [Comment("Embed Side Color, Use https://htmlcolorcodes.com/color-picker To Pick One")]
    public string SideColor { get; set; } = "#22D3EE";

    [Comment("Fields Shown Inside The Embed, Key = Field Title, Value = Field Text")]
    [Comment("Add Or Remove As Many As You Like")]
    [Comment("Use \"{EMPTY}\": \"{EMPTY}\" To Leave A Blank Slot And Control How Fields Wrap")]
    [Comment("Empty = No Fields")]
    public Dictionary<string, string> Fields { get; set; } = new()
    {
        ["⚡  Trigger"] = "```{EVENT}```",
        ["⚙️  Method"]  = "```{METHOD}```",
        ["{EMPTY}"]     = "{EMPTY}",
        ["🗺️  Map"]     = "```{MAP}```",
        ["👥  Players"] = "```{PLAYERS}```"
    };

    [Comment("Show Fields Side By Side Instead Of Stacked?")]
    [Comment("true = Yes, Side By Side")]
    [Comment("false = No, Stacked")]
    public bool FieldsInline { get; set; } = true;

    [Comment("Footer Text")]
    [Comment("Empty = No Footer")]
    public string FooterText { get; set; } = "{HOSTNAME}  •  {SERVER_IP}  •  {DATE_UTC}  {TIME_UTC}";

    [Comment("Footer Icon Url")]
    [Comment("Empty = No Icon")]
    public string FooterIcon { get; set; } = "https://github.com/oqyh/cs2-Auto-Restart-Server-GoldKingZ/blob/main/Resources/footer.png?raw=true";

    [Comment("Big Image At The Bottom Of The Embed")]
    [Comment("Empty = No Image")]
    public string Image { get; set; } = "https://github.com/oqyh/cs2-Auto-Restart-Server-GoldKingZ/blob/main/Resources/banner.png?raw=true";

    [Comment("Small Image At The Top Right Of The Embed")]
    [Comment("Empty = No Thumbnail")]
    public string Thumbnail { get; set; } = "https://github.com/oqyh/cs2-Auto-Restart-Server-GoldKingZ/blob/main/Resources/icon.png?raw=true";
}

public partial class Configs
{
    [BreakLine("----------------------------[ ↓ Plugin Info ↓ ]----------------------------{nextline}")]
    [Info("Version")]
    [Info("Github")]
    public object __InfoSection { get; set; } = null!;

    [BreakLine("----------------------------[ ↓ Main Config ↓ ]----------------------------{nextline}")]

    [Comment("Reload Auto Restart Server Plugin")]
    public Reload_Plugin Reload_Plugin { get; set; } = new();

    [BreakLine("----------------------------[ ↓ Last Player Disconnect Config ↓ ]----------------------------{nextline}")]

    [Comment("Which Method Do We Use On Restart When The Last Player Disconnects")]
    [Comment("0 = Disable This Feature")]
    [Comment("1 = Quit Method")]
    [Comment("2 = Crash Method, If Method 1 Is Not Working")]
    [Comment("3 = Relaunch Method, If Method 2 Is Not Working [Useful For Windows]")]
    [Comment("4 = Normal Restart Method")]
    [Range(0, 4)]
    public int OnDisconnect_RestartMethod { get; set; } = 3;

    [Comment("Only Do [OnDisconnect_RestartMethod] When The Server Player Count Is (x) Or Less")]
    [Comment("Note: Bots Are Excluded From This Count")]
    [Range(0, 999)]
    public int OnDisconnect_WhenXPlayersInServerORLess { get; set; } = 0;

    [Comment("If [OnDisconnect_RestartMethod] Triggers, How Long Do We Wait Before Restarting")]
    [Comment("Note: Recalculates On Player Join, Cancels If Players Count Over [OnDisconnect_WhenXPlayersInServerORLess]")]
    [Comment("\"1y\" \"1year\" \"1 years\" = 1 Year")]
    [Comment("\"2mon\" \"2month\" \"2 months\" = 2 Months")]
    [Comment("\"5d\" \"5day\" \"5 days\" = 5 Days")]
    [Comment("\"2h\" \"2hour\" \"2 hours\" = 2 Hours")]
    [Comment("\"5m\" \"5min\" \"5 mins\" = 5 Mins")]
    [Comment("\"30\" = 30 Secs | \"30s\" \"30sec\" \"30 secs\" = 30 Secs")]
    [Comment("Empty = Instant")]
    public string OnDisconnect_RestartMethod_Wait { get; set; } = "5 mins";

    [Comment("Excute Commands After [OnDisconnect_RestartMethod_Wait], (Before Restart At Last Phase)")]
    [Comment("Example: \"sv_cheats 0; bot_kick; mp_warmup_end\"")]
    [Comment(" - Or You Can Also Do \"exec commands.cfg\" in csgo/cfg/ ")]
    public string OnDisconnect_Commands { get; set; } = "";
    
    [Comment("If [OnDisconnect_RestartMethod = 4], Which Map Do You Want As Default")]
    [Comment("Using ds: Means What map list in ds_workshop_listmaps (ex: ds: surf_boreas)")]
    [Comment("Using host: To Get Any Workshop Map example https://steamcommunity.com/sharedfiles/filedetails/?id=3112654794 (ex: host: 3112654794)")]
    [Comment("Using Without any ds: or host: means what inside /../csgo/maps/  (ex: de_dust2)")]
    public string OnDisconnect_RestartMethod_Mode4 { get; set; } = "de_dust2";
    

    [BreakLine("----------------------------[ ↓ Up Running Server Config ↓ ]----------------------------{nextline}")]

    [Comment("Triggers [UpRunningServer_RestartMethod] After The Server Has Been Running For X Or More")]
    [Comment("Note: Counter Resets To 0 After Each Restart")]
    [Comment("\"1y\" \"1year\" \"1 years\" = 1 Year")]
    [Comment("\"2mon\" \"2month\" \"2 months\" = 2 Months")]
    [Comment("\"5d\" \"5day\" \"5 days\" = 5 Days")]
    [Comment("\"2h\" \"2hour\" \"2 hours\" = 2 Hours")]
    [Comment("\"5m\" \"5min\" \"5 mins\" = 5 Mins")]
    [Comment("\"30\" = 30 Secs | \"30s\" \"30sec\" \"30 secs\" = 30 Secs")]
    [Comment("Empty = Disable This Feature")]
    public string UpRunningServer { get; set; } = "";

    [Comment("Which Method Do We Use When [UpRunningServer] Triggers")]
    [Comment("1 = Quit Method")]
    [Comment("2 = Crash Method, If Method 1 Is Not Working")]
    [Comment("3 = Relaunch Method, If Method 2 Is Not Working [Useful For Windows]")]
    [Comment("Note: Method 4 (Normal Restart) Is Not Available Here, Normal Restart Does Not Reset The Server Uptime")]
    [Range(1, 3)]
    public int UpRunningServer_RestartMethod { get; set; } = 3;

    [Comment("Only Do [UpRunningServer_RestartMethod] When The Server Player Count Is (x) Or Less")]
    [Comment("Note: Bots Are Excluded From This Count")]
    [Range(0, 999)]
    public int UpRunningServer_WhenXPlayersInServerORLess { get; set; } = 0;

    [Comment("If [UpRunningServer_RestartMethod] Triggers, How Long Do We Wait Before Restarting")]
    [Comment("Note: Recalculates On Player Join, Cancels If Players Count Over [UpRunningServer_WhenXPlayersInServerORLess]")]
    [Comment("\"1y\" \"1year\" \"1 years\" = 1 Year")]
    [Comment("\"2mon\" \"2month\" \"2 months\" = 2 Months")]
    [Comment("\"5d\" \"5day\" \"5 days\" = 5 Days")]
    [Comment("\"2h\" \"2hour\" \"2 hours\" = 2 Hours")]
    [Comment("\"5m\" \"5min\" \"5 mins\" = 5 Mins")]
    [Comment("\"30\" = 30 Secs | \"30s\" \"30sec\" \"30 secs\" = 30 Secs")]
    [Comment("Empty = Instant")]
    public string UpRunningServer_RestartMethod_Wait { get; set; } = "5 mins";

    [Comment("Excute Commands After [UpRunningServer_RestartMethod_Wait], (Before Restart At Last Phase)")]
    [Comment("Example: \"sv_cheats 0; bot_kick; mp_warmup_end\"")]
    [Comment(" - Or You Can Also Do \"exec commands.cfg\" in csgo/cfg/ ")]
    public string UpRunningServer_Commands { get; set; } = "";

    [BreakLine("----------------------------[ ↓ Schedule Config ↓ ]----------------------------{nextline}")]

    [Comment("Triggers [Schedule_RestartMethod] From X UTC To Y UTC, Or From X UTC Until It Happens")]
    [Comment("Note: If You Only Set X UTC, It Keeps Trying Until The Restart Happens")]
    [Comment("Note: If You Set X To Y, It Skips The Day Once Y UTC Has Passed")]
    [Comment("If You Dont Know Your Time In UTC, Use https://dateful.com/convert/utc To Convert It")]
    [Comment("Example: \"04:00 AM To 06:00 AM\" Or \"04:00 AM\"")]
    [Comment("Empty = Disable This Feature")]
    public string Schedule { get; set; } = "";

    [Comment("Which Method Do We Use When [Schedule] Triggers")]
    [Comment("1 = Quit Method")]
    [Comment("2 = Crash Method, If Method 1 Is Not Working")]
    [Comment("3 = Relaunch Method, If Method 2 Is Not Working [Useful For Windows]")]
    [Comment("4 = Normal Restart Method")]
    [Range(1, 4)]
    public int Schedule_RestartMethod { get; set; } = 3;

    [Comment("Only Do [Schedule_RestartMethod] When The Server Player Count Is (x) Or Less")]
    [Comment("Note: Bots Are Excluded From This Count")]
    [Range(0, 999)]
    public int Schedule_WhenXPlayersInServerORLess { get; set; } = 0;

    [Comment("If [Schedule_RestartMethod] Triggers, How Long Do We Wait Before Restarting")]
    [Comment("Note: Recalculates On Player Join, Cancels If Players Count Over [Schedule_WhenXPlayersInServerORLess]")]
    [Comment("\"1y\" \"1year\" \"1 years\" = 1 Year")]
    [Comment("\"2mon\" \"2month\" \"2 months\" = 2 Months")]
    [Comment("\"5d\" \"5day\" \"5 days\" = 5 Days")]
    [Comment("\"2h\" \"2hour\" \"2 hours\" = 2 Hours")]
    [Comment("\"5m\" \"5min\" \"5 mins\" = 5 Mins")]
    [Comment("\"30\" = 30 Secs | \"30s\" \"30sec\" \"30 secs\" = 30 Secs")]
    [Comment("Empty = Instant")]
    public string Schedule_RestartMethod_Wait { get; set; } = "5 mins";

    [Comment("Excute Commands After [Schedule_RestartMethod_Wait], (Before Restart At Last Phase)")]
    [Comment("Example: \"sv_cheats 0; bot_kick; mp_warmup_end\"")]
    [Comment(" - Or You Can Also Do \"exec commands.cfg\" in csgo/cfg/ ")]
    public string Schedule_Commands { get; set; } = "";

    [Comment("If [Schedule_RestartMethod = 4], Which Map Do You Want As Default")]
    [Comment("Using ds: Means What map list in ds_workshop_listmaps (ex: ds: surf_boreas)")]
    [Comment("Using host: To Get Any Workshop Map example https://steamcommunity.com/sharedfiles/filedetails/?id=3112654794 (ex: host: 3112654794)")]
    [Comment("Using Without any ds: or host: means what inside /../csgo/maps/  (ex: de_dust2)")]
    public string Schedule_RestartMethod_Mode4 { get; set; } = "de_dust2";
    
    [BreakLine("----------------------------[ ↓ Text Log Config ↓ ]----------------------------{nextline}")]

    [Comment("Enable Logging Text Located In Auto-Restart-Server-GoldKingZ/logs/ ?")]
    [Comment("true = Yes")]
    [Comment("false = No")]
    public bool TextLog_Enable { get; set; } = false;

    [Comment("If [TextLog_Enable true], How Do You Like The Log Line To Look")]
    [Comment("============[Placeholders]===============")]
    [Comment("{DATE} = Date (Server Local Timezone)")]
    [Comment("{DATE_UTC} = Date (UTC)")]
    [Comment("{DATE_GMT} = Date (GMT)")]
    [Comment("{TIME} = Time (Server Local Timezone)")]
    [Comment("{TIME_UTC} = Time (UTC)")]
    [Comment("{TIME_GMT} = Time (GMT)")]
    [Comment("{METHOD} = Which Method Was Used")]
    [Comment("{EVENT} = Which Event Triggered It")]
    [Comment("{MAP} = Current Map")]
    [Comment("{HOSTNAME} = Server Name")]
    [Comment("{SERVER_IP} = Server IP:Port")]
    [Comment("{IP} = Server IP")]
    [Comment("{PORT} = Server Port")]
    [Comment("{PLAYERS} = Player Count")]
    public string TextLog_MessageFormat { get; set; } = "[{DATE_UTC} - {TIME_UTC}] Server Restarted By [{EVENT}] Using [{METHOD}] Method On [{MAP}] With [{PLAYERS}] Players";

    [Comment("If [TextLog_Enable true], Auto Delete Logs Older Than X Days")]
    [Comment("0 = Never Delete")]
    [Range(0, 999)]
    public int TextLog_AutoDeleteLogsMoreThanXdaysOld { get; set; } = 7;

    [BreakLine("----------------------------[ ↓ Discord Log Config ↓ ]----------------------------{nextline}")]

    [Comment("Discord WebHook")]
    [Comment("Example: https://discord.com/api/webhooks/XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
    [Comment("Empty = Disable This Feature")]
    public string Discord_WebHook { get; set; } = "";

    [Comment("If [Discord_WebHook = Not Empty], Build Your Own Message Style")]
    [Comment("============[Placeholders]===============")]
    [Comment("{DATE} = Date (Server Local Timezone)")]
    [Comment("{DATE_UTC} = Date (UTC)")]
    [Comment("{DATE_GMT} = Date (GMT)")]
    [Comment("{TIME} = Time (Server Local Timezone)")]
    [Comment("{TIME_UTC} = Time (UTC)")]
    [Comment("{TIME_GMT} = Time (GMT)")]
    [Comment("{METHOD} = Which Method Was Used")]
    [Comment("{EVENT} = Which Event Triggered It")]
    [Comment("{MAP} = Current Map")]
    [Comment("{HOSTNAME} = Server Name")]
    [Comment("{SERVER_IP} = Server IP:Port")]
    [Comment("{IP} = Server IP")]
    [Comment("{PORT} = Server Port")]
    [Comment("{PLAYERS} = Player Count")]
    public Discord_Style Discord_Style { get; set; } = new();

    [BreakLine("----------------------------[ ↓ Utilities ↓ ]----------------------------{nextline}")]

    [Comment("Date Format Used In Text Log And Discord")]
    [Comment("\"dd/MM/yyyy\" = 04/09/2026")]
    [Comment("\"MM/dd/yyyy\" = 09/04/2026")]
    [Comment("\"yyyy-MM-dd\" = 2026-09-04")]
    [Comment("\"dd MMMM yyyy\" = 04 September 2026")]
    public string DateFormat { get; set; } = "MM/dd/yyyy";

    [Comment("Time Format Used In Text Log And Discord")]
    [Comment("\"hh:mm:ss tt\" = 07:59:30 PM")]
    [Comment("\"HH:mm:ss\" = 19:59:30")]
    [Comment("\"hh:mm tt\" = 07:59 PM")]
    public string TimeFormat { get; set; } = "hh:mm:ss tt";

    [Comment("Enable Debug Plugin In Server Console?")]
    [Comment("true = Yes")]
    [Comment("false = No")]
    public bool EnableDebug { get; set; } = false;
}
