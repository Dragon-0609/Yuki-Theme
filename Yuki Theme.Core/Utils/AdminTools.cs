using System.Security.Principal;
using Microsoft.Win32;
using Yuki_Theme.Core.Database;

namespace Yuki_Theme.Core.Utils
{
	public class AdminTools
	{
		public bool CurrentUserIsAdmin() 
		{
			if (DatabaseManager.IsLinux)
			{
				return true;
			}else
			{
				return CheckPrivileges ();
			}
		}
		
		public bool IsUACEnabled
		{
			get
			{
				if (DatabaseManager.IsLinux)
					return false;
				string UAC_key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
				int luaValue = (int) Registry.GetValue(UAC_key, "EnableLUA", 0);
				bool enabled = luaValue == 1;

				return enabled;
			}
		}

		private bool CheckPrivileges ()
		{
			return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
				.IsInRole(WindowsBuiltInRole.Administrator);
		}
	}
}