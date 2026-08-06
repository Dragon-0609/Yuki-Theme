using System;
using System.Windows;
using System.Windows.Media;
using YukiTheme.Engine;
using YukiTheme.Tools;

namespace YukiTheme.Components.Windows;

public partial class PopupWindow : SnapWindow
{
	private Action _primaryAction;
	private Action _secondaryAction;

	private bool _autoSizeMessage = true;

	public PopupWindow()
	{
		InitializeComponent();
		AlignX = AlignmentX.Right;
		AlignY = AlignmentY.Bottom;
		GetColors();

		ColorChanger.Instance.UpdatedColors += GetColors;
	}

	public bool AutoSizeMessage
	{
		get => _autoSizeMessage;
		set
		{
			_autoSizeMessage = value;
			if (_autoSizeMessage)
			{
				SizeToContent = SizeToContent.Height;
			}
			else
			{
				SizeToContent = SizeToContent.Manual;
				Height = MinHeight;
			}

			Width = _autoSizeMessage ? 400 : 320;

			if (IsLoaded)
				ResetPosition();
		}
	}

	protected override void OnClosed(EventArgs e)
	{
		base.OnClosed(e);
		// ColorChanger.Instance.UpdatedColors -= GetColors;
	}

	public void SetContent(string title, string message)
	{
		TitleText.Text = title;
		MessageText.Text = message;
	}

	public void SetPrimaryButton(string text, Action action)
	{
		PrimaryButton.Content = text;
		_primaryAction = action;
	}

	public void SetSecondaryButton(string text, Action action)
	{
		SecondaryButton.Content = text;
		SecondaryButton.Visibility = Visibility.Visible;
		_secondaryAction = action;
	}

	public void HideSecondaryButton()
	{
		SecondaryButton.Visibility = Visibility.Collapsed;
	}

	private void GetColors()
	{
		WpfConverter.ConvertGuiColorsNBrushes();
		if (WpfColorContainer.Instance == null) return;
		Background = WpfColorContainer.Instance.BackgroundBrush;
		Foreground = WpfColorContainer.Instance.ForegroundBrush;
		TitleText.Foreground = WpfColorContainer.Instance.BeginEndBrush;
		Tag = WpfColorContainer.Instance;
		Border.BorderBrush = WpfColorContainer.Instance.BorderBrush;
		InvalidateVisual();
	}

	private void PrimaryButton_OnClick(object sender, RoutedEventArgs e)
	{
		_primaryAction?.Invoke();
		Close();
	}

	private void SecondaryButton_OnClick(object sender, RoutedEventArgs e)
	{
		_secondaryAction?.Invoke();
		Close();
	}

	private void CloseButton_OnClick(object sender, RoutedEventArgs e)
	{
		Close();
	}

	public static void ShowWindow(string title, string message, string button1, Action button1Action,
		bool autoSize = false)
	{
		WpfConverter.ConvertGuiColorsNBrushes();
		PopupWindow window = new PopupWindow();
		window.AutoSizeMessage = autoSize;
		window.SetContent(title, message);
		window.SetPrimaryButton(button1, button1Action);
		window.HideSecondaryButton();
		window.SetOwner(IDEAlterer.Instance.Form1);
		window.ResetPosition();
		window.Show();
		DelayedCall.Run(0.02f, window.ResetPosition);
		DelayedCall.Run(0.1f, window.ResetPosition);
	}

	public static void ShowWindow(string title, string message, string button1, Action button1Action,
		string button2, Action button2Action, bool autoSize = false)
	{
		WpfConverter.ConvertGuiColorsNBrushes();
		PopupWindow window = new PopupWindow();
		window.AutoSizeMessage = autoSize;
		window.SetContent(title, message);
		window.SetPrimaryButton(button1, button1Action);
		window.SetSecondaryButton(button2, button2Action);
		window.SetOwner(IDEAlterer.Instance.Form1);
		window.ResetPosition();
		window.Show();
		DelayedCall.Run(0.1f, () => window.ResetPosition());
	}
}