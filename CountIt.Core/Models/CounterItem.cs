namespace CountIt.Core.Models;

public class CounterItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public int Points { get; set; } = 0;
    public string? IncrementHotkey { get; set; }
    public string? DecrementHotkey { get; set; }
    public double Volume { get; set; } = 0.5;

    public string? SoundPath { get; set; }
}