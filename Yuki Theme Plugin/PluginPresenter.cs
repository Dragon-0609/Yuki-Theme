using System;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using VisualPascalABC;
using VisualPascalABC.OptionsContent;
using WeifenLuo.WinFormsUI.Docking;
using Yuki_Theme.Core;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Controls;
using Yuki_Theme.Core.Utils;
using Yuki_Theme.Core.WPF;
using Yuki_Theme.Core.WPF.Windows;
using Yuki_Theme_Plugin.Communication;
using Yuki_Theme_Plugin.Controls.Helpers;
using Yuki_Theme_Plugin.Interfaces;
using static Yuki_Theme.Core.Communication.MessageTypes;
using Resources = Yuki_Theme_Plugin.Properties.Resources;
using Message = Yuki_Theme.Core.Communication.Message;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace Yuki_Theme_Plugin
{
	public class PluginPresenter : Plugin.IPresenter
	{
		private Plugin.View _view;
		private PictureBox  logoBox;

		private int lastFocused = -1;
		
		public PluginPresenter (Plugin.View view)
		{
			_view = view;
		}


		#region Initialization

		public void InitAPI ()
		{
			AdminTools tools = new AdminTools ();
			if (tools.IsUACEnabled && _view._helper.IsInProgramFiles ())
			{
				InitCommunicator ();
			} else
			{
				InitCommonAPI ();
			}
		}

		public void InitCommunicator ()
		{
			if (!_view.isCommonAPI)
			{
				_view._client = new Client (OutputConsoleLogger.Instance, _view._helper, (YukiTheme_VisualPascalABCPlugin) _view);
			}
		}

		public void InitClientAPI ()
		{
			_view.ideComponents.RunInUIThread (() =>
			{
				_view.isCommonAPI = false;
				ClientAPI api = new ClientAPI ();
				CentralAPI.Current = api;
				api.Client = _view._client;
				api.AddEvents ();
				api.AddEvent (RELEASE_RESOURCES, ReleaseResourcesForServer);
				api.AddEvent (APPLY_THEME, _ => _view.ReloadLayout ());
				api.AddEvent (APPLY_THEME_LIGHT, _ => _view.ReloadLayoutLight ());
				api.AddEvent (THEME_ADDED, _ => ThemeAdded ());
				api.AddEvent (RELOAD_SETTINGS, ReloadSettings);
				api.AddEvent(GET_TOOL_BAR_ITEMS, _ => GetToolBarItems());
				api.AddEvent(RESET_TOOL_BAR, _ => _view.camouflage.Reset());
				api.AddEvent(RELOAD_TOOL_BAR, _ => _view.camouflage.Reload());
				api.AddEvent(GET_ASSEMBLY_NAME, GetAssemblyName);
				api.AddEvent(SET_TOOL_BAR_VISIBILITY,
					_ => _view.camouflage.SetItemVisibility((TBarItemInfo) _.OtherContent));
				api.AddEvent(SET_TOOL_BAR_ALIGN,
					_ => _view.camouflage.SetItemAlign((TBarItemInfo) _.OtherContent));
				_view.StartIntegration ();
			});
		}

		private void GetAssemblyName(Message obj)
		{
			_view._client.SendMessage(new Message(SET_ASSEMBLY_NAME, Assembly.GetExecutingAssembly().Location));
		}

		private void ReloadSettings (Message message)
		{
			_view.ideComponents.RunInUIThread (() =>
			{
				Settings.Get ();
				ApplySettings ((ChangedSettings)message.OtherContent, false);
			});
		}
		
		private void ThemeAdded ()
		{
			System.Windows.Application.Current.Dispatcher.Invoke (() =>
			{
				WPFHelper.serverResponse = 1;
				if (WPFHelper.windowForDialogs is AddThemeWindow window)
				{
					window.ClickToAdd ();
				}
			});
		}

		public void InitCommonAPI ()
		{
			_view.isCommonAPI = true;
			CentralAPI.Current = new CommonAPI ();
			_view.StartIntegration ();
		}

		private void GetToolBarItems()
		{
			_view.SendMessage(new Message(SET_TOOLBAR_ITEMS, _view.camouflage.ItemInfos));
		}
		

		#endregion
		
		
		#region Logo Management

		public void ShowLogo ()
		{
			logoBox = new PictureBox ();
			logoBox.BackColor = YukiTheme_VisualPascalABCPlugin.Colors.bgdef;
			logoBox.Image = Resources.YukiTheme;
			logoBox.Location = new Point (_view.ideComponents.fm.ClientSize.Width / 2 - 50, _view.ideComponents.fm.ClientSize.Height / 2 - 50);
			logoBox.Name = "logoBox";
			logoBox.Size = new Size (100, 100);
			logoBox.SizeMode = PictureBoxSizeMode.Zoom;
			logoBox.TabIndex = 0;
			logoBox.TabStop = false;
			_view.ideComponents.fm.Controls.Add (logoBox);
		}

		public void HideLogo ()
		{
			_view.ideComponents.fm.Controls.Remove (logoBox);
			logoBox.Dispose ();
		}

		#endregion
		
		
		#region Server Actions

		private void ReleaseResourcesForServer (Message obj)
		{
			ReleaseResources ();
			_view._client.SendMessage (RELEASE_RESOURCES_OK);
		}

		#endregion
		
		
		public void RememberCurrentEditor ()
		{
			if (_view.ideComponents.fm.ActiveControl is UpdatePageControl)
			{
				UpdatePageControl update = (UpdatePageControl)_view.ideComponents.fm.ActiveControl;
				lastFocused = update.TabIndex;
			} else
			{
				lastFocused = _view.ideComponents.fm.CurrentCodeFileDocument.TabIndex;
			}
		}

		public void ReFocusCurrentEditor ()
		{
			IDockContent [] docs = _view.ideComponents.fm.MainDockPanel.DocumentsToArray ();
			IDockContent doc = docs [lastFocused];
			doc.DockHandler.Pane.Focus ();
			if (doc.DockHandler.Content is UpdatePageControl)
			{
				UpdatePageControl update = (UpdatePageControl)doc.DockHandler.Content;
				update.Focus ();
			} else
			{
				CodeFileDocumentControl cod = (CodeFileDocumentControl)doc.DockHandler.Content;
				cod.Focus ();
			}

			docs = null;
		}

		
		public void ReleaseResources ()
		{
			if (YukiTheme_VisualPascalABCPlugin.img != null)
			{
				YukiTheme_VisualPascalABCPlugin.img.Dispose ();
				YukiTheme_VisualPascalABCPlugin.img = null;
			}

			_view.Release ();
		}


		public void AddToSettings ()
		{
			var getopt = _view.ideComponents.fm.GetType ().GetField ("optionsContentEngine", BindingFlags.NonPublic | BindingFlags.Instance);
			OptionsContentEngine options = (OptionsContentEngine)getopt.GetValue (_view.ideComponents.fm);
			options.AddContent (new Controls.PluginSettingsControl ((YukiTheme_VisualPascalABCPlugin)_view));
		}

		public void ApplySettings (ChangedSettings settings, bool sync)
		{
			if (settings.ShowSticker != Settings.swSticker)
			{
				_view._model.UpdateStickerVisibility ();
			}

			if (settings.CustomSticker != Settings.useCustomSticker)
			{
				if (Settings.useCustomSticker)
				{
					_view._model.ReloadSticker ();
				} else
				{
					_view._model.LoadSticker ();
				}
			}

			if (settings.DimensionCap != Settings.useDimensionCap)
			{
				_view._model.ReadData ();
				_view._model.ResetStickerPosition ();
			}
			
			if (settings.ResetMargins)
			{
				Settings.database.UpdateData (SettingsConst.STICKER_POSITION, "");
				_view._model.ReloadStickerPositionData ();
				_view._model.ResetStickerPosition ();
			}

			if (!_view.isCommonAPI && sync)
			{
				_view._client.SendMessage (new Message (RELOAD_SETTINGS, settings));
			} else
			{
				if (_view.CoreWindow != null && PresentationSource.FromVisual (_view.CoreWindow) != null)
				{
					_view.CoreWindow.ApplySettingsChanges (settings);
				}
			}
			
			
		}

	}
}