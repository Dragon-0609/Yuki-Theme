using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ICSharpCode.SharpDevelop;
using Yuki_Theme.Core;
using Yuki_Theme.Core.Communication;
using Yuki_Theme.Core.Controls;
using Yuki_Theme.Core.Database;
using Yuki_Theme.Core.Interfaces;
using Yuki_Theme.Core.WPF;
using Yuki_Theme.Core.WPF.Controls;
using Yuki_Theme_Plugin.Controls.DockStyles;
using Message = Yuki_Theme.Core.Communication.Message;

namespace Yuki_Theme_Plugin
{
	public class ToolBarCamouflage : ICamouflage
	{
		private DatabaseManager      database => Settings.database;
		public  List <ToolItemGroup> groups;
		public  List <ToolStripItem> items;

		public TBarItemInfo[] ItemInfos;		
		
		private readonly ToolStrip tools;

		public ToolBarCamouflage (ToolStrip toolStrip)
		{
			tools = toolStrip;
			items = new List <ToolStripItem> ();
			groups = new List <ToolItemGroup> ();
			Init ();
		}

		public void Init ()
		{
			ToolItemGroup prev = null;
			var current = new ToolItemGroup ();
			List<TBarItemInfo> infos = new();
			for (var i = 0; i < tools.Items.Count; i++)
			{
				if (current.separator != null)
				{
					prev = current;
					current = new ToolItemGroup {prev = prev};
					groups.Add (prev);
				}

				ToolStripItem item = tools.Items [i];
				
				if (item.ToolTipText != null && item.Visible &&
				    item.AccessibleDescription != null && item.AccessibleDescription.Length > 1)
				{
					items.Add (item);
					current.items.Add (item);
					infos.Add(new TBarItemInfo(item.Name, item.AccessibleDescription, item.ToolTipText, item.Visible, false, item.Size));
				}

				if (item is ToolStripSeparator)
					if (current.separator == null && !current.IsEmpty ())
						current.separator = (ToolStripSeparator)item;
			}

			if (!current.IsEmpty ())
				groups.Add (current);
			groups [groups.Count - 1].isLast = true;
			ItemInfos = infos.ToArray();
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

				foreach (string s in GetHiddenItems().GetNames())
					if (s.Length > 1)
						foreach (ToolItemGroup itemGroup in groups)
						{
							if (itemGroup.HasItem (s))
							{
								ToolStripItem res = itemGroup.GetItem (s);
								res.Visible = false;
							}
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

				foreach (string s in GetRightItems().GetNames())
					if (s.Length > 1)
						foreach (var itemGroup in groups)
							if (itemGroup.HasItem (s))
							{
								ToolStripItem res = itemGroup.GetItem (s);
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

		private TBarItemInfo[] GetHiddenItems()
		{
			return ItemInfos.Where(info => !info.IsVisible).ToArray();
		}

		private TBarItemInfo[] GetRightItems()
		{
			return ItemInfos.Where(info => info.IsRight).ToArray();
		}

		public void PopulateList ()
		{
			string data = database.ReadData (SettingsConst.CAMOUFLAGE_HIDDEN, "");
			string[] parsed = ParseItemInfos(data);
			ItemInfos.Where(info => parsed.Contains(info.Name)).ForEach(info => info.IsVisible = false);

			data = database.ReadData (SettingsConst.CAMOUFLAGE_POSITIONS, "");
			parsed = ParseItemInfos(data);
			ItemInfos.Where(info => parsed.Contains(info.Name)).ForEach(info => info.IsRight = true);
		}

		private string[] ParseItemInfos(string data)
		{
			if (data != "")
			{
				if (data.Contains ("|"))
				{
					string[] cc = data.Split ('|');
					return cc;
				} else
				{
					if (data != null && data.Length > 1)
						return new string[] {data};
				}
			}

			return new string[0];
		}

		public void SaveData ()
		{
			var output = "";
			TBarItemInfo[] itemsToHide = GetHiddenItems();
			if (itemsToHide.Length > 0) output = CollectString (itemsToHide.GetNames());
			database.UpdateData (SettingsConst.CAMOUFLAGE_HIDDEN, output);
			output = "";
			TBarItemInfo[] itemsToRight = GetHiddenItems();
			if (itemsToRight.Length > 0) output = CollectString (itemsToRight.GetNames());

			database.UpdateData (SettingsConst.CAMOUFLAGE_POSITIONS, output);
		}

		private string CollectString (string[] list)
		{
			var outp = "";
			foreach (var s in list) outp += $"{s}|";

			outp = outp.Substring (0, outp.Length - 1);
			return outp;
		}

		public bool IsVisible (string item)
		{
			return ItemInfos.First(info => info.Name == item).IsVisible;
		}

		public bool IsRight (string item)
		{
			return ItemInfos.First(info => info.Name == item).IsRight;
		}

		public void SetVisible (string item, bool value)
		{
			ItemInfos.First(info => info.Name == item).IsVisible = value;
			StartToHide ();
		}

		public void SetRight (string item, bool value)
		{
			ItemInfos.First(info => info.Name == item).IsRight = value;
			StartToHide ();
		}

		public void Reset ()
		{
			foreach (TBarItemInfo barItemInfo in ItemInfos)
			{
				barItemInfo.IsVisible = true;
				barItemInfo.IsRight = false;
			}

			StartToHide ();
		}

		public void Reload()
		{
			Reset();
			PopulateList();
			StartToHide();
			if (!YukiTheme_VisualPascalABCPlugin.plugin.isCommonAPI)
			{
				YukiTheme_VisualPascalABCPlugin.plugin._client.SendMessage(new Message(MessageTypes.SET_TOOLBAR_ITEMS, ItemInfos));
			}
		}
	}
}