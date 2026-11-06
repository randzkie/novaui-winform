---
title: NovaProgressBar
description: Progress bar with determinate and marquee modes plus optional percent text.
---

# NovaProgressBar

A custom-drawn progress bar with two modes: **determinate** (0-100%) and
**marquee** (indeterminate sliding block).

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `Minimum` / `Maximum` | `int` | `0` / `100` | range |
| `Value` | `int` | `0` | current progress (clamped to range) |
| `Marquee` | `bool` | `false` | indeterminate animation mode |
| `MarqueeSpeed` | `int` | `25` | timer interval in ms (min 10) |
| `ShowPercent` | `bool` | `false` | draw "NN%" centered on the bar |
| `CornerRadius` | `int` | `8` | corner radius in px |
| `BarColor` | `Color` | empty | fill color (empty = `NovaTheme.Accent`) |

## C#

```csharp
var upload = new NovaProgressBar();
upload.SetBounds(24, 24, 380, 18);
upload.ShowPercent = true;

var busy = new NovaProgressBar();   // indeterminate
busy.Marquee = true;
busy.SetBounds(24, 52, 380, 12);

// feed it from real work
for (int i = 0; i <= 100; i++)
{
    upload.Value = i;               // safe to call from a loop / worker callback
    // do chunk of work...
}
```

## VB.NET

```vb
Dim upload As New NovaProgressBar()
upload.SetBounds(24, 24, 380, 18)
upload.ShowPercent = True

Dim busy As New NovaProgressBar()  ' indeterminate
busy.Marquee = True
busy.SetBounds(24, 52, 380, 12)

For i As Integer = 0 To 100
    upload.Value = i                ' safe to call from a loop / worker callback
Next
```

## Notes

- `Value` is clamped between `Minimum` and `Maximum`, so out-of-range assignments
  never throw.
- The marquee runs on an internal `Timer` and stops when the control is disposed.
- Use `BarColor = NovaTheme.Success` for "completed" bars, `NovaTheme.Danger` for
  failures.
