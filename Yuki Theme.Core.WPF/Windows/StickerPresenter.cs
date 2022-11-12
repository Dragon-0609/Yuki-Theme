using System.Drawing;
using System.Windows;
using Yuki_Theme.Core.WPF.Interfaces;
using Size = System.Drawing.Size;

namespace Yuki_Theme.Core.WPF.Windows
{
	public class StickerPresenter : Sticker.IPresenter
	{
		private Sticker.IModel _stickerModel;
		private Sticker.IView  _stickerView;

		public static bool AutoRefocus = true;
		private StickerWindow _window;

		public StickerPresenter (Sticker.IModel stickerModel, Sticker.IView stickerView)
		{
			_stickerModel = stickerModel;
			_stickerView = stickerView;
			_window = (StickerWindow) _stickerView;
		}

		public void SetStickerSize ()
		{
			if (!_stickerModel.IsRenderImageNull ())
			{
				Size dimensionSize = Helper.CalculateDimension (_stickerModel.GetRenderImage ().Size);
				_stickerView.SetSize (dimensionSize);
			}
		}

		public void LoadSticker ()
		{
			if (!_stickerModel.IsRenderImageNull ())
			{
				_stickerModel.ReleaseRenderImage ();
			}

			Visibility visibility = Visibility.Visible;
			
			if (Settings.swSticker)
			{
				if (Settings.useCustomSticker && Settings.customSticker.Exist ())
				{
					_stickerModel.LoadCustomSticker ();
				} else
				{
					if (!_stickerModel.IsOriginalImageNull ())
					{
						_stickerModel.PrepareRenderImage ();
					} else
					{
						_stickerModel.ReleaseRenderImage ();
						visibility = Visibility.Hidden;
					}
				}

				if (!_stickerModel.IsRenderImageNull ())
				{
					SetStickerSize ();
					_stickerView.ChangeImage (_stickerModel.GetRenderImage ());
					UpdatePosition ();
				} else
				{
					
					visibility = Visibility.Hidden;
				}
			} else
			{
				visibility = Visibility.Hidden;
			}

			_stickerView.ChangeVisibility (visibility);
			if (AutoRefocus)
				RefocusParent();
		}

		public bool StickerAvailable ()
		{
			return Settings.swSticker && !_stickerModel.IsOriginalImageNull () && !_stickerModel.IsRenderImageNull ();
		}

		/// <summary>
		/// Doesn't Work.
		/// </summary>
		private void RefocusParent()
		{
			if (!_stickerView.IsTargetWindowNull())
			{
				_window.Logger.Write("Sticker parent is Window");
				_stickerView.GetTargetWindow().Activate();
			}else if (!_stickerView.IsTargetFormNull())
			{
				_stickerView.GetTargetForm().Activate();
				_window.Logger.Write("Sticker parent is Form");
			}
			else
			{
				_window.Logger.Write("Sticker doesn't have a parent");
			}
		}
		
		private void UpdatePosition () => _stickerView.ResetPosition ();

		public void LoadImage (Image image)
		{
			_stickerModel.LoadImage (image);
			LoadSticker ();
		}
	}
}