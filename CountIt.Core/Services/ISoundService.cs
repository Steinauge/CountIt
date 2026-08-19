namespace CountIt.Core.Services;

public interface ISoundService
{
    void Play(string? filePath, double volume = 0.5);
}