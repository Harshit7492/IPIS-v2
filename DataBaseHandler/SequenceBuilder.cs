using DataBaseHandler;
using DataModel;
using Helper;

public class SequenceBuilder
{
    private readonly string _basePath = Path.Combine(AppContext.BaseDirectory, "SOUND", "TRAIN");

    public List<string> BuildSequence(train_updated_data train, string language)
    {
        var priorityOrder = new List<string> { "regional", "hindi", "english" };
        var sequence = new List<string>();

        if (train == null || string.IsNullOrEmpty(language))
            return sequence;

        var langs = language
                    .Split('+', StringSplitOptions.RemoveEmptyEntries)
                    .Select(l => l.Trim().ToLower())
                    .ToList();

        var orderedLangs = langs
                    .OrderBy(l => priorityOrder.IndexOf(l))
                    .ToList();

        foreach (var langRaw in orderedLangs)
        {
            string lang = langRaw.Trim().ToLower();
            string langUpper = lang.ToUpper();
            string langPath = Path.Combine(_basePath, langUpper);

            string column = lang switch
            {
                "english" => "ANNOUNCE_ENGLISH_PATTERN",
                "hindi" => "ANNOUNCE_HINDI_PATTERN",
                "regional" => "ANNOUNCE_REGIONAL_PATTERN",
                _ => ""
            };

            if (string.IsNullOrEmpty(column))
                continue;

            string pattern = _dbService.GET_SOUND_PATTERNS(train.TRAIN_TYPE, train.TRAIN_STATUS_ENGLISH, column);

            if (string.IsNullOrWhiteSpace(pattern))
                continue;


            if (string.IsNullOrEmpty(train.SCHEDULE_DEPARTURE_TIME))
            {
                pattern = pattern.Replace("KOPAHUCHNEWALI", "KOPAHUCHNEWALI_NEW");
                pattern = pattern.Replace("SEAANEWALI", "SECHALKAR");
            }

            bool Arrstatus = false;
            bool DepStatus = false;

            var tokens = pattern.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var raw in tokens)
            {
                string token = raw.Trim().ToUpper();

                switch (token)
                {
                    case "TRAINNUMBER":
                        {
                            string trainNo = train.TRAIN_NUMBER;

                            if (string.IsNullOrEmpty(trainNo)) break;

                            if (Config.old_Ann_Patrn)
                                Add(sequence, Path.Combine(langPath, "TRAINNUMBER", trainNo + ".wav"));

                            else
                            {
                                Add(sequence, Path.Combine(langPath, "TRAINNUMBER", "TraniAnnc.wav"));

                                foreach (char d in trainNo)
                                    Add(sequence, Path.Combine(langPath, "TRAINNUMBER", d + ".wav"));
                                Add(sequence, Path.Combine(langPath, "TRAINNUMBER", trainNo + ".wav"));
                            }
                            break;
                        }
                    case "MERGEDTRAINNUMBER":
                        {
                            string mergedTrainNo = train.MERGED_TRAIN;

   
                            if (string.IsNullOrWhiteSpace(mergedTrainNo))
                                break;

                            if (Config.old_Ann_Patrn)
                            {
                                Add( sequence, Path.Combine(langPath, "TRAINNUMBER",mergedTrainNo + ".wav" ));
                            }
                            else
                            {
                                Add(  sequence,Path.Combine(  langPath,  "TRAINNUMBER",  "TraniAnnc.wav" ) );

                                foreach (char d in mergedTrainNo)
                                {
                                    Add( sequence, Path.Combine( langPath, "TRAINNUMBER", d + ".wav" ));
                                }

                                Add( sequence, Path.Combine( langPath,"TRAINNUMBER",mergedTrainNo + ".wav" ) );
                            }

                            break;
                        }

                    case "ARRTRAINROUTE":
                        {
                            if (lang == "english")
                            {
                                bool viaExists =
                                    Exists(langPath, "STATION", train.VIA1_CODE) ||
                                    Exists(langPath, "STATION", train.VIA2_CODE);

                                if (viaExists)
                                    Add(sequence, Path.Combine(langPath, "SPEECH", "VIA.wav"));
                            }

                            if (!IsBlank(train.VIA1_CODE))
                                Arrstatus = Add(sequence, Path.Combine(langPath, "STATION", train.VIA1_CODE + ".wav"));

                            if (!IsBlank(train.VIA2_CODE))
                                Arrstatus = Add(sequence, Path.Combine(langPath, "STATION", train.VIA2_CODE + ".wav"));

                            if ((lang == "hindi" || lang == "regional") && Arrstatus)
                                Add(sequence, Path.Combine(langPath, "SPEECH", "KERASTE.wav"));

                            break;
                        }

                    case "DEPTRAINROUTE":
                        {
                            if (lang == "english")
                            {
                                bool viaExists =
                                    Exists(langPath, "STATION", train.VIA3_CODE) ||
                                    Exists(langPath, "STATION", train.VIA4_CODE);

                                if (viaExists)
                                    Add(sequence, Path.Combine(langPath, "SPEECH", "VIA.wav"));
                            }

                            if (!IsBlank(train.VIA3_CODE))
                                DepStatus = Add(sequence, Path.Combine(langPath, "STATION", train.VIA3_CODE + ".wav"));

                            if (!IsBlank(train.VIA4_CODE))
                                DepStatus = Add(sequence, Path.Combine(langPath, "STATION", train.VIA4_CODE + ".wav"));

                            if ((lang == "hindi" || lang == "regional") && DepStatus)
                                Add(sequence, Path.Combine(langPath, "SPEECH", "KERASTE.wav"));

                            break;
                        }

                    case "ORIGINSTATION":
                        if (!IsBlank(train.ORIGIN_CODE))
                            Add(sequence, Path.Combine(langPath, "STATION", train.ORIGIN_CODE + ".wav"));
                        break;

                    case "DESTSTATION":
                        if (!IsBlank(train.DESTINATION_CODE))
                            Add(sequence, Path.Combine(langPath, "STATION", train.DESTINATION_CODE + ".wav"));
                        break;

                    case "SCHEDULEARRIVALTIME":
                        AddTime(sequence, lang, langPath, train.ACTUAL_ARRIVAL_TIME);
                        break;

                    case "SCHEDULEDEPARTURETIME":
                        AddTime(sequence, lang, langPath, train.ACTUAL_DEPARTURE_TIME);
                        break;

                    case "ACTUALARRIVALTIME":
                        AddTime(sequence, lang, langPath, train.ACTUAL_ARRIVAL_TIME);
                        break;

                    case "ACTUALDEPARTURETIME":
                        AddTime(sequence, lang, langPath, train.ACTUAL_DEPARTURE_TIME);
                        break;

                    case "DELAYARRIVAL":
                    case "DELAYDEPARTURE":
                        AddDelay(sequence, lang, langPath, train.LATE);
                        break;

                    case "PLATEFORM":
                    case "PLATFORM":
                        Add(sequence, Path.Combine(langPath, "PLATFORM", train.ACTUAL_PLATFORM_NUMBER + ".wav"));
                        break;

                    case "STATION":
                        if (!IsBlank(train.DIV_HELD_TERM_STATION_CODE))
                            Add(sequence, Path.Combine(langPath, "STATION", train.DIV_HELD_TERM_STATION_CODE + ".wav"));
                        break;

                    default:
                        Add(sequence, Path.Combine(langPath, "SPEECH", token + ".wav"));
                        break;
                }
            }
        }

        return sequence;
    }

    public List<string> BuildSequenceCoach(train_updated_data train, string language)
    {
        var priorityOrder = new List<string> { "english" };
        var sequence = new List<string>();

        if (train == null || string.IsNullOrEmpty(language) || string.IsNullOrEmpty(train.TRAIN_NUMBER))
            return sequence;

        var langs = language
                    .Split('+', StringSplitOptions.RemoveEmptyEntries)
                    .Select(l => l.Trim().ToLower())
                    .ToList();

        var orderedLangs = langs
                    .OrderBy(l => priorityOrder.IndexOf(l))
                    .ToList();

        foreach (var langRaw in orderedLangs)
        {
            string lang = langRaw.Trim().ToLower();
            string langUpper = lang.ToUpper();

            string _basePath = Path.Combine(AppContext.BaseDirectory, "SOUND");
            Add(sequence, Path.Combine(_basePath, "COACH\\TRAIN\\ENGLISH\\SPEECH\\TADA.wav"));
            Add(sequence, Path.Combine(_basePath, "COACH\\TRAIN\\ENGLISH\\SPEECH\\YOURATTENTIONPLEASE.wav"));
            Add(sequence, Path.Combine(_basePath, "COACH\\THECOACHESOFTHETRAINNO.wav"));
            foreach (char _number in train.TRAIN_NUMBER)
            {
                int num = (int)char.GetNumericValue(_number);
                Add(sequence, Path.Combine(_basePath, "TRAIN\\ENGLISH\\TRAINUMBER\\" + num + ".Wav"));
            }
            Add(sequence, Path.Combine(_basePath, "COACH\\ARE.Wav"));
            Add(sequence, Path.Combine(_basePath, "COACH_NUMBER\\ENG SE PAHLA.wav"));
            if (!string.IsNullOrEmpty(train.COACH_SEQUENCE))
                foreach (string _coach in train.COACH_SEQUENCE.Split('-'))
                {
                    int counter = 1;
                    if (!string.IsNullOrEmpty(_coach) && _coach != "ENG")
                    {
                        if (counter != 1)
                            Add(sequence, Path.Combine(_basePath, "COACH_NUMBER\\" + counter + ".wav"));
                        Add(sequence, Path.Combine(_basePath, "COACH\\" + _coach + ".wav"));
                        counter++;
                    }
                }

        }

        return sequence;
    }
    private bool Add(List<string> list, string path)
    {
        if (File.Exists(path))
        {
            list.Add(path);
            return true;
        }
        return false;
    }

    private bool Exists(string basePath, string folder, string code)
    {
        if (IsBlank(code)) return false;
        return File.Exists(Path.Combine(basePath, folder, code + ".wav"));
    }

    private bool IsBlank(string val)
    {
        return string.IsNullOrWhiteSpace(val);
    }

    private void AddTime(List<string> seq, string lang, string path, string timeStr)
    {
        if (string.IsNullOrEmpty(timeStr)) return;

        var t = Convert.ToDateTime(timeStr);

        if (lang == "english")
        {
            Add(seq, Path.Combine(path, "HOUR", t.ToString("HH") + ".wav"));
            if (t.Minute > 0)
                Add(seq, Path.Combine(path, "MINUTE", t.ToString("mm") + ".wav"));
        }
        else
        {
            if (t.Minute > 0)
            {
                Add(seq, Path.Combine(path, "HOUR", "BAJKAR", t.ToString("HH") + ".wav"));
                Add(seq, Path.Combine(path, "MINUTE", t.ToString("mm") + ".wav"));
                Add(seq, Path.Combine(path, "SPEECH", "PAR.wav"));
            }
            else
            {
                Add(seq, Path.Combine(path, "HOUR", "BAJE", t.ToString("HH") + ".wav"));
            }
        }
    }

    private void AddDelay(List<string> seq, string lang, string path, string late)
    {
        if (string.IsNullOrEmpty(late)) return;

        var parts = late.Split(':');
        if (parts.Length != 2) return;

        if (int.TryParse(parts[0], out int hr) && hr > 0)
        {
            if (lang == "english")
                Add(seq, Path.Combine(path, "HOUR", parts[0] + ".wav"));
            else
                Add(seq, Path.Combine(path, "GHANTE", parts[0] + ".wav"));
        }

        if (int.TryParse(parts[1], out int min) && min > 0)
        {
            Add(seq, Path.Combine(path, "MINUTE", parts[1] + ".wav"));
        }
    }
}