using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using YukiTheme.Components.StatusBar;
using YukiTheme.Engine;
using YukiTheme.Tools;
using Size = System.Drawing.Size;

namespace YukiTheme.Components.Windows;

public partial class SettingsPanel : UserControl
{
	private static string[] _themes =>
		DefaultThemeNames.Themes.Union(DokiThemeNames.Themes).Union(YukiThemeNames.Themes).ToArray();

	private bool _resetStickerMargin;
	public SettingsPanelUtilities Utilities;

	private string _customStickerPath;
	private string _customBackgroundPath;

	public SettingsPanel()
	{
		InitializeComponent();
		_resetStickerMargin = false;
		Utilities = new SettingsPanelUtilities(this);
		Utilities.LoadSettings();
		YukiThemeNames.Update();
		Themes.ItemsSource = _themes;
		SelectCurrentTheme();
		LoadSvg();
		_customStickerPath = DataSaver.Load(SettingsConst.CUSTOM_STICKER_PATH, "");
		_customBackgroundPath = DataSaver.Load(SettingsConst.CUSTOM_BACKGROUND_PATH, "");

		string build = "";
		if (SettingsConst.CURRENT_BUILD.Length > 0)
		{
			build = $"Build: {SettingsConst.CURRENT_BUILD}";
		}
		
		VersionText.Text = $"Version: {YukiTheme_VisualPascalABCPlugin.VersionStatic} {(build)}";
		CompletionFontSameAsEditor_Changed(this, null);
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

	private void ChooseCustomBackground(object sender, RoutedEventArgs e)
	{
		CustomStickerWindow imageSelector = new CustomStickerWindow
		{
			Tag = Tag,
			Background = Background,
			Foreground = Foreground,
		};
		imageSelector.ChangeTitle("Custom Background");
		imageSelector.WindowStartupLocation = WindowStartupLocation.CenterOwner;

		WindowInteropHelper helper = new WindowInteropHelper(imageSelector);
		helper.Owner = IDEAlterer.Instance.GetSettingsParent().Handle;

		imageSelector.ImagePath.Text = _customBackgroundPath;
		if (imageSelector.ShowDialog() == true)
		{
			_customBackgroundPath = imageSelector.ImagePath.Text;
		}
	}

	internal void SaveSettings()
	{
		bool customStickerEnabled = DataSaver.Load(SettingsConst.USE_CUSTOM_STICKER, false);
		bool customBackgroundEnabled = DataSaver.Load(SettingsConst.USE_CUSTOM_BACKGROUND, false);
		int alignIndex = DataSaver.Load(SettingsConst.CUSTOM_BACKGROUND_ALIGN, 1);
		
		Utilities.SaveSettings();
		string prevStickerPath = DataSaver.Load(SettingsConst.CUSTOM_STICKER_PATH, "");
		string prevBackgroundPath = DataSaver.Load(SettingsConst.CUSTOM_BACKGROUND_PATH, "");
		
		DataSaver.Save(SettingsConst.CUSTOM_STICKER_PATH, _customStickerPath);
		DataSaver.Save(SettingsConst.CUSTOM_BACKGROUND_PATH, _customBackgroundPath);

		if (_resetStickerMargin)
		{
			Utilities.ResetStickerMargin();
		}

		if (prevStickerPath != _customStickerPath ||
		    customStickerEnabled != DataSaver.Load(SettingsConst.USE_CUSTOM_STICKER, false))
		{
			Utilities.ReLoadCustomSticker();
		}

		if (prevBackgroundPath != _customBackgroundPath ||
		    customBackgroundEnabled != DataSaver.Load(SettingsConst.USE_CUSTOM_BACKGROUND, false))
		{
			Utilities.ReLoadCustomBackground();
		}else if (alignIndex != DataSaver.Load(SettingsConst.CUSTOM_BACKGROUND_ALIGN, 1))
		{
			Utilities.ReLoadCustomBackgroundAlign();
		}

		_resetStickerMargin = false;
		if (Themes.SelectedItem.ToString() != NameBar.ThemeName)
			PluginEvents.Instance.OnThemeChanged(Themes.SelectedItem.ToString());
	}

	public void UpdateThemeNames()
	{
		YukiThemeNames.Update();
		Themes.ItemsSource = _themes;
	}

	private void CompletionFontSameAsEditor_Changed(object sender, RoutedEventArgs e)
	{
		Visibility visible = CompletionFontSameAsEditor.IsChecked == true ? Visibility.Collapsed : Visibility.Visible;
		CompletionFontSetPanel.Visibility = visible;
	}
	private void UpdateCheck_Changed(object sender, RoutedEventArgs e)
	{
		
	}

	private void CheckUpdate_Click(object sender, RoutedEventArgs e)
	{
		if (IDEAlterer.UpdateManager.CurrentDownloadWindow != null)
		{
			return;
		}
		
		UpdateCheck.IsEnabled = false;
		StartUpdateSpinner();

		bool startedChecking = IDEAlterer.UpdateManager.Check(result =>
		{
			UpdateCheck.IsEnabled = true;
			StopUpdateSpinner();

			IDEAlterer.UpdateManager.HandleUpdate(result, true);
		});

		if (!startedChecking)
		{
			UpdateCheck.IsEnabled = true;
			StopUpdateSpinner();
		}
	}

	private void StartUpdateSpinner()
	{
		UpdateCheckLabel.SetResourceReference(TextBlock.TextProperty, "settings.general.update.checking");
		UpdateCheckSpinner.Visibility = Visibility.Visible;
		DoubleAnimation animation = new DoubleAnimation(0, 360, new Duration(TimeSpan.FromSeconds(1)))
		{
			RepeatBehavior = RepeatBehavior.Forever
		};
		RotateTransform rotate = ((TransformGroup)UpdateCheckSpinner.RenderTransform)
			.Children.OfType<RotateTransform>().First();

		rotate.BeginAnimation(RotateTransform.AngleProperty, animation);
	}

	private void StopUpdateSpinner()
	{
		UpdateCheckLabel.SetResourceReference(TextBlock.TextProperty, "settings.general.update.check");
		RotateTransform rotate = ((TransformGroup)UpdateCheckSpinner.RenderTransform)
			.Children.OfType<RotateTransform>().First();

		rotate.BeginAnimation(RotateTransform.AngleProperty, null);
		
		UpdateCheckSpinner.Visibility = Visibility.Collapsed;
	}
}