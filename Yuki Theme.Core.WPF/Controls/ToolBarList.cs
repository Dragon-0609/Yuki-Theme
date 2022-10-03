using System.Windows;
using System.Windows.Controls;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Controls;
using Yuki_Theme.Core.WPF.Interfaces;

namespace Yuki_Theme.Core.WPF.Controls
{
	public class ToolBarList : ListView
	{
		internal SettingsPanel _panel;
		public IToolBarController _controller;
		
		public ToolBarList ()
		{
			// DefaultStyleKeyProperty.OverrideMetadata (typeof (Manageable), new FrameworkPropertyMetadata (null));
		}
		
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new ToolBarListItem(this);
		}
		
		public void Init(SettingsPanel panel)
		{
			if (Helper.mode == ProductMode.Program)
			{
				_panel.ToolBarDockPanel.Visibility = Visibility.Collapsed;
				_panel.UnavailablePanel.Visibility = Visibility.Visible;
			}else if (Helper.mode == ProductMode.Plugin)
			{
				InitToolBarPlace(panel);
			}
		}

		private void InitToolBarPlace(SettingsPanel panel)
		{
			if (Settings.Location == SettingsPanelLocation.App && CentralAPI.Current is ServerAPI or ClientAPI)
				_controller = new TBListController(this, panel);
			else if (CentralAPI.Current is CommonAPI)
			{
				
			}

			_controller.FillList();
		}

	}
}