# NovaProgressBar

A rounded progress bar painted with GDI+. Supports a determinate `Value` fill, an animated **marquee** (indeterminate) mode, and an optional caption (percent or value).

- **Base class:** `System.Windows.Forms.Control`
- **Namespace:** `NovaUI.WinForms`
- **Default property:** `Value`

## Quick start (C#)

```csharp
var progress = new NovaProgressBar();
progress.Location = new Point(24, 32);
progress.Size = new Size(420, 20);
progress.Maximum = 100;
progress.Value = 35;
progress.TextDisplay = NovaProgressTextDisplay.Percent;  // draws "35%"
this.Controls.Add(progress);

progress.PerformStep();   // +Step (default 10)
progress.Increment(5);    // +5
```

## Quick start (Visual Basic)

```vb
Dim progress As New NovaProgressBar()
progress.Location = New Point(24, 32)
progress.Size = New Size(420, 20)
progress.Maximum = 100
progress.Value = 35
progress.TextDisplay = NovaProgressTextDisplay.Percent   ' draws "35%"
Me.Controls.Add(progress)

progress.PerformStep()   ' +Step (default 10)
progress.Increment(5)    ' +5
```

## A worker loop with live progress

**C#**

```csharp
progress.Minimum = 0;
progress.Maximum = files.Count;
progress.Value = 0;

foreach (var file in files)
{
    ProcessFile(file);
    progress.Value += 1;
}
```

**Visual Basic**

```vb
progress.Minimum = 0
progress.Maximum = files.Count
progress.Value = 0

For Each file In files
    ProcessFile(file)
    progress.Value += 1
Next
```

## Marquee (indeterminate) mode

Use marquee while you cannot measure the work (connecting, loading):

**C#**

```csharp
var marquee = new NovaProgressBar();
marquee.Size = new Size(420, 20);
marquee.Marquee = true;                    // sliding segment
marquee.MarqueeSpeed = 30;                 // animation interval (ms)
marquee.MarqueeWidth = 0.35f;              // segment length (fraction of track)
marquee.FillColor = NovaTheme.SuccessColor;

// When the work finishes:
marquee.Marquee = false;
```

**Visual Basic**

```vb
Dim marquee As New NovaProgressBar()
marquee.Size = New Size(420, 20)
marquee.Marquee = True                     ' sliding segment
marquee.MarqueeSpeed = 30                  ' animation interval (ms)
marquee.MarqueeWidth = 0.35F               ' segment length (fraction of track)
marquee.FillColor = NovaTheme.SuccessColor

' When the work finishes:
marquee.Marquee = False
```

## Reacting to value changes

**C#**

```csharp
progress.ValueChanged += (s, e) =>
    Console.WriteLine("Progress: " + progress.Value + "/" + progress.Maximum);
```

**Visual Basic**

```vb
AddHandler progress.ValueChanged,
    Sub(s, e) Console.WriteLine("Progress: " & progress.Value & "/" & progress.Maximum)
```

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `Minimum` | `int` | `0` | Lower bound of the range |
| `Maximum` | `int` | `100` | Upper bound of the range |
| `Value` | `int` | `0` | Current position (clamped to the range) |
| `Step` | `int` | `10` | Amount added by `PerformStep()` |
| `Marquee` | `bool` | `false` | Indeterminate sliding-segment mode |
| `MarqueeSpeed` | `int` | `30` | Animation tick interval in ms |
| `MarqueeWidth` | `float` | `0.35` | Segment length as a fraction of the track (0.05-1) |
| `CornerRadius` | `int` | `8` | Corner radius (0 = square) |
| `FillColor` | `Color` | `NovaTheme.Accent` | Progress segment color |
| `TrackColor` | `Color` | `NovaTheme.SurfaceColor` | Unfilled track color |
| `TextDisplay` | `NovaProgressTextDisplay` | `None` | `None`, `Percent` or `Value` |
| `TextColor` | `Color` | `White` | Caption color |

## Methods and events

| Member | Description |
| --- | --- |
| `PerformStep()` | `Value += Step` |
| `Increment(int amount)` | `Value += amount` |
| `ValueChanged` event | Fires after `Value` changes |

## Notes

- `Value` is always clamped between `Minimum` and `Maximum` - no exceptions, no invalid states.
- The marquee timer only runs while the control is visible and not in the designer, so hidden bars cost nothing.
- For captions to stay readable, `TextColor` should contrast with `FillColor` (default white on accent).
