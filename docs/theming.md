# Theming with NovaTheme

Every NovaUI control reads its default colors from the static `NovaTheme` class. Change a value there and the whole application follows; change a control's own property to override just that one control.

## The palette

| Property | Default (light) | Used for |
| --- | --- | --- |
| `Accent` | `#5865F2` | Buttons, focus borders, progress fill, links |
| `AccentHover` | `#4752C4` | Hover states |
| `BackColor` | `White` | Window / control background |
| `SurfaceColor` | `#F6F7F9` | Cards, panels, progress track |
| `TextColor` | `#171A21` | Primary text |
| `MutedColor` | `#7A808C` | Secondary text, arrows |
| `BorderColor` | `#D6DBE4` | 1px borders |
| `SuccessColor` | `#35B273` | Positive states |
| `DangerColor` | `#EB455E` | Destructive actions |
| `WarningColor` | `#F0A62C` | Warning states |

## Switch to dark mode and back

**C#**

```csharp
private void ToggleTheme()
{
    bool isLight = NovaTheme.BackColor.R >= 128;

    if (isLight)
        NovaTheme.ApplyDark();
    else
        NovaTheme.ApplyLight();

    // Existing controls keep their assigned colors, so re-apply the palette
    // to plain surfaces (forms, tab pages). NovaUI controls pick the theme
    // up the next time they are created; see the note below.
    BackColor = NovaTheme.BackColor;
}
```

**Visual Basic**

```vb
Private Sub ToggleTheme()
    Dim isLight As Boolean = NovaTheme.BackColor.R >= 128

    If isLight Then
        NovaTheme.ApplyDark()
    Else
        NovaTheme.ApplyLight()
    End If

    BackColor = NovaTheme.BackColor
End Sub
```

## Custom accent (brand colors)

**C#**

```csharp
NovaTheme.Accent = Color.FromArgb(0, 122, 204);      // VS blue
NovaTheme.AccentHover = Color.FromArgb(0, 100, 175);
NovaTheme.SurfaceColor = Color.FromArgb(245, 245, 245);

// New controls pick these up automatically:
var saveButton = new NovaButton();   // will paint in the new accent
```

**Visual Basic**

```vb
NovaTheme.Accent = Color.FromArgb(0, 122, 204)      ' VS blue
NovaTheme.AccentHover = Color.FromArgb(0, 100, 175)
NovaTheme.SurfaceColor = Color.FromArgb(245, 245, 245)

Dim saveButton As New NovaButton()   ' will paint in the new accent
```

## Per-control overrides

Any control can opt out of the shared palette by setting its own colors - useful for danger buttons or status bars:

**C#**

```csharp
var deleteButton = new NovaButton();
deleteButton.Text = "Delete";
deleteButton.AccentColor = NovaTheme.DangerColor;
deleteButton.HoverColor = Color.FromArgb(200, 50, 70);

var statusBar = new NovaPanel();
statusBar.FillColor = Color.FromArgb(32, 34, 39);
```

**Visual Basic**

```vb
Dim deleteButton As New NovaButton()
deleteButton.Text = "Delete"
deleteButton.AccentColor = NovaTheme.DangerColor
deleteButton.HoverColor = Color.FromArgb(200, 50, 70)

Dim statusBar As New NovaPanel()
statusBar.FillColor = Color.FromArgb(32, 34, 39)
```

## Tips

- `NovaTheme` values are applied when a control is **constructed**. If you change the theme at runtime and want already-created controls to follow, either recreate them or set their color properties in a loop.
- Keep one place (e.g. `AppTheme.Apply()` method) that configures `NovaTheme` at startup so the palette is predictable.
