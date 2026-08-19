using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using CountIt.Core.Models;
using CountIt.Core.Services;

namespace CountIt.Tests;

[TestClass]
public class JsonStorageServiceTests
{
    private string _testFilePath = null!;

    [TestInitialize]
    public void Setup()
    {
        // Einen eindeutigen Pfad im Temp-Ordner für jeden Testrun generieren
        _testFilePath = Path.Combine(Path.GetTempPath(), $"countit_test_{Guid.NewGuid()}.json");
    }

    [TestCleanup]
    public void Cleanup()
    {
        // Nach dem Test die temporäre Datei sauber löschen
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }

    [TestMethod]
    public void SaveAndLoad_ShouldPersistAndRetrieveItemsCorrectly()
    {
        // Arrange
        var service = new JsonStorageService(_testFilePath);
        var itemsToSave = new List<CounterItem>
        {
            new CounterItem
            {
                Name = "Kills",
                Points = 15,
                IncrementHotkey = "Ctrl+F1",
                DecrementHotkey = "Ctrl+F2"
            },
            new CounterItem
            {
                Name = "Wins",
                Points = 3
            }
        };

        // Act
        service.Save(itemsToSave);
        var loadedItems = service.Load();

        // Assert
        Assert.IsTrue(File.Exists(_testFilePath), "Die JSON-Datei wurde nicht erstellt.");
        Assert.AreEqual(2, loadedItems.Count);

        Assert.AreEqual("Kills", loadedItems[0].Name);
        Assert.AreEqual(15, loadedItems[0].Points);
        Assert.AreEqual("Ctrl+F1", loadedItems[0].IncrementHotkey);
        Assert.AreEqual("Ctrl+F2", loadedItems[0].DecrementHotkey);

        Assert.AreEqual("Wins", loadedItems[1].Name);
        Assert.AreEqual(3, loadedItems[1].Points);
    }

    [TestMethod]
    public void Load_ShouldReturnEmptyList_WhenFileDoesNotExist()
    {
        // Arrange
        var nonExistentPath = Path.Combine(Path.GetTempPath(), $"non_existent_{Guid.NewGuid()}.json");
        var service = new JsonStorageService(nonExistentPath);

        // Act
        var result = service.Load();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }
}