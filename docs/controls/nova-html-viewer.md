---
title: NovaHtmlViewer
description: HTML viewer with template parameter injection, custom link bar and print support.
---

# NovaHtmlViewer

An HTML viewer built on the built-in WebBrowser engine (IE/Trident). Designed for
**printable documents and reports**: load an HTML file (A4 layouts work great),
inject runtime values into it, add a toolbar of custom links, and print it.

## Loading content

| Method | Behavior |
| --- | --- |
| `LoadFile(path)` | reads an HTML file, applies the injection pipeline, renders it |
| `LoadHtml(html)` | renders an HTML string through the same pipeline |
| `LoadUrl(url)` | navigates to a URL, appending `Parameters` as a query string |
| `Navigate(url)` | navigates exactly where told - no injection |
| `RefreshDocument()` | re-loads the current source with the current parameters |

Property `Html` returns the raw (untransformed) HTML of the last source.

When loading a **file**, the viewer automatically inserts `<base href="...">` so
relative images and CSS in that file keep working.

# Injecting parameters into the HTML

The core feature: write your HTML file with **tokens**, and NovaHtmlViewer replaces
them with runtime values **before** the browser engine renders the document.

## Step 1 - put tokens in the HTML file

```html
<!-- form.html -->
<h1>{{ClinicName}}</h1>
<table>
  <tr><th>Patient</th><td>{{PatientName}}</td></tr>
  <tr><th>Visit date</th><td>{{VisitDate}}</td></tr>
  <tr><th>Blood pressure</th><td>{{BloodPressure}}</td></tr>
</table>
```

## Step 2 - fill the values and load (C#)

```csharp
var viewer = new NovaHtmlViewer();
viewer.Dock = DockStyle.Fill;
Controls.Add(viewer);

viewer.Parameters["ClinicName"] = "Nova Wellness Clinic";
viewer.Parameters["PatientName"] = "Juan Dela Cruz";
viewer.Parameters["VisitDate"] = DateTime.Today.ToString("yyyy-MM-dd");
viewer.Parameters["BloodPressure"] = "120/80";

viewer.LoadFile("form.html");   // tokens are replaced just before rendering
```

## Step 2 - fill the values and load (VB.NET)

```vb
Dim viewer As New NovaHtmlViewer()
viewer.Dock = DockStyle.Fill
Controls.Add(viewer)

viewer.Parameters("ClinicName") = "Nova Wellness Clinic"
viewer.Parameters("PatientName") = "Juan Dela Cruz"
viewer.Parameters("VisitDate") = Date.Today.ToString("yyyy-MM-dd")
viewer.Parameters("BloodPressure") = "120/80"

viewer.LoadFile("form.html")   ' tokens are replaced just before rendering
```

## Changing values later

Call `RefreshDocument()` after changing parameters - it re-runs the whole
injection pipeline on the current source:

```csharp
viewer.Parameters["PatientName"] = "Maria Santos";
viewer.RefreshDocument();
```

```vb
viewer.Parameters("PatientName") = "Maria Santos"
viewer.RefreshDocument()
```

> [!TIP]
> `RefreshDocument()` re-reads the file from disk too - if you edit the HTML file
> while the app runs, calling it picks up your edits.

## Custom token delimiters

Default tokens are `{{key}}`. Change `TokenPrefix` / `TokenSuffix` for other
styles, e.g. `%key%`:

```csharp
viewer.TokenPrefix = "%";
viewer.TokenSuffix = "%";
viewer.Parameters["PatientName"] = "Juan Dela Cruz";
viewer.LoadFile("form.html");    // %PatientName% is replaced
```

```vb
viewer.TokenPrefix = "%"
viewer.TokenSuffix = "%"
viewer.Parameters("PatientName") = "Juan Dela Cruz"
viewer.LoadFile("form.html")     ' %PatientName% is replaced
```

`KeepUnknownTokens = true` (default) leaves `{{unfilled}}` tokens visible;
set it to `false` to strip any token that has no matching parameter.

## Injecting CSS / script into the head

`HeadInjection` inserts raw HTML just before `</head>` on every render - useful
for print styles, scripts or fonts:

```csharp
viewer.HeadInjection =
    "<style>body{font-family:'Segoe UI'} h1{color:#4F6BFF}</style>";
viewer.LoadFile("form.html");
```

```vb
viewer.HeadInjection =
    "<style>body{font-family:'Segoe UI'} h1{color:#4F6BFF}</style>"
viewer.LoadFile("form.html")
```

## Full control: the HtmlTransforming event

`HtmlTransforming` fires with the **raw HTML** before tokens are applied. Rewrite
`e.Html` however you like (or set `e.Cancel = true` to skip the load). Example:
inject a JSON object consumed by the page's JavaScript:

```csharp
viewer.HtmlTransforming += (s, e) =>
{
    string json = "{\"patient\":\"" + name + "\",\"bp\":\"" + bp + "\"}";
    e.Html = e.Html.Replace("/*__DATA__*/", "window.__DATA__ = " + json + ";");
};
```

```vb
AddHandler viewer.HtmlTransforming,
    Sub(s, e)
        Dim json As String = "{""patient"":""" & name & """,""bp"":""" & bp & """}"
        e.Html = e.Html.Replace("/*__DATA__*/", "window.__DATA__ = " & json & ";")
    End Sub
```

Pipeline order per render:

1. `HtmlTransforming` event (raw template)
2. automatic `<base href>` for file loads
3. `HeadInjection`
4. `Parameters` token replacement

## URLs and parameters

`LoadUrl` appends the parameter dictionary as a query string instead of doing
token replacement (the viewer cannot rewrite a remote page):

```csharp
viewer.Parameters["PatientName"] = "Juan";
viewer.LoadUrl("https://example.com/report");
// -> https://example.com/report?PatientName=Juan
```

## Testing your tokens in isolation

The static helper is exposed so you can dry-run the replacement (e.g. in unit
tests):

```csharp
string html = NovaHtmlViewer.ApplyTokens(
    "<b>{{Name}}</b>",
    new Dictionary<string, string> { { "Name", "Juan" } },
    "{{", "}}", keepUnknown: true);
// -> "<b>Juan</b>"
```

# Custom link bar

Add clickable links above or below the document:

```csharp
viewer.LinksPosition = NovaLinksPosition.Top;
viewer.AddLink("Home", "about:blank", "Back to start", false);
viewer.AddLink("Manual", "https://example.com/manual", "Opens in browser", true);

viewer.LinkClicked += (s, e) =>
{
    if (e.Url.StartsWith("https://audit-log"))
        e.Cancel = true;          // block this one
};
```

```vb
viewer.LinksPosition = NovaLinksPosition.Top
viewer.AddLink("Home", "about:blank", "Back to start", False)
viewer.AddLink("Manual", "https://example.com/manual", "Opens in browser", True)
```

- `openExternally: true` opens the URL in the system browser (default browser)
  instead of the embedded engine.
- `LinkColor` / `LinkHoverColor` theme the bar; `ClearLinks()` empties it.
- `LinksPosition` = `None`, `Top` or `Bottom` (`None` hides the bar).

# Printing

```csharp
viewer.ShowPrintDialog();      // system print dialog
viewer.ShowPrintPreview();     // print preview window
```

```vb
viewer.ShowPrintDialog()
viewer.ShowPrintPreview()
```

Use CSS `@media print` in your HTML to hide toolbars and margins when printing -
see `TestApp/sample.html` for a complete A4 example.

# Other members

| Member | Description |
| --- | --- |
| `DocumentCompleted` | passthrough event of the inner WebBrowser |
| `ObjectForScripting` | expose a .NET object to the page's JavaScript |
| `ScriptErrorsSuppressed` | hide script error dialogs (default `true`) |
| `Url`, `IsBusy` | current document URL / navigation state |
