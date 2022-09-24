using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using Yuki_Theme.Core.API;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class AddThemeWindow : Window
	{
		public int result;
		
		public AddThemeWindow ()
		{
			InitializeComponent ();
			WPFHelper.windowForDialogs = this;
			WPFHelper.checkDialog = TryToAdd;
		}

		public void AddThemes (string themeToSelect)
		{
			Themes.Items.Clear ();
			foreach (string theme in CentralAPI.Current.Schemes.ToArray ())
			{
				Themes.Items.Add (theme);
			}

			if (themeToSelect.Length > 1 && Themes.Items.Contains (themeToSelect))
			{
				Themes.SelectedIndex = Themes.Items.IndexOf (themeToSelect);
			}else
			{
				Themes.SelectedIndex = 0;
			}
		}

		public void ClickToAdd ()
		{
			Button btn = (Button)Template.FindName ("SaveButton", this);
			ButtonAutomationPeer peer = new ButtonAutomationPeer(btn);
			IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
			invokeProv.Invoke();
		}

		private bool TryToAdd ()
		{
			bool canReturn = false;

			string from = Themes.SelectedItem.ToString (), to = TName.Text;


			if (from != to)
			{
				if (Helper.mode == ProductMode.Plugin && CentralAPI.Current is not CommonAPI)
				{
					if (WPFHelper.serverResponse == 0)
					{
						CentralAPI.Current.AddTheme (from, to);
						WPFHelper.serverResponse = 2;
					} else
					{
						canReturn = WPFHelper.serverResponse == 1;
						WPFHelper.serverResponse = 0;
					}
				} else
				{
					result = CentralAPI.Current.AddTheme (from, to);

					canReturn = result != 0;
				}
			} else
			{
				API_Events.showError (CentralAPI.Current.Translate ("messages.name.equal.message"), CentralAPI.Current.Translate ("messages.name.equal.title"));
			}

			return canReturn;
		}
	}
}