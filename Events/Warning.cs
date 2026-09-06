namespace Auto_Restart_Server_GoldKingZ;

public static class Warning
{
    private const string Chat   = "PrintToChatToAll.Restart.Warning";
    private const string Center = "PrintCenterToPlayer.Restart.Warning";
    private const string Bottom = "PrintCenterBottomToPlayer.Restart.Warning";

    private static bool _loaded;
    private static readonly Dictionary<string, Dictionary<int, string>> _points = new();
    private static readonly Dictionary<string, string> _base = new();
    private static readonly Dictionary<string, (int At, string Key)> _startAt = new();

    public static void Reload() => _loaded = false;

    private static void Load()
    {
        if (_loaded) return;
        _loaded = true;

        _points.Clear(); _base.Clear(); _startAt.Clear();

        foreach (var b in new[] { Chat, Center, Bottom })
        {
            _points[b] = new Dictionary<int, string>();
            _startAt[b] = (0, "");
        }

        foreach (var s in MainPlugin.Instance.Localizer.GetAllStrings(true))
        {
            if (string.IsNullOrWhiteSpace(s.Value)) continue;

            var b = new[] { Chat, Center, Bottom }.FirstOrDefault(x => s.Name.StartsWith(x, StringComparison.OrdinalIgnoreCase));
            if (b == null) continue;

            var tail = s.Name.Substring(b.Length).TrimStart('.');

            if (tail.Length == 0) { _base[b] = s.Name; continue; }

            if (tail.StartsWith("StartAt.", StringComparison.OrdinalIgnoreCase))
            {
                var at = (int)tail.Substring(8).ToSeconds();
                if (at > 0) _startAt[b] = (at, s.Name);
                continue;
            }

            var secs = (int)tail.ToSeconds();
            if (secs > 0 || tail == "0") _points[b][secs] = s.Name;
        }

        Helper.Debug($"[Warning] chat {_points[Chat].Count}, center {(_base.ContainsKey(Center) ? "base" : $"{_points[Center].Count} points")}, bottom {(_base.ContainsKey(Bottom) ? "base" : $"{_points[Bottom].Count} points")}");
    }

    public static string Text(string baseKey, Globals.EventTimer Event, Globals.TimerCheckClass data, int left)
    {
        Load();

        if (baseKey != Chat && _base.TryGetValue(baseKey, out var b))
            return Helper.Lang(b, Event, data, left);

        var (at, key) = _startAt[baseKey];
        if (baseKey != Chat && at > 0 && left <= at)
            return Helper.Lang(key, Event, data, left);

        return _points[baseKey].TryGetValue(left, out var k) ? Helper.Lang(k, Event, data, left) : "";
    }

    public static void SendChat(Globals.EventTimer Event, Globals.TimerCheckClass data, int left, bool final = false)
    {
        var text = final && _base.TryGetValue(Chat, out var b)
            ? Helper.Lang(b, Event, data, 0)
            : Text(Chat, Event, data, left);

        if (text.Length > 0) Helper.AdvancedServerPrintToChatAll(text);
    }

    public static void SendCenter(Globals.EventTimer Event, Globals.TimerCheckClass data, int left)
    {
        Globals.CenterHtml = Text(Center, Event, data, left);
        Globals.CenterBottom = Text(Bottom, Event, data, left);
    }
}