using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Web.WebView2.Core;

namespace Diplom.Windows;

public partial class HelpWindow : Window
{
    private readonly string url;

    public HelpWindow(string url)
    {
        this.url = url;
        InitializeComponent();
        Loaded += HelpWindow_Loaded;
    }

    private async void HelpWindow_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string exeDir = System.IO.Path.GetDirectoryName(exePath);

            string resourcesPath = System.IO.Path.Combine(exeDir, "Resources");

            if (!Directory.Exists(resourcesPath))
            {
                MessageBox.Show($"Папка Resources не найдена по пути: {resourcesPath}");
                return;
            }

            await HelpBrowser.EnsureCoreWebView2Async(null);

            HelpBrowser.CoreWebView2.SetVirtualHostNameToFolderMapping(
                "help.local",
                resourcesPath,
                Microsoft.Web.WebView2.Core.CoreWebView2HostResourceAccessKind.Allow);

            string htmlPath = System.IO.Path.Combine(exeDir, "help.html");
            HelpBrowser.CoreWebView2.Navigate($"https://help.local/help.html");
        }
        catch (Exception ex)
        {
            string errorHtml = @"
                <html>
                <body style='font-family: Arial; padding: 20px;'>
                    <h2 style='color: red;'>Ошибка загрузки</h2>
                    <p>" + ex.Message + @"</p>
                </body>
                </html>";
            HelpBrowser.NavigateToString(errorHtml);
        }
    }
}