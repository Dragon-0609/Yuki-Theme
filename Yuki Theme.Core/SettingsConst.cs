using System.IO;
using System.Reflection;

namespace Yuki_Theme.Core
{
	public static class SettingsConst
	{
		public const int PASCAL_PATH           = 1;
		public const int ACTIVE                = 2;
		public const int ASK_CHOICE            = 4;
		public const int CHOICE_INDEX          = 5;
		public const int SETTING_MODE          = 6;
		public const int AUTO_UPDATE           = 7;
		public const int BG_IMAGE              = 8;
		public const int STICKER               = 9;
		public const int STATUS_BAR            = 10;
		public const int LOGO                  = 11;
		public const int LOCATION              = 12;
		public const int EDITOR                = 13;
		public const int BETA                  = 14;
		public const int LOGIN                 = 15;
		public const int CAMOUFLAGE_HIDDEN     = 16;
		public const int STICKER_POSITION      = 17;
		public const int CAMOUFLAGE_POSITIONS  = 18;
		public const int STICKER_POSITION_UNIT = 19;
		public const int ALLOW_POSITIONING     = 20;
		public const int SHOW_GRIDS            = 21;
		public const int USE_CUSTOM_STICKER    = 22;
		public const int CUSTOM_STICKER_PATH   = 23;
		public const int LICENSE               = 24;
		public const int GOOGLE_ANALYTICS      = 25;
		public const int DON_T_TRACK           = 26;
		public const int AUTO_FIT_WIDTH        = 27;

		/// <summary>
		/// It's useful, when app asks to save, if you change to another theme without saving previous one. But, it also can be annoying.
		/// I set isEdited in API , so it won't ask you each time. It'll ask when a theme is edited, and you're gonna change to another theme.
		/// </summary>
		public const int ASK_TO_SAVE = 28;

		public const int SAVE_AS_OLD        = 29;
		public const int SHOW_PREVIEW       = 30;
		public const int LOCALIZATION       = 31;
		public const int USE_DIMENSION_CAP  = 32;
		public const int DIMENSION_CAP_MAX  = 33;
		public const int DIMENSION_CAP_UNIT = 34;
		public const int COLOR_PICKER       = 35;
		public const int HIDE_ON_HOVER      = 36;
		public const int HIDE_DELAY         = 37;
		public const int PORTABLE_MODE      = 38;
		public const int EDITOR_READ_ONLY   = 39;
		public const int EDITOR_SAVED_FILE  = 40;

		public const  double CURRENT_VERSION     = 8.0;
		public const  string CURRENT_VERSION_ADD = "beta";
		public static string CurrentPath         = Path.GetDirectoryName (Assembly.GetEntryAssembly ()?.Location);
	}
}