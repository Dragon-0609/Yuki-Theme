using System.Drawing;

namespace Yuki_Theme.Core.WPF.Interfaces
{
	public class Main
	{
		public abstract  class Model
		{
			public Image wallpaperRender;
			public Image wallpaperOriginal;
			
			public Image sticker;

			public Rectangle calculatedWallpaperSize;
		}

		public interface IView
		{
			void SelectDefaultTheme ();
			
			void OpenSettings ();
		}

		public interface IPresenter
		{
			void SetAPIActions ();
		}
	}
}