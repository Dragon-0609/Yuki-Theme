using System;

namespace Yuki_Theme.Core.API
{
	public static class CentralAPI
	{
		private static API_Base _apiBase;
		public static  bool     DefaultAPIInited => _apiBase is CommonAPI;

		public static API_Base Current
		{
			get
			{
				return _apiBase ??= new CommonAPI ();
			}
			set => _apiBase = value;
		}

	}
}