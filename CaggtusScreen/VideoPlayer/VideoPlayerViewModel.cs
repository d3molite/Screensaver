using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CaggtusScreen.Commands;

namespace CaggtusScreen;

public class VideoPlayerViewModel : NotificationBase
{
    private readonly TimeSpan _duration = TimeSpan.FromSeconds(5);
    private readonly TimeSpan _tipDuration = TimeSpan.FromSeconds(8);
    private readonly MediaElement _playerOne;
    private readonly MediaElement _playerTwo;
    private readonly VideoSourceManager _sourceManager = new();
    private readonly TipSourceManager _tipManager = new();

    private readonly PeriodicTimer _timer;
    private readonly PeriodicTimer _tipCycle;

    private bool _playerOneVisible = true;
    private bool _playerTwoVisible = false;

    private bool SwitchPlayer => CurrentSource != _nextSource;

    private Uri CurrentSource => _playerOneVisible ? SourceOne : SourceTwo;

    private Uri _nextSource;
    private string _hintText;
    private string _tipText;

    public VideoPlayerViewModel(MediaElement playerOne, MediaElement playerTwo)
    {
        _playerOne = playerOne;
        _playerTwo = playerTwo;

        _timer = new PeriodicTimer(_duration);
        _tipCycle = new PeriodicTimer(_tipDuration);

        SourceOne = _sourceManager.GetStartingSource();
        SourceTwo = _sourceManager.GetRandomSource();
        _playerOne.Play();

        SetTip();
        
        Task.Run(PlayerLoop);
        Task.Run(TipLoop);
    }

    public Uri SourceOne { get; set; }

    public Uri SourceTwo { get; set; }

    public SolidColorBrush Pink => (SolidColorBrush)new BrushConverter().ConvertFrom("#e93479")!;

    public string HintText
    {
        get => _hintText;
        set
        {
            if (value == _hintText) return;
            _hintText = value;
            OnPropertyChanged();
        }
    }

    public string TipText
    {
        get => _tipText;
        set
        {
            if (value == _tipText) return;
            _tipText = value;
            OnPropertyChanged();
        }
    }

    public Visibility PlayerOneVisibility => Visibility.Visible;
    public Visibility PlayerTwoVisibility => _playerTwoVisible.ToVisibility();

    private async Task PlayerLoop()
    {
        while (await _timer.WaitForNextTickAsync(CancellationToken.None))
        {
            if (SwitchPlayer) SwapPlayer();
            RestartCurrentPlayer();
            
            _nextSource = await _sourceManager.GetRandomSourceAsync();

            if (_nextSource == CurrentSource) continue;
            
            UpdateNextSource();
        }
    }

    private async Task TipLoop()
    {
        while (await _tipCycle.WaitForNextTickAsync(CancellationToken.None))
        {
            SetTip();
        }
    }

    private void SetTip()
    {
        var (num, tip) = _tipManager.GetRandomTip();
        TipText = $"Caggtus Tip No. {num}";
        HintText = tip;
    }

    private void UpdateNextSource()
    {
        if (_playerOneVisible)
        {
            SourceTwo = _nextSource;
            return;
        }

        SourceOne = _nextSource;
    }

    private void SwapPlayer()
    {
        _playerOneVisible = !_playerOneVisible;
        _playerTwoVisible = !_playerTwoVisible;
        OnPropertyChanged(nameof(PlayerOneVisibility));
        OnPropertyChanged(nameof(PlayerTwoVisibility));
    }

    private void RestartCurrentPlayer()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            if (_playerOneVisible)
            {
                _playerOne.Stop();
                _playerOne.Play();
                _playerTwo.Stop();
                _playerTwo.Play();
            }
            else
            {
                _playerTwo.Stop();
                _playerTwo.Play();
                _playerOne.Stop();
                _playerOne.Play();
            }
        });
    }
}