---
title: NovaPanel
description: Rounded card container with optional border for dashboards and forms.
---

# NovaPanel

A rounded card container. Use it as the visual building block of dashboards and
forms: drop other controls inside and they get the card's padding automatically.

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `CornerRadius` | `int` | `12` | corner radius in px |
| `ShowBorder` | `bool` | `true` | draw the 1px outline |
| `CardColor` | `Color` | empty | card fill (empty = `NovaTheme.Surface`) |
| `BorderColor` | `Color` | empty | outline color (empty = `NovaTheme.Border`) |
| `BackColor` | `Color` | theme background | the "page" color visible at the corners |

## C#

```csharp
var card = new NovaPanel();
card.SetBounds(24, 24, 360, 180);
card.Padding = new Padding(20);

var title = new NovaLabel();
title.Text = "Vital signs";
title.AccentDot = true;
title.Font = new Font("Segoe UI Semibold", 12f);
title.Location = new Point(20, 18);

card.Controls.Add(title);
Controls.Add(card);
```

## VB.NET

```vb
Dim card As New NovaPanel()
card.SetBounds(24, 24, 360, 180)
card.Padding = New Padding(20)

Dim title As New NovaLabel()
title.Text = "Vital signs"
title.AccentDot = True
title.Font = New Drawing.Font("Segoe UI Semibold", 12.0F)
title.Location = New Point(20, 18)

card.Controls.Add(title)
Controls.Add(card)
```

## Notes

- `NovaPanel` inherits from `Panel`, so all layout features (docking, anchoring,
  auto-scroll) work as usual.
- The rounded corners are painted; child controls still have rectangular bounds,
  so keep children a few px away from the corners.
