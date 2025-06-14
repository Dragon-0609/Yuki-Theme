using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Yuki_Theme_Plugin
{
	public partial class Props : Form
	{
		private bool changing = false;
		public Control root;
		private int max = 0;

		public Props()
		{
			InitializeComponent();
		}

		private void propertyGrid1_SelectedObjectsChanged(object sender, EventArgs e)
		{
			Control ct = (Control)propertyGrid1.SelectedObject;
			changing = true;
			listBox1.Items.Clear();
			if (ct.Parent != null)
			{
				bool can = true;
				Control pr = ct.Parent;
				List<string> pas = new List<string>();
				while (can)
				{
					pas.Add($"[{pr.Name}] < ({pr.GetHashCode()})");
					if (pr.Parent == null) can = false;
					else pr = pr.Parent;
				}

				for (int i = pas.Count - 1; i >= 0; i--)
				{
					listBox1.Items.Add(pas[i]);
				}

				GC.Collect();
				GC.WaitForPendingFinalizers();
			}

			if (ct.Controls.Count > 0)
			{
				AddControls(ct.Controls);
			}

			if (propertyGrid1.SelectedObject is DockPane)
			{
				// MessageBox.Show ("Dock Pane");
				DockPane dock = (DockPane)ct;
				int number = 0;
				foreach (DockPane nestedPane in dock.NestedPanesContainer.NestedPanes)
				{
					listBox1.Items.Add($"[{nestedPane.Name}] -{number}- >_ ({nestedPane.GetHashCode()})");
					number++;
				}

				AddControls(dock.Controls);
			}

			changing = false;
			listBox1.Items.Add(max.ToString());
			max = 0;
		}

		private void AddControls(Control.ControlCollection col)
		{
			int u = 0;
			foreach (Control control in col)
			{
				listBox1.Items.Add($"[{control.Name}] -{u}- > ({control.GetHashCode()})");
				max++;
				// MessageBox.Show ($"Item child: {u}");
				GC.Collect();
				GC.WaitForPendingFinalizers();
				u++;
				// AddControls (control.Controls);
			}
		}

		private Tuple<bool, Control> Find(string hash, Control.ControlCollection collection)
		{
			for (var i = 0; i < collection.Count; i++)
			{
				// MessageBox.Show ($"Item: {i}");
				if (collection[i].GetHashCode().ToString() == hash)
				{
					return new Tuple<bool, Control>(true, collection[i]);
				}
			}

			GC.Collect();
			GC.WaitForPendingFinalizers();

			return new Tuple<bool, Control>(false, null);
		}

		private Tuple<bool, object> Find(string hash, NestedPaneCollection collection)
		{
			for (var i = 0; i < collection.Count; i++)
			{
				if (collection[i].GetHashCode().ToString() == hash)
				{
					return new Tuple<bool, object>(true, collection[i]);
				}
			}

			GC.Collect();
			GC.WaitForPendingFinalizers();

			return new Tuple<bool, object>(false, null);
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			// MessageBox.Show ($"Selected Index");
			if (changing || listBox1.SelectedIndex < 0) return;
			if (!listBox1.SelectedItem.ToString().Contains("(")) return;
			string sel = listBox1.SelectedItem.ToString();
			string hash = sel.Split(new char[] { '(', ')' })[1];
			// MessageBox.Show (hash);
			// MessageBox.Show ($"Start to search");
			if (hash != root.GetHashCode().ToString())
			{
				Tuple<bool, Control> fg = new Tuple<bool, Control>(false, null);
				Control cl = (Control)propertyGrid1.SelectedObject;
				string name = sel.Split(new char[] { '[', ']' })[1];
				if (cl.Controls.ContainsKey(name))
				{
					fg = new Tuple<bool, Control>(true, cl.Controls.Find(name, false)[0]);
					if (fg.Item1)
					{
						propertyGrid1.SelectedObject = fg.Item2;
					}
					else
					{
						MessageBox.Show("Can't find");
					}
				}
				else
				{
					if (sel.Contains(">"))
					{
						if (sel.Contains(">_"))
						{
							DockPane dock = (DockPane)propertyGrid1.SelectedObject;
							Tuple<bool, object> fga = Find(hash, dock.NestedPanesContainer.NestedPanes);
							if (fga.Item1)
							{
								propertyGrid1.SelectedObject = fga.Item2;
							}
							else
							{
								MessageBox.Show("Can't find");
							}
						}
						else
						{
							// MessageBox.Show ("CN: " + cl.Controls.Count);
							fg = Find(hash, cl.Controls);
							// MessageBox.Show ($"RT: {fg.Item1}, {fg.Item2.GetHashCode ()}");
							if (fg.Item1)
							{
								propertyGrid1.SelectedObject = fg.Item2;
							}
							else
							{
								MessageBox.Show("Can't find");
							}
						}
					}
					else if (sel.Contains("<"))
					{
						fg = new Tuple<bool, Control>(true, cl.Parent);
						if (fg.Item1)
						{
							propertyGrid1.SelectedObject = fg.Item2;
						}
						else
						{
							MessageBox.Show("Can't find");
						}
					}
				}
			}
			else
			{
				MessageBox.Show($"Can't find. Retrieving to root");
				propertyGrid1.SelectedObject = root;
			}

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		private void listBox1_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.V)
			{
				Control ctrl = (Control)propertyGrid1.SelectedObject;
				ctrl.Paint += CtrlOnPaint;
				ctrl.Invalidate();
				MessageBox.Show($"Added");
			}
			else if (e.KeyData == Keys.X)
			{
				Control ctrl = (Control)propertyGrid1.SelectedObject;
				ctrl.Paint -= CtrlOnPaint;
				ctrl.Invalidate();
				MessageBox.Show($"Removed");
			}
		}

		private void CtrlOnPaint(object sender, PaintEventArgs e)
		{
			e.Graphics.FillRectangle(Brushes.Indigo, e.ClipRectangle);
		}
	}
}