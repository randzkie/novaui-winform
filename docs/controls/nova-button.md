---
title: NovaButton
description: Rounded button with filled and outline styles, hover states and per-button accent colors.
---

# NovaButton

A custom-drawn rounded button. Two built-in styles (`Filled`, `Outline`), hover /
pressed / focus visuals, and an optional per-button accent color.

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `Style` | `NovaButtonStyle` | `Filled` | `Filled` solid accent, `Outline` bordered |
| `CornerRadius` | `int` | `8` | corner radius in px (0 = square) |
| `TextAlign` | `ContentAlignment` | `MiddleCenter` | label alignment |
| `Accent` | `Color` | empty | override the theme accent (empty = follow theme) |
| `ForeColor` | `Color` | empty | text color override |

Plus everything inherited from `Control` (`Text`, `Font`, `Enabled`, ...). A
`PerformClick()` method raises `Click` from code.

## C#

```csharp
var save = new NovaButton();
save.Text = "Save record";
save.Size = new Size(140, 36);
save.Click += (s, e) => SaveRecord();

var cancel = new NovaButton();
cancel.Text = "Cancel";
cancel.Style = NovaButtonStyle.Outline;   // ghost button
cancel.SetBounds(160, 0, 120, 36);

var delete = new NovaButton();
delete.Text = "Delete";
delete.Accent = NovaTheme.Danger;         // one-off red button
delete.Location = new Point(0, 46);
```

## VB.NET

```vb
Dim save As New NovaButton()
save.Text = "Save record"
save.Size = New Drawing.Size(140, 36)
AddHandler save.Click, Sub(s, e) SaveRecord()

Dim cancel As New NovaButton()
cancel.Text = "Cancel"
cancel.Style = NovaButtonStyle.Outline   ' ghost button
cancel.SetBounds(160, 0, 120, 36)

Dim delete As New NovaButton()
delete.Text = "Delete"
delete.Accent = NovaTheme.Danger         ' one-off red button
delete.Location = New Point(0, 46)
```

## Notes

- The text color is chosen automatically (white on filled, theme text on outline);
  set `ForeColor` to force your own.
- Buttons subscribe to `NovaTheme.ThemeChanged`, so theme switches repaint them live.
