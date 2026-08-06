# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Yuki-Theme is a **PascalABC.NET IDE plugin** that provides comprehensive IDE customization: 60+ anime-themed color schemes (from the Doki Theme collection), 9 built-in default themes, wallpaper/sticker overlays, JetBrains-style icon replacement, and code completion styling. It targets **.NET Framework 4.8** and uses a hybrid **WinForms + WPF** UI.

## Solution Structure

- **Yuki-Theme** — Main plugin library (.dll), root namespace `YukiTheme`
- **YukiTest** — NUnit 3.5 test project
- **YukiUtility** — Console tool for managing symbolic links between theme resources and PascalABC.NET directories
- **DokiNameNormalizer** — Console tool for normalizing theme file names
- **YukiUpdater** — Update checker utility

## Build & Test

```bash
# Build the solution (requires .NET Framework 4.8 SDK and MSBuild)
msbuild Yuki-Theme.sln /p:Configuration=Release

# Run tests (NUnit)
nunit3-console YukiTest/bin/Release/YukiTest.dll
```

**Critical external dependency:** PascalABC.NET IDE binaries are referenced at `..\..\PascalABCNET\bin\` (relative to project root). These are not in the repo and must be present for compilation.

## Architecture

### Plugin Entry Point

`YukiTheme_VisualPascalABCPlugin` implements `IVisualPascalABCPlugin`. The `GetGUI()` method triggers initialization via the `IDEAlterer` singleton, which orchestrates the full alteration chain: component discovery → color extraction → image loading → menu alteration → style changes → plugin injection → window management.

### Key Namespaces

| Namespace | Role |
|---|---|
| `YukiTheme.Engine` | Core transformation: `IDEAlterer`, `ColorChanger`, `EditorAlterer`, `IconAlterer`, `StickerManager`, `WallpaperManager` |
| `YukiTheme.Engine.Plugins` | Reflection-based injection into other PascalABC.NET plugins (CodeTemplate, CompilerController, InternalErrorReport, LanguageConverter) |
| `YukiTheme.Export` | Theme export chain: `DokiExporter`, `YukiExporter`, `PlainExporter`, `ExternalExporter` |
| `YukiTheme.Components` | UI windows (XAML/WPF): SettingsPanel, StickerWindow, WallpaperWindow, ThemeSelectWindow |
| `YukiTheme.Style` | IDE rendering: MenuRenderer, ToolRenderer, dock pane styling, code completion styling |
| `YukiTheme.Tools` | Helpers: settings constants, color/image utilities, ZIP handling |
| `YukiTheme.Tools.Database` | JSON-based local settings persistence (`FileDatabase`) stored in user's Documents/YukiTheme |
| `YukiTheme.Tools.Doki` | Doki theme JSON parsing and key translation |

### Theme System

Themes come from two sources:
- **Default themes** (9): XSHD files (XML syntax highlighting definitions) in `Resources/Default/`
- **Doki themes** (60+): JSON definitions in `Resources/Themes/Doki/` with corresponding PNG stickers and wallpapers

All theme assets (JSON, XSHD, images, SVG icons) are **embedded resources**.

### Plugin Injection

The `PluginInjector` uses reflection to patch colors and styling in other PascalABC.NET plugins at runtime, via processor classes in `YukiTheme.Engine.Plugins`.

## Key Conventions

- Settings are defined as constants in `SettingsConst` (27 settings total)
- Color values support RGBA with opacity throughout
- SVG icons are rendered to bitmaps via `SvgRenderer`/`IconRenderer`
- DPI-aware rendering for wallpapers and stickers
- Windows-only: uses P/Invoke for symlink creation (YukiUtility)