using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CountIt.Core.Models;
using CountIt.Core.Services;

namespace CountIt.Core.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IStorageService _storageService;
    private readonly ISoundService _soundService;

    public ObservableCollection<SectionViewModel> Sections { get; } = new();

    public Func<string?>? OpenFileDialogAction { get; set; }
    public Func<string?>? SaveFileDialogAction { get; set; }

    public MainViewModel(IStorageService storageService, ISoundService soundService)
    {
        _storageService = storageService;
        _soundService = soundService;
        LoadData();
    }

    [RelayCommand]
    public void AddSection()
    {
        var newSectionModel = new SectionItem
        {
            Name = $"Abschnitt {Sections.Count + 1}"
        };

        if (Sections.LastOrDefault() is SectionViewModel lastSection)
        {
            foreach (var oldItem in lastSection.Items)
            {
                newSectionModel.Items.Add(new CounterItem
                {
                    Name = oldItem.Name,
                    Points = oldItem.Points,         // Gesamtpunktestand übernehmen
                    SectionDelta = 0,                // <-- Startet im neuen Abschnitt IMMER bei 0!
                    IncrementHotkey = oldItem.IncrementHotkey,
                    DecrementHotkey = oldItem.DecrementHotkey,
                    SoundPath = oldItem.SoundPath,
                    Volume = oldItem.Volume
                });

                oldItem.IncrementHotkey = null;
                oldItem.DecrementHotkey = null;
            }
        }
        else
        {
            newSectionModel.Items.Add(new CounterItem { Name = "Zähler 1", Points = 0, SectionDelta = 0 });
        }

        var newSectionVm = new SectionViewModel(newSectionModel, _soundService);
        Sections.Add(newSectionVm);
        SaveData();
    }

    [RelayCommand]
    public void RemoveSection(SectionViewModel section)
    {
        Sections.Remove(section);
        SaveData();
    }

    [RelayCommand]
    public void SaveToFile()
    {
        var filePath = SaveFileDialogAction?.Invoke();
        if (string.IsNullOrWhiteSpace(filePath)) return;

        SaveDataToPath(filePath);
    }

    [RelayCommand]
    public void LoadFromFile()
    {
        var filePath = OpenFileDialogAction?.Invoke();
        if (string.IsNullOrWhiteSpace(filePath)) return;

        LoadDataFromPath(filePath);
    }

    public void SaveDataToPath(string filePath)
    {
        var models = Sections.Select(s => {
            s.Model.Items = s.Items.Select(i => i.Model).ToList();
            return s.Model;
        }).ToList();

        // Speichert in die gewählte Datei
        var storage = new JsonStorageService(filePath);
        storage.Save(models);
    }

    public void LoadDataFromPath(string filePath)
    {
        var storage = new JsonStorageService(filePath);
        var models = storage.Load();

        Sections.Clear();
        foreach (var model in models)
        {
            Sections.Add(new SectionViewModel(model, _soundService));
        }
    }


    public void SaveData()
    {
        var models = Sections.Select(s => {
            s.Model.Items = s.Items.Select(i => i.Model).ToList();
            return s.Model;
        }).ToList();

        // Hinweis: IStorageService speichert jetzt List<SectionItem>
        _storageService.Save(models);
    }

    private void LoadData()
    {
        var models = _storageService.Load(); // Lädt List<SectionItem>
        Sections.Clear();

        if (!models.Any())
        {
            AddSection(); // Mindestens einen Abschnitt zum Start anlegen
            return;
        }

        foreach (var model in models)
        {
            Sections.Add(new SectionViewModel(model, _soundService));
        }
    }
}