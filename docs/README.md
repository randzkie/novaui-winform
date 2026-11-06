# NovaUI.WinForms Documentation

Welcome to the **NovaUI.WinForms** documentation - the complete guide to every control in the library, with working examples in **C#** and **Visual Basic .NET**.

NovaUI.WinForms is a small, clean-room UI library for Windows Forms. Every control is painted with plain GDI+ (no images, no third-party dependencies), ships as a single DLL, and targets **.NET Framework 4.7.2/4.8** and **.NET 8** from identical source code.

## Control catalog

| Control | Base class | What it does |
| --- | --- | --- |
| [NovaButton](controls/nova-button.md) | `Control` | Flat rounded button with hover/press states, filled and outline styles |
| [NovaPanel](controls/nova-panel.md) | `Panel` | Rounded card container with optional title and border |
| [NovaTextBox](controls/nova-textbox.md) | `UserControl` | Rounded text box with placeholder text and focus accent |
| [NovaLabel](controls/nova-label.md) | `Control` | Custom-drawn label with optional accent dot |
| [NovaComboBox](controls/nova-combobox.md) | `ComboBox` | Flat owner-drawn dropdown with rounded border and custom rows |
| [NovaProgressBar](controls/nova-progressbar.md) | `Control` | Rounded progress bar with marquee mode and captions |
| [NovaHtmlViewer](controls/nova-html-viewer.md) | `UserControl` | HTML document viewer (like a PDF viewer, but for HTML) with custom links and printing |

Supporting types: [`NovaTheme`](theming.md) (shared palette), `NovaHtmlLink`, `NovaHtmlLinkEventArgs`, `NovaLinksBarPosition`, `NovaProgressTextDisplay`.

## Requirements

- Windows 7 or newer
- .NET Framework **4.7.2** or 4.8, **or** .NET 8 with the Windows Desktop runtime
- Visual Studio 2017+ / 2022 for designer support (optional - everything can be done in code)

## Where to start

1. [Getting started](getting-started.md) - reference the DLL and build your first window (C# and VB).
2. [Theming](theming.md) - recolor the whole app from one place with `NovaTheme`.
3. Jump to any control page from the catalog above.

## Sample

```csharp
var button = new NovaButton();
button.Text = "Save changes";
button.Location = new Point(24, 24);
this.Controls.Add(button);
```

```vb
Dim button As New NovaButton()
button.Text = "Save changes"
button.Location = New Point(24, 24)
Me.Controls.Add(button)
```
