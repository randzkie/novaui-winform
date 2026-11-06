# NovaUI.WinForms

Clean-room WinForms UI library - custom-drawn controls, dark/light theming, and an
HTML viewer with template-parameter injection. Multi-target: **net472** + **net8.0-windows**,
single code base, C# 7.3, zero third-party dependencies.

## Controls (7)

| Control | Highlights |
| --- | --- |
| `NovaButton` | rounded, Filled/Outline, per-button accent, `PerformClick()` |
| `NovaPanel` | rounded card container with border |
| `NovaTextBox` | rounded input, placeholder, password/readonly modes |
| `NovaLabel` | label with accent dot |
| `NovaComboBox` | owner-drawn dropdown with themed popup |
| `NovaProgressBar` | determinate + marquee modes, percent text |
| `NovaHtmlViewer` | HTML viewer with **parameter injection** (`{{token}}`), custom link bar, print dialog/preview |

## Parameter injection (new in 1.3)

```csharp
viewer.Parameters["PatientName"] = "Juan Dela Cruz";
viewer.Parameters["VisitDate"] = "2026-11-06";
viewer.LoadFile("form.html");      // replaces {{PatientName}}, {{VisitDate}} ...
```

- Tokens: `TokenPrefix` / `TokenSuffix` (default `{{` `}}`), case-insensitive
- `HeadInjection` - inject CSS/JS before `</head>`
- `HtmlTransforming` event - rewrite raw HTML, inject JSON, cancel the load
- `LoadUrl` - appends parameters as a query string
- `RefreshDocument()` - re-render after changing parameters

Full guide: `docs/controls/nova-html-viewer.md`.

## GitBook-ready documentation

`docs/` is a complete GitBook space:

```
docs/
├── README.md               # home page (+ banner from assets/nova-banner.svg)
├── SUMMARY.md              # GitBook table of contents
├── getting-started.md      # C# + VB quick start
├── theming.md              # NovaTheme palette & dark mode
├── gitbook-publishing.md   # how to publish & customize pages on GitBook
├── assets/nova-banner.svg
└── controls/               # one page per control, C# + VB examples
```

Every page carries GitBook front matter (`title`, `description`). `.gitbook.yaml`
at the repo root points GitBook to `docs/` when you Git-Sync the whole repo.
See `docs/gitbook-publishing.md` for step-by-step publishing instructions.

## Repository layout

```
winforms-ui-project/
├── .gitbook.yaml           # GitBook config (root: ./docs)
├── NovaUI.WinForms/        # the library (Core/, Controls/, dist/)
├── TestApp/                # 4-tab demo app + sample.html (token template)
├── docs/                   # GitBook-ready docs
└── tools/analyze_guna.py   # dnfile metadata verifier
```

## Build from source

```bash
dotnet build NovaUI.WinForms/NovaUI.WinForms.csproj -c Release   # net472 + net8.0-windows
dotnet build TestApp/TestApp.csproj -c Release                   # demo app
```

Requirements: .NET SDK 8 (`EnableWindowsTargeting=true` already set). Windows is
needed to *run* the TestApp.

## Verified (dnfile metadata)

| Target | TFM | Types | Methods | Properties |
| --- | --- | --- | --- | --- |
| net472 | `.NETFramework,Version=v4.7.2` | 17 | 243 | 72 |
| net8.0-windows | `.NETCoreApp,Version=v8.0` | 17 | 243 | 72 |

Build result: 0 errors / 0 warnings on both targets.
