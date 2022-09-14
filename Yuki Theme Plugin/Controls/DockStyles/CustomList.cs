using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Yuki_Theme.Core.Controls;

namespace Yuki_Theme_Plugin.Controls.DockStyles
{
	public class CustomList : ListBox
	{
		public CustomText searchBar;

		public string [] list;

		public int selectionindex;

		public CustomList ()
		{
			BorderStyle = BorderStyle.None;
		}


		protected override void OnKeyDown (KeyEventArgs e)
		{
			searchBar.CallKeyDown (e);
		}

		protected override void OnKeyUp (KeyEventArgs e)
		{
			searchBar.CallKeyUp (e);
		}

		protected override void OnKeyPress (KeyPressEventArgs e)
		{
			searchBar.CallKeyPress (e);
		}

		public void InitSearchBar ()
		{
			searchBar = new CustomText ();
			searchBar.BackColor = YukiTheme_VisualPascalABCPlugin.Colors.bg;
			searchBar.ForeColor = YukiTheme_VisualPascalABCPlugin.Colors.clr;
			searchBar.BorderColor = YukiTheme_VisualPascalABCPlugin.Colors.bgBorder;
			searchBar.Font = Font;
			searchBar.KeyUp += (sender, args) =>
			{
				if (args.KeyCode == Keys.Escape)
				{
					((CustomPanel)Parent.Parent).CallClick (); // Close panel
				} else if (args.KeyCode == Keys.Back)
				{
					SearchText (searchBar.Text);
				} else if (args.KeyCode == Keys.Enter || args.KeyCode == Keys.Return)
				{
					if (Items.Count > 0)
					{
						if (selectionindex >= Items.Count)
							SelectedIndex = 0;
						else
							SelectedIndex = selectionindex;
						// ((CustomPanel)Parent).CallClick ();
					}
				} else if (args.KeyCode == Keys.Up)
				{
					UpdateHighlighting (selectionindex - 1);
				} else if (args.KeyCode == Keys.Down)
				{
					UpdateHighlighting (selectionindex + 1);
				} else
				{
					SearchText (searchBar.Text);
				}
			};
		}

		public void SearchText (string str)
		{
			SelectedIndex = -1;
			List <string> nList = new List <string> ();

			if (str.Length == 0)
			{
				nList.AddRange (list);
			} else
			{
				foreach (string item in list)
				{
					if (Contains (item, str, StringComparison.OrdinalIgnoreCase))
					{
						nList.Add (item);
					}
				}
			}

			if (!AreEqual (Items.Cast <string> ().ToArray (), nList.ToArray ()))
			{
				UpdateHighlighting (0);
				Items.Clear ();
				Items.AddRange (nList.ToArray ());
				
			}
		}

		public static bool Contains (string source, string toCheck, StringComparison comp)
		{
			return source?.IndexOf (toCheck, comp) >= 0;
		}

		public void UpdateHighlighting (int index)
		{
			if (index < 0 || index >= Items.Count) return;
			if (selectionindex != index)
			{
				int oldindex = selectionindex;
				selectionindex = index;
				Invalidate (GetItemRectangle (oldindex));
				Invalidate (GetItemRectangle (index));
			}
		}
		
		bool AreEqual(string[] templateArr, string[] dataArr)
		{
			return templateArr.SequenceEqual (dataArr);
		}
	}
}