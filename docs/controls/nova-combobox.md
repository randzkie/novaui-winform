---
title: NovaComboBox
description: Owner-drawn dropdown with a themed popup list and keyboard support.
---

# NovaComboBox

An owner-drawn dropdown. The closed state is a rounded field with a chevron; opening
it shows a themed popup list (max 8 rows visible, then the list scrolls).

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `Items` | `List<object>` | empty | the item list (`AddItem`, `ClearItems`) |
| `SelectedIndex` | `int` | `-1` | index of the selected item |
| `SelectedItem` | `object` | `null` | the selected item itself |
| `Placeholder` | `string` | `""` | hint shown when nothing is selected |
| `CornerRadius` | `int` | `8` | corner radius in px |

Event: `SelectedIndexChanged`.

## C#

```csharp
var program = new NovaComboBox();
program.Placeholder = "Select program...";
program.SetBounds(24, 24, 260, 34);
program.Items.Add("Cardio basics");
program.Items.Add("Strength level 1");
program.Items.Add("Strength level 2");
program.Items.Add("Rehabilitation");

program.SelectedIndexChanged += (s, e) =>
{
    string chosen = Convert.ToString(program.SelectedItem);
    MessageBox.Show("Program: " + chosen);
};
```

## VB.NET

```vb
Dim program As New NovaComboBox()
program.Placeholder = "Select program..."
program.SetBounds(24, 24, 260, 34)
program.Items.Add("Cardio basics")
program.Items.Add("Strength level 1")
program.Items.Add("Strength level 2")
program.Items.Add("Rehabilitation")

AddHandler program.SelectedIndexChanged,
    Sub(s, e) MessageBox.Show("Program: " & Convert.ToString(program.SelectedItem))
```

## Notes

- Keyboard: `Up`/`Down` change the selection, `Enter` opens/closes the dropdown,
  `Esc` closes it.
- The popup closes automatically when it loses focus (click outside).
- `ClearItems()` also resets `SelectedIndex` to `-1`.
