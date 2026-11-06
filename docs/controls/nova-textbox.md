# NovaTextBox

A rounded, single-line text box with placeholder text and an accent border on focus. It hosts a real `TextBox` internally, so IME input, clipboard, undo and the native context menu all behave exactly like the stock control.

- **Base class:** `System.Windows.Forms.UserControl`
- **Namespace:** `NovaUI.WinForms`
- **Default event:** `TextChanged`

## Quick start (C#)

```csharp
var nameBox = new NovaTextBox();
nameBox.Placeholder = "Juan Dela Cruz";
nameBox.Location = new Point(24, 48);
nameBox.Size = new Size(280, 34);
nameBox.TextChanged += (s, e) => Console.WriteLine(nameBox.Text);
this.Controls.Add(nameBox);
```

## Quick start (Visual Basic)

```vb
Dim nameBox As New NovaTextBox()
nameBox.Placeholder = "Juan Dela Cruz"
nameBox.Location = New Point(24, 48)
nameBox.Size = New Size(280, 34)
AddHandler nameBox.TextChanged, Sub(s, e) Console.WriteLine(nameBox.Text)
Me.Controls.Add(nameBox)
```

## Handling input in VB with `Handles`

Controls placed with the designer can use the classic `Handles` pattern:

```vb
Private Sub NameBox_TextChanged(sender As Object, e As EventArgs) Handles NameBox.TextChanged
    StatusLabel.Text = "Typing: " & NameBox.Text
End Sub

Private Sub NameBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles NameBox.KeyPress
    If e.KeyChar = ChrW(13) Then          ' Enter pressed
        e.Handled = True
        SaveRecord()
    End If
End Sub
```

## Reading / writing the text

**C#**

```csharp
string name = nameBox.Text;        // read
nameBox.Text = "";                 // clear
nameBox.FocusInput();              // move keyboard focus to the inner box
```

**Visual Basic**

```vb
Dim name As String = nameBox.Text  ' read
nameBox.Text = ""                  ' clear
nameBox.FocusInput()               ' move keyboard focus to the inner box
```

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | Current text |
| `Placeholder` | `string` | `""` | Gray hint shown while empty and unfocused |
| `CornerRadius` | `int` | `8` | Corner radius (0 = square) |
| `BorderColor` | `Color` | `NovaTheme.BorderColor` | Border when unfocused |
| `FocusColor` | `Color` | `NovaTheme.Accent` | Border when focused (drawn slightly thicker) |
| `FillColor` | `Color` | `NovaTheme.BackColor` | Background fill |
| `ReadOnly` | `bool` | `false` | Prevent editing (selection/copy still allowed) |
| `MaxLength` | `int` | `32767` | Maximum character count |

## Events

| Event | When it fires |
| --- | --- |
| `TextChanged` | Text content changed |
| `KeyDown` | Physical key pressed (forwarded from the inner box) |
| `KeyPress` | Character key pressed (forwarded from the inner box) |

## Notes

- Keep the height at ~34px for the standard font; the inner text box is vertically centered automatically.
- `ReadOnly` tints the fill slightly gray to signal the state.
- Pressing `Tab` moves focus normally; the border switches to `FocusColor` while focused.
