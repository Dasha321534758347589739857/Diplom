using System.IO;
using System.Text;
using System.Windows;

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
            await HelpBrowser.EnsureCoreWebView2Async();

            // Включаем JavaScript
            HelpBrowser.CoreWebView2.Settings.IsScriptEnabled = true;

            // Проверяем, существует ли файл
            if (File.Exists(url))
            {
                // Читаем содержимое файла
                string htmlContent = File.ReadAllText(url, Encoding.UTF8);

                // Загружаем HTML в WebView2
                HelpBrowser.NavigateToString(htmlContent);
            }
            else
            {
                // Если файл не найден, показываем ошибку
                string errorHtml = @"
                    <html>
                    <body style='font-family: Arial; padding: 20px;'>
                        <h2 style='color: red;'>Файл не найден</h2>
                        <p>Не удалось найти файл:</p>
                        <code>" + url + @"</code>
                        <p>Проверьте путь к файлу.</p>
                    </body>
                    </html>";
                HelpBrowser.NavigateToString(errorHtml);
            }
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