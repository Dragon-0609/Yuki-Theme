using System.Windows.Forms;

namespace Yuki_Theme.Core
{
	public class Populater
	{
		public bool isInList (string str, ListBox.ObjectCollection coll)
		{
			bool res = false;
			switch (str)
			{
					
				case "SpaceMarkers" : case "TabMarkers" :
				{
					res = coll.Contains ("EOLMarkers");
				}
					break;
				case "LineComment" :
				case "BlockComment" :
				case "BlockComment2" :
				{
					res = coll.Contains ("LineBigComment");
				}
					break;

				case "ProgramSections" :
				case "Async" :
				case "RaiseStatement" :
				case "JumpStatements" :
				case "JumpProcedures" :
				case "Modifiers" :
				case "AccessModifiers" :
				{
					res = coll.Contains ("KeyWords");
				}
					break;
				
				case "SpecialDireciveNames" :
				case "DireciveValues" :
				{
					res = coll.Contains ("DireciveNames");
				}
					break;
			}

			return res;
		}
		
		public string[] getDependencies (string str)
		{
			string [] res = new string [] { };
			switch (str)
			{
				case "EOLMarkers" :
				{
					res = new string [] {"SpaceMarkers","TabMarkers"};
				}
					break;
				
				case "LineBigComment" :
				{
					res = new string [] {"LineComment", "BlockComment", "BlockComment2"};
				}
					break;

				case "KeyWords" :

				{
					res = new string []
					{
						"ProgramSections", "Async", "RaiseStatement", "JumpStatements", "JumpProcedures", "Modifiers",
						"AccessModifiers"
					};
				}
					break;
				
				case "DireciveNames" :

				{
					res = new string [] {"SpecialDireciveNames", "DireciveValues"};
				}
					break;
			}

			return res;
		}
	}
}