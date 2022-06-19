using System.Collections.Generic;
using System.Windows.Forms;

namespace Yuki_Theme.Core.WPF.Controls
{
	public class SettingsPanelUtilities
	{
		private SettingsPanel _settingsPanel;

		public static List <ToolStripItem> items;
		
		public SettingsPanelUtilities (SettingsPanel panel)
		{
			_settingsPanel = panel;
		}
		
		public void PopulateToolBarList ()
		{
			_settingsPanel.IconsList.Items.Clear ();
			foreach (ToolStripItem item in items)
			{
				_settingsPanel.IconsList.Items.Add (
					new ToolBarListItem (item.ToolTipText, item));
			}
			// _settingsPanel.IconsList.Items.Add ();
		}
		
		public void ToolBarItemSelection (ToolBarListItem item)
		{
			// MessageBox.Show ($"{item.item.ToolTipText} => V:{item.IsShown}, R:{item.IsRight}");
			_settingsPanel.ToolBarItemShow.IsChecked = item.IsShown;
			_settingsPanel.ToolBarItemRight.IsChecked = item.IsRight;
			_settingsPanel.ToolBarIcon.Source =
				ToolBarListItem.manager.RenderToolBarItemImage (item.item).ToWPFImage ();
		}

		public void ResetToolBar ()
		{
			if (Helper.mode == ProductMode.Plugin)
			{
				ToolBarListItem.camouflage.Reset ();
				ToolBarListItem.camouflage.PopulateList ();
				ToolBarListItem.camouflage.StartToHide ();
			}
		}

		public void SaveSettings ()
		{
			_settingsPanel.SaveSettings ();
			Settings.SaveData ();
			if (Helper.mode == ProductMode.Plugin)
				ToolBarListItem.camouflage.SaveData ();
		}
	}
}