using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Input;
using CountIt.Core.Services;

namespace CountIt.WPF.Services;

public class HotkeyService : IHotkeyService, IDisposable
{
    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    private const int WM_HOTKEY = 0x0312;
    private readonly Dictionary<int, Action> _hotkeyCallbacks = new();
    private IntPtr _windowHandle;
    private HwndSource? _hwndSource;
    private int _currentId;

    public void Initialize(IntPtr windowHandle)
    {
        _windowHandle = windowHandle;
        _hwndSource = HwndSource.FromHwnd(_windowHandle);
        _hwndSource?.AddHook(HwndHook);
    }

    public void RegisterHotkey(string keyCombination, Action callback)
    {
        if (_windowHandle == IntPtr.Zero || string.IsNullOrWhiteSpace(keyCombination)) return;

        // Bsp-Format: "Ctrl+Alt+NumPad1" oder "F10"
        ParseKeyCombination(keyCombination, out uint modifiers, out uint key);

        int id = ++_currentId;
        if (RegisterHotKey(_windowHandle, id, modifiers, key))
        {
            _hotkeyCallbacks[id] = callback;
        }
    }

    public void UnregisterAll()
    {
        foreach (var id in _hotkeyCallbacks.Keys)
        {
            UnregisterHotKey(_windowHandle, id);
        }
        _hotkeyCallbacks.Clear();
    }

    private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == WM_HOTKEY)
        {
            int id = wParam.ToInt32();
            if (_hotkeyCallbacks.TryGetValue(id, out var callback))
            {
                callback.Invoke();
                handled = true;
            }
        }
        return IntPtr.Zero;
    }

    private static void ParseKeyCombination(string input, out uint modifiers, out uint vk)
    {
        modifiers = 0;
        vk = 0;

        var parts = input.Split('+');
        foreach (var part in parts)
        {
            var cleanPart = part.Trim();
            if (cleanPart.Equals("Ctrl", StringComparison.OrdinalIgnoreCase)) modifiers |= 0x0002;
            else if (cleanPart.Equals("Alt", StringComparison.OrdinalIgnoreCase)) modifiers |= 0x0001;
            else if (cleanPart.Equals("Shift", StringComparison.OrdinalIgnoreCase)) modifiers |= 0x0004;
            else if (Enum.TryParse<Key>(cleanPart, true, out var parsedKey))
            {
                vk = (uint)KeyInterop.VirtualKeyFromKey(parsedKey);
            }
        }
    }

    public void Dispose()
    {
        UnregisterAll();
        _hwndSource?.RemoveHook(HwndHook);
    }
}