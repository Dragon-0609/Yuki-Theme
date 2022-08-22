using System;
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
		Send (EXPORT_THEME);
	}

	public override void AddEvents ()
	{
		ActionsDictionary.Add (TEST_CONNECTION_OK, SetPascalPath);
		Client.recieved += ParseMessage;
	}
	
	private void SetPascalPath (Message message)
	{
		Send (SET_PASCAL_PATH, Settings.pascalPath);
	}
	
	private void Send (int id) => Client.SendMessage (id);

	private void Send (int id, string content) => Client.SendMessage (id, content);
}