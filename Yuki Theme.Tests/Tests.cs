using System;
using System.IO;
using NUnit.Framework;
using Yuki_Theme.Core;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Tests
{
	[TestFixture]
	public class Tests
	{
		private bool isInitialized = false;
		private bool isThemeAdded  = false;
		private bool isThemeEdited = false;

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
		public void AddNewTheme ()
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
			try
			{
				if(!isThemeEdited)
				{
					if (!isThemeAdded)
						AddNewTheme ();
					
					
					
					isThemeEdited = true;
				}
			} catch (Exception e)
			{
				Assert.Fail ("Expected no exception, but got: " + e.Message);
			}
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
			CLI_Actions.showError = (s, s1) => { Console.WriteLine ($"{s1}: {s}"); };
		}

		private void ClearTestThemes ()
		{
			string [] files = Directory.GetFiles (Path.Combine (CLI.currentPath, "Themes"), "_Test");
			foreach (string file in files)
			{
				File.Delete (file);
			}
		}
	}
}