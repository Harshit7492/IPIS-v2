using DataBaseHandler;
using LibVLCSharp.Shared;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.IO;
using System.Threading.Tasks;

public sealed class MediaManager
{
    private static readonly Lazy<MediaManager> _instance = new Lazy<MediaManager>(() => new MediaManager());
    public static MediaManager Instance => _instance.Value;
    private MediaPlayer _recordingPlayer;
    private Media _recordingMedia;
    private bool _isRecording = false;
    public bool IsRecording => _isRecording;
    public LibVLC LibVLC { get; private set; }
    public MediaPlayer MediaPlayer { get; private set; }
    private bool _initialized = false;
    private WaveInEvent _waveIn;
    private WaveFileWriter _writer;
    private MediaManager() { }

    public void Initialize()
    {
        if (_initialized) return;

        Core.Initialize();
        LibVLC = new LibVLC();
        MediaPlayer = new MediaPlayer(LibVLC);

        _initialized = true;
    }
    public void Play(string path)
    {
        Initialize();

        if (string.IsNullOrEmpty(path) || !File.Exists(path))
            return;

        MediaPlayer.Stop();

        using (var media = new Media(LibVLC, path, FromType.FromPath))
        {
            MediaPlayer.Play(media);
        }
    }
    public bool IsExternalMicrophoneAvailable()
    {
        var enumerator = new MMDeviceEnumerator();
        var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

        return devices.Any(d =>
        {
            var name = d.FriendlyName.ToLower();

            bool isInternal =
                name.Contains("internal") ||
                name.Contains("array") ||
                name.Contains("realtek") ||
                name.Contains("built");

            return !isInternal;
        });
    }
    public async Task<long> GetMediaDurationAsync(string path)
    {
        Initialize();

        using (var media = new Media(LibVLC, path, FromType.FromPath))
        {
            var tcs = new TaskCompletionSource<bool>();

            void Parsed(object s, EventArgs e)
            {
                media.ParsedChanged -= Parsed;
                tcs.TrySetResult(true);
            }
            media.ParsedChanged += Parsed;
            media.Parse(MediaParseOptions.ParseLocal);
            await Task.WhenAny(tcs.Task, Task.Delay(2000));
            return media.Duration > 0 ? media.Duration : 5000;
        }
    }
    public async Task PlayAsync(string path)
    {
        Initialize();

        if (string.IsNullOrEmpty(path) || !File.Exists(path))
            return;

        var tcs = new TaskCompletionSource<bool>();

        void EndHandler(object s, EventArgs e)
        {
            tcs.TrySetResult(true);
        }

        void ErrorHandler(object s, EventArgs e)
        {
            tcs.TrySetResult(true);
        }

        MediaPlayer.EndReached += EndHandler;
        MediaPlayer.EncounteredError += ErrorHandler;

        try
        {
            long duration = await GetMediaDurationAsync(path);
            MediaPlayer.Stop();

            using (var media = new Media(LibVLC, path, FromType.FromPath))
            {
                MediaPlayer.Play(media);
            }
            await Task.WhenAny(tcs.Task, Task.Delay((int)duration + 1000));
        }
        finally
        {
            MediaPlayer.EndReached -= EndHandler;
            MediaPlayer.EncounteredError -= ErrorHandler;
        }
    }
    public void Stop()
    {
        MediaPlayer?.Stop();
    }
    public void Pause()
    {
        if (MediaPlayer?.CanPause == true)
            MediaPlayer.Pause();
    }
    public void Resume()
    {
        if (MediaPlayer != null)
            MediaPlayer.SetPause(false);
    }
    public void SetVolume(int volume)
    {
        Initialize();
        if (MediaPlayer == null)
            return;

        volume = Math.Clamp(volume, 0, 100);
        MediaPlayer.Volume = volume;
    }
    public string StartRecording(string title, string folderPath)
    {
        if (_isRecording)
            return null;

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string filePath = Path.Combine(folderPath, $"{title}_{DateTime.Now:yyyyMMdd_HHmmss}.wav");

        _waveIn = new WaveInEvent();
        _waveIn.WaveFormat = new WaveFormat(44100, 1);
        _writer = new WaveFileWriter(filePath, _waveIn.WaveFormat);
        _waveIn.DataAvailable += (s, e) =>
        {
            for (int i = 0; i < e.BytesRecorded; i += 2)
            {
                short sample = BitConverter.ToInt16(e.Buffer, i);

                // Noise gate
                if (sample > -500 && sample < 500)
                {
                    sample = 0;
                }

                byte[] bytes = BitConverter.GetBytes(sample);
                e.Buffer[i] = bytes[0];
                e.Buffer[i + 1] = bytes[1];
            }

            _writer.Write(e.Buffer, 0, e.BytesRecorded);
        };
        _waveIn.StartRecording();
        _isRecording = true;
        return filePath;
    }
    public void StopRecording()
    {
        if (!_isRecording)
            return;
        _waveIn?.StopRecording();
        _waveIn?.Dispose();
        _writer?.Dispose();
        _waveIn = null;
        _writer = null;
        _isRecording = false;
    }
    public bool IsPlaying => MediaPlayer?.IsPlaying ?? false;

}