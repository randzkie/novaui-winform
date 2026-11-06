# NovaButton

A flat, rounded button painted entirely with GDI+. It has built-in hover and press states and supports two looks: **filled** (default) and **outline** (ghost).

- **Base class:** `System.Windows.Forms.Control`
- **Namespace:** `NovaUI.WinForms`
- **Inherits:** `Click`, `Enabled`, `Font`, and all other standard `Control` members

## Quick start (C#)

```csharp
var saveButton = new NovaButton();
saveButton.Text = "Save changes";
saveButton.Size = new Size(150, 38);
saveButton.Location = new Point(24, 24);
saveButton.Click += (s, e) => Save();
this.Controls.Add(saveButton);
```

## Quick start (Visual Basic)

```vb
Dim saveButton As New NovaButton()
saveButton.Text = "Save changes"
saveButton.Size = New Size(150, 38)
saveButton.Location = New Point(24, 24)
AddHandler saveButton.Click, Sub(s, e) Save()
Me.Controls.Add(saveButton)
```

## Styles

**Outline (ghost) button**

```csharp
var cancelButton = new NovaButton();
cancelButton.Text = "Cancel";
cancelButton.Outline = true;            // transparent fill, colored border + text
cancelButton.BorderColor = NovaTheme.Accent;
cancelButton.Size = new Size(110, 38);
```

```vb
Dim cancelButton As New NovaButton()
cancelButton.Text = "Cancel"
cancelButton.Outline = True             ' transparent fill, colored border + text
cancelButton.BorderColor = NovaTheme.Accent
cancelButton.Size = New Size(110, 38)
```

**Danger button**

```csharp
var deleteButton = new NovaButton();
deleteButton.Text = "Delete";
deleteButton.AccentColor = NovaTheme.DangerColor;
deleteButton.HoverColor = Color.FromArgb(200, 50, 70);
```

```vb
Dim deleteButton As New NovaButton()
deleteButton.Text = "Delete"
deleteButton.AccentColor = NovaTheme.DangerColor
deleteButton.HoverColor = Color.FromArgb(200, 50, 70)
```

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | Caption shown on the button |
| `CornerRadius` | `int` | `10` | Corner radius in pixels (0 = square) |
| `AccentColor` | `Color` | `NovaTheme.Accent` | Fill color in filled mode; border+text color in outline mode |
| `HoverColor` | `Color` | `NovaTheme.AccentHover` | Fill color while hovered (filled mode) |
| `TextColor` | `Color` | `White` | Caption color in filled mode |
| `BorderColor` | `Color` | `NovaTheme.Accent` | Border color in outline mode |
| `BorderWidth` | `float` | `1.5` | Border thickness in outline mode |
| `Outline` | `bool` | `false` | `true` = ghost look (transparent fill) |
| `TextAlign` | `ContentAlignment` | `MiddleCenter` | Caption alignment |

## Events

| Event | When it fires |
| --- | --- |
| `Click` | The button is clicked (keyboard `Enter`/`Space` included) |
| `MouseEnter` / `MouseLeave` | Pointer enters/leaves - hover state repaints automatically |
| `EnabledChanged` | The button is enabled/disabled (repaints grayed out) |

## Notes

- The button repaints itself on hover/press - you never need to handle `Paint` manually.
- Disabled buttons automatically render in gray.
- Because it derives from `Control` (not `Button`), it never shows the Windows focus rectangle or native chrome - that is intentional.
