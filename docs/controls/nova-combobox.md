# NovaComboBox

A flat, rounded, owner-drawn dropdown. The closed control repaints its whole face (rounded fill, selected text, custom chevron), and the open list repaints each row with a soft hover highlight.

- **Base class:** `System.Windows.Forms.ComboBox`
- **Namespace:** `NovaUI.WinForms`
- **Default event:** `SelectedIndexChanged`
- **Defaults set by the constructor:** `DropDownStyle = DropDownList`, `DrawMode = OwnerDrawFixed`, `ItemHeight = 24`

## Quick start (C#)

```csharp
var roleCombo = new NovaComboBox();
roleCombo.Location = new Point(24, 92);
roleCombo.Size = new Size(280, 34);
roleCombo.Items.AddRange(new object[] { "Instructor", "Student", "Admin" });
roleCombo.SelectedIndex = 0;
roleCombo.SelectedIndexChanged += (s, e) =>
    Console.WriteLine("Role: " + roleCombo.SelectedItem);
this.Controls.Add(roleCombo);
```

## Quick start (Visual Basic)

```vb
Dim roleCombo As New NovaComboBox()
roleCombo.Location = New Point(24, 92)
roleCombo.Size = New Size(280, 34)
roleCombo.Items.AddRange(New Object() {"Instructor", "Student", "Admin"})
roleCombo.SelectedIndex = 0
AddHandler roleCombo.SelectedIndexChanged,
    Sub(s, e) Console.WriteLine("Role: " & roleCombo.SelectedItem)
Me.Controls.Add(roleCombo)
```

## Working with custom objects

Because it inherits from `ComboBox`, `DisplayMember`/`ValueMember` data binding works normally:

**C#**

```csharp
var students = new List<Student>
{
    new Student { Id = 1, FullName = "Juan Dela Cruz" },
    new Student { Id = 2, FullName = "Maria Santos" }
};

var studentCombo = new NovaComboBox();
studentCombo.DataSource = students;
studentCombo.DisplayMember = "FullName";
studentCombo.ValueMember = "Id";

int selectedId = (int)studentCombo.SelectedValue;
```

**Visual Basic**

```vb
Dim students As New List(Of Student) From {
    New Student With {.Id = 1, .FullName = "Juan Dela Cruz"},
    New Student With {.Id = 2, .FullName = "Maria Santos"}
}

Dim studentCombo As New NovaComboBox()
studentCombo.DataSource = students
studentCombo.DisplayMember = "FullName"
studentCombo.ValueMember = "Id"

Dim selectedId As Integer = CInt(studentCombo.SelectedValue)
```

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `CornerRadius` | `int` | `8` | Corner radius (0 = square) |
| `FillColor` | `Color` | `NovaTheme.BackColor` | Closed-control and list-row background |
| `BorderColor` | `Color` | `NovaTheme.BorderColor` | Border when unfocused |
| `FocusBorderColor` | `Color` | `NovaTheme.Accent` | Border when focused/dropped down |
| `ItemHoverColor` | `Color` | `#EEF0FC` | Row highlight under the pointer in the open list |
| `ArrowColor` | `Color` | `NovaTheme.MutedColor` | Chevron color |
| `ForeColor` | `Color` | `NovaTheme.TextColor` | Text color (inherited from `ComboBox`) |

Everything from `ComboBox` is still available: `Items`, `SelectedItem`, `SelectedIndex`, `DataSource`, `Sorted`, `SelectedIndexChanged`, `TextChanged`, ...

## Events

| Event | When it fires |
| --- | --- |
| `SelectedIndexChanged` | A different item is selected |
| `DropDown` / `DropDownClosed` | The list opens/closes (face repaints automatically) |
| `GotFocus` / `LostFocus` | Border switches to `FocusBorderColor` |

## Notes

- Best used with `DropDownStyle = DropDownList` (the default). Editable mode works, but the native edit box paints over the styled face, so visual polish is reduced.
- Row height is `ItemHeight` (24 by default); increase it together with `Font` for larger text.
