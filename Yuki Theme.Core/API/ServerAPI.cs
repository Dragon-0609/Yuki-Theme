using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Yuki_Theme.Core.Communication;
using Yuki_Theme.Core.Interfaces;
using Yuki_Theme.Core.Utils;
using static Yuki_Theme.Core.Communication.MessageTypes;
namespace Yuki_Theme.Core.API
{
	public class ServerAPI : NetworkAPI
	{
		public Communicator.IServer Server;

		public override void ExportTheme (Image wallpaper, Image sticker, Action setTheme = null, Action startSettingTheme = null,
										  bool wantToKeep = false)
		{
			Send (RELEASE_RESOURCES);
		}

		public override void AddEvents ()
		{
			ActionsDictionary.Add (SET_PASCAL_PATH, SetPascalPath);
			ActionsDictionary.Add (EXPORT_THEME, _ => ExportTheme (null, null, null, null, true));
			ActionsDictionary.Add (SELECT_THEME, message => SelectTheme (message.Content));
			ActionsDictionary.Add (RELOAD_THEME, message => Restore (bool.Parse (message.Content)));
			ActionsDictionary.Add (PREVIEW_THEME, message => Preview ((PreviewOptions)message.OtherContent, null));
			ActionsDictionary.Add (RELEASE_RESOURCES_OK, ResourcesReleased);
			Server.recieved += ParseMessage;
		}

		private void ResourcesReleased (Message message)
		{
			string path = Path.Combine (Settings.pascalPath, "Highlighting", $"{pathToLoad}.xshd");

			_themeManager.DeleteOldThemeIfNeed ();

			_themeManager.CopyThemeToDirectory (path);

			PrepareToExport (path);

			Actions.ShowEndMessage (currentTheme.Name);

			Helper.currentTheme = currentTheme.Name;
			Send (SET_CURRENT_NAME, currentTheme.Name);
			Send (APPLY_THEME);
		}

		private void SetPascalPath (Message message)
		{
			Settings.pascalPath = message.Content;
		}

		public override void Preview (PreviewOptions options, Action setTheme = null)
		{
			string path = Path.Combine (Settings.pascalPath, "Highlighting", $"{pathToLoad}.xshd");
			if (options.NeedToDelete)
			{
				_themeManager.DeleteOldTheme ();
			}

			if (options.Syntax != SyntaxType.NULL)
			{
				string dir = Path.GetDirectoryName (path);
				Actions.MergeSyntax (dir, options.Syntax);
			} else
			{
				PrepareToExport (path);
			}

			Helper.currentTheme = currentTheme.Name;
			Send (SET_CURRENT_NAME, currentTheme.Name);
			if (options.HasAction)
				Send (APPLY_THEME_LIGHT);
		}
	
		private void Send (int id) => Server.SendMessage (id);

		private void Send (int id, string content) => Server.SendMessage (id, content);
	}
}