using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.IO;
using System.Threading.Tasks;

namespace ACDCs;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        this.UserAppTheme = AppTheme.Dark;
        MainPage = new AppShell();
    }

    public static async Task<string> LoadMauiAssetAsString(string name)
    {
        await using var stream = await FileSystem.OpenAppPackageFileAsync(name);
        using var reader = new StreamReader(stream);

        var contents = await reader.ReadToEndAsync();
        return contents;
    }
}