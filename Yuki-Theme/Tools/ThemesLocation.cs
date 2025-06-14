using System.Reflection;

namespace YukiTheme.Tools
{
	public static class ThemesLocation
	{
		private static string[] _paths;

		public static string[] Paths => _paths;

		static ThemesLocation()
		{
			_paths =
			[
				System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
			];
		}
	}
}