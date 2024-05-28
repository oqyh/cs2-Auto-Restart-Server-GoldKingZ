using System.Text.Json;
using System.Text.Json.Serialization;

namespace Auto_Restart_Server_GoldKingZ.Config
{
    public static class Configs
    {
        public static class Shared {
            public static string? CookiesModule { get; set; }
        }
        
        private static readonly string ConfigDirectoryName = "config";
        private static readonly string ConfigFileName = "config.json";
        private static string? _configFilePath;
        private static ConfigData? _configData;

        private static readonly JsonSerializerOptions SerializationOptions = new()
        {
            Converters =
            {
                new JsonStringEnumConverter()
            },
            WriteIndented = true,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
        };

        public static bool IsLoaded()
        {
            return _configData is not null;
        }

        public static ConfigData GetConfigData()
        {
            if (_configData is null)
            {
                throw new Exception("Config not yet loaded.");
            }
            
            return _configData;
        }

        public static ConfigData Load(string modulePath)
        {
            var configFileDirectory = Path.Combine(modulePath, ConfigDirectoryName);
            if(!Directory.Exists(configFileDirectory))
            {
                Directory.CreateDirectory(configFileDirectory);
            }
            
            _configFilePath = Path.Combine(configFileDirectory, ConfigFileName);
            if (File.Exists(_configFilePath))
            {
                _configData = JsonSerializer.Deserialize<ConfigData>(File.ReadAllText(_configFilePath), SerializationOptions);
            }
            else
            {
                _configData = new ConfigData();
            }

            if (_configData is null)
            {
                throw new Exception("Failed to load configs.");
            }

            SaveConfigData(_configData);
            
            return _configData;
        }

        private static void SaveConfigData(ConfigData configData)
        {
            if (_configFilePath is null)
            {
                throw new Exception("Config not yet loaded.");
            }
            string json = JsonSerializer.Serialize(configData, SerializationOptions);


            File.WriteAllText(_configFilePath, json);
        }

        public class ConfigData
        {
            private int _RestartMode;
            public int RestartMode
            {
                get => _RestartMode;
                set
                {
                    _RestartMode = value;
                    if (_RestartMode < 0 || _RestartMode > 3)
                    {
                        RestartMode = 1;
                        Console.WriteLine("|||||||||||||||||||||||||||||||||||||||||||||||| I N V A L I D ||||||||||||||||||||||||||||||||||||||||||||||||");
                        Console.WriteLine("[Auto-Restart-Server-GoldKingZ] RestartMode: is invalid, setting to default value (1) Please Choose 0 or 1 or 2 or 3.");
                        Console.WriteLine("[Auto-Restart-Server-GoldKingZ] RestartMode (0) = Disable");
                        Console.WriteLine("[Auto-Restart-Server-GoldKingZ] RestartMode (1) = Restart Method");
                        Console.WriteLine("[Auto-Restart-Server-GoldKingZ] RestartMode (2) = Quit Method");
                        Console.WriteLine("[Auto-Restart-Server-GoldKingZ] RestartMode (3) = Crash Method");
                        Console.WriteLine("|||||||||||||||||||||||||||||||||||||||||||||||| I N V A L I D ||||||||||||||||||||||||||||||||||||||||||||||||");
                    }
                }
            }
            public float RestartXTimerInMins { get; set; }
            public int RestartWhenXPlayersInServerORLess { get; set; }
            public string Mode1_RestartServerDefaultMap { get; set; }
            public bool EnableSchedule { get; set; }
            public string ScheduleOnEvery { get; set; }
            public string empty { get; set; }
            public bool TextLog_Enable { get; set; }
            public string TextLog_MessageFormat { get; set; }
            public string TextLog_DateFormat { get; set; }
            public string TextLog_TimeFormat { get; set; }
            public int TextLog_AutoDeleteLogsMoreThanXdaysOld { get; set; }
            public string empty2 { get; set; }
            private int _DiscordLog_EnableMode;
            public int DiscordLog_EnableMode
            {
                get => _DiscordLog_EnableMode;
                set
                {
                    _DiscordLog_EnableMode = value;
                    if (_DiscordLog_EnableMode < 0 || _DiscordLog_EnableMode > 3)
                    {
                        DiscordLog_EnableMode = 0;
                        Console.WriteLine("|||||||||||||||||||||||||||||||||||||||||||||||| I N V A L I D ||||||||||||||||||||||||||||||||||||||||||||||||");
                        Console.WriteLine("[Auto-Restart-Server-GoldKingZ] DiscordLog_EnableMode: is invalid, setting to default value (0) Please Choose 0 or 1 or 2 or 3.");
                        Console.WriteLine("[Auto-Restart-Server-GoldKingZ] DiscordLog_EnableMode (0) = Disable");
                        Console.WriteLine("[Auto-Restart-Server-GoldKingZ] DiscordLog_EnableMode (1) = Text Only");
                        Console.WriteLine("[Auto-Restart-Server-GoldKingZ] DiscordLog_EnableMode (2) = Text With Saparate Date And Time From Message");
                        Console.WriteLine("[Auto-Restart-Server-GoldKingZ] DiscordLog_EnableMode (3) = Text With Saparate Date And Time From Message + Server Ip In Footer");
                        Console.WriteLine("|||||||||||||||||||||||||||||||||||||||||||||||| I N V A L I D ||||||||||||||||||||||||||||||||||||||||||||||||");
                    }
                }
            }
            public string DiscordLog_MessageFormat { get; set; }
            public string DiscordLog_DateFormat { get; set; }
            public string DiscordLog_TimeFormat { get; set; }
            private string? _DiscordLog_SideColor;
            public string DiscordLog_SideColor
            {
                get => _DiscordLog_SideColor!;
                set
                {
                    _DiscordLog_SideColor = value;
                    if (_DiscordLog_SideColor.StartsWith("#"))
                    {
                        DiscordLog_SideColor = _DiscordLog_SideColor.Substring(1);
                    }
                }
            }
            public string DiscordLog_WebHookURL { get; set; }
            public string DiscordLog_FooterImage { get; set; }
            public string empty3 { get; set; }
            public string Information_For_You_Dont_Delete_it { get; set; }
            
            public ConfigData()
            {
                RestartMode = 1;
                RestartXTimerInMins = 5.0f;
                RestartWhenXPlayersInServerORLess = 0;
                Mode1_RestartServerDefaultMap = "de_dust2";
                EnableSchedule = false;
                ScheduleOnEvery = "06:00";
                empty = "-----------------------------------------------------------------------------------";
                TextLog_Enable = false;
                TextLog_MessageFormat = "[{DATE} - {TIME}] Server Has Less Players Sending [{MODE} Method]";
                TextLog_DateFormat = "MM-dd-yyyy";
                TextLog_TimeFormat = "HH:mm:ss";
                TextLog_AutoDeleteLogsMoreThanXdaysOld = 0;
                empty2 = "-----------------------------------------------------------------------------------";
                DiscordLog_EnableMode = 0;
                DiscordLog_MessageFormat = "[{DATE} - {TIME}] Server Has Less Players Sending [{MODE} Method]";
                DiscordLog_DateFormat = "MM-dd-yyyy";
                DiscordLog_TimeFormat = "HH:mm:ss";
                DiscordLog_SideColor = "00FFFF";
                DiscordLog_WebHookURL = "https://discord.com/api/webhooks/XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
                DiscordLog_FooterImage = "https://github.com/oqyh/cs2-Auto-Restart-Server-GoldKingZ/blob/main/Resources/serverip.png?raw=true";
                empty3 = "-----------------------------------------------------------------------------------";
                Information_For_You_Dont_Delete_it = " Vist  [https://github.com/oqyh/cs2-Auto-Restart-Server-GoldKingZ/tree/main?tab=readme-ov-file#-configuration-] To Understand All Above";
            }
        }
    }
}