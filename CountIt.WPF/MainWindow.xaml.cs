using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using CountIt.Core.Services;
using CountIt.Core.ViewModels;
using CountIt.WPF.Services;

namespace CountIt.WPF;

public partial class MainWindow : Window
{
    private readonly HotkeyService _hotkeyService = new();
    private readonly MainViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();

        IStorageService storageService = new JsonStorageService("items.json");
        ISoundService soundService = new WpfSoundService();

        _viewModel = new MainViewModel(storageService, soundService);
        DataContext = _viewModel;

        // Speicher-Dialog verdrahten
        _viewModel.SaveFileDialogAction = () =>
        {
            var dialog = new SaveFileDialog
            {
                Filter = "JSON Datei (*.json)|*.json",
                DefaultExt = "json",
                Title = "Zählerstand speichern unter..."
            };
            return dialog.ShowDialog() == true ? dialog.FileName : null;
        };

        // Lade-Dialog verdrahten
        _viewModel.OpenFileDialogAction = () =>
        {
            var dialog = new OpenFileDialog
            {
                Filter = "JSON Datei (*.json)|*.json",
                Title = "Zählerstand laden..."
            };
            return dialog.ShowDialog() == true ? dialog.FileName : null;
        };

        // Sound-Picker für bestehende Items in allen Abschnitten verknüpfen
        foreach (var section in _viewModel.Sections)
        {
            foreach (var item in section.Items)
            {
                AttachSoundPicker(item);
            }
        }
    }

    private void AttachSoundPicker(CounterItemViewModel item)
    {
        item.SelectSoundFileAction = () =>
        {
            var dialog = new OpenFileDialog
            {
                // Unterstützt jetzt MP3, WAV und weitere Audioformate:
                Filter = "Audio-Dateien (*.mp3;*.wav;*.wma)|*.mp3;*.wav;*.wma|Alle Dateien (*.*)|*.*",
                Title = "Sound für Zähler auswählen"
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        };
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        var handle = new WindowInteropHelper(this).Handle;
        _hotkeyService.Initialize(handle);

        RegisterAllHotkeys();
        SubscribeToPropertyChanged();
    }

    private void SubscribeToPropertyChanged()
    {
        // Auf neue/entfernte Abschnitte reagieren
        _viewModel.Sections.CollectionChanged += (s, e) =>
        {
            RegisterAllHotkeys();
            if (e.NewItems != null)
            {
                foreach (SectionViewModel section in e.NewItems)
                {
                    SubscribeToSectionItems(section);
                }
            }
        };

        foreach (var section in _viewModel.Sections)
        {
            SubscribeToSectionItems(section);
        }
    }

    private void SubscribeToSectionItems(SectionViewModel section)
    {
        section.Items.CollectionChanged += (s, e) =>
        {
            RegisterAllHotkeys();
            if (e.NewItems != null)
            {
                foreach (CounterItemViewModel item in e.NewItems)
                {
                    AttachSoundPicker(item);
                    item.PropertyChanged += Item_PropertyChanged;
                }
            }
        };

        foreach (var item in section.Items)
        {
            AttachSoundPicker(item);
            item.PropertyChanged += Item_PropertyChanged;
        }
    }

    private void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CounterItemViewModel.IncrementHotkey) ||
            e.PropertyName == nameof(CounterItemViewModel.DecrementHotkey))
        {
            RegisterAllHotkeys();
            _viewModel.SaveData();
        }
        else if (e.PropertyName == nameof(CounterItemViewModel.SoundPath) ||
                 e.PropertyName == nameof(CounterItemViewModel.Points) ||
                 e.PropertyName == nameof(CounterItemViewModel.Name))
        {
            _viewModel.SaveData();
        }
    }

    private void RegisterAllHotkeys()
    {
        // 1. Erst alle alten Hotkeys aus dem System entfernen
        _hotkeyService.UnregisterAll();

        // 2. Nur den neuesten (letzten) Abschnitt holen
        var activeSection = _viewModel.Sections.LastOrDefault();
        if (activeSection == null) return;

        // 3. Nur für die Zähler des AKTIVEN Abschnitts Hotkeys registrieren
        foreach (var item in activeSection.Items)
        {
            if (!string.IsNullOrWhiteSpace(item.IncrementHotkey))
            {
                _hotkeyService.RegisterHotkey(item.IncrementHotkey, item.Increment);
            }

            if (!string.IsNullOrWhiteSpace(item.DecrementHotkey))
            {
                _hotkeyService.RegisterHotkey(item.DecrementHotkey, item.Decrement);
            }
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        _hotkeyService.Dispose();
        base.OnClosed(e);
    }
}