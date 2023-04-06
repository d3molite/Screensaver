using System;
using System.Threading;
using System.Threading.Tasks;
using CaggtusScreen.Commands;

namespace CaggtusScreen.Player;

public class TipManager : NotificationBase
{
    private static readonly TimeSpan TipDuration = TimeSpan.FromSeconds(8);
    private readonly PeriodicTimer _tipCycle = new(TipDuration);
    
    private readonly TipSourceManager _tipSourceManager = new();

    private string _hintText = null!;
    private string _tipText = null!;

    public TipManager()
    {
        SetTip();
    }
    
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
    
    private void SetTip()
    {
        var (num, tip) = _tipSourceManager.GetRandomTip();
        TipText = $"Caggtus Tip No. {num}";
        HintText = tip;
    }
    
    private async Task TipLoop()
    {
        while (await _tipCycle.WaitForNextTickAsync(CancellationToken.None))
        {
            SetTip();
        }
    }
}