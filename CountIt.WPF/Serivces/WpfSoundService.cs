// CountIt.WPF/Services/WpfSoundService.cs
using System;
using System.IO;
using System.Windows.Media;
using CountIt.Core.Services;

namespace CountIt.WPF.Services;

public class WpfSoundService : ISoundService
{
    public void Play(string? filePath, double volume = 0.5)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            return;

        try
        {
            var mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri(filePath, UriKind.Absolute));
            mediaPlayer.Volume = Math.Clamp(volume, 0.0, 1.0); // Lautstärke setzen
            mediaPlayer.Play();
        }
        catch
        {
            // Ignorieren, falls die Datei nicht geladen werden kann
        }
    }
}