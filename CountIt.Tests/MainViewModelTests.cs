using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CountIt.Core.Models;
using CountIt.Core.Services;
using CountIt.Core.ViewModels;

namespace CountIt.Tests;

[TestClass]
public class MainViewModelTests
{
    private Mock<IStorageService> _storageMock = null!;
    private Mock<ISoundService> _soundMock = null!;

    [TestInitialize]
    public void Setup()
    {
        _storageMock = new Mock<IStorageService>();
        _soundMock = new Mock<ISoundService>();

        _storageMock.Setup(s => s.Load()).Returns(new List<CounterItem>());
    }

    [TestMethod]
    public void AddItem_ShouldAddNewItemAndSave()
    {
        // Arrange
        var mainVm = new MainViewModel(_storageMock.Object, _soundMock.Object);

        // Act
        mainVm.AddItem();

        // Assert
        Assert.AreEqual(1, mainVm.Items.Count);
        _storageMock.Verify(s => s.Save(It.IsAny<List<CounterItem>>()), Times.Once);
    }

    [TestMethod]
    public void RemoveItem_ShouldRemoveItemAndSave()
    {
        // Arrange
        var mainVm = new MainViewModel(_storageMock.Object, _soundMock.Object);
        mainVm.AddItem();
        var itemToRemove = mainVm.Items[0];

        // Act
        mainVm.RemoveItem(itemToRemove);

        // Assert
        Assert.AreEqual(0, mainVm.Items.Count);
        _storageMock.Verify(s => s.Save(It.IsAny<List<CounterItem>>()), Times.Exactly(2));
    }
}