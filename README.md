# NovaUI.WinForms

Clean-room WinForms UI library for our own projects - flat GDI+ controls inspired by the layout conventions studied in the reverse-engineering exercise, written from scratch with zero third-party dependencies.

**Targets:** .NET Framework **4.7.2** / 4.8 and **.NET 8** (multi-targeted from identical C# 7.3 sources, so everything works on 4.7.2).

## Controls (7)

| Control | Base | Highlights |
| --- | --- | --- |
| `NovaButton` | Control | Flat rounded button, filled/outline/danger looks, hover+press states |
| `NovaPanel` | Panel | Rounded card with optional title/border, child clipping via Region |
| `NovaTextBox` | UserControl | Rounded text box, placeholder, focus accent, native editing behavior |
| `NovaLabel` | Control | Crisp text label, optional accent dot, AutoSize support |
| `NovaComboBox` | ComboBox | Owner-drawn flat dropdown, rounded border, custom chevron and row highlight |
| `NovaProgressBar` | Control | Rounded bar, marquee mode, Percent/Value captions, ValueChanged event |
| `NovaHtmlViewer` | UserControl | HTML viewer ("PDF viewer, but for HTML"): string/file/URL content, **custom links bar**, click interception, open-externally, print + print preview |

Supporting types: `NovaTheme` (shared palette + dark mode), `NovaHtmlLink`, `NovaHtmlLinkEventArgs`, `NovaLinksBarPosition`, `NovaProgressTextDisplay`.

## Build

```bash
dotnet build NovaUI.WinForms/NovaUI.WinForms.csproj -c Release
dotnet build TestApp/TestApp.csproj -c Release
```

Produces both `net472` and `net8.0-windows` DLLs. On Linux, the net472 build uses the `Microsoft.NETFramework.ReferenceAssemblies` package (already referenced in the csproj).

`TestApp` is a tabbed demo form exercising every control, including the HTML viewer with custom links and print buttons.

## Prebuilt DLLs

- `NovaUI.WinForms/dist/net472/NovaUI.WinForms.dll` - reference this from .NET Framework 4.7.2/4.8 projects
- `NovaUI.WinForms/dist/net8.0-windows/NovaUI.WinForms.dll` - reference this from .NET 8 projects

## Documentation

Full per-control documentation with **C# and Visual Basic examples**, GitBook-ready:

- [`docs/README.md`](docs/README.md) - home + control catalog
- [`docs/SUMMARY.md`](docs/SUMMARY.md) - GitBook table of contents (drop `docs/` into GitBook as-is)
- [`docs/getting-started.md`](docs/getting-started.md) - referencing the DLL, first window (C# + VB)
- [`docs/theming.md`](docs/theming.md) - NovaTheme palette, dark mode
- `docs/controls/*.md` - one page per control: quick start (C# + VB), properties, events, methods, notes

## Verification (RE toolkit)

`tools/analyze_guna.py` (dnfile) against the built net472 DLL:

- TFM attribute: `.NETFramework,Version=v4.7.2`
- 15 types, 257 methods, 97 fields, 77 properties
- 7 controls with bases: Control x3, Panel, ComboBox, UserControl x2
- References only in-box assemblies: mscorlib, System, System.Drawing, System.Windows.Forms

## Notes

- Clean-room: no target code was copied; the library is written from a behavioral spec and verified by metadata analysis only.
- `LangVersion` is pinned to C# 7.3 so net472 and net8 compile from identical sources.
