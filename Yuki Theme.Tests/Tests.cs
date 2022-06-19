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
					Core.CLI.currentPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
					ResetForTests ();
					ClearTestThemes ();
					Settings.connectAndGet ();
					Core.CLI.load_schemes ();
					bool cnd = Core.CLI.SelectTheme (Helper.GetRandomElement (Core.CLI.schemes));

					Assert.IsTrue (cnd);

					Core.CLI.restore (false);
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
					copyFrom = Helper.GetRandomElement (Core.CLI.schemes);
					copyTo = $"{copyFrom}_Test";
					Core.CLI.add (copyFrom, copyTo);
					Core.CLI.SelectTheme (copyTo);
					isThemeAdded = true;
				}
			} catch (Exception e)
			{
				if (copyTo != null && copyFrom != null)
				{
					string patsh = Path.Combine (Core.CLI.currentPath,
					                             $"Themes/{Helper.ConvertNameToPath (copyTo)}{Helper.GetExtension (Core.CLI.ThemeInfos [copyFrom].isOld)}");
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
					Core.CLI.restore (false, onFieldsLoaded);
					foreach (string field in fields)
					{
						if (values.ContainsKey (field))
						{
							ThemeField fiel = values [field];
							Core.CLI.currentTheme.Fields [field].SetValues (fiel);
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
							ThemeField fiel2 = Core.CLI.currentTheme.Fields [field];
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
					foreach (KeyValuePair <string, ThemeField> themeField in Core.CLI.currentTheme.Fields)
					{
						Console.WriteLine ("{0}: {1}", themeField.Key, themeField.Value.ToString ());
					}

					// Console.WriteLine(Core.CLI.currentTheme.Fields ["Default Text"].Background);
					// Console.WriteLine(Core.CLI.currentTheme.Fields ["Default Text"].Foreground);
					Core.CLI.save (img2, img3);

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
					Core.CLI.remove (Core.CLI.nameToLoad, (s, s1) => true, null, null);
					
					isThemeRemoved = true;
				}
			} catch (Exception e)
			{
				Assert.Fail ("Expected no exception, but got: " + e.Message);
			}
		}

		private void onFieldsLoaded ()
		{
			fields = Core.CLI.names.ToArray ();
		}

		private void ResetForTests ()
		{
			Core.CLI.schemes.Clear ();
			DefaultThemes.categories.Clear ();
			DefaultThemes.headers.Clear ();
			DefaultThemes.names.Clear ();
			DefaultThemes.categoriesList.Clear ();
			DefaultThemes.headersList.Clear ();
		}

		private void SetDefaultActions ()
		{
			Core.CLI_Actions.showError = (s,    s1) => { Console.WriteLine ($"{s1}: {s}"); };
			Core.CLI_Actions.SaveInExport = (s, s1) => true;
			Core.CLI_Actions.ifHasImage = image => { img2 = image; };
			Core.CLI_Actions.ifHasSticker = image => { img3 = image; };
			Core.CLI_Actions.ifDoesntHave = () => { img2 = null; };
			Core.CLI_Actions.ifDoesntHaveSticker = () => { img3 = null; };
		}

		private void ClearTestThemes ()
		{
			if (Directory.Exists (Path.Combine (Core.CLI.currentPath, "Themes")))
			{
				string [] files = Directory.GetFiles (Path.Combine (Core.CLI.currentPath, "Themes"), "*_Test.yukitheme");
				foreach (string file in files)
				{
					File.Delete (file);
				}
			}
		}
	}
}