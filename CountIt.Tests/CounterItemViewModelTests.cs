using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CountIt.Core.Models;
using CountIt.Core.Services;
using CountIt.Core.ViewModels;

namespace CountIt.Tests;

[TestClass]
public class CounterItemViewModelTests
{
    private Mock<ISoundService> _soundMock = null!;

    [TestInitialize]
    public void Setup()
    {
        _soundMock = new Mock<ISoundService>();
    }

    [TestMethod]
    public void Increment_ShouldIncreasePointsAndTriggerSound()
    {
        // Arrange
        var model = new CounterItem { Points = 5, SoundPath = "C:\\sounds\\click.wav" };
        var vm = new CounterItemViewModel(model, _soundMock.Object);

        // Act
        vm.Increment();

        // Assert
        Assert.AreEqual(6, vm.Points);
        Assert.AreEqual(6, vm.Model.Points);

        // Prüft, ob _soundMock.Play mit dem SoundPath genau 1x aufgerufen wurde
        _soundMock.Verify(s => s.Play("C:\\sounds\\click.wav"), Times.Once);
    }

    [TestMethod]
    public void Decrement_ShouldDecreasePointsAndTriggerSound()
    {
        // Arrange
        var model = new CounterItem { Points = 10, SoundPath = "C:\\sounds\\beep.wav" };
        var vm = new CounterItemViewModel(model, _soundMock.Object);

        // Act
        vm.Decrement();

        // Assert
        Assert.AreEqual(9, vm.Points);
        Assert.AreEqual(9, vm.Model.Points);

        _soundMock.Verify(s => s.Play("C:\\sounds\\beep.wav"), Times.Once);
    }

    [TestMethod]
    public void ClearSound_ShouldResetSoundPathToNull()
    {
        // Arrange
        var model = new CounterItem { SoundPath = "C:\\sounds\\test.wav" };
        var vm = new CounterItemViewModel(model, _soundMock.Object);

        // Act
        vm.ClearSound();

        // Assert
        Assert.IsNull(vm.SoundPath);
        Assert.IsNull(vm.Model.SoundPath);
    }

    [TestMethod]
    public void SelectSound_ShouldUpdateSoundPath_WhenActionReturnsPath()
    {
        // Arrange
        var model = new CounterItem();
        var vm = new CounterItemViewModel(model, _soundMock.Object);

        // Simuliere die Dateiauswahl (z. B. OpenFileDialog)
        vm.SelectSoundFileAction = () => "C:\\sounds\\new_sound.wav";

        // Act
        vm.SelectSound();

        // Assert
        Assert.AreEqual("C:\\sounds\\new_sound.wav", vm.SoundPath);
        Assert.AreEqual("C:\\sounds\\new_sound.wav", vm.Model.SoundPath);
    }
}