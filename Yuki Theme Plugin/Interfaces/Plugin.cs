using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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

		public class Model
		{

		}
	}
}