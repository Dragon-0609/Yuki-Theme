using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Interop;
using VisualPascalABC;
using WeifenLuo.WinFormsUI.Docking;
using Yuki_Theme.Core;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.WPF;
using Yuki_Theme.Core.WPF.Windows;
using Yuki_Theme_Plugin.Controls.Helpers;
using Yuki_Theme_Plugin.Interfaces;
namespace Yuki_Theme_Plugin
{
	public class PluginHelper : Plugin.IHelper, IDispatcher
	{
		internal const string UPDATE_TITLE = "Yuki Theme Update";

		private IdeComponents _components;

		public PluginHelper (IdeComponents components)
		{
			_components = components;
		}
		
		public void ResetBrush (ref Brush brush, Color color)
		{
			if (brush != null) brush.Dispose ();
			brush = new SolidBrush (color);
		}

		public void ResetPen (ref Pen pen, Color color, float width, PenAlignment alignment)
		{
			if (pen != null) pen.Dispose ();
			pen = new Pen (color, width) { Alignment = alignment };
		}
		
		public void ResetBrushesAndPens ()
		{
			ResetBrush (ref YukiTheme_VisualPascalABCPlugin.Colors.bgdefBrush, YukiTheme_VisualPascalABCPlugin.Colors.bgdef);
			ResetBrush (ref YukiTheme_VisualPascalABCPlugin.Colors.bgBrush, YukiTheme_VisualPascalABCPlugin.Colors.bg);
			ResetBrush (ref YukiTheme_VisualPascalABCPlugin.Colors.selectionBrush, YukiTheme_VisualPascalABCPlugin.Colors.bgSelection);
			ResetBrush (ref YukiTheme_VisualPascalABCPlugin.Colors.bgClickBrush, YukiTheme_VisualPascalABCPlugin.Colors.bgClick);
			ResetBrush (ref YukiTheme_VisualPascalABCPlugin.Colors.bgClick3Brush, YukiTheme_VisualPascalABCPlugin.Colors.bgClick3);
			ResetBrush (ref YukiTheme_VisualPascalABCPlugin.Colors.bgInactiveBrush, YukiTheme_VisualPascalABCPlugin.Colors.bgInactive);
			ResetBrush (ref YukiTheme_VisualPascalABCPlugin.Colors.clrBrush, YukiTheme_VisualPascalABCPlugin.Colors.clr);
			ResetBrush (ref YukiTheme_VisualPascalABCPlugin.Colors.typeBrush, YukiTheme_VisualPascalABCPlugin.Colors.bgType);

			ResetPen (ref YukiTheme_VisualPascalABCPlugin.Colors.separatorPen, YukiTheme_VisualPascalABCPlugin.Colors.bgClick3, 1, default);
			ResetPen (ref YukiTheme_VisualPascalABCPlugin.Colors.bgBorderPen, YukiTheme_VisualPascalABCPlugin.Colors.bgBorder, 8, default);
		}

		public void AddTabWithUrl (DockPanel tabControl, string title, string url)
		{
			WebBrowserControl tp = null; //new UpdatePageControl();
			FieldInfo field = typeof (Form1).GetField ("OpenBrowserDocuments", BindingFlags.Instance | BindingFlags.NonPublic);
			Dictionary <string, WebBrowserControl> OpenBrowserDocuments =
				(Dictionary <string, WebBrowserControl>)field.GetValue (_components.fm);
			if (!OpenBrowserDocuments.TryGetValue (title, out tp))
			{
				tp = new UpdatePageControl ();
				tp.OpenPage (title, url);
				_components.fm.AddWindowToDockPanel (tp, tabControl, tp.Dock, DockState.Document, tp.IsFloat, null, 0);
				OpenBrowserDocuments.Add (title, tp);
			} else if (tp is UpdatePageControl)
			{
				tp.Activate ();
			} else
			{
				MessageBox.Show (CentralAPI.Current.Translate ("plugin.browser.error"));
			}
		}

		
		public void openUpdate ()
		{
			string version = SettingsConst.CURRENT_VERSION.ToString ("0.0").Replace (',', '.');
			if (SettingsConst.CURRENT_VERSION_ADD != null && SettingsConst.CURRENT_VERSION_ADD.Length > 1)
				version += "-" + SettingsConst.CURRENT_VERSION_ADD;
			AddTabWithUrl (_components.fm.MainDockPanel, UPDATE_TITLE, $"https://dragon-0609.github.io/Yuki-Theme/updates/{version}");
		}

		public bool IsUpdated ()
		{
			bool updated = false;
			int inst = Helper.RecognizeInstallationStatus ();
			if (inst == 1)
			{
				openUpdate ();
				updated = true;
				Helper.DeleteInstallationStatus ();
			}

			return updated;
		}

		public void ShowLicense ()
		{
			if (!Settings.license)
			{
				WPFHelper.InitAppForWinforms ();
				
				LicenseWindow licenseWindow = new LicenseWindow ()
				{
					Background = WPFHelper.bgBrush,
					Foreground = WPFHelper.fgBrush,
					BorderBrush = WPFHelper.borderBrush,
					Tag = WPFHelper.GenerateTag
				};
				WindowInteropHelper helper = new WindowInteropHelper (licenseWindow);
				helper.Owner = _components.fm.Handle;
				
				licenseWindow.Closed += (_, _) =>
				{
					GC.Collect ();
					GC.WaitForPendingFinalizers ();
				};

				licenseWindow.ShowDialog ();
				
				Settings.license = true;
				Settings.database.UpdateData (SettingsConst.LICENSE, "True");
			}
		}
		
		public void InvokeUI (Delegate method)
		{
			_components.fm.BeginInvoke (method);
		}
		
	}
}