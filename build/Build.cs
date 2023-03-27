using System.IO;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = Configuration.Release;

    [Solution] readonly Solution Solution;

    AbsolutePath SourceDirectory => RootDirectory / "source";

    AbsolutePath OutputDirectory => RootDirectory / "Output";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(OutputDirectory);
            foreach (var file in Directory.GetFiles(OutputDirectory))
            {
                File.Delete(file);
            }
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

    Target Publish => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetPublish(s => s
                .SetOutput(OutputDirectory)
                .SetConfiguration(Configuration.Release)
                .SetSelfContained(false)
                .SetPublishTrimmed(false)
                .SetPublishSingleFile(true)
            );
        });

    Target FormatOutput => _ => _
        .DependsOn(Publish)
        .Executes(() =>
        {
            foreach (var directory in Directory.GetDirectories(OutputDirectory))
            {
                if (!directory.Contains("Resources"))
                {
                    Directory.Delete(directory);
                }
            }
            
            foreach (var file in Directory.GetFiles(OutputDirectory))
            {
                if (file.EndsWith(".scr"))
                {
                    File.Delete(file);
                }
                
                if (file.EndsWith(".pdb")) File.Delete(file);
            }

            foreach (var file in Directory.GetFiles(OutputDirectory))
            {
                if (file.EndsWith(".exe"))
                {
                    File.Move(file, file.Replace(".exe", ".scr"));
                }
            }
        });

    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft VisualStudio     https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.FormatOutput);
}