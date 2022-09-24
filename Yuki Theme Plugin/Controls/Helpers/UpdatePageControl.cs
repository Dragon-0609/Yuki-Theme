using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VisualPascalABC;
using Yuki_Theme.Core;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme_Plugin.Controls.Helpers
{
	[ComVisible (true)]
	public class UpdatePageControl : WebBrowserControl
	{
		private TableLayoutPanel updateTable;
		private WebBrowser       updateBrowser;
		private TextBox          updateUrl;
		private Button           updatePrev;

		public UpdatePageControl ()
		{
			updateTable = (TableLayoutPanel)GetValueFromField ("tableLayoutPanel1");
			updateBrowser = (WebBrowser)GetValueFromField ("webBrowser1");
			updateUrl = (TextBox)GetValueFromField ("tbUrl");
			updatePrev = (Button)GetValueFromField ("btnPrev");
			updateBrowser.AllowWebBrowserDrop = false;
			updateBrowser.IsWebBrowserContextMenuEnabled = false;
			Icon = Yuki_Theme.Core.Helper.GetYukiThemeIcon (new Size (32, 32));
			updateBrowser.Navigated += (sender, e) =>
			{
				HtmlDocument doc = updateBrowser.Document;

				try
				{
					string srtr = doc.Domain;
				} catch (Exception)
				{
					string err_element =
						"<h1 style=' text-align: center; height: 100vh; width: 100vw; display: flex; flex-direction: column; justify-content: center;'><div>Something went wrong. Try to load the page later</div></h1>";
					string bg = ColorTranslator.ToHtml (YukiTheme_VisualPascalABCPlugin.Colors.bgdef);
					string clr = ColorTranslator.ToHtml (YukiTheme_VisualPascalABCPlugin.Colors.clr);
					string htmlHeader =
						"<html><head><title>Error></title><meta content=\"IE=Edge,11,10,9,7,8\" http-equiv=\"X-UA-Compatible\"/> <style>body { background-color: " +
						bg + ";color: " + clr + ";}</style>  </head>";
					updateBrowser.Document?.Write (htmlHeader + $"<body style='overflow: hidden;'>{err_element}</body></html>");
				}
			};

			updatePrev.Visible = false;
			updateTable.Controls.Remove (updateBrowser);
			Controls.Remove (updateTable);

			updateBrowser.ObjectForScripting = new SiteScripting ();

			Controls.Add (updateBrowser);
			// tableLayoutPanel1
		}

		public new void OpenPage (string title, string url)
		{
			TabText = title;
			updateBrowser.Navigate (url);
		}

		private object GetValueFromField (string fieldName)
		{
			FieldInfo field = typeof (WebBrowserControl).GetField (fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			return field.GetValue (this);
		}

		public void UpdateTheme ()
		{
			updateBrowser.Document.InvokeScript ("resetTheme", null);
		}
	}

	[ComVisible (true)]
	public class SiteScripting
	{
		public string GetThemeName ()
		{
			string name = DefaultThemes.getCategory (Yuki_Theme.Core.Helper.currentTheme);
			name = name != "Doki Theme" ? null : Yuki_Theme.Core.Helper.currentTheme;
			return name;
		}

		public void OpenURL (string url)
		{
			Process.Start (url);
		}

		public void SetTheme (string theme)
		{
			CentralAPI.Current.SelectTheme (theme);

			CentralAPI.Current.selectedItem = CentralAPI.Current.nameToLoad;
			CentralAPI.Current.Restore (false);
			CentralAPI.Current.ExportTheme (null, null,
				YukiTheme_VisualPascalABCPlugin.plugin.ReloadLayout,
				YukiTheme_VisualPascalABCPlugin.plugin._presenter.ReleaseResources, true);
		}

		public string ReadImage ()
		{
			string b64 = "";
			ImageConverter imageConverter = new ImageConverter ();
			byte[] buffer = (byte[])imageConverter.ConvertTo (YukiTheme_VisualPascalABCPlugin.img, typeof (byte[]));
			string base64 = Convert.ToBase64String (buffer);
			b64 = "'data:image/png;base64," + base64 + "'";

			return b64;
		}
	}
}