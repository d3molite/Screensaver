using System.Windows;

namespace CaggtusScreen;

public static class VisibilityHelper
{
    public static Visibility ToVisibility(this bool isVisible)
    {
        return isVisible ? Visibility.Visible : Visibility.Collapsed;
    }
}