using System.Linq;
using System.Reflection;
using Yuki_Theme.Core.Controls;
using Yuki_Theme.Core.Utils;
using Yuki_Theme.Core.WPF;
using Yuki_Theme.Core.WPF.Controls;
using Yuki_Theme.Core.WPF.Interfaces;
using SettingsPanel = Yuki_Theme.Core.WPF.Controls.SettingsPanel;

namespace Yuki_Theme_Plugin.Controls
{
	public class PBarController : IToolBarController
	{
		private ToolBarList                     _list;
		private SettingsPanel                   _panel;
		private YukiTheme_VisualPascalABCPlugin _plugin;

		public PBarController (ToolBarList list, SettingsPanel panel, YukiTheme_VisualPascalABCPlugin plugin)
		{
			_list = list;
			_panel = panel;
			_plugin = plugin;
		}

		public void FillList ()
		{
			_list.Items.Clear ();
			foreach (TBarItemInfo item in _plugin.camouflage.ItemInfos)
			{
				_list.Items.Add (new ToolBarListItem (item.Text, item.Name, _list));
			}
		}

		public void UpdateInfo (ToolBarListItem item)
		{
			_panel.lockToolBarCheckboxes = 2;
			_panel.ToolBarItemShow.IsChecked = item.IsShown;
			_panel.ToolBarItemRight.IsChecked = item.IsRight;
			TBarItemInfo info = GetInfo (item.itemName);
			_plugin.ideComponents.WriteToConsole (info.AccessibleName);
			_panel.ToolBarIcon.Source = IconRenderer.RenderToolBarItemImage (info).ToWPFImage ();
		}

		public void SetIconContainer () => IconRenderer.IconContainer = Assembly.GetExecutingAssembly ();

		public bool IsVisible (string item)
		{
			return _plugin.camouflage.ItemInfos.First (info => info.Name == item).IsVisible;
		}

		public bool IsRight (string item)
		{
			return _plugin.camouflage.ItemInfos.First (info => info.Name == item).IsRight;
		}

		public TBarItemInfo GetInfo (string item)
		{
			return _plugin.camouflage.ItemInfos.First (info => info.Name == item);
		}

		public void SetVisible (string item, bool val)
		{
			_plugin.camouflage.ItemInfos.First (info => info.Name == item).IsVisible = val;
			_plugin.camouflage.StartToHide ();
		}

		public void SetRight (string item, bool val)
		{
			_plugin.camouflage.ItemInfos.First (info => info.Name == item).IsRight = val;
			_plugin.camouflage.StartToHide ();
		}

		public void ResetToolBar ()
		{
			_plugin.camouflage.Reset ();
		}

		public void ReloadToolBar ()
		{
			_plugin.camouflage.Reload ();
		}
	}
}