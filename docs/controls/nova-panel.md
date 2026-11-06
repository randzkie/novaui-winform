# NovaPanel

A rounded card container. Children are clipped to the rounded shape through a `Region`, so nothing ever paints over the corners. Optionally shows a title line and a 1px border.

- **Base class:** `System.Windows.Forms.Panel`
- **Namespace:** `NovaUI.WinForms`
- **Inherits:** everything from `Panel`, including `AutoScroll`, `DockPadding`, and normal container behavior

## Quick start (C#)

```csharp
var card = new NovaPanel();
card.Size = new Size(380, 220);
card.Location = new Point(24, 24);
card.ShowTitle = true;
card.Title = "Employee";
card.ShowBorder = true;

var nameBox = new NovaTextBox();
nameBox.Location = new Point(16, 48);   // inside the card
card.Controls.Add(nameBox);

this.Controls.Add(card);
```

## Quick start (Visual Basic)

```vb
Dim card As New NovaPanel()
card.Size = New Size(380, 220)
card.Location = New Point(24, 24)
card.ShowTitle = True
card.Title = "Employee"
card.ShowBorder = True

Dim nameBox As New NovaTextBox()
nameBox.Location = New Point(16, 48)    ' inside the card
card.Controls.Add(nameBox)

Me.Controls.Add(card)
```

## Using it as a form section

**C#**

```csharp
var detailsCard = new NovaPanel();
detailsCard.Dock = DockStyle.Fill;
detailsCard.Padding = new Padding(16);       // default is already 16
detailsCard.FillColor = NovaTheme.SurfaceColor;
detailsCard.CornerRadius = 14;
```

**Visual Basic**

```vb
Dim detailsCard As New NovaPanel()
detailsCard.Dock = DockStyle.Fill
detailsCard.Padding = New Padding(16)        ' default is already 16
detailsCard.FillColor = NovaTheme.SurfaceColor
detailsCard.CornerRadius = 14
```

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `CornerRadius` | `int` | `12` | Corner radius in pixels (0 = square) |
| `FillColor` | `Color` | `NovaTheme.SurfaceColor` | Card background |
| `BorderColor` | `Color` | `NovaTheme.BorderColor` | Border color (only when `ShowBorder`) |
| `ShowBorder` | `bool` | `false` | Draw a 1px border |
| `Title` | `string` | `""` | Title text (top-left) |
| `TitleColor` | `Color` | `NovaTheme.TextColor` | Title text color |
| `ShowTitle` | `bool` | `false` | Render the title above the content |

## Notes

- Children are clipped to the rounded region; `CornerRadius` is applied on `Resize` automatically.
- The default `Padding` is 16px on all sides, so docked children align with the card edge nicely.
- Use it everywhere you would use a `GroupBox` for a modern look.
