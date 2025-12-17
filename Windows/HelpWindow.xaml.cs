using System.Windows;

namespace Diplom.Windows;

public partial class HelpWindow : Window
{
    public HelpWindow(string url)
    {
        InitializeComponent();
        HelpBrowser.Navigate(url);
    }
}