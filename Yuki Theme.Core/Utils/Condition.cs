using System;
using System.Collections.Generic;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core.Utils
{
	public class Condition
	{
		public static Dictionary <string, Func <ThemeInfo, Condition, bool>> Conditions = new ()
		{
			{ "group", CheckGroupCondition },
			{ "token", CheckTokenCondition }
		};

		public static Dictionary <string, Func <string, ThemeInfo, Condition, ThemeInfo>> Fields = new ()
		{
			{ "group", SetGroup },
			{ "token", SetToken },
			
		};

		public static Dictionary <string, bool> NeedToLoadThemeInConditions = new ()
		{
			{ "group", false }, { "token", false },
		};

		public static Dictionary <string, bool> NeedToLoadThemeInFields = new ()
		{
			{ "group", false }
		};

		public string Target   = "";
		public string Equality = "";

		public bool CouldSetOneOfThem (string value)
		{
			if (Target == "")
			{
				if (Conditions.ContainsKey (value))
					Target = value;
			} else if (Equality == "") Equality = value;
			else return false;

			return true;
		}

		public bool CouldSetOneOfThem (string value, string value2)
		{
			if (Target == "") Target = value;
			else if (Equality == "") Equality = value2;
			else return false;
			return true;
		}


		private static bool CheckGroupCondition (ThemeInfo info, Condition condition)
		{
			if (condition.Equality.ToLower () == "null")
				return info.group == "" || info.group == CentralAPI.Current.Translate ("messages.theme.group.custom");
			else
				return info.group == condition.Equality;
		}

		private static bool CheckTokenCondition (ThemeInfo info, Condition condition)
		{
			bool parsed = false;
			bool.TryParse (condition.Equality, out parsed);
			return info.isTokenValid == parsed;
		}

		private static ThemeInfo SetGroup (string name, ThemeInfo info, Condition value)
		{
			info.group = value.Equality;
			Theme theme = API.CentralAPI.Current.GetTheme (name);
			theme.Group = info.group;
			API.CentralAPI.Current.SaveTheme (theme, null, null, true);
			return info;
		}
		
		private static ThemeInfo SetToken (string name, ThemeInfo info, Condition value)
		{
			Theme theme = API.CentralAPI.Current.GetTheme (name);
			if (value.Equality == "null")
			{
				info.isTokenValid = false;
				theme.Token = value.Equality;
			}
			API.CentralAPI.Current.SaveTheme (theme, null, null, true);
			return info;
		}
		
		
		/*private Condition toCheckAnd  = null;
		private Condition toCheckAnd2 = null;

		private Condition toCheckOr  = null;
		private Condition toCheckOr2 = null;

		public Condition ToCheckAnd
		{
			get => toCheckAnd;
			set
			{
				toCheckAnd = value;
				if (value != null){
					value.toCheckAnd = this;
				}
			}
		}

		public Condition ToCheckAnd2
		{
			get => toCheckAnd2;
			set
			{
				toCheckAnd2 = value;
				if (value != null){
					value.toCheckAnd2 = this;
				}
			}
		}

		public Condition ToCheckOr
		{
			get => toCheckOr;
			set
			{
				toCheckOr = value;
				if (value != null){
					value.toCheckOr = this;
				}
			}
		}

		public Condition ToCheckOr2
		{
			get => toCheckOr2;
			set
			{
				toCheckOr2 = value;
				if (value != null){
					value.toCheckOr2 = this;
				}
			}
		
		}*/

	}
}