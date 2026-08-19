using Plugin.Maui.Audio;
using CountIt.Core.Services;

namespace CountIt.Maui.Services;

public class MauiSoundService : ISoundService
{
    private readonly IAudioManager _audioManager;

    public MauiSoundService(IAudioManager audioManager)
    {
        _audioManager = audioManager;
    }

    public async void Play(string? filePath, double volume = 0.5)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            return;

        try
        {
            using var stream = File.OpenRead(filePath);
            var player = _audioManager.CreatePlayer(stream);
            player.Volume = Math.Clamp(volume, 0.0, 1.0);
            player.Play();
        }
        catch
        {
            // Ignorieren, falls die Datei unlesbar ist
        }
    }
}