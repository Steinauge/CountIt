namespace CountIt.Core.Services;

public interface IHotkeyService
{
    void RegisterHotkey(string keyCombination, Action callback);
    void UnregisterAll();
}