using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using CaggtusScreen.Commands;
using LibVLCSharp.Shared;
using LibVLCSharp.WPF;
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;

namespace CaggtusScreen.Player;

public class PlayerViewModel : NotificationBase
{
    private readonly PlayerManager _playerManager;

    public SolidColorBrush Pink => (SolidColorBrush)new BrushConverter().ConvertFrom("#e93479")!;
    
    public TipManager TipManager { get; set; }

    public PlayerViewModel(VideoView videoView)
    {
        
        _playerManager = new PlayerManager(videoView);
        TipManager = new TipManager();
    }
}