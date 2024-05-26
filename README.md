# [CS2] Auto-Restart-Server-GoldKingZ (1.0.0)

### Auto Restart Server On Last Player Disconnect

![Mode3](https://github.com/oqyh/cs2-Auto-Restart-Server-GoldKingZ/assets/48490385/65e671ea-4dd6-4124-bad9-f845158ab97b)

![Mode1](https://github.com/oqyh/cs2-Auto-Restart-Server-GoldKingZ/assets/48490385/f55572e4-7b98-463b-8ffb-344a78ceb98a)


## .:[ Dependencies ]:.
[Metamod:Source (2.x)](https://www.sourcemm.net/downloads.php/?branch=master)

[CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp/releases)

[Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json)

## .:[ Configuration ]:.

> [!CAUTION]
> Config Located In ..\addons\counterstrikesharp\plugins\Auto-Restart-Server-GoldKingZ\config\config.json                                           
>

```json
{
  // (0) = Disable
  // (1) = Restart Method
  // (2) = Quit Method (if 1 is not working)
  // (3) = Crash Method (if 2 is not working)
  "RestartMode": 1,

  //Wait X Mins Before You Do RestartMode 
  "RestartXTimerInMins": 5,

  //Do RestartMode When X Players In Server OR Less
  "RestartWhenXPlayersInServerORLess": 0,

  //If RestartMode 1 Which Map You Like To Be As Default
  //Using ds: Means What map list in ds_workshop_listmaps (ex: ds:surf_boreas)
  //Using host: To Get Any Workshop Map example https://steamcommunity.com/sharedfiles/filedetails/?id=3112654794 (ex: host:3112654794)
  //Using Without any ds: or host: means what inside /../csgo/maps/  (ex: de_dust2)
  "Mode1_RestartServerDefaultMap": "de_dust2",

//-----------------------------------------------------------------------------------------

  //Enable Logging Text Located In Auto-Restart-Server-GoldKingZ/logs/ ?
  "TextLog_Enable": false,

  //Log Message Format
  //{TIME} == Time
  //{DATE} == Date
  //{MODE} == Which Method Did It Used
  "TextLog_MessageFormat": "[{DATE} - {TIME}] Server Has Less Players Sending [{MODE} Method]",

  //Date and Time Formate
  "TextLog_DateFormat": "MM-dd-yyyy",
  "TextLog_TimeFormat": "HH:mm:ss",

  //Auto Delete Logs If More Than X (Days) Old
  "TextLog_AutoDeleteLogsMoreThanXdaysOld": 0,

//-----------------------------------------------------------------------------------------

  //Send Log To Discord Via WebHookURL
  // (0) = Disable
  // (1) = Text Only (Result Image : https://github.com/oqyh/Auto-Restart-Server-GoldKingZ/blob/main/Resources/Mode1.png?raw=true)
  // (2) = Text With Saparate Date And Time From Message (Result Image : https://github.com/oqyh/Auto-Restart-Server-GoldKingZ/blob/main/Resources/Mode2.png?raw=true)
  // (3) = Text With Saparate Date And Time From Message + Server Ip In Footer (Result Image : https://github.com/oqyh/Auto-Restart-Server-GoldKingZ/blob/main/Resources/Mode3.png?raw=true)
  "DiscordLog_EnableMode": 0,

  //Discord Log Message Format
  //{TIME} == Time
  //{DATE} == Date
  //{MODE} == Which Method Did It Used
  "DiscordLog_MessageFormat": "[{DATE} - {TIME}] Server Has Less Players Sending [{MODE} Method]",

  //Date and Time Formate
  "DiscordLog_DateFormat": "MM-dd-yyyy",
  "DiscordLog_TimeFormat": "HH:mm:ss",

  //If DiscordLog_EnableMode (2) or (3) How Would You Side Color Message To Be Check (https://www.color-hex.com/) For Colors
  "DiscordLog_SideColor": "00FFFF",

  //Discord WebHookURL
  "DiscordLog_WebHookURL": "https://discord.com/api/webhooks/XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",

  //If DiscordLog_EnableMode (3) Image Url Footer
  "DiscordLog_FooterImage": "https://github.com/oqyh/cs2-Auto-Restart-Server-GoldKingZ/blob/main/Resources/serverip.png?raw=true",

}
```

## .:[ Change Log ]:.
```
(1.0.0)
-Initial Release
```

## .:[ Donation ]:.

If this project help you reduce time to develop, you can give me a cup of coffee :)

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://paypal.me/oQYh)
