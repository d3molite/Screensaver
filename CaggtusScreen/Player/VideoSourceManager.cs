using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CaggtusScreen.Player;

public class VideoSourceManager
{
    private const int SpecialChance = 10;

    private readonly Random _random = new();

    private const string Folder = "./Resources";
    private static readonly string Root = AppDomain.CurrentDomain.BaseDirectory;

    private static readonly List<string> Sources = new();

    public VideoSourceManager()
    {
       var files =  Directory.GetFiles(Folder);
       
       foreach (var file in files)
       {
           var name = Path.GetFileName(file);
           Sources.Add(name);
       }
    }

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

    public static Uri GetStartingSource()
    {
        var source = Sources[0];
        return GetSourceUri(source);
    }

    private static Uri GetSourceUri(string source)
    {
        return new Uri(Path.Combine(Root, Folder, source));
    }

    private string GetSource()
    {
        var isSpecial = _random.Next(0, 100) < SpecialChance;
        return !isSpecial ? Sources[0] : Sources[_random.Next(0, Sources.Count)];
    }
}