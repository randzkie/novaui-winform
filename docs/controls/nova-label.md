---
title: NovaLabel
description: Label with an optional accent dot and theme-aware colors.
---

# NovaLabel

A label with an optional colored accent dot in front of the text - handy for
section titles, status legends and list headers.

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | label text |
| `AccentDot` | `bool` | `false` | draw the dot |
| `DotColor` | `Color` | empty | dot color (empty = `NovaTheme.Accent`) |
| `DotSize` | `int` | `8` | dot diameter in px |
| `TextAlign` | `ContentAlignment` | `MiddleLeft` | text alignment |
| `AutoSize` | `bool` | `true` | grow to fit text + dot |

## C#

```csharp
var section = new NovaLabel();
section.Text = "Vital signs";
section.AccentDot = true;
section.Font = new Font("Segoe UI Semibold", 12f);
section.Location = new Point(20, 20);

var status = new NovaLabel();
status.Text = "Synced";
status.AccentDot = true;
status.DotColor = NovaTheme.Success;
status.Location = new Point(20, 50);
```

## VB.NET

```vb
Dim section As New NovaLabel()
section.Text = "Vital signs"
section.AccentDot = True
section.Font = New Drawing.Font("Segoe UI Semibold", 12.0F)
section.Location = New Point(20, 20)

Dim status As New NovaLabel()
status.Text = "Synced"
status.AccentDot = True
status.DotColor = NovaTheme.Success
status.Location = New Point(20, 50)
```

## Notes

- `AutoSize = true` accounts for the dot width; the control resizes as text changes.
- `ForeColor` overrides the theme text color for warnings/annotations.
