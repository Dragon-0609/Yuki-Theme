using System;
using System.Collections.Generic;
using Yuki_Theme.Core.Database;

namespace Yuki_Theme.Core;

public static class Settings
{
	#region FIELDS

	public static int          actionChoice;
	public static bool         askChoice;
	public static bool         update;
	public static string       pascalPath = "empty";
	public static bool         bgImage;
	public static bool         swSticker;
	public static bool         swStatusbar;
	public static bool         swLogo;
	public static bool         Editor;
	public static bool         Beta;
	public static bool         Logged;
	public static bool         positioning;
	public static SettingMode  settingMode;
	public static RelativeUnit unit = RelativeUnit.Pixel;
	public static bool         showGrids;
	public static bool         useCustomSticker;
	public static string       customSticker = "";
	public static bool         license;
	public static bool         googleAnalytics;
	public static bool         dontTrack;
	public static bool         autoFitByWidth;
	public static bool         askToSave;
	public static bool         saveAsOld;
	public static bool         showPreview;
	public static string       localization = "";
	public static bool         useDimensionCap;
	public static int          dimensionCapMax;
	public static int          dimensionCapUnit;
	public static int          colorPicker;

	#endregion
	
	#region CONST

	public const int PASCAL_PATH          = 1;
	public const int ACTIVE              = 2;
	public const int ASK_CHOICE           = 4;
	public const int CHOICE_INDEX         = 5;
	public const int SETTING_MODE         = 6;
	public const int AUTO_UPDATE          = 7;
	public const int BG_IMAGE             = 8;
	public const int STICKER             = 9;
	public const int STATUS_BAR           = 10;
	public const int LOGO                = 11;
	public const int LOCATION            = 12;
	public const int EDITOR              = 13;
	public const int BETA                = 14;
	public const int LOGIN               = 15;
	public const int CAMOUFLAGE_HIDDEN    = 16;
	public const int STICKER_POSITION     = 17;
	public const int CAMOUFLAGE_POSITIONS = 18;
	public const int STICKER_POSITION_UNIT = 19;
	public const int ALLOW_POSITIONING    = 20;
	public const int SHOW_GRIDS           = 21;
	public const int USE_CUSTOM_STICKER    = 22;
	public const int CUSTOM_STICKER_PATH       = 23;
	public const int LICENSE             = 24;
	public const int GOOGLE_ANALYTICS     = 25;
	public const int DON_T_TRACK           = 26;
	public const int AUTO_FIT_WIDTH        = 27;

	/// <summary>
	/// It's useful, when app asks to save, if you change to another theme without saving previous one. But, it also can be annoying.
	/// I set isEdited in API , so it won't ask you each time. It'll ask when a theme is edited, and you're gonna change to another theme.
	/// </summary>
	public const int ASK_TO_SAVE = 28;

	public const int SAVE_AS_OLD    = 29;
	public const int SHOW_PREVIEW  = 30;
	public const int LOCALIZATION = 31;
	public const int USE_DIMENSION_CAP = 32;
	public const int DIMENSION_CAP_MAX = 33;
	public const int DIMENSION_CAP_UNIT = 34;
	public const int COLOR_PICKER = 35;
	
	
	public const  double CURRENT_VERSION     = 7.0;
	public const  string CURRENT_VERSION_ADD = "beta";

	#endregion

	public static DatabaseManager database = new DatabaseManager ();

	public static Localization.Localization translation = new Localization.Localization ();

	/// <summary>
	/// Get settings
	/// </summary>
	public static void connectAndGet ()
	{
		var data = database.ReadData ();
		pascalPath = data [PASCAL_PATH] == "empty" ? null : data [PASCAL_PATH];
		if (Helper.mode == ProductMode.Plugin)
		{
			pascalPath = API.currentPath;
		}

		if (pascalPath == null)
		{
			string defpas = "";
			if (Environment.Is64BitOperatingSystem)
			{
				defpas = Environment.GetFolderPath (Environment.SpecialFolder.ProgramFilesX86) + "PascalABC.NET";
				if (API.IsPascalDirectory (defpas))
				{
					pascalPath = defpas;
				} else
				{
					defpas = Environment.GetFolderPath (Environment.SpecialFolder.ProgramFiles) + "PascalABC.NET";
					if (API.IsPascalDirectory (defpas))
					{
						pascalPath = defpas;
					}
				}
			} else
			{
				defpas = Environment.GetFolderPath (Environment.SpecialFolder.ProgramFilesX86) + "PascalABC.NET";
				if (API.IsPascalDirectory (defpas))
				{
					pascalPath = defpas;
				}
			}
		}

		if (pascalPath == null) pascalPath = "";

		askChoice = bool.Parse (data [ASK_CHOICE]);
		update = bool.Parse (data [AUTO_UPDATE]);
		bgImage = bool.Parse (data [BG_IMAGE]);
		swSticker = bool.Parse (data [STICKER]);
		swStatusbar = bool.Parse (data [STATUS_BAR]);
		swLogo = bool.Parse (data [LOGO]);
		Editor = bool.Parse (data [EDITOR]);
		Beta = bool.Parse (data [BETA]);
		Logged = bool.Parse (data [LOGIN]);
		positioning = bool.Parse (data [ALLOW_POSITIONING]);
		showGrids = bool.Parse (data [SHOW_GRIDS]);
		useCustomSticker = bool.Parse (data [USE_CUSTOM_STICKER]);
		customSticker = data [CUSTOM_STICKER_PATH];

		license = bool.Parse (data [LICENSE]);
		googleAnalytics = bool.Parse (data [GOOGLE_ANALYTICS]);
		dontTrack = bool.Parse (data [DON_T_TRACK]);
		autoFitByWidth = bool.Parse (data [AUTO_FIT_WIDTH]);
		askToSave = bool.Parse (data [ASK_TO_SAVE]);
		saveAsOld = bool.Parse (data [SAVE_AS_OLD]);
		showPreview = bool.Parse (data [SHOW_PREVIEW]);
		useDimensionCap = bool.Parse (data [USE_DIMENSION_CAP]);
		dimensionCapMax = int.Parse (data [DIMENSION_CAP_MAX]);
		dimensionCapUnit = int.Parse (data [DIMENSION_CAP_UNIT]);
		colorPicker = int.Parse (data [COLOR_PICKER]);
		localization = data [LOCALIZATION];

		API.selectedItem = data [ACTIVE];
		var os = 0;
		int.TryParse (data [CHOICE_INDEX], out os);
		actionChoice = os;
		int.TryParse (data [SETTING_MODE], out os);
		settingMode = (SettingMode) os;
		int.TryParse (data [STICKER_POSITION_UNIT], out os);
		unit = (RelativeUnit) os;
		
		translation.LoadLocalization ();
	}

	/// <summary>
	/// Save current settings
	/// </summary>
	public static void SaveData ()
	{
		Dictionary <int, string> dict = PrepareToSave;
		database.UpdateData (dict);
		if (API_Events.onBGIMAGEChange != null) API_Events.onBGIMAGEChange ();
		if (API_Events.onSTICKERChange != null) API_Events.onSTICKERChange ();
		if (API_Events.onSTATUSChange != null) API_Events.onSTATUSChange ();
	}

	private static Dictionary <int, string> PrepareToSave
	{
		get
		{
			Dictionary <int, string> dict = new Dictionary <int, string>
			{
				{ PASCAL_PATH, pascalPath },
				{ ACTIVE, API.selectedItem },
				{ ASK_CHOICE, askChoice.ToString () },
				{ CHOICE_INDEX, actionChoice.ToString () },
				{ SETTING_MODE, ((int)settingMode).ToString () },
				{ AUTO_UPDATE, update.ToString () },
				{ BG_IMAGE, bgImage.ToString () },
				{ STICKER, swSticker.ToString () },
				{ STATUS_BAR, swStatusbar.ToString () },
				{ LOGO, swLogo.ToString () },
				{ EDITOR, Editor.ToString () },
				{ BETA, Beta.ToString () },
				{ ALLOW_POSITIONING, positioning.ToString () },
				{ SHOW_GRIDS, showGrids.ToString () },
				{ STICKER_POSITION_UNIT, ((int)unit).ToString () },
				{ USE_CUSTOM_STICKER, useCustomSticker.ToString () },
				{ CUSTOM_STICKER_PATH, customSticker },
				{ LICENSE, license.ToString () },
				{ GOOGLE_ANALYTICS, googleAnalytics.ToString () },
				{ DON_T_TRACK, dontTrack.ToString () },
				{ AUTO_FIT_WIDTH, autoFitByWidth.ToString () },
				{ ASK_TO_SAVE, askToSave.ToString () },
				{ SAVE_AS_OLD, saveAsOld.ToString () },
				{ SHOW_PREVIEW, showPreview.ToString () },
				{ USE_DIMENSION_CAP, useDimensionCap.ToString () },
				{ DIMENSION_CAP_MAX, dimensionCapMax.ToString () },
				{ DIMENSION_CAP_UNIT, dimensionCapUnit.ToString () },
				{ COLOR_PICKER, colorPicker.ToString () },
				{ LOCALIZATION, localization }
			};
			return dict;
		}
	}

	public static SortedDictionary <int, string> PrepareAll
	{
		get
		{
			Dictionary <int, string> dict = PrepareToSave;
			dict.Add (LOCATION, DatabaseManager.GetLocation ());
			dict.Add (CAMOUFLAGE_HIDDEN, database.ReadData (CAMOUFLAGE_HIDDEN, ""));
			dict.Add (LOGIN, Logged.ToString ());
			SortedDictionary <int, string> sorted = new SortedDictionary <int, string> (dict);
			
			return sorted;
		}
	}
}