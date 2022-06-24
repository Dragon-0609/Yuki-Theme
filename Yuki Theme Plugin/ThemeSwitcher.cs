using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Yuki_Theme.Core;
using Yuki_Theme.Core.Controls;
using Yuki_Theme_Plugin.Controls.DockStyles;

namespace Yuki_Theme_Plugin
{
	public class ThemeSwitcher
	{
		private YukiTheme_VisualPascalABCPlugin plugin;

		internal Panel      panel_bg;
		internal CustomList themeList;
		internal Label      lbl;

		internal bool     needToReturnTheme        = false;
		internal bool     needToFullExportTheme    = false;
		internal string   oldThemeNameForPreExport = "";
		internal DateTime prevPreExportTime;
		internal bool     hideBG = false;

		public ThemeSwitcher (YukiTheme_VisualPascalABCPlugin yukiPlugin)
		{
			plugin = yukiPlugin;
		}

		internal void SwitchTheme ()
		{
			if (!plugin.ideComponents.fm.Controls.ContainsKey ("Custom Panel Switcher"))
			{
				// if (mf == null || mf.IsDisposed)
				// {
				plugin.RememberCurrentEditor ();

				panel_bg = new CustomPanel (0);
				panel_bg.Name = "Custom Panel Switcher";
				needToReturnTheme = true;
				needToFullExportTheme = false;
				prevPreExportTime = DateTime.Now;
				Font fnt = new Font (FontFamily.GenericSansSerif, 10, GraphicsUnit.Point);

				lbl = new Label ();
				lbl.BackColor = YukiTheme_VisualPascalABCPlugin.bg;
				lbl.ForeColor = YukiTheme_VisualPascalABCPlugin.clr;
				lbl.Font = fnt;
				lbl.Text = API.Translate ("plugin.themes");
				lbl.TextAlign = ContentAlignment.MiddleCenter;
				lbl.Size = new Size (200, 25);

				themeList = new CustomList ();
				themeList.BackColor = YukiTheme_VisualPascalABCPlugin.bgdef;
				themeList.ForeColor = YukiTheme_VisualPascalABCPlugin.clr;
				themeList.BorderStyle = BorderStyle.None;
				themeList.list = API.schemes.ToArray ();
				themeList.SearchText ("");
				themeList.BorderStyle = BorderStyle.None;
				themeList.Font = fnt;
				themeList.ItemHeight = themeList.Font.Height;
				themeList.Size = new Size (200, 300);
				themeList.MouseMove += ThemeListMouseHover;
				themeList.InitSearchBar ();

				themeList.searchBar.Size = new Size (themeList.Size.Width, themeList.searchBar.Size.Height);
				themeList.Size = new Size (themeList.Size.Width, themeList.Size.Height - (themeList.searchBar.Size.Height + 2));

				panel_bg.Location = Point.Empty;
				panel_bg.Size = plugin.ideComponents.fm.ClientSize;
				themeList.DrawMode = DrawMode.OwnerDrawFixed;
				themeList.DrawItem += list_1_DrawItem;
				int x = (panel_bg.Width / 2) - (themeList.Width / 2);
				int y = (panel_bg.Height / 2) - ((themeList.Height + themeList.searchBar.Size.Height + lbl.Size.Height + 8) / 2);

				lbl.Location = new Point (x, y - 33);
				themeList.searchBar.Location = new Point (x, lbl.Location.Y + lbl.Size.Height + 2);

				themeList.Location = new Point (x, themeList.searchBar.Location.Y + themeList.searchBar.Size.Height + 4);

				if (themeList.Items.Contains (Helper.currentTheme))
					themeList.SelectedItem = Helper.currentTheme;
				else
					themeList.SelectedIndex = 0;
				themeList.selectionindex = themeList.SelectedIndex;
				themeList.SelectedIndexChanged += ThemeListOnSelectedIndexChanged;
				themeList.AccessibleName = themeList.SelectedItem.ToString ();
				panel_bg.Click += CloseOnClick;
				oldThemeNameForPreExport = themeList.AccessibleName;

				panel_bg.Controls.Add (themeList);
				panel_bg.Controls.Add (lbl);
				panel_bg.Controls.Add (themeList.searchBar);

				setBorder (themeList, lbl, themeList.searchBar);

				plugin.ideComponents.fm.Controls.Add (panel_bg);
				panel_bg.BringToFront ();
				themeList.searchBar.Focus ();

				// } else
				// {
				// 	MessageBox.Show (API.Translate ("plugin.close"));
				// }
			}
		}

		#region Methods For Theme Switcher

		private void list_1_DrawItem (object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0) return;
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				e = new DrawItemEventArgs (e.Graphics, e.Font, e.Bounds,
				                           e.Index, e.State ^ DrawItemState.Selected,
				                           e.ForeColor, YukiTheme_VisualPascalABCPlugin.bgClick2);
			} else if (e.Index == themeList.selectionindex)
			{
				e = new DrawItemEventArgs (e.Graphics, e.Font, e.Bounds,
				                           e.Index, e.State,
				                           e.ForeColor, YukiTheme_VisualPascalABCPlugin.bgClick);
			}

			e.DrawBackground ();
			e.Graphics.DrawString (((ListBox)sender).Items [e.Index].ToString (), e.Font, YukiTheme_VisualPascalABCPlugin.clrBrush,
			                       e.Bounds);

			e.DrawFocusRectangle ();
		}

		internal void CloseOnClick (object sender, EventArgs e)
		{
			if (panel_bg != null)
			{
				if (Settings.showPreview)
				{
					if (needToReturnTheme)
					{
						needToFullExportTheme = true;
						PreviewTheme (themeList.AccessibleName, oldThemeNameForPreExport);
						needToReturnTheme = false;
						needToFullExportTheme = false;
					} else
					{
						hideBG = !API.currentTheme.HasWallpaper;
						plugin.stickerControl.Visible = Settings.swSticker && API.currentTheme.HasSticker;
					}
				}

				plugin.ideComponents.fm.Controls.Remove (panel_bg);
				panel_bg?.Dispose ();
				themeList?.searchBar.Dispose ();
				themeList?.Dispose ();
				panel_bg = null;
				if (plugin.tmpImage1 != null)
				{
					plugin.tmpImage1.Dispose ();
					plugin.tmpImage1 = null;
				}

				if (plugin.tmpImage2 != null)
				{
					plugin.tmpImage2.Dispose ();
					plugin.tmpImage2 = null;
				}

				plugin.ReFocusCurrentEditor ();
			}
		}

		private void ThemeListOnSelectedIndexChanged (object sender, EventArgs e)
		{
			if (themeList.SelectedIndex >= 0)
			{
				if (themeList.SelectedItem.ToString () != themeList.AccessibleName)
				{
					bool cnd = API.SelectTheme (themeList.SelectedItem.ToString ());

					if (cnd)
					{
						API.selectedItem = API.nameToLoad;
						API_Actions.ifHasImage2 = plugin.ifHsImage;
						API_Actions.ifHasSticker2 = plugin.ifHsSticker;
						API_Actions.ifDoesntHave2 = plugin.ifDNIMG;
						API_Actions.ifDoesntHaveSticker2 = plugin.ifDNSTCK;
						API.Restore (false, null);
						API.Export (plugin.tmpImage1, plugin.tmpImage2, plugin.ReloadLayout, plugin.ReleaseResources);

						API_Actions.ifHasImage2 = null;
						API_Actions.ifHasSticker2 = null;
						API_Actions.ifDoesntHave2 = null;
						API_Actions.ifDoesntHaveSticker2 = null;
					}
				}

				needToReturnTheme = false;
				CloseOnClick (sender, e);
			}
		}

		private void ThemeListMouseHover (object sender, EventArgs e)
		{
			InvalidateItem ();
			if (Settings.showPreview)
			{
				string nm = themeList.Items [themeList.selectionindex].ToString ();
				if ((DateTime.Now - prevPreExportTime).TotalMilliseconds >= 25 &&
				    nm != oldThemeNameForPreExport) // Preview Theme if delay is more than 25 milliseconds
				{
					prevPreExportTime = DateTime.Now;
					PreviewTheme (nm, oldThemeNameForPreExport);
					lbl.BackColor = YukiTheme_VisualPascalABCPlugin.bg;
					lbl.ForeColor = YukiTheme_VisualPascalABCPlugin.clr;
					themeList.BackColor = YukiTheme_VisualPascalABCPlugin.bgdef;
					themeList.ForeColor = YukiTheme_VisualPascalABCPlugin.clr;
					oldThemeNameForPreExport = themeList.Items [themeList.selectionindex].ToString ();
				}
			}
		}

		private void PreviewTheme (string name, string oldName)
		{
			if (name != oldName)
			{
				if (API.SelectTheme (name))
				{
					API.Restore ();
					hideBG = !API.currentTheme.HasWallpaper;
					plugin.stickerControl.Visible = Settings.swSticker && API.currentTheme.HasSticker;
					if (needToFullExportTheme)
					{
						API.Preview (SyntaxType.NULL, true, plugin.ReloadLayoutLight);
					} else
					{
						SyntaxType type =
							ShadowNames.GetSyntaxByExtension (Path.GetExtension (plugin.ideComponents.fm.CurrentCodeFileDocument.FileName));
						if (type != SyntaxType.Pascal)
						{
							API.Preview (type, true, null);                                   // Not to reload layout
							API.Preview (SyntaxType.Pascal, false, plugin.ReloadLayoutLight); // Pascal theme is necessary for UI
						} else
						{
							API.Preview (type, true, plugin.ReloadLayoutLight);
						}
					}
				}
			}
		}

		private void InvalidateItem ()
		{
			Point point = themeList.PointToClient (Cursor.Position);
			int index = themeList.IndexFromPoint (point);
			//Do any action with the item
			themeList.UpdateHighlighting (index);
		}

		void setBorder (Control ctl, Control ctl2, Control ctl3)
		{
			Panel pan = new Panel ();
			pan.BorderStyle = BorderStyle.None;
			pan.Size = new Size (ctl.ClientRectangle.Width + 2,
			                     ctl.ClientRectangle.Height + ctl2.ClientRectangle.Height + ctl3.ClientRectangle.Height + 6);
			pan.Location = new Point (ctl.Left - 1, ctl.Top - 1);
			pan.BackColor = YukiTheme_VisualPascalABCPlugin.bgInactive;
			pan.Parent = ctl.Parent;
			ctl.Parent = pan;
			ctl2.Parent = pan;
			ctl3.Parent = pan;

			ctl3.Location = new Point (1, Math.Abs (ctl2.Top - ctl3.Top));
			ctl2.Location = new Point (1, 1);
			ctl.Location = new Point (1, ctl3.Bottom + 1);
		}

		internal void SetFocus ()
		{
			if (panel_bg != null && !panel_bg.IsDisposed)
			{
				panel_bg.Visible = true;
				panel_bg.BringToFront ();
				themeList.searchBar.Focus ();
			}
		}

		#endregion
	}
}