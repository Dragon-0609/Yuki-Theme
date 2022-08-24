using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Yuki_Theme.Core.Communication;
using Yuki_Theme.Core.Interfaces;
using Yuki_Theme.Core.Utils;
using static Yuki_Theme.Core.Communication.MessageTypes;
namespace Yuki_Theme.Core.API;

public class ServerAPI : NetworkAPI
{
	public Communicator.IServer Server;

	public override void ExportTheme (Image wallpaper, Image sticker, Action setTheme = null, Action startSettingTheme = null,
									  bool wantToKeep = false)
	{
		Send (RELEASE_RESOURCES);
		string path = Path.Combine (Settings.pascalPath, "Highlighting", $"{pathToLoad}.xshd");

		_themeManager.DeleteOldThemeIfNeed ();

		_themeManager.CopyThemeToDirectory (path);

		PrepareToExport (path);

		Actions.ShowEndMessage (currentTheme.Name);

		Helper.currentTheme = currentTheme.Name;
		Send (APPLY_THEME);
	}

	public override void AddEvents ()
	{
		ActionsDictionary.Add (SET_PASCAL_PATH, SetPascalPath);
		ActionsDictionary.Add (EXPORT_THEME, _ => ExportTheme (null, null, null, null, true));
		ActionsDictionary.Add (SELECT_THEME, message => this.SelectTheme (message.Content));
		ActionsDictionary.Add (RELOAD_THEME, message => Restore (bool.Parse (message.Content)));
		Server.recieved += ParseMessage;
	}

	private void SetPascalPath (Message message)
	{
		Settings.pascalPath = message.Content;
	}

	public override void Preview (SyntaxType syntax, bool needToDelete, Action setTheme = null)
	{
		string path = Path.Combine (Settings.pascalPath, "Highlighting", $"{pathToLoad}.xshd");
		if (needToDelete)
		{
			_themeManager.DeleteOldTheme ();
		}

		if (syntax != SyntaxType.NULL)
		{
			string dir = Path.GetDirectoryName (path);
			Actions.MergeSyntax (dir, syntax);
		} else
		{
			PrepareToExport (path);
		}

		Helper.currentTheme = currentTheme.Name;
		if (setTheme != null)
			setTheme ();
	}
	
	private void Send (int id) => Server.SendMessage (id);

	private void Send (int id, string content) => Server.SendMessage (id, content);
}