using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace NovaUI.WinForms
{
    /// <summary>Where the custom links bar is rendered inside the document.</summary>
    public enum NovaLinksBarPosition
    {
        /// <summary>Do not render the links bar (links stay in the collection).</summary>
        None = 0,
        /// <summary>Render the links bar at the top of the document.</summary>
        Top = 1,
        /// <summary>Render the links bar at the bottom of the document.</summary>
        Bottom = 2
    }

    /// <summary>One custom link rendered in the NovaHtmlViewer links bar.</summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class NovaHtmlLink
    {
        private Uri _uri;

        public NovaHtmlLink() { }

        public NovaHtmlLink(string text, string url)
        {
            Text = text;
            Url = url;
        }

        /// <summary>Visible caption of the link.</summary>
        public string Text { get; set; } = "";

        /// <summary>Absolute target URL (http/https/mailto/...).</summary>
        public string Url { get; set; } = "";

        /// <summary>Optional tooltip (HTML title attribute).</summary>
        public string Tooltip { get; set; } = "";

        /// <summary>Force this link to open in the default system browser.</summary>
        public bool OpenExternally { get; set; }

        internal Uri ResolvedUri
        {
            get
            {
                if (_uri == null && !string.IsNullOrEmpty(Url))
                {
                    Uri parsed;
                    if (Uri.TryCreate(Url, UriKind.Absolute, out parsed)) _uri = parsed;
                }
                return _uri;
            }
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Text) ? (Url ?? "") : Text + " -> " + Url;
        }
    }

    /// <summary>Arguments for the NovaHtmlViewer.LinkClicked event.</summary>
    public class NovaHtmlLinkEventArgs : CancelEventArgs
    {
        public NovaHtmlLinkEventArgs(NovaHtmlLink link, Uri url)
        {
            Link = link;
            Url = url;
        }

        /// <summary>The custom link that was clicked (null when not a custom link).</summary>
        public NovaHtmlLink Link { get; private set; }

        /// <summary>Target URI of the navigation.</summary>
        public Uri Url { get; private set; }

        /// <summary>Set true to open the URL in the user's default browser.</summary>
        public bool OpenExternally { get; set; }
    }

    /// <summary>Handler for NovaHtmlViewer link events.</summary>
    public delegate void NovaHtmlLinkEventHandler(object sender, NovaHtmlLinkEventArgs e);

    /// <summary>
    /// HTML document viewer, analogous to a PDF viewer but for HTML pages.
    /// Hosts the in-box WebBrowser engine and adds: a configurable bar of
    /// custom links injected into the document, click interception with
    /// "open in default browser" support, and print / print-preview methods.
    /// Content can come from a string, a local file or a URL.
    /// </summary>
    [DefaultProperty("Html")]
    [DefaultEvent("LinkClicked")]
    public class NovaHtmlViewer : UserControl
    {
        private readonly WebBrowser _browser;
        private readonly List<NovaHtmlLink> _links = new List<NovaHtmlLink>();
        private string _userHtml = "";
        private Uri _currentUrl;
        private NovaLinksBarPosition _linksPosition = NovaLinksBarPosition.Top;
        private Color _linksBarBackColor = NovaTheme.SurfaceColor;
        private Color _linkColor = NovaTheme.Accent;
        private Color _linkHoverColor = NovaTheme.AccentHover;
        private bool _openLinksExternally;
        private bool _showBorder = true;
        private Color _borderColor = NovaTheme.BorderColor;

        /// <summary>Raised when a custom link is clicked. Set e.Cancel to block, e.OpenExternally to open in the default browser.</summary>
        [Category("Nova")]
        public event NovaHtmlLinkEventHandler LinkClicked;

        /// <summary>Raised after the document finished loading.</summary>
        [Category("Nova")]
        public event WebBrowserDocumentCompletedEventHandler DocumentLoaded;

        /// <summary>Raised when the underlying engine reports progress/state changes.</summary>
        [Category("Nova")]
        public event WebBrowserNavigatedEventHandler Navigated;

        public NovaHtmlViewer()
        {
            SetStyle(Ui.PaintStyles, true);
            UpdateStyles();
            _browser = new WebBrowser();
            _browser.ScriptErrorsSuppressed = true;
            _browser.IsWebBrowserContextMenuEnabled = false;
            _browser.AllowNavigation = true;
            _browser.WebBrowserShortcutsEnabled = true;
            _browser.Dock = DockStyle.Fill;
            _browser.Navigating += OnBrowserNavigating;
            _browser.DocumentCompleted += OnBrowserDocumentCompleted;
            _browser.Navigated += OnBrowserNavigated;
            Controls.Add(_browser);
            Size = new Size(560, 420);
            BackColor = Color.White;
            Padding = new Padding(1);
        }

        // ---------------- content ----------------

        /// <summary>HTML string currently displayed. Assigning re-renders the document (with the links bar if configured).</summary>
        [Category("Nova")]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor))]
        public string Html
        {
            get { return _userHtml; }
            set
            {
                _userHtml = value ?? "";
                _currentUrl = null;
                _browser.DocumentText = ComposeDocument(_userHtml);
            }
        }

        /// <summary>Loads an HTML document from a local file path.</summary>
        public void LoadFile(string path)
        {
            string full = System.IO.Path.GetFullPath(path);
            _userHtml = "";
            Navigate(new Uri(full));
        }

        /// <summary>Loads a document from a web or file:// URL.</summary>
        public void LoadUrl(string urlString)
        {
            Uri uri = new Uri(urlString);
            Navigate(uri);
        }

        /// <summary>Navigates to the given Uri.</summary>
        public void Navigate(Uri uri)
        {
            _currentUrl = uri;
            _browser.Navigate(uri);
        }

        /// <summary>Reloads the current document string (use after changing CustomLinks).</summary>
        public void RefreshDocument()
        {
            _currentUrl = null;
            _browser.DocumentText = ComposeDocument(_userHtml);
        }

        // ---------------- custom links ----------------

        /// <summary>Collection of custom links rendered as a bar inside the document.</summary>
        [Category("Nova")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<NovaHtmlLink> CustomLinks
        {
            get { return _links; }
        }

        /// <summary>Where the links bar is rendered (None / Top / Bottom).</summary>
        [Category("Nova"), DefaultValue(NovaLinksBarPosition.Top)]
        public NovaLinksBarPosition LinksPosition
        {
            get { return _linksPosition; }
            set { _linksPosition = value; RefreshDocument(); }
        }

        /// <summary>Background color of the links bar.</summary>
        [Category("Nova")]
        public Color LinksBarBackColor
        {
            get { return _linksBarBackColor; }
            set { _linksBarBackColor = value; RefreshDocument(); }
        }

        /// <summary>Color of custom links.</summary>
        [Category("Nova")]
        public Color LinkColor
        {
            get { return _linkColor; }
            set { _linkColor = value; RefreshDocument(); }
        }

        /// <summary>Color of custom links on hover.</summary>
        [Category("Nova")]
        public Color LinkHoverColor
        {
            get { return _linkHoverColor; }
            set { _linkHoverColor = value; RefreshDocument(); }
        }

        /// <summary>Open every http/https navigation in the default system browser instead of inside the viewer.</summary>
        [Category("Nova"), DefaultValue(false)]
        public bool OpenLinksExternally
        {
            get { return _openLinksExternally; }
            set { _openLinksExternally = value; }
        }

        // ---------------- appearance / printing ----------------

        /// <summary>Draw a 1px frame around the viewer.</summary>
        [Category("Nova"), DefaultValue(true)]
        public bool ShowBorder
        {
            get { return _showBorder; }
            set { _showBorder = value; Padding = new Padding(value ? 1 : 0); Invalidate(); }
        }

        /// <summary>Frame color.</summary>
        [Category("Nova")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; Invalidate(); }
        }

        /// <summary>Suppress script error dialogs (default true).</summary>
        [Category("Nova"), DefaultValue(true)]
        public bool SuppressScriptErrors
        {
            get { return _browser.ScriptErrorsSuppressed; }
            set { _browser.ScriptErrorsSuppressed = value; }
        }

        /// <summary>True while the document is loading.</summary>
        [Browsable(false)]
        public bool IsLoading { get; private set; }

        /// <summary>Underlying WebBrowser engine (advanced use).</summary>
        [Browsable(false)]
        public WebBrowser InnerBrowser
        {
            get { return _browser; }
        }

        /// <summary>Adds a custom link and re-renders the document.</summary>
        public NovaHtmlLink AddLink(string text, string url)
        {
            return AddLink(text, url, "", false);
        }

        /// <summary>Adds a custom link with tooltip and external-open flag, then re-renders the document.</summary>
        public NovaHtmlLink AddLink(string text, string url, string tooltip, bool openExternally)
        {
            NovaHtmlLink link = new NovaHtmlLink(text, url);
            link.Tooltip = tooltip ?? "";
            link.OpenExternally = openExternally;
            _links.Add(link);
            RefreshDocument();
            return link;
        }

        /// <summary>Removes all custom links and re-renders the document.</summary>
        public void ClearLinks()
        {
            _links.Clear();
            RefreshDocument();
        }

        /// <summary>Shows the system print dialog for the current document.</summary>
        public void ShowPrintDialog()
        {
            if (!IsDisposed) _browser.ShowPrintDialog();
        }

        /// <summary>Shows a print preview for the current document.</summary>
        public void ShowPrintPreview()
        {
            if (!IsDisposed) _browser.ShowPrintPreviewDialog();
        }

        public void GoBack() { if (_browser.CanGoBack) _browser.GoBack(); }
        public void GoForward() { if (_browser.CanGoForward) _browser.GoForward(); }
        public void Stop() { _browser.Stop(); }
        public void Reload() { _browser.Refresh(); }

        // ---------------- internals ----------------

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_showBorder)
            {
                using (Pen p = new Pen(_borderColor, 1f))
                {
                    e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
                }
            }
        }

        private static string Hex(Color c)
        {
            return "#" + c.R.ToString("X2", CultureInfo.InvariantCulture)
                       + c.G.ToString("X2", CultureInfo.InvariantCulture)
                       + c.B.ToString("X2", CultureInfo.InvariantCulture);
        }

        private static string Esc(string s)
        {
            return WebUtility.HtmlEncode(s ?? "");
        }

        private string BuildLinksBarHtml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<style>");
            sb.Append(".nova-links{background:").Append(Hex(_linksBarBackColor)).Append(";");
            sb.Append("padding:9px 14px;font-family:'Segoe UI',Arial,sans-serif;font-size:9pt;}");
            sb.Append(".nova-links a{color:").Append(Hex(_linkColor)).Append(";");
            sb.Append("text-decoration:none;font-weight:600;margin-right:18px;}");
            sb.Append(".nova-links a:hover{color:").Append(Hex(_linkHoverColor)).Append(";text-decoration:underline;}");
            sb.Append("</style><div class=\"nova-links\">");
            foreach (NovaHtmlLink link in _links)
            {
                string title = string.IsNullOrEmpty(link.Tooltip) ? "" : " title=\"" + Esc(link.Tooltip) + "\"";
                sb.Append("<a href=\"").Append(Esc(link.Url)).Append("\"").Append(title).Append(">")
                  .Append(Esc(string.IsNullOrEmpty(link.Text) ? link.Url : link.Text))
                  .Append("</a>");
            }
            sb.Append("</div>");
            return sb.ToString();
        }

        private string ComposeDocument(string body)
        {
            if (_links.Count == 0 || _linksPosition == NovaLinksBarPosition.None)
            {
                return body;
            }
            string bar = BuildLinksBarHtml();
            if (_linksPosition == NovaLinksBarPosition.Top)
            {
                int bodyTag = IndexOfIgnoreCase(body, "<body");
                if (bodyTag >= 0)
                {
                    int end = body.IndexOf('>', bodyTag);
                    if (end >= 0)
                    {
                        return body.Substring(0, end + 1) + bar + body.Substring(end + 1);
                    }
                }
                return bar + body;
            }
            int close = LastIndexOfIgnoreCase(body, "</body>");
            if (close >= 0)
            {
                return body.Substring(0, close) + bar + body.Substring(close);
            }
            return body + bar;
        }

        private static int IndexOfIgnoreCase(string haystack, string needle)
        {
            if (haystack == null) return -1;
            return haystack.IndexOf(needle, StringComparison.OrdinalIgnoreCase);
        }

        private static int LastIndexOfIgnoreCase(string haystack, string needle)
        {
            if (haystack == null) return -1;
            return haystack.LastIndexOf(needle, StringComparison.OrdinalIgnoreCase);
        }

        private NovaHtmlLink FindLink(Uri url)
        {
            if (url == null) return null;
            string raw = url.ToString();
            foreach (NovaHtmlLink link in _links)
            {
                if (string.Equals(link.Url, raw, StringComparison.OrdinalIgnoreCase)) return link;
                Uri resolved = link.ResolvedUri;
                if (resolved != null &&
                    Uri.Compare(resolved, url, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return link;
                }
            }
            return null;
        }

        private static bool IsWebUrl(Uri url)
        {
            return url != null && (string.Equals(url.Scheme, "http", StringComparison.OrdinalIgnoreCase)
                || string.Equals(url.Scheme, "https", StringComparison.OrdinalIgnoreCase));
        }

        private static void OpenExternal(Uri url)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(url.ToString());
                psi.UseShellExecute = true;
                Process.Start(psi);
            }
            catch (Win32Exception)
            {
                // no default browser configured - swallow, this is a UI nicety
            }
        }

        private void OnBrowserNavigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            // navigations we initiated ourselves are always allowed
            if (_currentUrl != null && Uri.Compare(_currentUrl, e.Url, UriComponents.AbsoluteUri,
                    UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return;
            }

            NovaHtmlLink link = FindLink(e.Url);
            if (link != null)
            {
                NovaHtmlLinkEventArgs args = new NovaHtmlLinkEventArgs(link, e.Url);
                NovaHtmlLinkEventHandler handler = LinkClicked;
                if (handler != null) handler(this, args);
                if (args.Cancel) { e.Cancel = true; return; }
                bool external = link.OpenExternally || args.OpenExternally || _openLinksExternally;
                if (external && IsWebUrl(e.Url))
                {
                    e.Cancel = true;
                    OpenExternal(e.Url);
                }
                return;
            }

            if (_openLinksExternally && IsWebUrl(e.Url))
            {
                e.Cancel = true;
                OpenExternal(e.Url);
            }
        }

        private void OnBrowserNavigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            IsLoading = false;
            WebBrowserNavigatedEventHandler handler = Navigated;
            if (handler != null) handler(this, e);
        }

        private void OnBrowserDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            IsLoading = false;
            WebBrowserDocumentCompletedEventHandler handler = DocumentLoaded;
            if (handler != null) handler(this, e);
        }
    }
}
