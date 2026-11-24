using MemberDomain;
using System.Text.Json.Nodes;
using DataModel;
namespace IPISv2.HealthStatus
{
    public static class DisplayHelper
    {
        public static string NodeToString(JsonNode? node)
        {
            if (node == null)
                return string.Empty;

            try
            {
                return node.GetValue<string>() ?? string.Empty;
            }
            catch
            {
                return node.ToString();
            }
        }

        public static Task DelaySeconds(double seconds, CancellationToken token)
        {
            if (seconds <= 0)
                return Task.CompletedTask;

            return Task.Delay(TimeSpan.FromSeconds(seconds), token);
        }

        public static int GetInt(JsonObject obj, string name)
        {
            JsonNode? node = obj[name];

            if (node == null)
                return 0;

            return int.TryParse(NodeToString(node), out int value) ? value : 0;
        }

        public static string GetString(JsonObject obj, string name)
        {
            return NodeToString(obj[name]);
        }

        public static int GetPto(JsonArray? lineConf, int defaultValue)
        {
            if (lineConf == null || lineConf.Count == 0 || lineConf[0] is not JsonObject config)
            {
                return defaultValue;
            }

            JsonNode? ptoNode = config["PTO"];

            if (ptoNode == null)
                return defaultValue;

            return int.TryParse(NodeToString(ptoNode), out int value) ? value : defaultValue;
        }

        public static string GetTrainStatus(
            JsonObject train,
            string language,
            int flipIndex)
        {
            JsonNode? sta = train["STA"];

            if (sta == null)
                return string.Empty;

            if (sta is JsonObject statusObject)
            {
                JsonArray? values = statusObject[language] as JsonArray;

                if (values == null || values.Count == 0)
                    return string.Empty;

                if (flipIndex >= values.Count)
                    flipIndex = 0;

                return NodeToString(values[flipIndex]);
            }

            return NodeToString(sta);
        }
    }


    public static class TrainDisplayHelper
    {
        public static TrainDisplayData BuildTrainData(JsonObject train, string language, int flipIndex)
        {
            string status = DisplayHelper.GetTrainStatus(train, language, flipIndex);

            return new TrainDisplayData
            {
                TrainNo = DisplayHelper.GetString(train, "TNO"),

                Names = new Dictionary<string, string>
                {
                    ["E"] = DisplayHelper.GetString(train, "TNE"),
                    ["H"] = DisplayHelper.GetString(train, "TNH"),
                    ["R"] = DisplayHelper.GetString(train, "TNR")
                },

                Sat = DisplayHelper.GetString(train, "SAT"),
                Sdt = DisplayHelper.GetString(train, "SDT"),
                Eat = !string.IsNullOrEmpty(DisplayHelper.GetString(train, "ETA")) ? DisplayHelper.GetString(train, "ETA") : DisplayHelper.GetString(train, "SAT"),
                Edt = !string.IsNullOrEmpty(DisplayHelper.GetString(train, "EDT")) ? DisplayHelper.GetString(train, "EDT") : DisplayHelper.GetString(train, "SDT"),
                Late = DisplayHelper.GetString(train, "LATE"),
                Adf = DisplayHelper.GetString(train, "ADF"),

                Platform = DisplayHelper.GetString(train, "PNO"),

                Status = status,

                SpecialStatus = DisplayHelper.GetInt( train, "SPECIAL_STATUS") == 1,

                FlipRequired = DisplayHelper.GetInt( train, "FLIP_REQ") == 1
            };
        }
    }


    public static class DisplayPageHelper
    {
        public static List<List<JsonObject?>> CreatePages(JsonArray trains, int boardLines)
        {
            List<JsonObject> trainList = trains.OfType<JsonObject>().ToList();

            List<List<JsonObject?>> pages = new();

            for (int start = 0; start < trainList.Count; start += boardLines)
            {
                List<JsonObject?> page = trainList.Skip(start).Take(boardLines).Cast<JsonObject?>().ToList();

                while (page.Count < boardLines)
                {
                    page.Add(null);
                }

                pages.Add(page);
            }

            return pages;
        }
    }

    
        


}