﻿using System;
using System.Drawing;
using System.IO;
using Yuki_Theme.Core.Communication;
using Yuki_Theme.Core.Interfaces;
using static Yuki_Theme.Core.Communication.MessageTypes;
namespace Yuki_Theme.Core.API;

public class ClientAPI : NetworkAPI
{
	public Communicator.IClient Client;



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
		ActionsDictionary.Add (EXPORT_THEME, ExportTheme);
		Client.recieved += ParseMessage;
	}

	private void SetPascalPath (Message message)
	{
		Settings.pascalPath = message.Content;
	}

	private void ExportTheme (Message message)
	{
		ExportTheme (null, null, null, null, true);
	}

	private void Send (int id)
	{
		Client.SendMessage (id);
	}
}