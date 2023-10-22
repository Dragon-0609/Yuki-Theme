using System.Linq;
using System.Reflection;

namespace YukiTheme.Tools
{
	public static class IconRenderer
	{
		public static readonly string[] _iconsWithDarkVersion =
		{
			"menu-cut",
			"menu-saveall",
			"menu-paste",
			"copy",
			"addFile",
			"undo",
			"redo",
			"stepOut",
			"traceInto",
			"console",
			"intentionBulb",
			"magicResolve",
			"menu-open",
			"restartDebugger",
			"traceOver",
			"back",
			"forward",
			"print",
			"projectTab",
			"close",
			"find",
			"findForward",
			"replace",
			"moveToBottomLeft",
			"toolWindowMessages",
			"dynamicUsages",
			"showHiddens",
			"MoveTo2",
			"gearPlain",
			"showReadAccess",
			"showWriteAccess",
			"externalTools",
			"help"
		};

		public const string IconFolder = "Yuki_Theme_Plugin.Resources.icons";

		public static Assembly IconContainer;

		private static bool HasDark(string name)
		{
			return _iconsWithDarkVersion.Contains(name);
		}

		/*
		public static Image RenderToolBarItemImage(TBarItemInfo info)
		{
			bool asDark = HasDark (info.AccessibleName);
			string dark = "";
			if (asDark)
			{
				bool isDark = Helper.IsDark (ColorKeeper.bgColor);
				dark = isDark ? "" : "_dark";
			}

			if (IconContainer == null)
				throw new NullReferenceException("IconContainer wasn't set");

			return Helper.RenderSvg (info.Size, Helper.LoadSvg (info.AccessibleName + dark, IconContainer, IconFolder),
				false, Size.Empty, true, ColorKeeper.bgBorder);
		}*/
	}
}