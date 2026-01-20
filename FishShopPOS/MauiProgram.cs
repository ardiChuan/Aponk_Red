using Microsoft.Extensions.Logging;
using FishShopPOS.Services;
using FishShopPOS.ViewModels;
using CommunityToolkit.Maui;

namespace FishShopPOS;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register Services
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<FileService>();

        // Register ViewModels
        builder.Services.AddTransient<BaseViewModel>();
    
        // Register Views

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}