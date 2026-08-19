
using Plugin.Maui.Audio;
using CountIt.Core.Services;
using CountIt.Core.ViewModels;
using CountIt.Maui.Services;

namespace CountIt.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Audio-Manager von MAUI registrieren
        builder.Services.AddSingleton(AudioManager.Current);

        // Deine Core-Services für MAUI registrieren
        builder.Services.AddSingleton<IStorageService>(sp =>
            new JsonStorageService(MauiStorageHelper.GetDefaultFilePath()));

        builder.Services.AddSingleton<ISoundService, MauiSoundService>();

        // ViewModels und Pages
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<MainPage>();

        return builder.Build();
    }
}