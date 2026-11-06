# NovaLabel

A custom-drawn label with crisp GDI text, an optional accent dot prefix, and correct `AutoSize` support.

- **Base class:** `System.Windows.Forms.Control`
- **Namespace:** `NovaUI.WinForms`
- **Default property:** `Text`

## Quick start (C#)

```csharp
var heading = new NovaLabel();
heading.Text = "Enrollment summary";
heading.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
heading.Location = new Point(24, 20);
this.Controls.Add(heading);

var status = new NovaLabel();
status.Text = "All records saved";
status.ShowDot = true;                       // small colored dot before the text
status.DotColor = NovaTheme.SuccessColor;
status.Location = new Point(24, 52);
this.Controls.Add(status);
```

## Quick start (Visual Basic)

```vb
Dim heading As New NovaLabel()
heading.Text = "Enrollment summary"
heading.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
heading.Location = New Point(24, 20)
Me.Controls.Add(heading)

Dim status As New NovaLabel()
status.Text = "All records saved"
status.ShowDot = True                        ' small colored dot before the text
status.DotColor = NovaTheme.SuccessColor
status.Location = New Point(24, 52)
Me.Controls.Add(status)
```

## Status-dot pattern

The accent dot is handy for list rows and status lines:

**C#**

```csharp
void SetStatus(NovaLabel label, string text, Color state)
{
    label.Text = text;
    label.DotColor = state;
}

SetStatus(statusLabel, "Connected", NovaTheme.SuccessColor);
SetStatus(statusLabel, "Reconnecting...", NovaTheme.WarningColor);
SetStatus(statusLabel, "Offline", NovaTheme.DangerColor);
```

**Visual Basic**

```vb
Sub SetStatus(label As NovaLabel, text As String, state As Color)
    label.Text = text
    label.DotColor = state
End Sub

SetStatus(statusLabel, "Connected", NovaTheme.SuccessColor)
SetStatus(statusLabel, "Reconnecting...", NovaTheme.WarningColor)
SetStatus(statusLabel, "Offline", NovaTheme.DangerColor)
```

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | The label text |
| `ShowDot` | `bool` | `false` | Draw a small dot before the text |
| `DotColor` | `Color` | `NovaTheme.Accent` | Dot color |
| `AutoEllipsis` | `bool` | `true` | Truncate overflow with "..." |
| `TextAlign` | `ContentAlignment` | `MiddleLeft` | Text alignment |
| `ForeColor` | `Color` | `NovaTheme.TextColor` | Text color (inherited from `Control`) |

## Notes

- `AutoSize` works: the control measures its text with `TextRenderer` and grows to fit.
- Default `BackColor` is transparent, so it sits cleanly on `NovaPanel` cards and form backgrounds.
