using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Yuki_Theme_Plugin
{
	public partial class Props : Form
	{
		private bool    changing = false;
		public  Control root;
		private int     max      = 0;

		public Props ()
		{
			InitializeComponent ();
		}

		private void propertyGrid1_SelectedObjectsChanged (object sender, EventArgs e)
		{
			Control ct = (Control) propertyGrid1.SelectedObject;
			changing = true;
			listBox1.Items.Clear ();
			if (ct.Parent != null)
			{
				bool can = true;
				Control pr = ct.Parent;
				List <string> pas = new List <string> ();
				while (can)
				{
					pas.Add ($"[{pr.Name}] < ({pr.GetHashCode ()})");
					if (pr.Parent == null) can = false;
				}

				for (int i = pas.Count - 1; i >= 0; i--)
				{
					listBox1.Items.Add (pas [i]);
				}
			}

			if (ct.Controls.Count > 0)
			{
				AddControls (ct.Controls);
			}

			changing = false;
			listBox1.Items.Add (max.ToString ());
			max = 0;
		}

		private void AddControls (Control.ControlCollection col)
		{
			foreach (Control control in col)
			{
				listBox1.Items.Add ($"[{control.Name}] > ({control.GetHashCode ()})");
				max++;
				// AddControls (control.Controls);
			}
		}

		private Tuple <bool, Control> Find (string hash, Control.ControlCollection collection)
		{
			for (var i = 0; i < collection.Count; i++)
			{
				if (collection[i].GetHashCode ().ToString () == hash)
				{
					return new Tuple <bool, Control> (true, collection [i]);
				}
			}

			return new Tuple <bool, Control> (false, null);
		}

		private void listBox1_SelectedIndexChanged (object sender, EventArgs e)
		{
			if (changing) return;
			if (!listBox1.SelectedItem.ToString ().Contains ("(")) return;
			string sel = listBox1.SelectedItem.ToString ();
			string hash = sel.Split (new char [] {'(', ')'}) [1];
			// MessageBox.Show (hash);
			if (hash != root.GetHashCode ().ToString ())
			{
				Tuple <bool, Control> fg = new Tuple <bool, Control> (false, null);
				Control cl = (Control) propertyGrid1.SelectedObject;
				string name = sel.Split (new char [] {'[', ']'}) [1];
				if (cl.Controls.ContainsKey (name))
				{
					
				} else
				{
					if (sel.Contains (">"))
					{
						MessageBox.Show ("CN: " + cl.Controls.Count);
						fg = Find (hash, cl.Controls);
						MessageBox.Show ($"RT: {fg.Item1}, {fg.Item2.GetHashCode ()}");
					} else if (sel.Contains ("<"))
					{
						fg = new Tuple <bool, Control> (true, cl.Parent);
					}

					if (fg.Item1)
					{
						propertyGrid1.SelectedObject = fg.Item2;
					} else
					{
						MessageBox.Show ("Can't find");
					}
				}
			} else
			{
				propertyGrid1.SelectedObject = root;
			}
		}
	}
}