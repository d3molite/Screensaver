using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CaggtusScreen;

public class VideoSourceManager
{
    private const int SpecialChance = 10;

    private readonly Random _random = new();

    private static List<string> Sources => new()
    {
        "base.mkv",
        "spin.mkv",
        "stars.mkv"
    };

    public async Task<Uri> GetRandomSourceAsync()
    {
        var source = await Task.Run(GetSource);
        return GetSourceUri(source);
    }
    
    public Uri GetRandomSource()
    {
        var source = GetSource();
        return GetSourceUri(source);
    }

    public Uri GetStartingSource()
    {
        var source = Sources[0];
        return GetSourceUri(source);
    }

    private Uri GetSourceUri(string source)
    {
        return new Uri(Path.Combine("./Resources/", source), UriKind.Relative);
    }

    private string GetSource()
    {
        var isSpecial = _random.Next(0, 100) > SpecialChance;
        return !isSpecial ? Sources[0] : Sources[_random.Next(0, Sources.Count)];
    }
}