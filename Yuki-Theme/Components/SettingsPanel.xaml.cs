using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using YukiTheme.Engine;
using YukiTheme.Tools;
using Size = System.Drawing.Size;

namespace YukiTheme.Components;

public partial class SettingsPanel : UserControl
{
	private readonly string[] _themes = DefaultThemeNames.Themes.Union(DokiThemeNames.Themes).ToArray();
	private bool _resetStickerMargin;
	public SettingsPanelUtilities Utilities;

	private string _customStickerPath;

	public SettingsPanel()
	{
		InitializeComponent();
		_resetStickerMargin = false;
		Utilities = new SettingsPanelUtilities(this);
		Utilities.LoadSettings();
		Themes.ItemsSource = _themes;
		SelectCurrentTheme();
		LoadSvg();
		_customStickerPath = DatabaseManager.Load(SettingsConst.CUSTOM_STICKER_PATH, "");
		VersionText.Text = $"Version: {YukiTheme_VisualPascalABCPlugin.VersionStatic}";
	}

	private void LoadSvg()
	{
		SetResourceSvg("InfoImage", "balloonInformation");
	}

	private void SetResourceSvg(string name, string source)
	{
		Resources[name] = SvgRenderer
			.RenderSvg(new Size(16, 16), new SvgRenderInfo(SvgRenderer.LoadSvg(source, "Icons."))).ToWPFImage();
	}


	private void SelectCurrentTheme()
	{
		var name = NameBar.ThemeName;
		// IDEConsole.Log(name);
		if (Themes.Items.Contains(name))
			Themes.SelectedItem = name;
		else
			Themes.SelectedIndex = 0;

		Themes.ScrollToCenterOfView(Themes.SelectedItem);
	}

	internal void GetColors()
	{
		Background = WpfColorContainer.Instance.BackgroundBrush;
		Foreground = WpfColorContainer.Instance.ForegroundBrush;
		Themes.Background = Background;
		Themes.Foreground = Foreground;
		Themes.Tag = Tag = WpfColorContainer.Instance;
	}

	private void ResetMargin_Click(object sender, RoutedEventArgs e)
	{
		_resetStickerMargin = true;
	}

	private void ChooseCustomSticker(object sender, RoutedEventArgs e)
	{
		CustomStickerWindow stickerWindow = new CustomStickerWindow
		{
			Tag = Tag,
			Background = Background,
			Foreground = Foreground
		};
		stickerWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

		WindowInteropHelper helper = new WindowInteropHelper(stickerWindow);
		helper.Owner = IDEAlterer.Instance.GetSettingsParent().Handle;

		stickerWindow.ImagePath.Text = _customStickerPath;
		if (stickerWindow.ShowDialog() == true)
		{
			_customStickerPath = stickerWindow.ImagePath.Text;
		}
	}

	internal void SaveSettings()
	{
		bool customStickerEnabled = DatabaseManager.Load(SettingsConst.USE_CUSTOM_STICKER, false);
		Utilities.SaveSettings();
		string prevStickerPath = DatabaseManager.Load(SettingsConst.CUSTOM_STICKER_PATH, "");
		DatabaseManager.Save(SettingsConst.CUSTOM_STICKER_PATH, _customStickerPath);

		if (_resetStickerMargin)
		{
			Utilities.ResetStickerMargin();
		}

		if (prevStickerPath != _customStickerPath ||
		    customStickerEnabled != DatabaseManager.Load(SettingsConst.USE_CUSTOM_STICKER, false))
		{
			Utilities.ReLoadCustomSticker();
		}

		_resetStickerMargin = false;
		if (Themes.SelectedItem.ToString() != NameBar.ThemeName)
			PluginEvents.Instance.OnThemeChanged(Themes.SelectedItem.ToString());
	}
}