using System.Collections.Generic;
using System.Windows.Forms;
using Yuki_Theme.Core.Database;
using Yuki_Theme.Core.Forms;

namespace Yuki_Theme_Plugin
{
	public class ToolBarCamouflage
	{
		private ToolStrip            tools;
		public  List <ToolStripItem> items;
		public  List <string> itemsToHide;
		private DatabaseManager      database;
		
		public ToolBarCamouflage (ToolStrip toolStrip)
		{
			tools = toolStrip;
			items = new List <ToolStripItem> ();
			itemsToHide = new List <string> ();
			database = new DatabaseManager ();
			Init ();
		}

		public void Init ()
		{
			for (var i = 0; i < tools.Items.Count; i++)
			{
				if (tools.Items [i].ToolTipText != null && tools.Items [i].Visible &&
				    tools.Items [i].AccessibleDescription != null && tools.Items [i].AccessibleDescription.Length > 1)
					items.Add (tools.Items [i]);
			}

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
				}

				foreach (string s in itemsToHide)
				{
					// MessageBox.Show ("TRY HIDE: " + s);
					if (s != null && s.Length > 1)
					{
						ToolStripItem [] res = tools.Items.Find (s, false);
						if (res != null && res.Length > 0)
						{
							res [0].Visible = false;
							// MessageBox.Show ("HIDED: " + s);
						}
					}
				}
			}
		}

		private void PopulateList ()
		{
			string data = database.ReadData (SettingsForm.ToolBarCamouflage, "");

			if (data != "")
			{
				if (data.Contains ("|"))
				{
					string [] cc = data.Split ('|');
					foreach (string s in cc)
					{
						if (s != null && s.Length > 1)
							itemsToHide.Add (s);
					}
				} else
				{
					if (data != null && data.Length > 1)
						itemsToHide.Add (data);
				}
			}
		}

		public void SaveData ()
		{
			string output = "";
			if (itemsToHide.Count > 0)
			{
				foreach (string s in itemsToHide)
				{
					output += $"{s}|";
				}

				output = output.Substring (0, output.Length - 1);
			}

			database.UpdateData (SettingsForm.ToolBarCamouflage, output);
		}

		public void Update (List <ToolStripItem> Uitems, List <string> UitemsToHide)
		{
			items = Uitems;
			itemsToHide = UitemsToHide;
			SaveData ();
			StartToHide ();
		}

	}
}