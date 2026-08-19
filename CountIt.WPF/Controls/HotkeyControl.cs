using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CountIt.WPF.Controls;

public class HotkeyControl : TextBox
{
    public static readonly DependencyProperty HotkeyProperty =
        DependencyProperty.Register(nameof(Hotkey), typeof(string), typeof(HotkeyControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnHotkeyChanged));

    public string? Hotkey
    {
        get => (string?)GetValue(HotkeyProperty);
        set => SetValue(HotkeyProperty, value);
    }

    public HotkeyControl()
    {
        IsReadOnly = true;
        Focusable = true;
        TextAlignment = TextAlignment.Center;
    }

    private static void OnHotkeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is HotkeyControl control)
        {
            control.Text = e.NewValue as string ?? "[Kein Hotkey]";
        }
    }

    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        e.Handled = true;

        // Escape, Backspace oder Delete löschen den Hotkey
        if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete)
        {
            Hotkey = null;
            return;
        }

        // Reine Steuerungstasten ignorieren
        var key = e.Key == Key.System ? e.SystemKey : e.Key;
        if (key == Key.LeftCtrl || key == Key.RightCtrl ||
            key == Key.LeftAlt || key == Key.RightAlt ||
            key == Key.LeftShift || key == Key.RightShift ||
            key == Key.LWin || key == Key.RWin)
        {
            return;
        }

        var sb = new StringBuilder();

        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control)) sb.Append("Ctrl+");
        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Alt)) sb.Append("Alt+");
        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift)) sb.Append("Shift+");

        sb.Append(key.ToString());

        Hotkey = sb.ToString();
    }
}