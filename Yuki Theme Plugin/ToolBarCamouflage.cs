using System.Collections.Generic;
using System.Windows.Forms;
using Yuki_Theme.Core;
using Yuki_Theme.Core.Database;
using Yuki_Theme.Core.Forms;
using Yuki_Theme_Plugin.Controls;
using Yuki_Theme_Plugin.Controls.DockStyles;

namespace Yuki_Theme_Plugin
{
	public class ToolBarCamouflage
	{
		private readonly DatabaseManager      database;
		public           List <ToolItemGroup> groups;
		public           List <ToolStripItem> items;
		public           List <string>        itemsToHide;
		public           List <string>        itemsToRight;
		private readonly ToolStrip            tools;

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
			var current = new ToolItemGroup ();

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
					if (current.separator == null && !current.IsEmpty ())
						current.separator = (ToolStripSeparator) tools.Items [i];
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
				foreach (var item in items)
				{
					item.Visible = true;
					item.Alignment = ToolStripItemAlignment.Left;
				}

				foreach (var itemGroup in groups)
					if (itemGroup.separator != null)
					{
						if (!itemGroup.isLast)
							itemGroup.separator.Visible = true;
						itemGroup.separator.Alignment = ToolStripItemAlignment.Left;
					}

				foreach (var s in itemsToHide)
					if (s != null && s.Length > 1)
						foreach (var itemGroup in groups)
							if (itemGroup.HasItem (s))
							{
								var res = itemGroup.GetItem (s);
								res.Visible = false;
							}

				foreach (var itemGroup in groups)
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

				foreach (var s in itemsToRight)
					if (s != null && s.Length > 1)
						foreach (var itemGroup in groups)
							if (itemGroup.HasItem (s))
							{
								var res = itemGroup.GetItem (s);
								res.Alignment = ToolStripItemAlignment.Right;
							}


				foreach (var itemGroup in groups)
					if (itemGroup.isAllRight ())
					{
						if (itemGroup.isLast)
						{
							if (itemGroup.prev.separator != null)
								itemGroup.prev.separator.Alignment = ToolStripItemAlignment.Right;
						} else
						{
							if (itemGroup.separator != null)
								itemGroup.separator.Alignment = ToolStripItemAlignment.Right;
						}
					}
			}
		}

		private void PopulateList ()
		{
			var data = database.ReadData (Settings.CAMOUFLAGEHIDDEN);

			PopulateLists (data, ref itemsToHide);

			data = database.ReadData (Settings.CAMOUFLAGEPOSITIONS);
			PopulateLists (data, ref itemsToRight);
		}

		private void PopulateLists (string data, ref List <string> target)
		{
			if (data != "")
			{
				if (data.Contains ("|"))
				{
					var cc = data.Split ('|');
					foreach (var s in cc)
						if (s != null && s.Length > 1)
							target.Add (s);
				} else
				{
					if (data != null && data.Length > 1)
						target.Add (data);
				}
			}
		}

		public void SaveData ()
		{
			var output = "";
			if (itemsToHide.Count > 0) output = CollectString (itemsToHide);
			database.UpdateData (Settings.CAMOUFLAGEHIDDEN, output);
			output = "";
			if (itemsToRight.Count > 0) output = CollectString (itemsToRight);

			database.UpdateData (Settings.CAMOUFLAGEPOSITIONS, output);
		}

		private string CollectString (List <string> list)
		{
			var outp = "";
			foreach (var s in list) outp += $"{s}|";

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