using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
namespace Yuki_Theme_Plugin.Interfaces
{
	public class Plugin
	{

		internal interface IPresenter
		{

		}

		internal interface IView
		{

		}

		internal interface IHelper
		{
			void ResetBrush (ref Brush brush, Color color);
			void ResetPen (ref Pen pen, Color color, float width, PenAlignment alignment);
			void ResetBrushesAndPens ();
			void AddTabWithUrl (DockPanel tabControl, string title, string url);
			void openUpdate ();
			bool IsUpdated ();
			void ShowLicense ();
			void InvokeUI (Delegate method);
		}

		public abstract class Model
		{

			internal abstract void InitSticker (Form form);
			internal abstract void LoadSticker ();
			internal abstract void ReloadSticker ();
			internal abstract void ResetStickerPosition ();
			internal abstract void ChangeSticker (Image image);
			internal abstract void CheckStickerVisibility ();
			internal abstract void Release ();
			internal abstract void ReadData ();
			internal abstract void UpdateLocation ();
			internal abstract void StickerPositioning (bool available);
		}
	}
}