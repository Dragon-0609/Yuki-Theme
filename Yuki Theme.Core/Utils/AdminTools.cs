using System.Security.Principal;
using Yuki_Theme.Core.Database;

namespace Yuki_Theme.Core.Utils;

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

	private bool CheckPrivileges ()
	{
		WindowsIdentity user = null;

		user = WindowsIdentity.GetCurrent();
		WindowsPrincipal principal = new WindowsPrincipal(user);
		bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
		return isAdmin;
	}
}