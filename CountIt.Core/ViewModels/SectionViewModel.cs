using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CountIt.Core.Models;
using CountIt.Core.Services;

namespace CountIt.Core.ViewModels;

public partial class SectionViewModel : ObservableObject
{
    private readonly ISoundService _soundService;

    public SectionItem Model { get; }

    [ObservableProperty] private string _name;

    public ObservableCollection<CounterItemViewModel> Items { get; } = new();

    public SectionViewModel(SectionItem model, ISoundService soundService)
    {
        Model = model;
        _soundService = soundService;
        _name = model.Name;

        foreach (var item in model.Items)
        {
            Items.Add(new CounterItemViewModel(item, soundService));
        }
    }

    [RelayCommand]
    public void AddCounter()
    {
        var newModel = new CounterItem { Name = $"Zähler {Items.Count + 1}", Points = 0 };
        var newVm = new CounterItemViewModel(newModel, _soundService);

        Items.Add(newVm);
        Model.Items.Add(newModel);
    }

    [RelayCommand]
    public void RemoveCounter(CounterItemViewModel counter)
    {
        Items.Remove(counter);
        Model.Items.Remove(counter.Model);
    }

    partial void OnNameChanged(string value) => Model.Name = value;
}