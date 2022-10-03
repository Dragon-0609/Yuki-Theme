using System.Linq;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Communication;
using Yuki_Theme.Core.Controls;
using Yuki_Theme.Core.Utils;
using Yuki_Theme.Core.WPF.Interfaces;

namespace Yuki_Theme.Core.WPF.Controls
{
	public class TBListController : IToolBarController
	{
		private ToolBarList _list;

		internal TBarItemInfo[] _infos;
		private SettingsPanel _panel;
		ServerAPI api => (ServerAPI) CentralAPI.Current;
		
		public TBListController(ToolBarList list, SettingsPanel panel)
		{
			_list = list;
			_panel = panel;
		}


		public void FillList()
		{
			API_Events.FillToolBarList = FillToolBarList;
			api.SendMessage(new Message(MessageTypes.GET_TOOL_BAR_ITEMS));
			API_Events.saveToolBarData = () => api.SendMessage(new Message(MessageTypes.SAVE_TOOL_BAR_DATA));
		}

		private void FillToolBarList(TBarItemInfo[] items)
		{
			_list.Items.Clear();
			_infos = items;
			foreach (TBarItemInfo item in _infos)
			{
				_list.Items.Add(new ToolBarListItem(item.Text, item.Name, _list));
			}
		}

		public void UpdateInfo(ToolBarListItem item)
		{
			_panel.lockToolBarCheckboxes = 2;
			_panel.ToolBarItemShow.IsChecked = item.IsShown;
			_panel.ToolBarItemRight.IsChecked = item.IsRight;
			_panel.ToolBarIcon.Source = IconRenderer.RenderToolBarItemImage(GetInfo(item.itemName)).ToWPFImage();
		}

		public bool IsVisible(string item)
		{
			return _infos.First(info => info.Name == item).IsVisible;
		}

		public bool IsRight(string item)
		{
			return _infos.First(info => info.Name == item).IsRight;
		}

		public TBarItemInfo GetInfo(string item)
		{
			return _infos.First(info => info.Name == item);
		}

		public void SetVisible(string item, bool val)
		{
			_infos.First(info => info.Name == item).IsVisible = val;
		}

		public void SetRight(string item, bool val)
		{
			_infos.First(info => info.Name == item).IsRight = val;
		}

		public void ResetToolBar()
		{
			api.SendMessage(new Message(MessageTypes.RESET_TOOL_BAR));
		}

		public void ReloadToolBar()
		{
			api.SendMessage(new Message(MessageTypes.RELOAD_TOOL_BAR));
		}
	}
}