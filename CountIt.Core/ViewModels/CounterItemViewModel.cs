using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CountIt.Core.Models;
using CountIt.Core.Services;

namespace CountIt.Core.ViewModels;

public partial class CounterItemViewModel : ObservableObject
{
    private readonly ISoundService _soundService;

    public CounterItem Model { get; }

    [ObservableProperty] private string _name;
    [ObservableProperty] private int _points;
    [ObservableProperty] private int _sectionDelta;
    [ObservableProperty] private string? _incrementHotkey;
    [ObservableProperty] private string? _decrementHotkey;
    [ObservableProperty] private string? _soundPath;

    [ObservableProperty] private double _volume;

    public Func<string?>? SelectSoundFileAction { get; set; }

    public CounterItemViewModel(CounterItem model, ISoundService soundService)
    {
        Model = model;
        _soundService = soundService;

        _name = model.Name;
        _points = model.Points;
        _sectionDelta = model.SectionDelta;
        _incrementHotkey = model.IncrementHotkey;
        _decrementHotkey = model.DecrementHotkey;
        _soundPath = model.SoundPath;
        _volume = model.Volume;
    }

    [RelayCommand]
    public void Increment()
    {
        Points++;
        SectionDelta++;
        Model.Points = Points;
        Model.SectionDelta = SectionDelta;
        _soundService.Play(SoundPath, Volume);
    }

    [RelayCommand]
    public void Decrement()
    {
        Points--;
        SectionDelta--;
        Model.Points = Points;
        Model.SectionDelta = SectionDelta;
    }

    [RelayCommand]
    public void SelectSound()
    {
        var path = SelectSoundFileAction?.Invoke();
        if (!string.IsNullOrEmpty(path))
        {
            SoundPath = path;
        }
    }

    [RelayCommand]
    public void ClearSound()
    {
        SoundPath = null;
    }

    [RelayCommand]
    public void ClearIncrementHotkey() => IncrementHotkey = null;

    [RelayCommand]
    public void ClearDecrementHotkey() => DecrementHotkey = null;

    partial void OnNameChanged(string value) => Model.Name = value;
    partial void OnIncrementHotkeyChanged(string? value) => Model.IncrementHotkey = value;
    partial void OnDecrementHotkeyChanged(string? value) => Model.DecrementHotkey = value;
    partial void OnSoundPathChanged(string? value) => Model.SoundPath = value;
    partial void OnVolumeChanged(double value) => Model.Volume = value;
}