using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
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
using Size = System.Drawing.Size;

namespace Yuki_Theme.Core.Forms
{
	public partial class ThemeManager : Form
	{
		private MForm         form;
		private Brush         bg,  fg, fgsp;
		private Color         cbg, cbgclick;
		public  List <ReItem> groups;
		private int           oldIndex = -1;
		private Font          fdef, fcat;

		public ThemeManager (MForm fm)
		{
			InitializeComponent ();
			this.StartPosition = FormStartPosition.CenterParent;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (MForm));
			Icon = ((Icon)(resources.GetObject ("$this.Icon")));
			form = fm;
			groups = new List <ReItem> ();
			fdef = new Font (new FontFamily ("Lucida Fax"), 9.75f, FontStyle.Regular, GraphicsUnit.Point);
			fcat = new Font (new FontFamily ("Lucida Fax"), 11f, FontStyle.Bold, GraphicsUnit.Point);
			CLI.onRename = onRename;
			// scheme.Columns [0].TextAlign = HorizontalAlignment.Center;
		}

		private void button2_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void button3_Click (object sender, EventArgs e)
		{
			if (form.selform == null)
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
				CLI.add (form.selform.comboBox1.SelectedItem.ToString (), form.selform.textBox1.Text);
				form.schemes.Items.Add (form.selform.textBox1.Text);
				form.schemes.SelectedItem = form.selform.textBox1.Text;
				DialogResult = DialogResult.OK;
			}
		}

		public bool askDelete (string content, string title)
		{
			return MessageBox.Show (content, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
		}

		public object afterAsk (string sel)
		{
			ListViewItem sifr = scheme.SelectedItems [0];
			if (form.selectedItem == sel || form.schemes.SelectedItem.ToString () == sel)
			{
				form.schemes.SelectedIndex = 0;
			}

			return (object)sifr;
		}

		public void afterDelete (string sel, object sifr)
		{
			scheme.Items.Remove ((ListViewItem)sifr);
			form.schemes.Items.Remove (sel);
		}

		public void onRename (string from, string to)
		{
			ListViewItem res = scheme.Items.Find (from, true) [0];
			res.Text = "       " + to;
			form.schemes.Items [form.schemes.Items.IndexOf (from)] = to;
		}

		private void remove_Click (object sender, EventArgs e)
		{
			if (scheme.SelectedItems.Count > 0)
			{
				CLI.remove (scheme.SelectedItems [0].Text.Substring (7), askDelete, afterAsk, afterDelete);
			}
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
				if (re.rgroupItem != null && !re.rgroupItem.Name.Equals ("default", StringComparison.OrdinalIgnoreCase)
				                          && !re.rgroupItem.Name.Equals (
					                             "doki theme", StringComparison.OrdinalIgnoreCase))
				{
					remove.Visible = rename_btn.Visible = regenerate.Visible = true;
				}
			}
		}

		private void ThemeManager_Shown (object sender, EventArgs e)
		{
			add.BackColor = scheme.BackColor = BackColor = button2.BackColor = cbg = Helper.bgColor;

			add.ForeColor = remove.ForeColor = regenerate.ForeColor = rename_btn.ForeColor = scheme.ForeColor = ForeColor = Helper.fgColor;

			add.FlatAppearance.MouseOverBackColor = remove.FlatAppearance.MouseOverBackColor =
				regenerate.FlatAppearance.MouseOverBackColor =
					rename_btn.FlatAppearance.MouseOverBackColor = button2.FlatAppearance.MouseOverBackColor = Helper.bgClick;

			bg = new SolidBrush (BackColor);
			fg = new SolidBrush (ForeColor);
			fgsp = new SolidBrush (Helper.fgKeyword);
			cbgclick = Helper.bgClick;
			loadSVG ();
			remove.Visible = rename_btn.Visible = regenerate.Visible = false;
		}

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

				// TextRenderer.GetIntTextFormatFlags(flags)

				// e.DrawText();
				Rectangle rec = e.Bounds;
				if (re.isGroup)
				{
					Size sz = MeasureString (re.Text, re.Font);
					rec = new Rectangle (((e.Bounds.Width - 10) / 2) - (sz.Width / 2), e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
				} else
				{
					Brush brush = re.isOld ? Brushes.Maroon : Brushes.Orchid;
					int thickness = 10;
					e.Graphics.FillPolygon (
						brush,
						new PointF []
						{
							new PointF (e.Bounds.X, e.Bounds.Y), new PointF (e.Bounds.X + thickness, e.Bounds.Y),
							new PointF (e.Bounds.X + thickness, e.Bounds.Y + e.Bounds.Height),
							new PointF (e.Bounds.X, e.Bounds.Y + e.Bounds.Height)
						});
				}

				e.Graphics.DrawString (re.Text, re.isGroup ? fcat : fdef, re.isGroup ? fgsp : fg, rec);
			} else
				e.DrawDefault = true;
		}

		private void ThemeManager_Load (object sender, EventArgs e)
		{
		}

		private void scheme_ItemSelectionChanged (object sender, ListViewItemSelectionChangedEventArgs e)
		{
		}

		public void sortItems ()
		{
			scheme.Items.Clear ();
			foreach (ReItem item in groups)
			{
				item.SortChildren ();
			}

			foreach (ReItem reItem in groups)
			{
				scheme.Items.Add (reItem);
				scheme.Items.AddRange (reItem.childs.ToArray ());
			}
		}

		private void loadSVG ()
		{
			var a = Assembly.GetExecutingAssembly ();
			string adda = Helper.IsDark (Helper.bgColor) ? "" : "_dark";
			Helper.RenderSvg (add, Helper.LoadSvg ("add" + adda, a));
			Helper.RenderSvg (remove, Helper.LoadSvg ("remove" + adda, a));
			Helper.RenderSvg (rename_btn, Helper.LoadSvg ("edit" + adda, a));
			Helper.RenderSvg (regenerate, Helper.LoadSvg ("cwmPermissionEdit", a), false, Size.Empty, true, Helper.fgKeyword);
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

		private void rename_btn_Click (object sender, EventArgs e)
		{
			if (scheme.SelectedItems.Count > 0)
			{
				RenameForm rf = new RenameForm ();
				rf.fromTBox.Text = scheme.SelectedItems [0].Text.Substring (7);
				if (rf.ShowDialog (this) == DialogResult.OK)
				{
					if (rf.toTBox.Text.Length > 0)
					{
						if (rf.toTBox.Text != rf.fromTBox.Text)
						{
							CLI.rename (rf.fromTBox.Text, rf.toTBox.Text);
						} else
						{
							MessageBox.Show ("You didn't change the name", "Canceled",
							                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
						}
					} else
					{
						MessageBox.Show ("Invalid name. You must enter at least 1 character", "Invalid name",
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
			if (File.Exists (Path.Combine (CLI.currentPath, "Themes", $"{namep}{Helper.FILE_EXTENSTION_OLD}")))
			{
				pth = Path.Combine (CLI.currentPath, "Themes", $"{namep}{Helper.FILE_EXTENSTION_OLD}");
				npath = Path.Combine (CLI.currentPath, "Themes", $"{namep}{Helper.FILE_EXTENSTION_NEW}");
				format = "new";
			} else
			{
				pth = Path.Combine (CLI.currentPath, "Themes", $"{namep}{Helper.FILE_EXTENSTION_NEW}");
				npath = Path.Combine (CLI.currentPath, "Themes", $"{namep}{Helper.FILE_EXTENSTION_OLD}");
				format = "old";
			}

			CLI.CopyTheme (name, namep, npath, out pth2, false);
			CLI.ReGenerateTheme (npath, pth, name, name, true);
			File.Delete (pth);
			CLI.oldThemeList [name] = !CLI.IsOldTheme (pth);
			((ReItem)scheme.SelectedItems [0]).isOld = CLI.oldThemeList [name]; 
			scheme.Invalidate();
			MessageBox.Show ($"{name} has been regenerated to {format} format", "Regeneration compeleted");
		}
	}
}