# Yuki Theme

This program is for people, who want to customize PascalABC.NET IDE. By this program you can select and export default popular color schemes like: `Darcula`, `Monokai` and etc.
Also, it includes `Doki Theme`, with over 50 themes, I think you will find the best girl.

Or, you can make your own color scheme by clicking `plus` button. Also, you can import your color scheme from any JetBrains IDE, by clicking `Import`.

Before 

![Before](./Screenshots/screen.png)

After

![After](./Screenshots/screen1.png)

## Installation

There're 2 types of `Yuki Theme`. `Program (.exe)` and `Plugin (.dll)`.
I recommend you to use `Plugin` version, but if there is something that crashes PascalABC.NET, you can use `Program` version. 


If you want to use `Plugin`:

Download the [latest release](https://github.com/Dragon-0609/Yuki-Theme/releases/latest) of plugin and extract it in `PascalABC.NET` directory.

If you want to use `Program`:

Download the [latest release](https://github.com/Dragon-0609/Yuki-Theme/releases/latest) of program and extract the zip file to anywhere, after that open `Yuki Theme.exe`.

---

## What is difference between `Program` and `Plugin`?
Well, both of them are `Yuki Theme`. The difference is in integration of them with PascalABC.NET IDE.

If you use program version, you can change color syntax of the IDE, but it has limitations:

- [ ] Can set background image
- [ ] Can change color of some UI parts
- [x] You have to restart PascalABC.NET to change theme

`Plugin` version:
- [x] Can set background image
- [x] Can change color of some UI parts
- [x] Easily change theme at runtime

---

## Documentation
- [Configuration](#configuration)
- [Default Themes](#default-themes)
- [Doki Theme](#doki-theme)
- [Custom Themes](#custom-themes)

### Configuration

You can access the settings menu by clicking `Settings` button. Here's some fields, that you can set:

- [Path to PascalABC.NET](#path-to-pascalabcnet)
- [Active Scheme](#active-scheme)
- [Remember Active Scheme](#remember-active-scheme)
- [Ask if there are other themes in PascalABC directory](#ask-if-there-are-other-themes-in-pascalabc-directory)
- [Do action, if there are other themes](#do-action-if-there-are-other-themes)
- [Setting Mode](#setting-mode)
- [Check Update](#check-update)

#### Path to PascalABC.NET (You don't need it in `plugin` version):
It's necessary to export the scheme to the IDE.

#### Active Scheme
It will be shown in next program opening.

#### Remember Active Scheme
It sets current scheme to active scheme, so it will be shown in next program opening

#### Ask if there are other themes in PascalABC directory (You don't need it in `plugin` version)
It asks on exporting scheme to the IDE, if there are other themes in `Highlighting` directory inside `PascalABC.NET` directory

#### Do action if there are other themes  (You don't need it in `plugin` version)

If you uncheck `Ask if there are other themes in PascalABC directory`, the selected action will be done. There're 3 actions:
- Delete (old scheme)
- Import and Delete (old scheme)
- Ignore (old scheme)

#### Setting Mode
This checkbox is for custom coloring. It has 2 values: `Light` and `Advanced`.
`Light` is the easiest way to change colors. It shows only main syntax colors and applies to duplicate colors by itself. There're 4 types of comment colors. In `Light` mode it's shown as 1 color.
`Advanced` shows all colors. Also, there're duplicate colors. For example: there're 4 colors for Comments and etc.

#### Check Update
If the checkbox is checked, the programm will check updates in every program run. If there's update, the program will notify you.

---

### Default Themes
- Darcula (from JetBrains IDEA)
- Dracula
- Github Dark
- Github Light
- Monokai Dark
- Monokai Light
- Nightshade
- Oblivion
- Shades of Purple

![Themes](./Screenshots/screen2.png)

---

## Doki Theme

You can choose themes from various, Anime, Manga, or Visual Novels from [`Doki Theme`](https://github.com/doki-theme/doki-theme-jetbrains):

- Blend S
- Daily Life with a Monster Girl
- DanganRonpa
- Doki-Doki Literature Club
- Don't Toy With Me, Miss Nagatoro
- Fate/Type-Moon
- Future Diary
- Gate
- High School DxD
- Jahy-sama Will Not Be Discouraged!
- Kakegurui
- Kill La Kill
- KonoSuba
- Love Live!
- Lucky Star
- Miss Kobayashi's Dragon Maid
- Monogatari
- NekoPara
- Neon Genesis Evangelion
- OreGairu
- Quintessential Quintuplets
- Re:Zero
- Steins Gate
- Sword Art Online
- Yuru Camp

![Themes](./Screenshots/screen3.png)
---

### Custom Themes
You can create your own theme by pressing `Add` button. In there you can choose Name of the theme and default scheme for copy. After that, you can change colors and background image of the theme.
Also, you can import your favourite color scheme from any JetBrains IDE. Click to `Import` button and select the file of the scheme.

![Themes](./Screenshots/screen4.png)

---

### Attributions
Was inspired by [Doki Theme](https://github.com/doki-theme/doki-theme-jetbrains) <br>
Project uses [Fast Colored Text Box](https://github.com/PavelTorgashov/FastColoredTextBox), [Cyotek ColorPicker](https://github.com/cyotek/Cyotek.Windows.Forms.ColorPicker), [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json), [RJ ComboBox](https://github.com/RJCodeAdvance/Custom-ComboBox), [SVG.NET](https://github.com/svg-net/SVG), [Bootstrap Icons](https://icons.getbootstrap.com/), [Color Slider](https://github.com/fabricelacharme/ColorSlider), [WindowsAPICodePack](https://www.nuget.org/packages/WindowsAPICodePack), [FlatNumericUpDown](https://github.com/r-aghaei/FlatNumericUpDownExample).


---

## Contributions?

You probably have good ideas, so feel free to submit your feedback as [an issue](https://github.com/Dragon-0609/Yuki-Theme/issues/new). I'll read your feedback, so don't be shy!

Help make this plugin better!


---

<div align="center">
    <img src="./Yuki Theme.Core/yuki128_2.png" ></img>
</div>



