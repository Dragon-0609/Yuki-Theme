using System.Collections.Generic;
using System.Windows.Forms;
using Yuki_Theme.Core.Controls;
using Yuki_Theme.Core.Database;
using Yuki_Theme.Core.Forms;
using Yuki_Theme_Plugin.Controls;

namespace Yuki_Theme_Plugin
{
	public class ToolBarCamouflage
	{
		private ToolStrip            tools;
		public  List <ToolStripItem> items;
		public  List <ToolItemGroup> groups;
		public  List <string>        itemsToHide;
		public  List <string>        itemsToRight;
		private DatabaseManager      database;
		
		public ToolBarCamouflage (ToolStrip toolStrip)
		{
			tools = toolStrip;
			items = new List <ToolStripItem> ();
			groups = new List <ToolItemGroup> ();
			itemsToHide = new List <string> ();
			itemsToRight = new List <string> ();
			database = new DatabaseManager ();
			Init ();
		}

		public void Init ()
		{
			ToolItemGroup prev = null;
			ToolItemGroup current = new ToolItemGroup ();

			for (var i = 0; i < tools.Items.Count; i++)
			{
				if (current.separator != null)
				{
					prev = current;
					current = new ToolItemGroup ();
					current.prev = prev;
					groups.Add (prev);
				}

				if (tools.Items [i].ToolTipText != null && tools.Items [i].Visible &&
				    tools.Items [i].AccessibleDescription != null && tools.Items [i].AccessibleDescription.Length > 1)
				{
					items.Add (tools.Items [i]);
					current.items.Add (tools.Items [i]);
				}

				if (tools.Items [i] is ToolStripSeparator)
				{
					if (current.separator == null && !current.IsEmpty ())
						current.separator = (ToolStripSeparator) tools.Items [i];
				}
			}

			if (!current.IsEmpty ())
				groups.Add (current);
			groups [groups.Count - 1].isLast = true;
			PopulateList ();
			StartToHide ();
		}

		public void StartToHide ()
		{
			if (items.Count > 0)
			{
				foreach (ToolStripItem item in items)
				{
					item.Visible = true;
					item.Alignment = ToolStripItemAlignment.Left;
				}

				foreach (ToolItemGroup itemGroup in groups)
				{
					if(itemGroup.separator != null && !itemGroup.isLast)
						itemGroup.separator.Visible = true;
				}

				foreach (string s in itemsToHide)
				{
					if (s != null && s.Length > 1)
					{
						foreach (ToolItemGroup itemGroup in groups)
						{
							if (itemGroup.HasItem (s))
							{
								ToolStripItem res = itemGroup.GetItem (s);
								res.Visible = false;
							}
						}
					}
				}

				foreach (ToolItemGroup itemGroup in groups)
				{
					if (itemGroup.isAllHidden ())
					{
						if (itemGroup.isLast)
						{
							if (itemGroup.prev.separator != null)
								itemGroup.prev.separator.Visible = false;
						} else
						{
							if (itemGroup.separator != null)
								itemGroup.separator.Visible = false;
						}
					}
				}

				foreach (string s in itemsToRight)
				{
					if (s != null && s.Length > 1)
					{
						foreach (ToolItemGroup itemGroup in groups)
						{
							if (itemGroup.HasItem (s))
							{
								ToolStripItem res = itemGroup.GetItem (s);
								res.Alignment = ToolStripItemAlignment.Right;
							}
						}
					}
				}
			}
		}

		private void PopulateList ()
		{
			string data = database.ReadData (SettingsForm.CAMOUFLAGEHIDDEN, "");

			PopulateLists (data, ref itemsToHide);

			data = database.ReadData (SettingsForm.CAMOUFLAGEPOSITIONS, "");
			PopulateLists (data, ref itemsToRight);
		}

		private void PopulateLists (string data, ref List<string> target)
		{
			if (data != "")
			{
				if (data.Contains ("|"))
				{
					string [] cc = data.Split ('|');
					foreach (string s in cc)
					{
						if (s != null && s.Length > 1)
							target.Add (s);
					}
				} else
				{
					if (data != null && data.Length > 1)
						target.Add (data);
				}
			}
		}

		public void SaveData ()
		{
			string output = "";
			if (itemsToHide.Count > 0)
			{
				output = CollectString(itemsToHide);
			}
			database.UpdateData (SettingsForm.CAMOUFLAGEHIDDEN, output);
			output = "";
			if (itemsToRight.Count > 0)
			{
				output = CollectString(itemsToRight);
			}

			database.UpdateData (SettingsForm.CAMOUFLAGEPOSITIONS, output);
		}

		private string CollectString (List<string> list)
		{
			string outp = "";
			foreach (string s in list)
			{
				outp += $"{s}|";
			}

			outp = outp.Substring (0, outp.Length - 1);
			return outp;
		}

		public void Update (List <ToolStripItem> Uitems, List <string> UitemsToHide, List <string> UitemsToRight)
		{
			items = Uitems;
			itemsToHide = UitemsToHide;
			itemsToRight = UitemsToRight;
			SaveData ();
			StartToHide ();
		}

	}
}