using System.Windows;
using System.Windows.Media;
using Yuki_Theme.Core.WPF.Windows;
using YukiTheme.Style.Controls;

namespace YukiTheme.Engine;

public class StickerManager
{
	private StickerWindow _sticker;

	private bool hidden = false;

	public void Show()
	{
		_sticker = new StickerWindow()
		{
			AlignX = AlignmentX.Right,
			AlignY = AlignmentY.Bottom,
			borderOutlineX = 20,
			borderOutlineY = 35,
		};
		var targetForm = IDEAlterer.Alterer.Form1;
		_sticker.SetOwner(targetForm);
		_sticker.AutoSize = false;
		UpdateSticker();
		UpdateOpacity();
		_sticker.ResetPosition();
		_sticker.Show();
		_sticker.Focus();
		IDEAlterer.Alterer.Form1.Focus();
		_sticker.ResetPosition();
	}

	public void UpdateSticker()
	{
		if (_sticker != null)
		{
			if (IDEAlterer.HasSticker)
			{
				if (hidden)
				{
					hidden = false;
					_sticker.Visibility = Visibility.Visible;
				}

				_sticker.SetImage(IDEAlterer.GetStickerWPF);
				_sticker.SetSize();
			}
			else
			{
				hidden = true;
				_sticker.Visibility = Visibility.Hidden;
			}

			UpdateOpacity();
		}
	}

	private void UpdateOpacity()
	{
		if (_sticker != null)
		{
			_sticker.Opacity = 1;
		}
	}
}