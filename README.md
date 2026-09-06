---

<h2 align="center">.:[ Community | Support ]:.</h2>
<p align="center">
  <a href="https://discord.com/invite/U7AuQhu">
    <img src="https://img.shields.io/badge/Discord-Join-5865F2?style=for-the-badge&logo=discord&logoColor=white" />
  </a>
  <a href="https://ko-fi.com/goldkingz">
    <img src="https://img.shields.io/badge/Ko--fi-Support-FF5E5B?style=for-the-badge&logo=kofi&logoColor=white" />
  </a>
  <a href="https://paypal.me/oQYh">
    <img src="https://img.shields.io/badge/PayPal-Donate-00457C?style=for-the-badge&logo=paypal&logoColor=white" />
  </a>
</p>

---

# [CS2] Auto-Restart-Server-GoldKingZ (1.0.2)

### Auto Restart Your Server On 3 Triggers (Last Player Disconnect | Uptime | Schedule)


<img width="900" height="1122" alt="Auto-Restart-Server-GoldKingZ 1 0 2" src="https://github.com/user-attachments/assets/be868075-b639-4907-9857-eac4ff053881" />

<details>
<summary><b>🖼️ Log Previews</b> (Click to expand 🔽)</summary>
<br>

<p align="center">
  <a href="https://github.com/user-attachments/assets/448882ef-230e-4bff-8ad9-1058b5203ffd">
    <img src="https://github.com/user-attachments/assets/448882ef-230e-4bff-8ad9-1058b5203ffd" alt="Text log" width="900">
  </a>
</p>

<p align="center">
  <a href="https://github.com/user-attachments/assets/86bbf270-d5f0-4f54-bf21-2eadab96f804">
    <img src="https://github.com/user-attachments/assets/86bbf270-d5f0-4f54-bf21-2eadab96f804" alt="Discord log" width="400" valign="top">
  </a>
  <a href="https://github.com/user-attachments/assets/573b2f4c-bacd-43aa-be66-8e7783afa94e">
    <img src="https://github.com/user-attachments/assets/573b2f4c-bacd-43aa-be66-8e7783afa94e" alt="Discord log with image" width="400" valign="top">
  </a>
</p>

</details>

---

## 📦 Dependencies

[![Metamod:Source](https://img.shields.io/badge/Metamod:Source-REQUIRED_TO_DOWNLOAD-red?logo=sourceengine&labelColor=2d2d2d)](https://www.sourcemm.net)

[![CounterStrikeSharp](https://img.shields.io/badge/CounterStrikeSharp-REQUIRED_TO_DOWNLOAD-red?logo=github&labelColor=83358F)](https://github.com/roflmuffin/CounterStrikeSharp)

[![JSON](https://img.shields.io/badge/JSON-INCLUDED_IN_ZIP-brightgreen?logo=json&labelColor=000000)](https://www.newtonsoft.com/json)

---

## 📥 Installation

### Plugin Installation
1. Download the latest `Auto-Restart-Server-GoldKingZ.x.x.x.zip` release
2. Extract contents to your `csgo` directory
3. Configure settings in `Auto-Restart-Server-GoldKingZ/config/config.json`
4. Restart your server

---

## ⚙️ Configuration
 
> [!IMPORTANT]
> **Main Configuration**  
> `../Auto-Restart-Server-GoldKingZ/config/config.json`
 
## 🛠️ `config/config.json`
<details open>
<summary><b>Main Config</b> (Click to expand 🔽)</summary>
  
| Property | Description | Values | Required |
|----------|-------------|--------|----------|
| `Reload_Plugin_CommandsInGame` | Commands to reload the plugin (console/chat by `!` or `css_`) | `Console_Commands:` `Chat_Commands:`<br>Both empty = Disable | - |
| `Reload_Plugin_Flags` | Restrict reload command to SteamIDs, Flags, Groups | `SteamIDs:` `Flags:` `Groups:`<br>All empty = Allow everyone | `Reload_Plugin_CommandsInGame` |
| `Reload_Plugin_Hide` | Hide chat after executing reload command | `0`-No<br>`1`-Only after successful toggle<br>`2`-Hide all the time | `Reload_Plugin_Flags` |
 
</details>
<details>
<summary><b>Last Player Disconnect Config</b> (Click to expand 🔽)</summary>
 
| Property | Description | Values | Required |
|----------|-------------|--------|----------|
| `OnDisconnect_RestartMethod` | Method used when the last player disconnects | `0`-Disable this feature<br>`1`-Quit method<br>`2`-Crash method, if 1 is not working<br>`3`-Relaunch method, if 2 is not working (useful for Windows)<br>`4`-Normal restart method | - |
| `OnDisconnect_WhenXPlayersInServerORLess` | Only restart when the player count is x or less (bots excluded) | `0`-Only on an empty server<br>e.g. `2`-2 players or less | `OnDisconnect_RestartMethod` |
| `OnDisconnect_RestartMethod_Wait` | How long to wait before restarting<br>Recalculates on player join, cancels if the count goes over the limit above | `1y` `1year` `1 years`-1 year<br>`2mon` `2month` `2 months`-2 months<br>`5d` `5day` `5 days`-5 days<br>`2h` `2hour` `2 hours`-2 hours<br>`5m` `5min` `5 mins`-5 mins<br>`30` `30s` `30sec` `30 secs`-30 secs<br>Empty = Instant | `OnDisconnect_RestartMethod` |
| `OnDisconnect_Commands` | Commands executed after the wait, right before the restart | `sv_cheats 0; bot_kick; mp_warmup_end`<br>`exec commands.cfg`-From csgo/cfg/<br>Empty = Nothing | `OnDisconnect_RestartMethod` |
| `OnDisconnect_RestartMethod_Mode4` | Map loaded when the normal restart method is used | `de_dust2`-From /csgo/maps/<br>`ds: surf_boreas`-From ds_workshop_listmaps<br>`host: 3112654794`-Any workshop id | `OnDisconnect_RestartMethod` = `4` |
 
</details>
<details>
<summary><b>Up Running Server Config</b> (Click to expand 🔽)</summary>
 
| Property | Description | Values | Required |
|----------|-------------|--------|----------|
| `UpRunningServer` | Restart after the server has been up for x or more<br>Counter resets to 0 after each restart | `1y` `1year` `1 years`-1 year<br>`2mon` `2month` `2 months`-2 months<br>`5d` `5day` `5 days`-5 days<br>`2h` `2hour` `2 hours`-2 hours<br>`5m` `5min` `5 mins`-5 mins<br>`30` `30s` `30sec` `30 secs`-30 secs<br>Empty = Disable this feature | - |
| `UpRunningServer_RestartMethod` | Method used when the uptime is reached | `1`-Quit method<br>`2`-Crash method, if 1 is not working<br>`3`-Relaunch method, if 2 is not working (useful for Windows)<br>Method `4` is not available here, a normal restart does not reset the uptime | `UpRunningServer` |
| `UpRunningServer_WhenXPlayersInServerORLess` | Only restart when the player count is x or less (bots excluded) | `0`-Only on an empty server<br>e.g. `2`-2 players or less | `UpRunningServer` |
| `UpRunningServer_RestartMethod_Wait` | How long to wait before restarting<br>Recalculates on player join, cancels if the count goes over the limit above | `1y` `1year` `1 years`-1 year<br>`2mon` `2month` `2 months`-2 months<br>`5d` `5day` `5 days`-5 days<br>`2h` `2hour` `2 hours`-2 hours<br>`5m` `5min` `5 mins`-5 mins<br>`30` `30s` `30sec` `30 secs`-30 secs<br>Empty = Instant | `UpRunningServer` |
| `UpRunningServer_Commands` | Commands executed after the wait, right before the restart | `sv_cheats 0; bot_kick; mp_warmup_end`<br>`exec commands.cfg`-From csgo/cfg/<br>Empty = Nothing | `UpRunningServer` |
 
</details>
<details>
<summary><b>Schedule Config</b> (Click to expand 🔽)</summary>
 
| Property | Description | Values | Required |
|----------|-------------|--------|----------|
| `Schedule` | Restart from x UTC to y UTC, or from x UTC until it happens<br>With only x set it keeps trying until the restart happens<br>With x to y it skips the day once y UTC has passed | `04:00 AM To 06:00 AM`-Inside that window<br>`04:00 AM`-From then on<br>Empty = Disable this feature | - |
| `Schedule_RestartMethod` | Method used when the schedule triggers | `1`-Quit method<br>`2`-Crash method, if 1 is not working<br>`3`-Relaunch method, if 2 is not working (useful for Windows)<br>`4`-Normal restart method | `Schedule` |
| `Schedule_WhenXPlayersInServerORLess` | Only restart when the player count is x or less (bots excluded) | `0`-Only on an empty server<br>e.g. `2`-2 players or less | `Schedule` |
| `Schedule_RestartMethod_Wait` | How long to wait before restarting<br>Recalculates on player join, cancels if the count goes over the limit above | `1y` `1year` `1 years`-1 year<br>`2mon` `2month` `2 months`-2 months<br>`5d` `5day` `5 days`-5 days<br>`2h` `2hour` `2 hours`-2 hours<br>`5m` `5min` `5 mins`-5 mins<br>`30` `30s` `30sec` `30 secs`-30 secs<br>Empty = Instant | `Schedule` |
| `Schedule_Commands` | Commands executed after the wait, right before the restart | `sv_cheats 0; bot_kick; mp_warmup_end`<br>`exec commands.cfg`-From csgo/cfg/<br>Empty = Nothing | `Schedule` |
| `Schedule_RestartMethod_Mode4` | Map loaded when the normal restart method is used | `de_dust2`-From /csgo/maps/<br>`ds: surf_boreas`-From ds_workshop_listmaps<br>`host: 3112654794`-Any workshop id | `Schedule_RestartMethod` = `4` |
 
</details>
<details>
<summary><b>Text Log Config</b> (Click to expand 🔽)</summary>
  
| Property | Description | Values | Required |
|----------|-------------|--------|----------|
| `TextLog_Enable` | Write a log line on every restart, saved in `Auto-Restart-Server-GoldKingZ/logs/` | `true`/`false` | - |
| `TextLog_MessageFormat` | How the log line looks, see [Placeholders](#-placeholders) | e.g. `[{DATE_UTC} - {TIME_UTC}] Server Restarted By [{EVENT}] Using [{METHOD}] Method` | `TextLog_Enable` |
| `TextLog_AutoDeleteLogsMoreThanXdaysOld` | Auto delete logs older than x days | `0`-Never delete<br>e.g. `7`-Only keep the last 7 days | `TextLog_Enable` |
 
</details>
<details>
<summary><b>Discord Log Config</b> (Click to expand 🔽)</summary>
  
| Property | Description | Values | Required |
|----------|-------------|--------|----------|
| `Discord_WebHook` | Webhook the restart embed is sent to | `https://discord.com/api/webhooks/XXXXXXXX`<br>Empty = Disable this feature | - |
 
`Discord_Style` builds the message itself, every text field accepts [Placeholders](#placeholders)
 
| Property | Description | Values | Required |
|----------|-------------|--------|----------|
| `Content` | Plain text above the embed, use it to ping a role | `<@&123456789012345678>`<br>Empty = Send embed only | `Discord_WebHook` |
| `Title` | Embed title | Any text<br>Empty = No title | `Discord_WebHook` |
| `Description` | Embed main text | Any text<br>Empty = No description | `Discord_WebHook` |
| `SideColor` | Embed side color, pick one on [htmlcolorcodes.com](https://htmlcolorcodes.com/color-picker) | `#22D3EE` | `Discord_WebHook` |
| `Fields` | Fields inside the embed, key = field title, value = field text<br>Add or remove as many as you like | `"⚡  Trigger": "{EVENT}"`<br>`"{EMPTY}": "{EMPTY}"`-Blank slot to control how fields wrap<br>Empty = No fields | `Discord_WebHook` |
| `FieldsInline` | Show fields side by side instead of stacked | `true`-Side by side<br>`false`-Stacked | `Fields` |
| `FooterText` | Footer text | Any text<br>Empty = No footer | `Discord_WebHook` |
| `FooterIcon` | Footer icon url | Direct image url<br>Empty = No icon | `FooterText` |
| `Image` | Big image at the bottom of the embed | Direct image url<br>Empty = No image | `Discord_WebHook` |
| `Thumbnail` | Small image at the top right of the embed | Direct image url<br>Empty = No thumbnail | `Discord_WebHook` |
 
</details>
<details>
<summary><b>Utilities Config</b> (Click to expand 🔽)</summary>
  
| Property | Description | Values | Required |
|----------|-------------|--------|----------|
| `DateFormat` | Date format used in text log and Discord | `dd/MM/yyyy`-04/09/2026<br>`MM/dd/yyyy`-09/04/2026<br>`yyyy-MM-dd`-2026-09-04<br>`dd MMMM yyyy`-04 September 2026 | - |
| `TimeFormat` | Time format used in text log and Discord | `hh:mm:ss tt`-07:59:30 PM<br>`HH:mm:ss`-19:59:30<br>`hh:mm tt`-07:59 PM | - |
| `EnableDebug` | Enable debug in server console (helps debug issues) | `true`/`false` | - |
 
</details>

<a id="placeholders"></a>
<details>
<summary><b>Text Log & Discord Placeholders</b> (Click to expand 🔽)</summary>
 
| Placeholder | Description |
|-------------|-------------|
| `{DATE}` | Date (server local timezone) |
| `{DATE_UTC}` | Date (UTC) |
| `{DATE_GMT}` | Date (GMT) |
| `{TIME}` | Time (server local timezone) |
| `{TIME_UTC}` | Time (UTC) |
| `{TIME_GMT}` | Time (GMT) |
| `{METHOD}` | Which method was used |
| `{EVENT}` | Which event triggered it |
| `{MAP}` | Current map |
| `{HOSTNAME}` | Server name |
| `{SERVER_IP}` | Server IP:Port |
| `{IP}` | Server IP |
| `{PORT}` | Server Port |
| `{PLAYERS}` | Player count |
 
</details>

---

## 📜 Changelog

<details>
<summary><b>📋 View Version History</b> (Click to expand 🔽)</summary>

### [1.0.2]
- Upgrade Net.10
- CleanUp + Optimization
- Rework On config.json
- Rework On Discord Log Ability To Build Your Own Embed Design
- Rework On Text Log
- Rework On Schedule Now Uses UTC
- Added Reload_Plugin_CommandsInGame 
- Added Reload_Plugin_Flags 
- Added Reload_Plugin_Hide 
- Added OnDisconnect_RestartMethod
- Added OnDisconnect_WhenXPlayersInServerORLess
- Added OnDisconnect_RestartMethod_Wait
- Added OnDisconnect_Commands
- Added OnDisconnect_RestartMethod_Mode4
- Added UpRunningServer
- Added UpRunningServer_RestartMethod
- Added UpRunningServer_WhenXPlayersInServerORLess
- Added UpRunningServer_RestartMethod_Wait
- Added UpRunningServer_Commands
- Added Schedule
- Added Schedule_RestartMethod
- Added Schedule_WhenXPlayersInServerORLess
- Added Schedule_RestartMethod_Wait
- Added Schedule_Commands
- Added Schedule_RestartMethod_Mode4
- Added DateFormat In Utilities
- Added TimeFormat In Utilities
- Added Count Down In Lang

### [1.0.1]
- Added EnableSchedule
- Added ScheduleOnEvery

### [1.0.0]
- Initial plugin release

</details>

---
