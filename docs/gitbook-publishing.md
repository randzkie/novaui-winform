---
title: GitBook publishing guide
description: How this documentation is structured (README + SUMMARY) and step-by-step instructions for publishing it on GitBook.
---

# GitBook publishing guide

This `docs/` folder already follows the **GitBook format**. Three files make that
true:

| File | Role |
| --- | --- |
| `README.md` | The home page of the space (first page GitBook shows) |
| `SUMMARY.md` | The table of contents - every entry becomes a page in the sidebar |
| `.gitbook.yaml` (repo root) | Tells GitBook the docs live in `docs/` when you sync the whole repo |

Each page also starts with YAML **front matter** (`title` + `description`) which
GitBook uses for the sidebar label and search results. The banner in
`assets/nova-banner.svg` is referenced from the home page.

# Option A - Git Sync (recommended)

Publish the docs straight from your Git repository. Changes you push are mirrored
into GitBook automatically.

1. **Push this project to GitHub** (a public repo works on the free GitBook plan;
   private repos may need a paid plan).
2. In GitBook: open your **space** (or create one: *New* -> *Space*) -> open the
   space menu next to its name -> **Git Sync** (or *Settings* -> *Integrations* ->
   *Git Sync*).
3. Choose **GitHub** -> install the GitBook GitHub app -> pick your repository and
   branch (`main`).
4. Set the **root folder**:
   - If you synced the **whole project repo**: leave root at `/` - `.gitbook.yaml`
     already redirects GitBook to `docs/`.
   - If the repo contains **only the docs folder**: set root to `/` and delete
     `.gitbook.yaml` from your copy (its `root: ./docs` would point nowhere).
5. Click **Sync**. Within a minute the sidebar is built from `SUMMARY.md` and the
   home page is `README.md`.

From now on, editing files in the repo (and pushing) updates the documentation -
no manual re-upload.

# Option B - Create the pages manually

If you prefer building the space by hand in the GitBook editor:

1. **Add pages:** in the left sidebar (*Table of contents*), hover over an empty
   area or a page -> click **+** -> choose *New page* (or *New group* for a section
   like "Controls").
2. **Name a page:** double-click its title in the sidebar, or open it and click the
   big title at the top.
3. **Fill content:** either type directly in the editor, or create the page from
   our files: open `docs/controls/nova-button.md` on your machine, select all,
   copy (without the `---` front matter block at the top), then paste into the
   GitBook page - **GitBook converts pasted Markdown into real blocks** (headings,
   code blocks, tables, tips).
4. **Reorder:** drag pages in the sidebar to arrange them; groups can be collapsed.

# Option C - Import a whole folder

Some GitBook plans offer *Import* (from Notion/Confluence/Markdown). If you see
**Import** on your space, point it at this `docs/` folder - it keeps the
README/SUMMARY structure where possible.

# Customizing the landing page you created from a template

Template pages are just ordinary pages - the template only pre-fills them:

1. **Open the page** from the sidebar (click its name).
2. **Edit any text:** click directly into a paragraph or heading and type. Click
   outside the block to finish.
3. **Move/delete blocks:** hover a block -> drag it by the **⋮⋮** handle that
   appears on the left, or open its **⋯** menu -> *Delete*.
4. **Add blocks:** press **/** where you want content - pick code blocks, tables,
   hint/tip boxes, tabs, images.
5. **Replace the hero/banner:** template heroes are usually an image block or a
   page cover:
   - Image block: hover it -> *Replace* -> upload `docs/assets/nova-banner.svg`
     (or your own PNG).
   - Page cover: page **⋯** menu -> *Change cover*.
6. **Page title & description:** the top title is editable by clicking it; the
   description lives in the page **⋯** menu -> *Edit description* (or the
   front-matter `description` if you use Git Sync).
7. **Site branding (title, logo, colors shown to visitors):** open your published
   **site** overview -> **Customize** (or *Settings* -> *Appearance*): set the site
   title, header description, logo/icon, primary color and corner style. These are
   site-level - they apply to every page.
8. **Publish or update the link:** *Share* (top right) -> *Publish* / copy the
   live URL, add a custom domain if you have one.

> [!NOTE]
> Menu names move around a bit between GitBook versions and plans, but the pieces
> are always: sidebar = page tree, hover controls = edit/delete/drag, **/** = insert
> blocks, page ⋯ menu = rename/cover/description, site settings = branding.

# How the TOC maps to files

`SUMMARY.md` drives everything. To add a page permanently, add a line to
`SUMMARY.md` and create the matching `.md` file:

```
## Controls

* [NovaButton](controls/nova-button.md)      <- group heading + page entry
```

# Tips for GitBook-flavored pages

- Tip/warning boxes: `> [!TIP]` and `> [!WARNING]` blockquotes (used throughout
  these docs) render as colored asides.
- Code blocks keep their language tag (`csharp`, `vb`, `xml`, `html`) and get
  syntax highlighting automatically.
- Tables in Markdown render as GitBook tables; keep them under ~10 columns.
- Anchors: any heading gets an anchor - link across pages with
  `[text](controls/nova-html-viewer.md#injecting-parameters-into-the-html)`.
