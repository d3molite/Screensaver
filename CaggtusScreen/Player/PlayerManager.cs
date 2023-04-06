using System;
using System.Threading;
using System.Threading.Tasks;
using LibVLCSharp.Shared;
using LibVLCSharp.WPF;

namespace CaggtusScreen.Player;

public class PlayerManager
{
    private static readonly TimeSpan PlayerDuration = TimeSpan.FromSeconds(5);
    private readonly PeriodicTimer _playerCycle = new (PlayerDuration);
    
    private LibVLC _libVlc = null!;
    private MediaPlayer _mediaPlayer = null!;
    private readonly VideoView _videoView;
    private readonly VideoSourceManager _videoSourceManager;

    public PlayerManager(VideoView videoView)
    {

        _videoView = videoView;
        _videoSourceManager = new VideoSourceManager();
        
        InitializeLibVlc();
        InitializePlayback();

        Task.Run(PlayerLoop);
    }

    private void InitializeLibVlc()
    {
        _libVlc = new LibVLC();
        _mediaPlayer = new MediaPlayer(_libVlc);
        _videoView.MediaPlayer = _mediaPlayer;
    }

    private void InitializePlayback()
    {
        using var media = new Media(_libVlc, VideoSourceManager.GetStartingSource());
        _mediaPlayer.Media = media;
        _mediaPlayer.Play();
    }
    
    private async Task PlayerLoop()
    {
        while (await _playerCycle.WaitForNextTickAsync(CancellationToken.None))
        {
            using var media = new Media(_libVlc,  await _videoSourceManager.GetRandomSourceAsync());
            _mediaPlayer.Media = media;
            _mediaPlayer.Play();
        }
    }
}