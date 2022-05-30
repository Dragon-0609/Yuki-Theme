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

	public const int PASCALPATH          = 1;
	public const int ACTIVE              = 2;
	public const int ASKCHOICE           = 4;
	public const int CHOICEINDEX         = 5;
	public const int SETTINGMODE         = 6;
	public const int AUTOUPDATE          = 7;
	public const int BGIMAGE             = 8;
	public const int STICKER             = 9;
	public const int STATUSBAR           = 10;
	public const int LOGO                = 11;
	public const int LOCATION            = 12;
	public const int EDITOR              = 13;
	public const int BETA                = 14;
	public const int LOGIN               = 15;
	public const int CAMOUFLAGEHIDDEN    = 16;
	public const int STICKERPOSITION     = 17;
	public const int CAMOUFLAGEPOSITIONS = 18;
	public const int STICKERPOSITIONUNIT = 19;
	public const int ALLOWPOSITIONING    = 20;
	public const int SHOWGRIDS           = 21;
	public const int USECUSTOMSTICKER    = 22;
	public const int CUSTOMSTICKER       = 23;
	public const int LICENSE             = 24;
	public const int GOOGLEANALYTICS     = 25;
	public const int DONTTRACK           = 26;
	public const int AUTOFITWIDTH        = 27;

	/// <summary>
	/// It's useful, when app asks to save, if you change to another theme without saving previous one. But, it also can be annoying.
	/// I set isEdited in CLI , so it won't ask you each time. It'll ask when a theme is edited, and you're gonna change to another theme.
	/// </summary>
	public const int ASKTOSAVE = 28;

	public const int SAVEASOLD    = 29;
	public const int SHOWPREVIEW  = 30;
	public const int LOCALIZATION = 31;
	public const int USEDIMENSIONCAP = 32;
	public const int DIMENSIONCAPMAX = 33;
	public const int DIMENSIONCAPUNIT = 34;
	public const int COLORPICKER = 35;
	
	
	public const  double current_version     = 7.0;
	public const  string current_version_add = "";
	public static string next_version        = "";

	#endregion

	public static DatabaseManager database = new DatabaseManager ();

	public static Localization.Localization translation = new Localization.Localization ();

	/// <summary>
	/// Get settings
	/// </summary>
	public static void connectAndGet ()
	{
		var data = database.ReadData ();
		pascalPath = data [PASCALPATH] == "empty" ? null : data [PASCALPATH];
		if (Helper.mode == ProductMode.Plugin)
		{
			pascalPath = CLI.currentPath;
		}

		if (pascalPath == null)
		{
			string defpas = "";
			if (Environment.Is64BitOperatingSystem)
			{
				defpas = Environment.GetFolderPath (Environment.SpecialFolder.ProgramFilesX86) + "PascalABC.NET";
				if (CLI.isPasalDirectory (defpas))
				{
					pascalPath = defpas;
				} else
				{
					defpas = Environment.GetFolderPath (Environment.SpecialFolder.ProgramFiles) + "PascalABC.NET";
					if (CLI.isPasalDirectory (defpas))
					{
						pascalPath = defpas;
					}
				}
			} else
			{
				defpas = Environment.GetFolderPath (Environment.SpecialFolder.ProgramFilesX86) + "PascalABC.NET";
				if (CLI.isPasalDirectory (defpas))
				{
					pascalPath = defpas;
				}
			}
		}

		if (pascalPath == null) pascalPath = "";

		askChoice = bool.Parse (data [ASKCHOICE]);
		update = bool.Parse (data [AUTOUPDATE]);
		bgImage = bool.Parse (data [BGIMAGE]);
		swSticker = bool.Parse (data [STICKER]);
		swStatusbar = bool.Parse (data [STATUSBAR]);
		swLogo = bool.Parse (data [LOGO]);
		Editor = bool.Parse (data [EDITOR]);
		Beta = bool.Parse (data [BETA]);
		Logged = bool.Parse (data [LOGIN]);
		positioning = bool.Parse (data [ALLOWPOSITIONING]);
		showGrids = bool.Parse (data [SHOWGRIDS]);
		useCustomSticker = bool.Parse (data [USECUSTOMSTICKER]);
		customSticker = data [CUSTOMSTICKER];

		license = bool.Parse (data [LICENSE]);
		googleAnalytics = bool.Parse (data [GOOGLEANALYTICS]);
		dontTrack = bool.Parse (data [DONTTRACK]);
		autoFitByWidth = bool.Parse (data [AUTOFITWIDTH]);
		askToSave = bool.Parse (data [ASKTOSAVE]);
		saveAsOld = bool.Parse (data [SAVEASOLD]);
		showPreview = bool.Parse (data [SHOWPREVIEW]);
		useDimensionCap = bool.Parse (data [USEDIMENSIONCAP]);
		dimensionCapMax = int.Parse (data [DIMENSIONCAPMAX]);
		dimensionCapUnit = int.Parse (data [DIMENSIONCAPUNIT]);
		colorPicker = int.Parse (data [COLORPICKER]);
		localization = data [LOCALIZATION];

		CLI.selectedItem = data [ACTIVE];
		var os = 0;
		int.TryParse (data [CHOICEINDEX], out os);
		actionChoice = os;
		int.TryParse (data [SETTINGMODE], out os);
		settingMode = (SettingMode) os;
		int.TryParse (data [STICKERPOSITIONUNIT], out os);
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
		if (CLI_Actions.onBGIMAGEChange != null) CLI_Actions.onBGIMAGEChange ();
		if (CLI_Actions.onSTICKERChange != null) CLI_Actions.onSTICKERChange ();
		if (CLI_Actions.onSTATUSChange != null) CLI_Actions.onSTATUSChange ();
	}

	private static Dictionary <int, string> PrepareToSave
	{
		get
		{
			Dictionary <int, string> dict = new Dictionary <int, string>
			{
				{ PASCALPATH, pascalPath },
				{ ACTIVE, CLI.selectedItem },
				{ ASKCHOICE, askChoice.ToString () },
				{ CHOICEINDEX, actionChoice.ToString () },
				{ SETTINGMODE, ((int)settingMode).ToString () },
				{ AUTOUPDATE, update.ToString () },
				{ BGIMAGE, bgImage.ToString () },
				{ STICKER, swSticker.ToString () },
				{ STATUSBAR, swStatusbar.ToString () },
				{ LOGO, swLogo.ToString () },
				{ EDITOR, Editor.ToString () },
				{ BETA, Beta.ToString () },
				{ ALLOWPOSITIONING, positioning.ToString () },
				{ SHOWGRIDS, showGrids.ToString () },
				{ STICKERPOSITIONUNIT, ((int)unit).ToString () },
				{ USECUSTOMSTICKER, useCustomSticker.ToString () },
				{ CUSTOMSTICKER, customSticker },
				{ LICENSE, license.ToString () },
				{ GOOGLEANALYTICS, googleAnalytics.ToString () },
				{ DONTTRACK, dontTrack.ToString () },
				{ AUTOFITWIDTH, autoFitByWidth.ToString () },
				{ ASKTOSAVE, askToSave.ToString () },
				{ SAVEASOLD, saveAsOld.ToString () },
				{ SHOWPREVIEW, showPreview.ToString () },
				{ USEDIMENSIONCAP, useDimensionCap.ToString () },
				{ DIMENSIONCAPMAX, dimensionCapMax.ToString () },
				{ DIMENSIONCAPUNIT, dimensionCapUnit.ToString () },
				{ COLORPICKER, colorPicker.ToString () },
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
			dict.Add (CAMOUFLAGEHIDDEN, database.ReadData (CAMOUFLAGEHIDDEN, ""));
			dict.Add (LOGIN, Logged.ToString ());
			SortedDictionary <int, string> sorted = new SortedDictionary <int, string> (dict);
			
			return sorted;
		}
	}
}