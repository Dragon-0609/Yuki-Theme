using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Yuki_Theme.Core.Controls;
using Yuki_Theme.Core.Themes;
using Brush = System.Drawing.Brush;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;
using FlowDirection = System.Windows.FlowDirection;
using FontFamily = System.Drawing.FontFamily;
using FontStyle = System.Drawing.FontStyle;
using MessageBox = System.Windows.Forms.MessageBox;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace Yuki_Theme.Core.Forms
{
	public partial class ThemeManager : Form
	{
		// private MForm         form;
		private Brush         bg,  fg, fgsp, borderBrush;
		private Color         cbg, cbgclick;
		public  List <ReItem> groups;
		private int           oldIndex = -1;
		private Font          fdef, fcat;

		private Image expandImage, collapseImage;

		public ThemeManager (/*MForm fm*/)
		{
			InitializeComponent ();
			this.StartPosition = FormStartPosition.CenterParent;
			Icon = Helper.GetYukiThemeIcon (new Size (32, 32));
			// form = fm;
			groups = new List <ReItem> ();
			fdef = new Font (new FontFamily ("Lucida Fax"), 9.75f, FontStyle.Regular, GraphicsUnit.Point);
			fcat = new Font (new FontFamily ("Lucida Fax"), 11f, FontStyle.Bold, GraphicsUnit.Point);
			API_Actions.onRename = onRename;
			scheme.MouseClick += CheckFolding;
			label1.Text = API.Translate ("theme.manager.old");
			label2.Text = API.Translate ("theme.manager.new");
			closeButton.Text = API.Translate ("messages.buttons.close");
			// scheme.Columns [0].TextAlign = HorizontalAlignment.Center;
		}

		public bool askDelete (string content, string title)
		{
			return MessageBox.Show (content, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
		}

		public object afterAsk (string sel)
		{
			ListViewItem sifr = scheme.SelectedItems [0];
			/*if (form.selectedItem == sel || form.schemes.SelectedItem.ToString () == sel)
			{
				form.schemes.SelectedIndex = 0;
			}*/

			return (object)sifr;
		}

		public void afterDelete (string sel, object sifr)
		{
			scheme.Items.Remove ((ListViewItem)sifr);
			// form.schemes.Items.Remove (sel);
		}

		public void onRename (string from, string to)
		{
			ListViewItem res = scheme.Items.Find (from, true) [0];
			ReItem reit = (ReItem)res;
			reit.SetName (to);
			/*int indx = form.schemes.Items.IndexOf (from);
			bool needToReSelect = false;
			if (form.schemes.SelectedIndex == indx)
			{
				form.preventFromUpdate = true;
				form.schemes.SelectedIndex = 0;
				needToReSelect = true;
			}

			form.schemes.Items [indx] = to;
			if (needToReSelect)
				form.schemes.SelectedIndex = indx;
			scheme.Invalidate ();
			form.schemes.Invalidate ();*/
		}

		#region Events

		private void close_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void add_Click (object sender, EventArgs e)
		{
			/*if (form.selform == null)
				form.selform = new SelectionForm ();
			form.selform.textBox1.Text = "";
			form.selform.comboBox1.Items.Clear ();
			foreach (object item in form.schemes.Items)
			{
				form.selform.comboBox1.Items.Add (item);
			}

			form.selform.comboBox1.SelectedIndex = 0;
			if (scheme.SelectedItems.Count > 0 && (scheme.SelectedItems [0] is ReItem))
			{
				ReItem re = (ReItem)scheme.SelectedItems [0];
				if (!re.isGroup)
				{
					form.selform.comboBox1.SelectedItem = re.Name;
				}
			}

			if (form.selform.ShowDialog () == DialogResult.OK)
			{
				string from = form.selform.comboBox1.SelectedItem.ToString ();
				string to = form.selform.textBox1.Text;
				if (from != to)
				{
					if (!API.add (from, to))
					{
						form.schemes.Items.Add (to);
						form.schemes.SelectedItem = to;
					}

					DialogResult = DialogResult.OK;
				} else
				{
					API_Actions.showError (API.Translate ("messages.name.equal.message"), API.Translate ("messages.name.equal.title"));
				}
			}*/
		}

		private void remove_Click (object sender, EventArgs e)
		{
			if (scheme.SelectedItems.Count > 0 && scheme.SelectedItems [0] is ReItem)
			{
				ReItem item = (ReItem)scheme.SelectedItems [0];
				API.Remove (item.Name, askDelete, afterAsk, afterDelete);
			}
		}

		private void rename_btn_Click (object sender, EventArgs e)
		{
			if (scheme.SelectedItems.Count > 0)
			{
				RenameForm rf = new RenameForm ();
				rf.fromTBox.Text = rf.toTBox.Text = scheme.SelectedItems [0].Text.Substring (7);
				if (rf.ShowDialog (this) == DialogResult.OK)
				{
					if (rf.toTBox.Text.Length > 0)
					{
						if (rf.toTBox.Text != rf.fromTBox.Text)
						{
							API.Rename (rf.fromTBox.Text, rf.toTBox.Text);
						} else
						{
							MessageBox.Show (API.Translate ("messages.name.notchanged"), API.Translate ("download.canceled.title"),
							                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
						}
					} else
					{
						MessageBox.Show (API.Translate ("messages.name.invalid"), API.Translate ("messages.name.invalid.short"),
						                 MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}

				rf.Dispose ();
			}
		}

		private void regenerate_Click (object sender, EventArgs e)
		{
			string name = ((ReItem)scheme.SelectedItems [0]).Name;
			string namep = Helper.ConvertNameToPath (name);
			string pth = "";
			string pth2 = "";
			string npath = "";
			string format = "";

			bool isOldExists = File.Exists (PathGenerator.PathToFile (namep, true)); 
			pth = PathGenerator.PathToFile (namep, isOldExists);
			npath = PathGenerator.PathToFile (namep, !isOldExists);
			format = isOldExists ? "old" : "new";
			

			API.CopyTheme (name, namep, npath, out pth2, false);
			API_Actions.ReGenerateTheme (npath, pth, name, name, true);
			File.Delete (pth);
			API.themeInfos [name].isOld = API_Actions.IsOldTheme (pth);
			((ReItem)scheme.SelectedItems [0]).isOld = API.themeInfos [name].isOld;
			scheme.Invalidate ();

			// if ((string)form.schemes.SelectedItem == name) // Reload theme (extension, path)
			// {
				API.SelectTheme (name);
			// }

			MessageBox.Show (API.Translate ("messages.regeneration.completed.fromto", name, format),
			                 API.Translate ("messages.regeneration.completed.title"));
		}

		private void scheme_SelectedIndexChanged (object sender, EventArgs e)
		{
			var a = Assembly.GetExecutingAssembly ();
			remove.Visible = rename_btn.Visible = regenerate.Visible = false;
			if (scheme.SelectedItems.Count > 0)
			{
				if (!(scheme.SelectedItems [0] is ReItem)) return;
				if (oldIndex != -1 && oldIndex < scheme.Items.Count)
				{
					scheme.Items [oldIndex].BackColor = cbg;
				}

				oldIndex = scheme.SelectedIndices [0];
				ReItem re = (ReItem)scheme.SelectedItems [0];
				re.BackColor = cbgclick;
				if (re.ParentGroup != null && !re.ParentGroup.Name.Equals (API.Translate ("Default"))
				                           && !re.ParentGroup.Name.Equals (
					                              "doki theme", StringComparison.OrdinalIgnoreCase))
				{
					remove.Visible = rename_btn.Visible = regenerate.Visible = true;
				}
			}
		}

		private void ThemeManager_Shown (object sender, EventArgs e)
		{
			add.BackColor = scheme.BackColor =
				BackColor = closeButton.BackColor = mask_1.BackColor = mask_2.BackColor = cbg = Helper.bgColor;

			add.ForeColor = remove.ForeColor = regenerate.ForeColor = rename_btn.ForeColor = scheme.ForeColor = ForeColor = Helper.fgColor;

			add.FlatAppearance.MouseOverBackColor = remove.FlatAppearance.MouseOverBackColor =
				regenerate.FlatAppearance.MouseOverBackColor =
					rename_btn.FlatAppearance.MouseOverBackColor = closeButton.FlatAppearance.MouseOverBackColor = Helper.bgClick;

			panel1.BackColor = Helper.bgBorder;
			panel2.BackColor = panel2_2.BackColor = Helper.fgKeyword;

			bg = new SolidBrush (BackColor);
			fg = new SolidBrush (ForeColor);
			fgsp = new SolidBrush (Helper.fgKeyword);
			borderBrush = new SolidBrush (Helper.bgBorder);

			cbgclick = Helper.bgClick;
			loadSVG ();
			remove.Visible = rename_btn.Visible = regenerate.Visible = false;
		}

		private void ThemeManager_Load (object sender, EventArgs e)
		{
		}

		private void scheme_ItemSelectionChanged (object sender, ListViewItemSelectionChangedEventArgs e)
		{
		}

		private void ThemeManager_FormClosing (object sender, FormClosingEventArgs e)
		{
			expandImage?.Dispose ();
			collapseImage?.Dispose ();
		}

		#endregion

		private void scheme_DrawColumnHeader (object sender, DrawListViewColumnHeaderEventArgs e)
		{
			e.Graphics.FillRectangle (bg, e.Bounds);

			e.Graphics.DrawString (e.Header.Text, e.Font, fg, e.Bounds);
		}

		private void scheme_DrawItem (object sender, DrawListViewItemEventArgs e)
		{
			if (e.Item is ReItem)
			{
				ReItem re = (ReItem)e.Item;
				e.Graphics.FillRectangle (re.isGroup ? bg : new SolidBrush (re.BackColor), e.Bounds);

				Rectangle rec = e.Bounds;
				if (re.isGroup)
				{
					Size sz = MeasureString (re.Text, re.Font);
					rec = new Rectangle (((e.Bounds.Width - 10) / 2) - (sz.Width / 2), e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
				} else
				{
					Brush brush = re.isOld ? borderBrush : fgsp;
					int thickness = 10;
					int thickness_internal = 2;
					e.Graphics.FillPolygon (brush, GetThemeFigure (re.isOld, e.Bounds, thickness, thickness_internal));
				}

				rec.Y += (rec.Height - scheme.Font.Height); // To draw in center
				e.Graphics.DrawString (re.Text, re.isGroup ? fcat : fdef, re.isGroup ? fgsp : fg, rec);
				if (re.isGroup)
				{
					Image imgToDraw = null;
					if (re.isHidden)
					{
						imgToDraw = expandImage;
					} else
					{
						imgToDraw = collapseImage;
					}

					e.Graphics.DrawImage (imgToDraw, new System.Drawing.Point (e.Bounds.Width - (fcat.Height + 7), rec.Y));
				}
			} else
				e.DrawDefault = true;
		}

		public void ResetList ()
		{
			List <ReItem> items = new List <ReItem> ();
			foreach (ReItem group in groups)
			{
				if (!group.isHidden)
					group.SortChildren ();
			}

			foreach (ReItem reItem in groups)
			{
				items.Add (reItem);
				if (!reItem.isHidden)
					items.AddRange (reItem.children.ToArray ());
			}

			scheme.Items.Clear ();
			scheme.Items.AddRange (items.ToArray ());
		}

		private void loadSVG ()
		{
			var a = Assembly.GetExecutingAssembly ();
			string adda = Helper.IsDark (Helper.bgColor) ? "" : "_dark";
			Helper.RenderSvg (add, Helper.LoadSvg ("add" + adda, a));
			Helper.RenderSvg (remove, Helper.LoadSvg ("remove" + adda, a));
			Helper.RenderSvg (rename_btn, Helper.LoadSvg ("edit" + adda, a));
			Helper.RenderSvg (regenerate, Helper.LoadSvg ("cwmPermissionEdit", a), false, Size.Empty, true, Helper.fgKeyword);

			Size size = new Size (fcat.Height, fcat.Height);
			expandImage = Helper.RenderSvg (size, Helper.LoadSvg ("findAndShowNextMatches" + adda, a), false, default, false, default);

			collapseImage = Helper.RenderSvg (size, Helper.LoadSvg ("findAndShowPrevMatches" + adda, a), false, default, false,
			                                  default);
		}

		private Size MeasureString (string candidate, Font fnt)
		{
			var formatted = new FormattedText (
				candidate, CultureInfo.CurrentCulture,
				FlowDirection.LeftToRight,
				new Typeface (new System.Windows.Media.FontFamily (fnt.FontFamily.Name), FontStyles.Normal, FontWeights.Normal,
				              FontStretches.Normal),
				fnt.Size, System.Windows.Media.Brushes.Black, new NumberSubstitution (), TextFormattingMode.Display
			);

			return new Size (Convert.ToInt32 (formatted.Width), Convert.ToInt32 (formatted.Height));
		}

		private void CheckFolding (object sender, MouseEventArgs e)
		{
			if (scheme.SelectedIndices.Count == 1 && scheme.SelectedIndices [0] >= 0 && scheme.SelectedItems [0] is ReItem)
			{
				ReItem re = (ReItem)scheme.SelectedItems [0];
				if (re.isGroup && e.X > scheme.Columns [0].Width - (fcat.Height + 14))
				{
					re.isHidden = !re.isHidden;
					ResetList ();
					scheme.Invalidate ();
				}
			}
		}

		private PointF [] GetThemeFigure (bool isOld, Rectangle Bounds, int thickness, int thickness_internal)
		{
			if (isOld)
				return GetOldThemeFigure (Bounds, thickness, thickness_internal);
			else
				return GetNewThemeFigure (Bounds, thickness, thickness_internal);
		}

		private PointF [] GetOldThemeFigure (Rectangle Bounds, int thickness, int thickness_internal)
		{
			return new PointF []
			{
				new PointF (Bounds.X, Bounds.Y), new PointF (Bounds.X + thickness, Bounds.Y),
				new PointF (Bounds.X + thickness, Bounds.Y + Bounds.Height),
				new PointF (Bounds.X, Bounds.Y + Bounds.Height),
				new PointF (Bounds.X, Bounds.Y + Bounds.Height - thickness_internal),
				new PointF (Bounds.X + thickness - thickness_internal, Bounds.Y + Bounds.Height - thickness_internal),
				new PointF (Bounds.X + thickness - thickness_internal, Bounds.Y + thickness_internal),
				new PointF (Bounds.X, Bounds.Y + thickness_internal),
			};
		}

		private PointF [] GetNewThemeFigure (Rectangle Bounds, int thickness, int thickness_internal)
		{
			return new PointF []
			{
				new PointF (Bounds.X, Bounds.Y), new PointF (Bounds.X + thickness, Bounds.Y),
				new PointF (Bounds.X + thickness, Bounds.Y + Bounds.Height),
				new PointF (Bounds.X, Bounds.Y + Bounds.Height),
				new PointF (Bounds.X, Bounds.Y + Bounds.Height - thickness_internal),
				new PointF (Bounds.X + thickness - thickness_internal, Bounds.Y + Bounds.Height - thickness_internal),
				new PointF (Bounds.X + thickness - thickness_internal, Bounds.Y + thickness_internal),
				new PointF (Bounds.X, Bounds.Y + thickness_internal),
				new PointF (Bounds.X, Bounds.Y + thickness_internal * 3),
				new PointF (Bounds.X + thickness - thickness_internal * 2.5f, Bounds.Y + thickness_internal * 3),
				new PointF (Bounds.X + thickness - thickness_internal * 2.5f, Bounds.Y + Bounds.Height - thickness_internal * 3),
				new PointF (Bounds.X, Bounds.Y + Bounds.Height - thickness_internal * 3),
			};
		}
	}
}