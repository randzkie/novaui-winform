---
title: Theming
description: The NovaTheme palette, dark mode, custom accents and per-control overrides.
---

# Theming

All Nova controls read their colors from the static `NovaTheme` palette, so the whole
app changes look with one call. The current palette:

| Token | Light | Dark | Used for |
| --- | --- | --- | --- |
| `Background` | `#F3F5F9` | `#10141C` | form / page background |
| `Surface` | `#FFFFFF` | `#171C26` | cards, inputs, popups |
| `Text` | `#1B2432` | `#E8EBF2` | primary text |
| `TextMuted` | `#6B7280` | `#8A93A6` | placeholders, hints |
| `Border` | `#D7DCE5` | `#262D3B` | outlines, dividers |
| `Accent` | `#4F6BFF` | `#6C86FF` | buttons, progress, focus ring |
| `AccentHover` | `#3D57E0` | `#8598FF` | pressed / hover accent |
| `Success` | `#2FA36B` | `#3DD68C` | positive states |
| `Danger` | `#E5484D` | `#FF6369` | destructive buttons |

## Switching theme (C#)

```csharp
// at startup (light is the default)
NovaTheme.ApplyLight();

// when the user toggles dark mode
darkModeCheckbox.CheckedChanged += (s, e) =>
{
    if (darkModeCheckbox.Checked) NovaTheme.ApplyDark();
    else NovaTheme.ApplyLight();
    // every live Nova control repaints automatically - no extra code
};
```

## Switching theme (VB.NET)

```vb
AddHandler darkModeCheckbox.CheckedChanged,
    Sub(s, e)
        If darkModeCheckbox.Checked Then
            NovaTheme.ApplyDark()
        Else
            NovaTheme.ApplyLight()
        End If
    End Sub
```

## Custom accents (C#)

```csharp
NovaTheme.Accent = Color.FromArgb(0, 150, 136);      // teal brand color
NovaTheme.AccentHover = Color.FromArgb(0, 121, 107);
NovaTheme.RaiseThemeChanged();                        // repaint everything
```

## Custom accents (VB.NET)

```vb
NovaTheme.Accent = Drawing.Color.FromArgb(0, 150, 136)
NovaTheme.AccentHover = Drawing.Color.FromArgb(0, 121, 107)
NovaTheme.RaiseThemeChanged()
```

## Per-control overrides

Any control can opt out of the theme:

- `NovaButton.Accent` - a fixed color for one button
- `NovaPanel.CardColor` / `BorderColor`
- `NovaProgressBar.BarColor`
- `NovaLabel.DotColor`
- `NovaHtmlViewer.LinkColor` / `LinkHoverColor`

An empty (`Color.Empty`) value means "follow the theme".

> [!WARNING]
> `NovaTheme` colors are process-wide. Per-control overrides are the right tool for
> one-off colors; changing the palette itself affects every open form.

## Forms and layouts that match the theme

```csharp
public partial class ShellForm : Form
{
    public ShellForm()
    {
        InitializeComponent();
        BackColor = NovaTheme.Background;   // form follows the palette
    }

    void ToggleTheme()
    {
        if (BackColor == NovaTheme.Background) NovaTheme.ApplyDark();
        else NovaTheme.ApplyLight();
        BackColor = NovaTheme.Background;
    }
}
```
