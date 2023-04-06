using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CaggtusScreen.Player;

public class TipSourceManager
{
    private List<string> Tips { get; set; }
    private readonly Random _random = new();
    private int _current;

    public TipSourceManager()
    {
        Tips = File.ReadAllLines("tips.txt").ToList();
    }

    public Tuple<int, string> GetRandomTip()
    {
        var options = Enumerable.Range(0, Tips.Count).ToList();
        options.Remove(_current);
        
        var option = _random.Next(0, options.Count);
        var position = options[option];

        _current = position;
        
        return new Tuple<int, string>(position+1, Tips[position]);
    }
}