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
using Brushes = System.Windows.Media.Brushes;
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
			Icon = ((Icon) (resources.GetObject ("$this.Icon")));
			form = fm;
			groups = new List <ReItem> ();
			fdef = new Font (new FontFamily ("Lucida Fax"), 9.75f, FontStyle.Regular, GraphicsUnit.Point);
			fcat = new Font (new FontFamily ("Lucida Fax"), 11f, FontStyle.Bold, GraphicsUnit.Point);
			CLI.onRename = onRename;
			CLI.ErrorRename = ErrorRename;
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
				ReItem re = (ReItem) scheme.SelectedItems [0];
				if (!re.isGroup)
				{
					form.selform.comboBox1.SelectedItem = re.Name;
				}
			}

			if (form.selform.ShowDialog () == DialogResult.OK)
			{
				CLI.add (form.selform.textBox1.Text, form.selform.comboBox1.SelectedItem.ToString ());
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

			return (object) sifr;
		}
		
		public void afterDelete (string sel, object sifr)
		{
			scheme.Items.Remove ((ListViewItem) sifr);
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
				CLI.remove (scheme.SelectedItems[0].Text.Substring (7), askDelete, afterAsk, afterDelete);
			}
		}

		private void scheme_SelectedIndexChanged (object sender, EventArgs e)
		{
			var a = Assembly.GetExecutingAssembly ();
			remove.Enabled = rename_btn.Enabled = false;
			remove.BackColor = rename_btn.BackColor  = Helper.bgBorder;
			if (scheme.SelectedItems.Count > 0)
			{
				if (!(scheme.SelectedItems [0] is ReItem)) return;
				if (oldIndex != -1 && oldIndex < scheme.Items.Count)
				{
					scheme.Items[oldIndex].BackColor = cbg;					
				}

				oldIndex = scheme.SelectedIndices [0];
				ReItem re = (ReItem) scheme.SelectedItems [0];
				re.BackColor = cbgclick;
				if (re.rgroupItem != null && !re.rgroupItem.Name.Equals ("default", StringComparison.OrdinalIgnoreCase)
				                          && !re.rgroupItem.Name.Equals (
					                             "doki theme", StringComparison.OrdinalIgnoreCase))
				{
					remove.Enabled = rename_btn.Enabled = true;
					remove.BackColor = rename_btn.BackColor = cbg;
				}
			}
		}

		private void ThemeManager_Shown (object sender, EventArgs e)
		{
			add.BackColor = scheme.BackColor = BackColor = button2.BackColor = cbg = Helper.bgColor;
			
			add.ForeColor = remove.ForeColor = rename_btn.ForeColor = scheme.ForeColor = ForeColor = Helper.fgColor;

			add.FlatAppearance.MouseOverBackColor = remove.FlatAppearance.MouseOverBackColor =
				rename_btn.FlatAppearance.MouseOverBackColor =
					button2.FlatAppearance.MouseOverBackColor = Helper.bgClick;
				
			bg = new SolidBrush (BackColor);
			fg = new SolidBrush (ForeColor);
			fgsp = new SolidBrush (Helper.fgKeyword);
			cbgclick = Helper.bgClick;
			loadSVG();
			remove.Enabled = rename_btn.Enabled = false;
			remove.BackColor = rename_btn.BackColor = Helper.bgBorder;
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
				ReItem re = (ReItem) e.Item;
				e.Graphics.FillRectangle (re.isGroup ? bg : new SolidBrush (re.BackColor), e.Bounds);
				
				// TextRenderer.GetIntTextFormatFlags(flags)
				
				// e.DrawText();
				Rectangle rec = e.Bounds;
				if(re.isGroup)
				{
					Size sz = MeasureString (re.Text, re.Font);
					rec= new Rectangle (((e.Bounds.Width-10) / 2)-(sz.Width / 2), e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
				}
				e.Graphics.DrawString (re.Text, re.isGroup ? fcat : fdef, re.isGroup ? fgsp : fg, rec);
				
			}else
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
		
		private void loadSVG(){
			var a = Assembly.GetExecutingAssembly ();
        	Helper.renderSVG (add, Helper.loadsvg ("plus-square", a));
            Helper.renderSVG (remove, Helper.loadsvg ("dash-square", a));
            Helper.renderSVG (rename_btn, Helper.loadsvg ("edit", a));
		}

		private Size MeasureString (string candidate, Font fnt)
		{

			var formatted = new FormattedText (
				candidate, CultureInfo.CurrentCulture, 
				FlowDirection.LeftToRight,
				new Typeface(new System.Windows.Media.FontFamily(fnt.FontFamily.Name), FontStyles.Normal,FontWeights.Normal, FontStretches.Normal),
				fnt.Size, Brushes.Black, new NumberSubstitution(), TextFormattingMode.Display
			);

			return new Size (Convert.ToInt32 (formatted.Width), Convert.ToInt32 (formatted.Height));
		}

		private void ErrorRename (string content, string title)
		{
			MessageBox.Show (content, title,
			                 MessageBoxButtons.OK, MessageBoxIcon.Error);
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
	}
}