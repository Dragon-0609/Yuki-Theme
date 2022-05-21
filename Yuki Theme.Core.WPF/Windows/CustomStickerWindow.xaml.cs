using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Brush = System.Windows.Media.Brush;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class CustomStickerWindow : Window
	{
		public int result;
		
		public CustomStickerWindow ()
		{
			InitializeComponent ();
			WPFHelper.windowForDialogs = this;
			WPFHelper.checkDialog = Save;
		}

		public void SetColors (Brush bg, Brush fg)
		{
			Background = ImagePath.Background = bg;
			Foreground = ImagePath.Foreground = fg;
		}
		
		private bool Save ()
		{
			bool canReturn = false;

			string from = ImagePath.Text;
			
			if (File.Exists (from))
			{
				Settings.customSticker = from;
				canReturn = true;
			} else
			{
				CLI_Actions.showError (CLI.Translate ("messages.name.equal.message"), CLI.Translate ("messages.name.equal.title"));
			}
			
			return canReturn;
		}

		private void Initialization (object sender, RoutedEventArgs e)
		{
			string add = Helper.IsDark (Helper.bgColor) ? "" : "_dark";
			WPFHelper.SetSVGImage (ImagePathButton, "moreHorizontal" + add);
		}


		private void SelectImage (object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "PNG (*.png)|*.png";
			if (openFileDialog.ShowDialog () == true)
				ImagePath.Text = openFileDialog.FileName;
		}
	}
}