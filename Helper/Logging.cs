using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API;

namespace Auto_Restart_Server_GoldKingZ;

public partial class Helper
{
    private static Dictionary<string, string> GetPlaceholders(Globals.EventTimer Event, int Method)
    {
        var cfg = Configs.Instance;
        var utc = DateTime.UtcNow;
        var local = DateTime.Now;

        return new Dictionary<string, string>
        {
            ["{DATE_UTC}"] = utc.ToString(cfg.DateFormat),
            ["{DATE_GMT}"] = utc.ToString(cfg.DateFormat),
            ["{TIME_UTC}"] = utc.ToString(cfg.TimeFormat),
            ["{TIME_GMT}"] = utc.ToString(cfg.TimeFormat),
            ["{DATE}"] = local.ToString(cfg.DateFormat),
            ["{TIME}"] = local.ToString(cfg.TimeFormat),
            ["{METHOD}"] = Method switch
            {
                1 => "Quit",
                2 => "Crash",
                3 => "Relaunch",
                4 => "Normal Restart",
                _ => "Unknown"
            },
            ["{EVENT}"] = Event switch
            {
                Globals.EventTimer.OnDisconnect => "Last Player Disconnect",
                Globals.EventTimer.UpRunningServer => "Up Running Server",
                Globals.EventTimer.Schedule => "Schedule",
                _ => Event.ToString()
            },
            ["{MAP}"] = Server.MapName ?? "",
            ["{HOSTNAME}"] = ConVar.Find("hostname")?.StringValue ?? "",
            ["{SERVER_IP}"] = Server_Utils.GetServerIp(),
            ["{IP}"] = Server_Utils.GetIp(),
            ["{PORT}"] = Server_Utils.GetPort().ToString(),
            ["{PLAYERS}"] = GetPlayersController().Count.ToString()
        };
    }

    private static string Fill(string text, Dictionary<string, string> reps)
    {
        if (string.IsNullOrWhiteSpace(text)) return "";

        foreach (var r in reps)
        {
            text = text.Replace(r.Key, r.Value);
        }

        return text;
    }

    private static void WriteTextLog(Dictionary<string, string> reps)
    {
        var cfg = Configs.Instance;
        if (!cfg.TextLog_Enable) return;

        try
        {
            var now = DateTime.UtcNow;
            var msg = Fill(cfg.TextLog_MessageFormat, reps);

            var dir = Path.Combine(MainPlugin.Instance.ModuleDirectory, "logs");
            Directory.CreateDirectory(dir);
            File.AppendAllText(Path.Combine(dir, $"{now:yyyy-MM-dd}.txt"), msg + Environment.NewLine);

            var days = cfg.TextLog_AutoDeleteLogsMoreThanXdaysOld;
            if (days <= 0) return;

            foreach (var f in Directory.GetFiles(dir, "*.txt"))
            {
                if (File.GetLastWriteTimeUtc(f) < now.AddDays(-days))
                {
                    File.Delete(f);
                }
            }
        }
        catch (Exception ex)
        {
            Debug($"Cant Write Log : {ex.Message}", true);
        }
    }

    private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(5) };

    private static void SendDiscord(Dictionary<string, string> reps)
    {
        var Discord_Style = Configs.Instance.Discord_Style;

        if (string.IsNullOrWhiteSpace(Configs.Instance.Discord_WebHook)) return;

        try
        {
            var embed = new Dictionary<string, object>();
            var hasEmbed = false;

            if (int.TryParse(Discord_Style.SideColor.TrimStart('#'), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var color))
            {
                embed["color"] = color;
            }

            if (!string.IsNullOrWhiteSpace(Discord_Style.Title)) { embed["title"] = Fill(Discord_Style.Title, reps); hasEmbed = true; }
            if (!string.IsNullOrWhiteSpace(Discord_Style.Description)) { embed["description"] = Fill(Discord_Style.Description, reps); hasEmbed = true; }
            if (!string.IsNullOrWhiteSpace(Discord_Style.Image)) { embed["image"] = new { url = Fill(Discord_Style.Image, reps) }; hasEmbed = true; }
            if (!string.IsNullOrWhiteSpace(Discord_Style.Thumbnail)) { embed["thumbnail"] = new { url = Fill(Discord_Style.Thumbnail, reps) }; hasEmbed = true; }

            if (Discord_Style.Fields != null && Discord_Style.Fields.Count > 0)
            {
                const string Blank = "\u200b";
                var fields = new List<object>();

                foreach (var f in Discord_Style.Fields)
                {
                    if (fields.Count >= 25) break;

                    var name = Fill(f.Key, reps).Trim();
                    var value = Fill(f.Value, reps).Trim();

                    if (name.StartsWith("{EMPTY}", StringComparison.OrdinalIgnoreCase) &&
                        value.StartsWith("{EMPTY}", StringComparison.OrdinalIgnoreCase))
                    {
                        fields.Add(new { name = Blank, value = Blank, inline = Discord_Style.FieldsInline });
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(value)) continue;

                    fields.Add(new { name, value, inline = Discord_Style.FieldsInline });
                }

                if (fields.Count > 0) { embed["fields"] = fields.ToArray(); hasEmbed = true; }
            }

            if (!string.IsNullOrWhiteSpace(Discord_Style.FooterText) || !string.IsNullOrWhiteSpace(Discord_Style.FooterIcon))
            {
                embed["footer"] = new { text = Fill(Discord_Style.FooterText, reps), icon_url = Fill(Discord_Style.FooterIcon, reps) };
                hasEmbed = true;
            }

            var payload = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(Discord_Style.Content)) payload["content"] = Fill(Discord_Style.Content, reps);
            if (hasEmbed) payload["embeds"] = new[] { embed };
            if (payload.Count == 0) return;

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var res = _http.PostAsync(Configs.Instance.Discord_WebHook, content).GetAwaiter().GetResult();

            if (!res.IsSuccessStatusCode)
            {
                Debug($"Discord Returned {(int)res.StatusCode} : {res.ReasonPhrase}", true);
            }
        }
        catch (Exception ex)
        {
            Debug($"Discord Failed : {ex.Message}", true);
        }
    }
}
