using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using Yuki_Theme.Core;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Themes;
using Yuki_Theme.Core.Utils;

namespace Yuki_Theme.Tests
{
	[TestFixture]
	public class Tests
	{
		private bool      isInitialized  = false;
		private bool      isThemeAdded   = false;
		private bool      isThemeEdited  = false;
		private bool      isThemeChecked = false;
		private bool      isThemeSaved   = false;
		private bool      isThemeRemoved = false;
		private string [] fields;

		private Image img2, img3;

		private Dictionary <string, ThemeField> FieldValues = new Dictionary <string, ThemeField> ()
		{
			{ "Default Text", new ThemeField () { Background = "#323232", Foreground = "#DDDDDD" } },
			{ "Selection", new ThemeField () { Background = "#515151" } },
		};

		[Test]
		public void InitializationAndLoading ()
		{
			try
			{
				if (!isInitialized)
				{
					SetDefaultActions ();
					SettingsConst.CurrentPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
					ResetForTests ();
					ClearTestThemes ();
					Settings.ConnectAndGet ();
					CentralAPI.Current.LoadSchemes ();
					bool cnd = CentralAPI.Current.SelectTheme (Helper.GetRandomElement (CentralAPI.Current.Schemes));

					Assert.IsTrue (cnd);

					CentralAPI.Current.Restore (false);
					isInitialized = true;
				}
			} catch (Exception e)
			{
				Assert.Fail ("Expected no exception, but got: " + e.Message);
			}
		}

		[Test]
		public void AddNSelectNewTheme ()
		{
			if (!isInitialized)
				InitializationAndLoading ();
			string copyFrom = null;
			string copyTo = null;
			try
			{
				if (!isThemeAdded)
				{
					copyFrom = Helper.GetRandomElement (CentralAPI.Current.Schemes);
					copyTo = $"{copyFrom}_Test";
					CentralAPI.Current.AddTheme (copyFrom, copyTo);
					CentralAPI.Current.SelectTheme (copyTo);
					isThemeAdded = true;
				}
			} catch (Exception e)
			{
				if (copyTo != null && copyFrom != null)
				{
					string patsh = Path.Combine (SettingsConst.CurrentPath,
					                             $"Themes/{Helper.ConvertNameToPath (copyTo)}{Helper.GetExtension (CentralAPI.Current.ThemeInfos [copyFrom].isOld)}");
					if (File.Exists (patsh)) File.Delete (patsh);
				}

				Assert.Fail ("Expected no exception, but got: " + e.Message);
			}
		}

		[Test]
		public void EditNewTheme ()
		{
			if (!isThemeAdded)
				AddNSelectNewTheme ();
			try
			{
				if (!isThemeEdited)
				{
					Dictionary <string, ThemeField> values;
					if (Settings.settingMode == SettingMode.Light)
						values = FieldValues;
					else
						values = ThemeField.GetThemeFieldsWithRealNames (SyntaxType.Pascal, FieldValues);
					CentralAPI.Current.Restore (false, onFieldsLoaded);
					foreach (string field in fields)
					{
						if (values.ContainsKey (field))
						{
							ThemeField fiel = values [field];
							CentralAPI.Current.currentTheme.Fields [field].SetValues (fiel);
						}
					}

					isThemeEdited = true;
				}
			} catch (Exception e)
			{
				Assert.Fail ("Expected no exception, but got: " + e.Message);
			}
		}

		[Test]
		public void CheckNewTheme ()
		{
			if (!isThemeEdited)
				EditNewTheme ();
			try
			{
				if (!isThemeChecked)
				{
					bool equal = true;
					foreach (string field in fields)
					{
						if (FieldValues.ContainsKey (field))
						{
							ThemeField fiel = FieldValues [field];
							ThemeField fiel2 = CentralAPI.Current.currentTheme.Fields [field];
							if (fiel.Background != null)
								equal = equal && fiel.Background == fiel2.Background;

							if (fiel.Foreground != null)
								equal = equal && fiel.Foreground == fiel2.Foreground;
						}
					}

					if (!equal)
						Assert.Fail ("Fields aren't equal");
					isThemeChecked = true;
				}
			} catch (Exception e)
			{
				Assert.Fail ("Expected no exception, but got: " + e.Message);
			}
		}

		[Test]
		public void SaveNewTheme ()
		{
			if (!isThemeChecked)
				CheckNewTheme ();
			try
			{
				if (!isThemeSaved)
				{
					foreach (KeyValuePair <string, ThemeField> themeField in CentralAPI.Current.currentTheme.Fields)
					{
						Console.WriteLine ("{0}: {1}", themeField.Key, themeField.Value.ToString ());
					}

					// Console.WriteLine(Core.API_Base.Current.currentTheme.Fields ["Default Text"].Background);
					// Console.WriteLine(Core.API_Base.Current.currentTheme.Fields ["Default Text"].Foreground);
					CentralAPI.Current.Save (img2, img3);

					isThemeSaved = true;
				}
			} catch (Exception e)
			{
				Assert.Fail ("Expected no exception, but got: " + e.Message);
			}
		}

		[Test]
		public void RemoveNewTheme ()
		{
			if (!isThemeSaved)
				SaveNewTheme ();
			try
			{
				if (isThemeRemoved)
				{
					CentralAPI.Current.RemoveTheme (CentralAPI.Current.nameToLoad, (s, s1) => true, null, null);
					
					isThemeRemoved = true;
				}
			} catch (Exception e)
			{
				Assert.Fail ("Expected no exception, but got: " + e.Message);
			}
		}

		private void onFieldsLoaded ()
		{
			fields = CentralAPI.Current.names.ToArray ();
		}

		private void ResetForTests ()
		{
			CentralAPI.Current.Schemes.Clear ();
			DefaultThemes.Clear ();
		}

		private void SetDefaultActions ()
		{
			API_Events.showError = (s,    s1) => { Console.WriteLine ($"{s1}: {s}"); };
			API_Events.SaveInExport = (s, s1) => true;
			API_Events.ifHasImage = image => { img2 = image; };
			API_Events.ifHasSticker = image => { img3 = image; };
			API_Events.ifDoesntHave = () => { img2 = null; };
			API_Events.ifDoesntHaveSticker = () => { img3 = null; };
		}

		private void ClearTestThemes ()
		{
			if (Directory.Exists (Path.Combine (SettingsConst.CurrentPath, "Themes")))
			{
				string [] files = Directory.GetFiles (Path.Combine (SettingsConst.CurrentPath, "Themes"), "*_Test.yukitheme");
				foreach (string file in files)
				{
					File.Delete (file);
				}
			}
		}
	}
}