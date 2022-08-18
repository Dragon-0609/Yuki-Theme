using System.Drawing;
using System.Windows.Media;
using Yuki_Theme.Core.WPF.Interfaces;

namespace Yuki_Theme.Core.WPF.Windows
{
	public class StickerModel : Sticker.IModel
	{
		private Image originalImage = null;
		private Image renderImage   = null;

		public void LoadImage (Image image)
		{
			originalImage = image;
		}

		public bool IsRenderImageNull ()
		{
			return renderImage == null;
		}

		public void ReleaseRenderImage ()
		{
			if (!IsRenderImageNull ())
				renderImage.Dispose ();
			renderImage = null;
		}

		public bool IsOriginalImageNull ()
		{
			return originalImage == null;
		}

		public void LoadCustomSticker ()
		{
			renderImage = Image.FromFile (Settings.customSticker);
		}

		public void PrepareRenderImage ()
		{
			if (API_Base.Current.currentTheme.StickerOpacity != 100)
			{
				renderImage = Helper.SetOpacity (originalImage, API_Base.Current.currentTheme.StickerOpacity);
				originalImage.Dispose ();
			} else
				renderImage = originalImage;
		}

		public Image GetRenderImage () => renderImage;
	}
}