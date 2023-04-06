using System;
using System.Windows;
using System.Windows.Input;
using CaggtusScreen.Player;

namespace CaggtusScreen;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += InitializeDataContext;
    }

    private void InitializeDataContext(object sender, RoutedEventArgs e)
    {
        DataContext = new PlayerViewModel(VideoView);
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);

        Application.Current.Shutdown();
    }

    private void Quit(object sender, KeyEventArgs e)
    {
        Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        Application.Current.Shutdown();
    }

    private void QuitTwo(object sender, MouseButtonEventArgs e)
    {
        Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        Application.Current.Shutdown();
    }
}