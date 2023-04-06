using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace CaggtusScreen;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        Startup += ApplicationStartup;
    }

    private void ApplicationStartup(object sender, StartupEventArgs e)
    {

        if (e.Args.Length == 0)
        {
            var window = new MainWindow
            {
                WindowStyle = WindowStyle.SingleBorderWindow,
            };
            window.Show();
            return;
        }

        if (!e.Args[0].ToLower().StartsWith("/s")) return;
        {
            foreach (var s in Screen.AllScreens)
                if (!Equals(s, Screen.PrimaryScreen))
                {
                    var blackout = new Blackout
                    {
                        Left = s.WorkingArea.Left,
                        Top = s.WorkingArea.Top,
                        Width = s.WorkingArea.Width,
                        Height = s.WorkingArea.Height
                    };
                    blackout.Show();
                }
                else
                {
                    var window = new MainWindow
                    {
                        Left = s.WorkingArea.Left,
                        Top = s.WorkingArea.Top,
                        Width = s.WorkingArea.Width,
                        Height = s.WorkingArea.Height
                    };
                    window.Show();
                }
        }
    }
}