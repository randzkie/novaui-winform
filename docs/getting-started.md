---
title: Getting started
description: Reference the DLL and build your first NovaUI window in C# or VB.NET.
---

# Getting started

## 1. Reference the DLL

Pick the build that matches your project:

- .NET Framework 4.7.2 (or any 4.x) -> `NovaUI.WinForms/dist/net472/NovaUI.WinForms.dll`
- .NET 8 Windows -> `NovaUI.WinForms/dist/net8.0-windows/NovaUI.WinForms.dll`

**Visual Studio:** right-click *References* (or *Dependencies*) -> *Add Reference...* ->
*Browse* -> select the DLL.

**C# project file (.csproj):**

```xml
<ItemGroup>
  <Reference Include="NovaUI.WinForms">
    <HintPath>libs\NovaUI.WinForms.dll</HintPath>
  </Reference>
</ItemGroup>
```

**VB project file (.vbproj):** identical block - the XML is the same for both
languages.

After the first build you can also right-click the Toolbox -> *Choose Items...* ->
*Browse* and drop all Nova controls onto the designer.

## 2. First window (C#)

```csharp
using System;
using System.Windows.Forms;
using NovaUI.WinForms;

public class MainForm : Form
{
    public MainForm()
    {
        Text = "My first Nova window";
        Size = new System.Drawing.Size(480, 320);
        BackColor = NovaTheme.Background;

        var panel = new NovaPanel();
        panel.SetBounds(24, 24, 400, 200);

        var label = new NovaLabel();
        label.Text = "Welcome to NovaUI";
        label.AccentDot = true;
        label.SetBounds(20, 20, 260, 28);

        var button = new NovaButton();
        button.Text = "Say hello";
        button.SetBounds(20, 60, 120, 36);
        button.Click += (s, e) =>
            MessageBox.Show("Hello from " + label.Text);

        panel.Controls.Add(label);
        panel.Controls.Add(button);
        Controls.Add(panel);
    }
}

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm());
    }
}
```

## 3. First window (VB.NET)

```vb
Imports System.Windows.Forms
Imports NovaUI.WinForms

Public Class MainForm
    Inherits Form

    Public Sub New()
        Text = "My first Nova window"
        Size = New Drawing.Size(480, 320)
        BackColor = NovaTheme.Background

        Dim panel As New NovaPanel()
        panel.SetBounds(24, 24, 400, 200)

        Dim label As New NovaLabel()
        label.Text = "Welcome to NovaUI"
        label.AccentDot = True
        label.SetBounds(20, 20, 260, 28)

        Dim button As New NovaButton()
        button.Text = "Say hello"
        button.SetBounds(20, 60, 120, 36)
        AddHandler button.Click, Sub(s, e) MessageBox.Show("Hello from " & label.Text)

        panel.Controls.Add(label)
        panel.Controls.Add(button)
        Controls.Add(panel)
    End Sub
End Class

Module Program
    <STAThread>
    Sub Main()
        Application.EnableVisualStyles()
        Application.Run(New MainForm())
    End Sub
End Module
```

## 4. Design-time tip

Nova controls are code-first but fully usable in the Visual Studio designer: add them
via *Choose Toolbox Items*, set `CornerRadius`, `Placeholder`, `Style` etc. in the
property grid (look for the **Nova** category).

> [!TIP]
> Set `BackColor = NovaTheme.Background` on your forms so the page color follows the
> active theme automatically.

## Next steps

- [Theming](theming.md) - the full palette and dark mode
- [NovaHtmlViewer](controls/nova-html-viewer.md) - HTML forms with parameter injection
