using DataModel;
using Global_Data;
using Ipis.Snmp;

namespace Helper
{
    public class AnnouncementManager
    {
        private readonly SequenceBuilder _builder = new SequenceBuilder();

        public volatile bool _isPaused;
        public volatile bool _stopRequested;
        private readonly ManualResetEventSlim _pauseEvent = new ManualResetEventSlim(true);
        public bool IsPaused => _isPaused;
        public void Pause()
        {
            if (_stopRequested) return;

            _isPaused = true;
            _pauseEvent.Reset();
            MediaManager.Instance.Pause();
        }

        public void Resume()
        {
            if (!_isPaused) return;

            _isPaused = false;
            _pauseEvent.Set();
            MediaManager.Instance.Resume();
        }

        public void Stop()
        {
            _stopRequested = true;
            _isPaused = false;
            _pauseEvent.Set();
            MediaManager.Instance.Stop();
        }

        private async Task WaitIfPausedAsync()
        {
            while (_isPaused && !_stopRequested)
                await Task.Delay(100);
        }
        public async Task<bool> RunAnnouncementsAsync(List<train_updated_data> trains, int repeat, string language, string type)
        {
            bool audioOk = await AudioOutputMonitor.Instance.CheckAndReportAsync();
            if (!audioOk) if (GlobalData.CurrentUserAccessRight == null ||
                GlobalData.CurrentUserAccessRight.PLAY_ANNOUNCEMENT != 1)
            {
                _stopRequested = true;
                _isPaused = true;
                MediaManager.Instance.Pause();

                return false;
            }
            _stopRequested = false;
            _isPaused = false;
            var sortedAnnouncements = trains
                .OrderBy(x =>
                {
                    var time = x.ADFlag == "D"
                        ? x.SCHEDULE_DEPARTURE_TIME
                        : x.SCHEDULE_ARRIVAL_TIME;

                    return TimeSpan.Parse(time);
                })
                .ToList();
            var trainsCopy = sortedAnnouncements;


            for (int i = 0; i < repeat; i++)
            {
                if (_stopRequested) break;
                List<string> sequence = new List<string>();
                foreach (var train in trainsCopy)
                {
                    if (_stopRequested) break;
                   
                    if (type == "regular")
                        sequence = _builder.BuildSequence(train, language);
                    else if (type == "coach")
                        sequence = _builder.BuildSequenceCoach(train, language);
                    foreach (var file in sequence)
                    {
                        if (_stopRequested) break; 
                        await WaitIfPausedAsync();
                        await MediaManager.Instance.PlayAsync(file);
                        await WaitIfPausedAsync();
                    }
                } 
            }
            return true;
        }
    }
}
