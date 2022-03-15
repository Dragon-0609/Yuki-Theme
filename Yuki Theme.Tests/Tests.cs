using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using Yuki_Theme.Core;
using Yuki_Theme.Core.Themes;

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
					CLI.currentPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
					ResetForTests ();
					ClearTestThemes ();
					Settings.connectAndGet ();
					CLI.load_schemes ();
					bool cnd = CLI.SelectTheme (Helper.GetRandomElement (CLI.schemes));

					Assert.IsTrue (cnd);

					CLI.restore (false);
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
					copyFrom = Helper.GetRandomElement (CLI.schemes);
					copyTo = $"{copyFrom}_Test";
					CLI.add (copyFrom, copyTo);
					CLI.SelectTheme (copyTo);
					isThemeAdded = true;
				}
			} catch (Exception e)
			{
				if (copyTo != null && copyFrom != null)
				{
					string patsh = Path.Combine (CLI.currentPath,
					                             $"Themes/{Helper.ConvertNameToPath (copyTo)}" + (CLI.oldThemeList [copyFrom]
						                             ? Helper.FILE_EXTENSTION_OLD
						                             : Helper.FILE_EXTENSTION_NEW));
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
					CLI.restore (false, onFieldsLoaded);
					foreach (string field in fields)
					{
						if (values.ContainsKey (field))
						{
							ThemeField fiel = values [field];
							CLI.currentTheme.Fields [field].SetValues (fiel);
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
							ThemeField fiel2 = CLI.currentTheme.Fields [field];
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
					foreach (KeyValuePair <string, ThemeField> themeField in CLI.currentTheme.Fields)
					{
						Console.WriteLine ("{0}: {1}", themeField.Key, themeField.Value.ToString ());
					}

					// Console.WriteLine(CLI.currentTheme.Fields ["Default Text"].Background);
					// Console.WriteLine(CLI.currentTheme.Fields ["Default Text"].Foreground);
					CLI.save (img2, img3);

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
					CLI.remove (CLI.nameToLoad, (s, s1) => true, null, null);
					
					isThemeRemoved = true;
				}
			} catch (Exception e)
			{
				Assert.Fail ("Expected no exception, but got: " + e.Message);
			}
		}

		private void onFieldsLoaded ()
		{
			fields = CLI.names.ToArray ();
		}

		private void ResetForTests ()
		{
			CLI.schemes.Clear ();
			DefaultThemes.categories.Clear ();
			DefaultThemes.headers.Clear ();
			DefaultThemes.names.Clear ();
			DefaultThemes.categoriesList.Clear ();
			DefaultThemes.headersList.Clear ();
		}

		private void SetDefaultActions ()
		{
			CLI_Actions.showError = (s,    s1) => { Console.WriteLine ($"{s1}: {s}"); };
			CLI_Actions.SaveInExport = (s, s1) => true;
			CLI_Actions.ifHasImage = image => { img2 = image; };
			CLI_Actions.ifHasSticker = image => { img3 = image; };
			CLI_Actions.ifDoesntHave = () => { img2 = null; };
			CLI_Actions.ifDoesntHaveSticker = () => { img3 = null; };
		}

		private void ClearTestThemes ()
		{
			if (Directory.Exists (Path.Combine (CLI.currentPath, "Themes")))
			{
				string [] files = Directory.GetFiles (Path.Combine (CLI.currentPath, "Themes"), "*_Test.yukitheme");
				foreach (string file in files)
				{
					File.Delete (file);
				}
			}
		}
	}
}