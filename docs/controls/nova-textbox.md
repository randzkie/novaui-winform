---
title: NovaTextBox
description: Single-line rounded textbox with placeholder, password and read-only modes.
---

# NovaTextBox

A single-line, rounded text box with built-in placeholder support. The real input
is a borderless `TextBox` hosted inside a custom-drawn rounded shell, so copy/paste,
IME and keyboard behavior are native.

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | current text (proxied to the inner TextBox) |
| `Placeholder` | `string` | `""` | hint drawn when empty and unfocused |
| `CornerRadius` | `int` | `8` | corner radius in px |
| `ReadOnly` | `bool` | `false` | blocks editing |
| `PasswordChar` | `char` | `\0` | mask character (e.g. `'*'`) |
| `MaxLength` | `int` | `0` | max characters (0 = unlimited) |
| `BorderColor` | `Color` | empty | outline color override |

Events like `TextChanged` behave exactly like a normal `TextBox`.

## C#

```csharp
var patient = new NovaTextBox();
patient.Placeholder = "Patient name";
patient.SetBounds(24, 24, 260, 34);

var pin = new NovaTextBox();
pin.Placeholder = "4-digit PIN";
pin.PasswordChar = '*';
pin.MaxLength = 4;
pin.SetBounds(24, 66, 120, 34);

patient.TextChanged += (s, e) =>
    Console.WriteLine("Patient: " + patient.Text);
```

## VB.NET

```vb
Dim patient As New NovaTextBox()
patient.Placeholder = "Patient name"
patient.SetBounds(24, 24, 260, 34)

Dim pin As New NovaTextBox()
pin.Placeholder = "4-digit PIN"
pin.PasswordChar = "*"c
pin.MaxLength = 4
pin.SetBounds(24, 66, 120, 34)

AddHandler patient.TextChanged,
    Sub(s, e) Console.WriteLine("Patient: " & patient.Text)
```

## Notes

- The border switches to `NovaTheme.Accent` while the control has focus.
- Single-line by design; for multi-line notes use a `TextBox` inside a `NovaPanel`.
- `BackColor` is the card fill (default `NovaTheme.Surface`) - change it for
  read-only / highlighted states.
