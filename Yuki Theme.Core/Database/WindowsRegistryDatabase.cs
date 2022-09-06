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
			object value = _key.GetValue (name);
			if (value != null)
				res = value as string;

			return res;
		}

		public string GetValue (string name, string defaultValue)
		{
			string res = GetValue (name);
			if (res.Length == 0)
				res = defaultValue;
			return res;
		}

		public void DeleteValue (string name)
		{
		
		}
	}
}