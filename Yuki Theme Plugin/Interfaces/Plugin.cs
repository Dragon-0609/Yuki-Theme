using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Yuki_Theme.Core;
using Yuki_Theme.Core.WPF.Windows;
using Yuki_Theme_Plugin.Communication;
using Message = Yuki_Theme.Core.Communication.Message;

namespace Yuki_Theme_Plugin.Interfaces
{
	public class Plugin
	{

		internal interface IPresenter
		{
			
			
			void InitAPI ();
			void InitClientAPI ();
			void InitCommonAPI ();
			
			void RememberCurrentEditor ();
			void ReFocusCurrentEditor ();
			void ReleaseResources ();
			
			void AddToSettings ();
			void ApplySettings (ChangedSettings settings, bool sync);
			void ShowLogo ();
			void HideLogo ();
		}

		public abstract class View
		{
			internal IdeComponents ideComponents = new IdeComponents ();
			internal Plugin.Model  _model        = new PluginModel ();
			internal PluginHelper  _helper;
			internal MainWindow    CoreWindow;
			internal ToolBarCamouflage camouflage;

			internal Client _client;

			internal abstract void StartIntegration ();

			internal bool isCommonAPI = false;
			
			internal abstract void Release ();
			
			public abstract void ReloadLayout ();
			public abstract void ReloadLayoutLight ();
			public abstract void ReloadLayoutAll (bool lightReload);

			internal abstract void SendMessage(Message message);
		}

		internal interface IHelper
		{
			void ResetBrush (ref Brush brush, Color color);
			void ResetPen (ref Pen pen, Color color, float width, PenAlignment alignment);
			void ResetBrushesAndPens ();
			void AddTabWithUrl (DockPanel tabControl, string title, string url);
			void openUpdate ();
			bool IsUpdated ();
			void ShowLicense ();
			void InvokeUI (Delegate method);
		}

		public abstract class Model
		{
			internal abstract void InitSticker (Form form);
			internal abstract void LoadSticker ();
			internal abstract void ReloadSticker ();
			internal abstract void ResetStickerPosition ();
			internal abstract void ChangeSticker (Image image);
			internal abstract void UpdateStickerVisibility ();
			internal abstract void Release ();
			internal abstract void ReadData ();
			internal abstract void UpdateLocation ();
			internal abstract void StickerPositioning (bool available);
			internal abstract void ReloadStickerPositionData ();
		}
	}
}