using System.Collections.Generic;
using Microsoft.Win32;
using Yuki_Theme.Core.Interfaces;

namespace Yuki_Theme.Core.Database
{
	internal class WindowsRegistryDatabase : IDatabase
	{
		private RegistryKey _key;
	
		public WindowsRegistryDatabase ()
		{
			_key = Registry.CurrentUser.CreateSubKey (@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);
		}


		public void SetValue (string name, string value)
		{
			_key.SetValue (name, value);
		}

		public string GetValue (string name)
		{
			string res = "";
			if (ValueExists (name))
			{
				res = _key.GetValue (name) as string;
			}

			return res;
		}

		public string GetValue (string name, string defaultValue)
		{
			string res = GetValue (name);
			if (res.Length == 0)
				res = defaultValue;
			return res;
		}
		
		public bool ValueExists(string Value)
		{
			try
			{
				return _key.GetValue(Value) != null;
			}
			catch
			{
				return false;
			}
		}

		public void DeleteValue (string name)
		{
			if (ValueExists (name))
			{
				_key.DeleteValue (name);
			}
		}
		public void Wipe ()
		{
			Registry.CurrentUser.CreateSubKey (@"SOFTWARE", RegistryKeyPermissionCheck.ReadWriteSubTree)?.DeleteSubKey ("YukiTheme");
		}
	}
}