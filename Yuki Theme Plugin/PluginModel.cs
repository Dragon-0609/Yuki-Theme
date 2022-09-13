using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Yuki_Theme.Core;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.WPF.Interfaces;
using Yuki_Theme.Core.WPF.Windows;
using Yuki_Theme_Plugin.Interfaces;
namespace Yuki_Theme_Plugin
{
	public class PluginModel : Plugin.Model
	{

		#region Sticker

		private StickerWindow _stickerWindow;

		internal Image Sticker;

		internal override void InitSticker (Form form)
		{
			_stickerWindow = StickerWindow.CreateStickerControl (form);
			_stickerWindow.Show ();
		}

		internal override void LoadSticker ()
		{
			string pth = Path.Combine (Settings.pascalPath, "Highlighting", "sticker.png");
			Sticker = Image.FromFile (pth);
			_stickerWindow.LoadImage (Sticker);
		}

		internal override void ReloadSticker ()
		{
			_stickerWindow.LoadSticker ();
		}

		internal override void ChangeSticker (Image image)
		{
			_stickerWindow.LoadImage (image);
		}

		internal override void ResetStickerPosition ()
		{
			_stickerWindow.SetStickerSize ();
			_stickerWindow.ResetPosition ();
		}

		internal override void CheckStickerVisibility ()
		{
			_stickerWindow.Visibility = Settings.swSticker && CentralAPI.Current.currentTheme.HasSticker ? Visibility.Visible : Visibility.Hidden;
		}

		internal override void Release () => _stickerWindow.Release ();

		internal override void ReadData () => _stickerWindow.ReadData ();
		internal override void UpdateLocation () => _stickerWindow.ResetPosition ();

		internal override void StickerPositioning (bool available) => _stickerWindow.IsEnabled = available;

		#endregion
	}
}