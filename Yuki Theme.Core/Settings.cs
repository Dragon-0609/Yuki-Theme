using System;
using System.Collections.Generic;
using Yuki_Theme.Core.Database;

namespace Yuki_Theme.Core
{
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

		public static DatabaseManager database = new DatabaseManager (true);

		public static Localization.Localization translation = new Localization.Localization ();

		/// <summary>
		/// Get settings
		/// </summary>
		public static void ConnectAndGet ()
		{
			Dictionary<int, string> data = database.ReadData ();
			pascalPath = data [SettingsConst.PASCAL_PATH] == "empty" ? null : data [SettingsConst.PASCAL_PATH];
			if (Helper.mode == ProductMode.Plugin)
			{
				pascalPath = SettingsConst.CurrentPath;
			}

			if (pascalPath == null)
			{
				string defpas = "";
				if (Environment.Is64BitOperatingSystem)
				{
					defpas = Environment.GetFolderPath (Environment.SpecialFolder.ProgramFilesX86) + "PascalABC.NET";
					if (API_Base.Current.IsPascalDirectory (defpas))
					{
						pascalPath = defpas;
					} else
					{
						defpas = Environment.GetFolderPath (Environment.SpecialFolder.ProgramFiles) + "PascalABC.NET";
						if (API_Base.Current.IsPascalDirectory (defpas))
						{
							pascalPath = defpas;
						}
					}
				} else
				{
					defpas = Environment.GetFolderPath (Environment.SpecialFolder.ProgramFilesX86) + "PascalABC.NET";
					if (API_Base.Current.IsPascalDirectory (defpas))
					{
						pascalPath = defpas;
					}
				}
			}

			if (pascalPath == null) pascalPath = "";

			askChoice = bool.Parse (data [SettingsConst.ASK_CHOICE]);
			update = bool.Parse (data [SettingsConst.AUTO_UPDATE]);
			bgImage = bool.Parse (data [SettingsConst.BG_IMAGE]);
			swSticker = bool.Parse (data [SettingsConst.STICKER]);
			swStatusbar = bool.Parse (data [SettingsConst.STATUS_BAR]);
			swLogo = bool.Parse (data [SettingsConst.LOGO]);
			Editor = bool.Parse (data [SettingsConst.EDITOR]);
			Beta = bool.Parse (data [SettingsConst.BETA]);
			Logged = bool.Parse (data [SettingsConst.LOGIN]);
			positioning = bool.Parse (data [SettingsConst.ALLOW_POSITIONING]);
			showGrids = bool.Parse (data [SettingsConst.SHOW_GRIDS]);
			useCustomSticker = bool.Parse (data [SettingsConst.USE_CUSTOM_STICKER]);
			customSticker = data [SettingsConst.CUSTOM_STICKER_PATH];

			license = bool.Parse (data [SettingsConst.LICENSE]);
			googleAnalytics = bool.Parse (data [SettingsConst.GOOGLE_ANALYTICS]);
			dontTrack = bool.Parse (data [SettingsConst.DON_T_TRACK]);
			autoFitByWidth = bool.Parse (data [SettingsConst.AUTO_FIT_WIDTH]);
			askToSave = bool.Parse (data [SettingsConst.ASK_TO_SAVE]);
			saveAsOld = bool.Parse (data [SettingsConst.SAVE_AS_OLD]);
			showPreview = bool.Parse (data [SettingsConst.SHOW_PREVIEW]);
			useDimensionCap = bool.Parse (data [SettingsConst.USE_DIMENSION_CAP]);
			dimensionCapMax = int.Parse (data [SettingsConst.DIMENSION_CAP_MAX]);
			dimensionCapUnit = int.Parse (data [SettingsConst.DIMENSION_CAP_UNIT]);
			colorPicker = int.Parse (data [SettingsConst.COLOR_PICKER]);
			localization = data [SettingsConst.LOCALIZATION];

			API_Base.Current.selectedItem = data [SettingsConst.ACTIVE];
			var os = 0;
			int.TryParse (data [SettingsConst.CHOICE_INDEX], out os);
			actionChoice = os;
			int.TryParse (data [SettingsConst.SETTING_MODE], out os);
			settingMode = (SettingMode) os;
			int.TryParse (data [SettingsConst.STICKER_POSITION_UNIT], out os);
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
					{ SettingsConst.PASCAL_PATH, pascalPath },
					{ SettingsConst.ACTIVE, API_Base.Current.selectedItem },
					{ SettingsConst.ASK_CHOICE, askChoice.ToString () },
					{ SettingsConst.CHOICE_INDEX, actionChoice.ToString () },
					{ SettingsConst.SETTING_MODE, ((int)settingMode).ToString () },
					{ SettingsConst.AUTO_UPDATE, update.ToString () },
					{ SettingsConst.BG_IMAGE, bgImage.ToString () },
					{ SettingsConst.STICKER, swSticker.ToString () },
					{ SettingsConst.STATUS_BAR, swStatusbar.ToString () },
					{ SettingsConst.LOGO, swLogo.ToString () },
					{ SettingsConst.EDITOR, Editor.ToString () },
					{ SettingsConst.BETA, Beta.ToString () },
					{ SettingsConst.ALLOW_POSITIONING, positioning.ToString () },
					{ SettingsConst.SHOW_GRIDS, showGrids.ToString () },
					{ SettingsConst.STICKER_POSITION_UNIT, ((int)unit).ToString () },
					{ SettingsConst.USE_CUSTOM_STICKER, useCustomSticker.ToString () },
					{ SettingsConst.CUSTOM_STICKER_PATH, customSticker },
					{ SettingsConst.LICENSE, license.ToString () },
					{ SettingsConst.GOOGLE_ANALYTICS, googleAnalytics.ToString () },
					{ SettingsConst.DON_T_TRACK, dontTrack.ToString () },
					{ SettingsConst.AUTO_FIT_WIDTH, autoFitByWidth.ToString () },
					{ SettingsConst.ASK_TO_SAVE, askToSave.ToString () },
					{ SettingsConst.SAVE_AS_OLD, saveAsOld.ToString () },
					{ SettingsConst.SHOW_PREVIEW, showPreview.ToString () },
					{ SettingsConst.USE_DIMENSION_CAP, useDimensionCap.ToString () },
					{ SettingsConst.DIMENSION_CAP_MAX, dimensionCapMax.ToString () },
					{ SettingsConst.DIMENSION_CAP_UNIT, dimensionCapUnit.ToString () },
					{ SettingsConst.COLOR_PICKER, colorPicker.ToString () },
					{ SettingsConst.LOCALIZATION, localization }
				};
				return dict;
			}
		}

		public static SortedDictionary <int, string> PrepareAll
		{
			get
			{
				Dictionary <int, string> dict = PrepareToSave;
				dict.Add (SettingsConst.LOCATION, database.GetLocation ());
				dict.Add (SettingsConst.CAMOUFLAGE_HIDDEN, database.ReadData (SettingsConst.CAMOUFLAGE_HIDDEN, ""));
				dict.Add (SettingsConst.LOGIN, Logged.ToString ());
				SortedDictionary <int, string> sorted = new SortedDictionary <int, string> (dict);
			
				return sorted;
			}
		}
	}
}