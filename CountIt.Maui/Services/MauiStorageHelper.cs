using CountIt.Core.Services;

namespace CountIt.Maui.Services;

public static class MauiStorageHelper
{
    public static string GetDefaultFilePath()
    {
        // Speichert automatisch im App-Datenordner (funktioniert auf Windows & Android)
        return Path.Combine(FileSystem.AppDataDirectory, "items.json");
    }
}