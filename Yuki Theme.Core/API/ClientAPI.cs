using System;
using System.Drawing;
using System.IO;
using System.Windows;
using Yuki_Theme.Core.Communication;
using Yuki_Theme.Core.Interfaces;
using Yuki_Theme.Core.Themes;
using Yuki_Theme.Core.Utils;
using static Yuki_Theme.Core.Communication.MessageTypes;
namespace Yuki_Theme.Core.API
{
	public class ClientAPI : NetworkAPI
	{
		public Communicator.IClient Client;

		public ClientAPI ()
		{
			CreateThemesDirectory = false;
		}

		public override void ExportTheme (Image wallpaper, Image sticker, Action setTheme = null, Action startSettingTheme = null,
		                                  bool wantToKeep = false)
		{
			Send (EXPORT_THEME);
		}

		public override void AddEvents ()
		{
			ActionsDictionary.Add (TEST_CONNECTION_OK, SetPascalPath);
			ActionsDictionary.Add (SET_CURRENT_NAME, message => Helper.currentTheme = message.Content);
			Client.recieved += ParseMessage;
		}

		public override void SelectTheme (string name)
		{
			base.SelectTheme (name);
			Send (SELECT_THEME, name);
		}

		public override void Restore (bool wantClean = true, Action onSelect = null)
		{
			base.Restore (wantClean, onSelect);
			Send (RELOAD_THEME, wantClean.ToString ());
		}

		public override void Preview (PreviewOptions options, Action setTheme = null)
		{
			Send (new Message (PREVIEW_THEME, options));
		}

		public override int AddTheme (string copyFrom, string name)
		{
			if (name.Length < 3)
			{
				if (API_Events.showError != null)
					API_Events.showError (Translate ("messages.name.short.full"), Translate ("messages.name.short.short"));
				return 0;
			}

			string path = Helper.ConvertNameToPath (name);
			string destination = PathGenerator.PathToFile (path, _themeInfos [copyFrom].isOld);
			Helper.CreateThemeDirectory ();

			bool exist = false;

			if (File.Exists (destination))
			{
				if (_actions.AskToOverrideFile (destination, ref exist, out var add)) return add;
			}

			if (!DefaultThemes.isDefault (name))
			{
				object [] container = new object [] { copyFrom, name, destination, exist };
				Client.SendMessage (new Message (ADD_THEME, container));
				return 0;
			}

			if (API_Events.showError != null)
				API_Events.showError (Translate ("messages.name.default.full"), Translate ("messages.name.default.short"));

			return 0;
		}
		
		private void SetPascalPath (Message message)
		{
			Send (SET_PASCAL_PATH, Settings.pascalPath);
		}

		private void Send (int id) => Client.SendMessage (id);

		private void Send (int id, string content) => Client.SendMessage (id, content);

		private void Send (Message message) => Client.SendMessage (message);
	}
}