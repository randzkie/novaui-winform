# Getting started

This page walks you through referencing NovaUI.WinForms and building your first window, in both **C#** and **Visual Basic**.

## 1. Add the library to your project

### Option A - compiled DLL (fastest)

1. Copy `NovaUI.WinForms/dist/net472/NovaUI.WinForms.dll` (for .NET Framework 4.7.2/4.8 projects) or `dist/net8.0-windows/NovaUI.WinForms.dll` (for .NET 8 projects) into a `libs` folder of your solution.
2. Add the reference:
   - **C# / VB in Visual Studio:** Solution Explorer > your project > **Add** > **Reference...** > **Browse...** > select the DLL.
   - **C# project file:**
     ```xml
     <Reference Include="NovaUI.WinForms">
       <HintPath>..\libs\NovaUI.WinForms.dll</HintPath>
     </Reference>
     ```
   - **VB project file (.vbproj):**
     ```xml
     <Reference Include="NovaUI.WinForms">
       <HintPath>..\libs\NovaUI.WinForms.dll</HintPath>
     </Reference>
     ```

### Option B - project reference

Add the `NovaUI.WinForms` project to your solution and reference it:

```xml
<!-- C# (.csproj) or VB (.vbproj) - identical syntax -->
<ItemGroup>
  <ProjectReference Include="..\NovaUI.WinForms\NovaUI.WinForms.csproj" />
</ItemGroup>
```

## 2. Drop controls on a form

After adding the reference, the **NovaUI** controls appear in the Toolbox automatically (right-click Toolbox > **Reset** if they do not show). Drag **NovaButton**, **NovaTextBox**, **NovaComboBox**, **NovaProgressBar**, **NovaHtmlViewer**, **NovaPanel** or **NovaLabel** onto the form and set their `Nova`-category properties in the Properties window.

## 3. First window in C#

```csharp
using System;
using System.Drawing;
using System.Windows.Forms;
using NovaUI.WinForms;

namespace DemoApp
{
    public class MainForm : Form
    {
        public MainForm()
        {
            Text = "My first NovaUI window";
            Size = new Size(560, 380);
            BackColor = NovaTheme.BackColor;

            var nameLabel = new NovaLabel();
            nameLabel.Text = "Student name";
            nameLabel.Location = new Point(24, 24);

            var nameBox = new NovaTextBox();
            nameBox.Placeholder = "Juan Dela Cruz";
            nameBox.Location = new Point(24, 48);
            nameBox.Size = new Size(280, 34);

            var courseCombo = new NovaComboBox();
            courseCombo.Location = new Point(24, 92);
            courseCombo.Size = new Size(280, 34);
            courseCombo.Items.AddRange(new object[] { "Programming 101", "Data Structures", "Web Systems" });
            courseCombo.SelectedIndex = 0;

            var saveButton = new NovaButton();
            saveButton.Text = "Enroll";
            saveButton.Location = new Point(24, 140);
            saveButton.Click += (s, e) =>
                MessageBox.Show("Enrolled " + nameBox.Text + " in " + courseCombo.SelectedItem);

            Controls.Add(nameLabel);
            Controls.Add(nameBox);
            Controls.Add(courseCombo);
            Controls.Add(saveButton);
        }
    }
}
```

## 4. First window in Visual Basic

```vb
Imports System.Drawing
Imports System.Windows.Forms
Imports NovaUI.WinForms

Public Class MainForm
    Inherits Form

    Public Sub New()
        Text = "My first NovaUI window"
        Size = New Size(560, 380)
        BackColor = NovaTheme.BackColor

        Dim nameLabel As New NovaLabel()
        nameLabel.Text = "Student name"
        nameLabel.Location = New Point(24, 24)

        Dim nameBox As New NovaTextBox()
        nameBox.Placeholder = "Juan Dela Cruz"
        nameBox.Location = New Point(24, 48)
        nameBox.Size = New Size(280, 34)

        Dim courseCombo As New NovaComboBox()
        courseCombo.Location = New Point(24, 92)
        courseCombo.Size = New Size(280, 34)
        courseCombo.Items.AddRange(New Object() {"Programming 101", "Data Structures", "Web Systems"})
        courseCombo.SelectedIndex = 0

        Dim saveButton As New NovaButton()
        saveButton.Text = "Enroll"
        saveButton.Location = New Point(24, 140)
        AddHandler saveButton.Click,
            Sub(s, e) MessageBox.Show("Enrolled " & nameBox.Text & " in " & courseCombo.SelectedItem)

        Controls.Add(nameLabel)
        Controls.Add(nameBox)
        Controls.Add(courseCombo)
        Controls.Add(saveButton)
    End Sub
End Class
```

## 5. Program entry point

Make sure visual styles are enabled so NovaUI controls render with crisp fonts:

**C#**

```csharp
[STAThread]
static void Main()
{
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    Application.Run(new MainForm());
}
```

**Visual Basic**

```vb
' In a module or MyProject settings (VB enables visual styles by default)
Application.EnableVisualStyles()
Application.Run(New MainForm())
```

## Next steps

- Recolor everything at once in [Theming](theming.md).
- Embed printable HTML documents (forms, reports) with [NovaHtmlViewer](controls/nova-html-viewer.md).
