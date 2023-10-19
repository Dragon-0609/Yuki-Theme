using System.Windows;
using System.Windows.Media;
using YukiTheme.Components;

namespace YukiTheme.Engine;

public class StickerManager : WindowManager
{
	private StickerWindow _sticker;

	public override void Show()
	{
		_sticker = new StickerWindow()
		{
			AlignX = AlignmentX.Right,
			AlignY = AlignmentY.Bottom,
			BorderOutlineX = 20,
			BorderOutlineY = 35,
		};
		var targetForm = IDEAlterer.Instance.Form1;
		_sticker.SetOwner(targetForm);
		_sticker.AutoSize = false;
		_sticker.Show();
		_sticker.Focus();
		IDEAlterer.Instance.Form1.Focus();
		UpdateSticker();
		_sticker.LoadData();
		_sticker.ResetPosition();
		PluginEvents.Instance.StickerMarginReset += () =>
		{
			_sticker.LoadData();
			_sticker.ResetPosition();
		};
	}

	public override bool IsWindowNull() => _sticker == null;

	public override void SetVisibility(Visibility visibility)
	{
		_sticker.Visibility = visibility;
	}

	public void UpdateSticker()
	{
		if (!IsWindowNull())
		{
			if (IDEAlterer.HasSticker)
			{
				_sticker.SetImage(IDEAlterer.GetStickerWPF);
				_sticker.SetSize();
			}

			UpdateOpacity();
		}

		_sticker.ResetPosition();
		UpdateVisibility();
	}

	protected override bool CanShow() => IDEAlterer.CanShowSticker;

	private void UpdateOpacity()
	{
		if (!IsWindowNull())
		{
			_sticker.Opacity = 1;
		}
	}

	public override void ReloadSettings()
	{
		UpdateVisibility();
	}
}