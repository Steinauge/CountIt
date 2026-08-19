using System.IO;
using System.Text.Json;
using CountIt.Core.Models;

namespace CountIt.Core.Services;

public class JsonStorageService : IStorageService
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public JsonStorageService(string filePath)
    {
        _filePath = filePath;
    }

    public void Save(List<SectionItem> items)
    {
        string json = JsonSerializer.Serialize(items, _options);
        File.WriteAllText(_filePath, json);
    }

    public List<SectionItem> Load()
    {
        if (!File.Exists(_filePath))
            return new List<SectionItem>();

        try
        {
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<SectionItem>>(json) ?? new List<SectionItem>();
        }
        catch
        {
            // Falls das alte JSON-Format (ohne Abschnitte) noch vorhanden ist und beim Parsen fehlschlägt:
            return new List<SectionItem>();
        }
    }
}