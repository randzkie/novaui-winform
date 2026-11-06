# NovaHtmlViewer

An HTML document viewer - think "PDF viewer, but for HTML pages". It hosts the in-box Windows `WebBrowser` engine and adds what a document viewer needs:

- **Load HTML from a string, a local file, or a URL**
- **A bar of custom links** injected into the document (top or bottom), configurable from C#/VB
- **Click interception** - run your own code on link clicks, cancel them, or open them in the user's default browser
- **Print / print preview** for printable documents (A4 forms, reports)

- **Base class:** `System.Windows.Forms.UserControl`
- **Namespace:** `NovaUI.WinForms`
- **Default property / event:** `Html` / `LinkClicked`

> **Engine note:** the viewer uses the in-box `WebBrowser` engine (Trident). It is available on both .NET Framework 4.7.2 and .NET 8 and needs no extra packages. Static/styled documents (forms, reports, help pages) render perfectly; very modern CSS/JS is outside its scope.

## Quick start (C#)

```csharp
var viewer = new NovaHtmlViewer();
viewer.Location = new Point(24, 60);
viewer.Size = new Size(800, 460);
viewer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
viewer.Html = "<h1>Course Progress Assessment Report</h1><p>Period: 1st Semester.</p>";
this.Controls.Add(viewer);
```

## Quick start (Visual Basic)

```vb
Dim viewer As New NovaHtmlViewer()
viewer.Location = New Point(24, 60)
viewer.Size = New Size(800, 460)
viewer.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
viewer.Html = "<h1>Course Progress Assessment Report</h1><p>Period: 1st Semester.</p>"
Me.Controls.Add(viewer)
```

## Loading content: string, file, or URL

**C#**

```csharp
viewer.Html = htmlString;                     // 1. from a string
viewer.LoadFile(@"C:\forms\cpar-form.html");  // 2. from a local file
viewer.LoadUrl("https://example.org/report"); // 3. from the web
viewer.RefreshDocument();                     // re-render after changing links
```

**Visual Basic**

```vb
viewer.Html = htmlString                      ' 1. from a string
viewer.LoadFile("C:\forms\cpar-form.html")    ' 2. from a local file
viewer.LoadUrl("https://example.org/report")  ' 3. from the web
viewer.RefreshDocument()                      ' re-render after changing links
```

## Custom links

The **custom links** feature renders a styled bar of links inside the viewed document. Links live in the `CustomLinks` collection and appear wherever `LinksPosition` points (top by default). Add them before assigning `Html`, or add them later and call `RefreshDocument()`.

**C#**

```csharp
viewer.LinksPosition = NovaLinksBarPosition.Top;   // None / Top / Bottom
viewer.LinksBarBackColor = NovaTheme.SurfaceColor;
viewer.LinkColor = NovaTheme.Accent;
viewer.LinkHoverColor = NovaTheme.AccentHover;

// Simple overload:
viewer.AddLink("New Form", "https://example.org/new");

// Full overload: text, url, tooltip, open-in-default-browser:
viewer.AddLink("Records", "https://example.org/records", "Open the records list", true);
viewer.AddLink("Help", "https://example.org/help");

// Remove all links:
// viewer.ClearLinks();
```

**Visual Basic**

```vb
viewer.LinksPosition = NovaLinksBarPosition.Top   ' None / Top / Bottom
viewer.LinksBarBackColor = NovaTheme.SurfaceColor
viewer.LinkColor = NovaTheme.Accent
viewer.LinkHoverColor = NovaTheme.AccentHover

' Simple overload:
viewer.AddLink("New Form", "https://example.org/new")

' Full overload: text, url, tooltip, open-in-default-browser:
viewer.AddLink("Records", "https://example.org/records", "Open the records list", True)
viewer.AddLink("Help", "https://example.org/help")

' Remove all links:
' viewer.ClearLinks()
```

### Intercepting link clicks

The `LinkClicked` event fires for every custom link click. It is **cancelable** and lets you reroute the click:

**C#**

```csharp
viewer.LinkClicked += (s, e) =>
{
    if (e.Link.Text == "Records")
    {
        e.Cancel = true;                      // keep the viewer where it is
        OpenRecordsWindow();                  // ...and do something in the app
    }
    else if (e.Link.OpenExternally)
    {
        e.OpenExternally = true;              // default browser handles it
    }
};
```

**Visual Basic**

```vb
AddHandler viewer.LinkClicked,
    Sub(s, e)
        If e.Link.Text = "Records" Then
            e.Cancel = True                   ' keep the viewer where it is
            OpenRecordsWindow()               ' ...and do something in the app
        ElseIf e.Link.OpenExternally Then
            e.OpenExternally = True           ' default browser handles it
        End If
    End Sub
```

With designer-placed controls in VB you can use `Handles`:

```vb
Private Sub Viewer1_LinkClicked(sender As Object, e As NovaHtmlLinkEventArgs) Handles Viewer1.LinkClicked
    Log("Link clicked: " & e.Url.ToString())
End Sub
```

### Open everything externally

Set one property to turn the viewer into a "sandbox": every http/https click opens in the user's default browser instead of navigating inside the viewer.

**C#** `viewer.OpenLinksExternally = true;`
**Visual Basic** `viewer.OpenLinksExternally = True`

## Printing printable documents

Documents designed for paper (A4 forms with `@media print` CSS, like assessment-report templates) print correctly:

**C#**

```csharp
printButton.Click += (s, e) => viewer.ShowPrintDialog();      // system print dialog
previewButton.Click += (s, e) => viewer.ShowPrintPreview();   // print preview window
```

**Visual Basic**

```vb
AddHandler printButton.Click, Sub(s, e) viewer.ShowPrintDialog()      ' system print dialog
AddHandler previewButton.Click, Sub(s, e) viewer.ShowPrintPreview()   ' print preview window
```

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `Html` | `string` | `""` | HTML string currently displayed (assigning re-renders) |
| `CustomLinks` | `List&lt;NovaHtmlLink&gt;` | empty | The custom links collection |
| `LinksPosition` | `NovaLinksBarPosition` | `Top` | Where the links bar renders: `None`, `Top`, `Bottom` |
| `LinksBarBackColor` | `Color` | `NovaTheme.SurfaceColor` | Links bar background |
| `LinkColor` | `Color` | `NovaTheme.Accent` | Custom link color |
| `LinkHoverColor` | `Color` | `NovaTheme.AccentHover` | Custom link hover color |
| `OpenLinksExternally` | `bool` | `false` | Open all http/https navigations in the default browser |
| `ShowBorder` | `bool` | `true` | 1px frame around the viewer |
| `BorderColor` | `Color` | `NovaTheme.BorderColor` | Frame color |
| `SuppressScriptErrors` | `bool` | `true` | Hide engine script-error dialogs |
| `IsLoading` | `bool` | - | True while a document is loading (read-only) |
| `InnerBrowser` | `WebBrowser` | - | Underlying engine for advanced use (read-only) |

## Methods

| Method | Description |
| --- | --- |
| `LoadFile(string path)` | Load an HTML document from a local file |
| `LoadUrl(string urlString)` | Load from a web or `file://` URL |
| `Navigate(Uri uri)` | Navigate to a `Uri` |
| `RefreshDocument()` | Re-render the current HTML with current links/colors |
| `AddLink(string text, string url)` | Add a custom link |
| `AddLink(string text, string url, string tooltip, bool openExternally)` | Add a custom link with options |
| `ClearLinks()` | Remove all custom links |
| `ShowPrintDialog()` | Open the system print dialog |
| `ShowPrintPreview()` | Open print preview |
| `GoBack()` / `GoForward()` / `Stop()` / `Reload()` | Navigation commands |

## Events

| Event | Args | Description |
| --- | --- | --- |
| `LinkClicked` | `NovaHtmlLinkEventArgs` (`Link`, `Url`, `Cancel`, `OpenExternally`) | A custom link was clicked - cancel or reroute |
| `DocumentLoaded` | `WebBrowserDocumentCompletedEventArgs` | The document finished loading |
| `Navigated` | `WebBrowserNavigatedEventArgs` | The engine navigated to a URL |

## The NovaHtmlLink class

| Property | Type | Description |
| --- | --- | --- |
| `Text` | `string` | Visible caption (falls back to the URL when empty) |
| `Url` | `string` | Absolute target URL (`http/https/mailto/...`) |
| `Tooltip` | `string` | Optional HTML `title` tooltip |
| `OpenExternally` | `bool` | Always open this link in the default browser |

## Notes and tips

- Links with relative URLs cannot be matched reliably inside string-rendered documents - use absolute URLs in `NovaHtmlLink.Url`.
- The frame is a plain rectangle by design: the ActiveX engine does not clip to rounded corners (classic "airspace" behavior).
- `IsWebBrowserContextMenuEnabled` is disabled by default so users get a clean document surface; re-enable via `viewer.InnerBrowser.IsWebBrowserContextMenuEnabled = true` if you need it.
- For documents generated at runtime, build the string with your own template and assign `Html` - the viewer only injects its links bar, everything else is yours.
