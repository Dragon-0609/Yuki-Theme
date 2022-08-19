using System.Collections.Generic;
using System.Linq;
using Yuki_Theme.Core;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Themes;
using Yuki_Theme.Core.Utils;

namespace Yuki_Theme.CLI
{
	public class ConditionalEdition
	{
		
		/// <summary>
		/// Set parameter by conditional.<br/> Syntax: features edit if &lt;parameter: group, token&gt; &lt;value: null, true, false&gt; set &lt;parameter: group&gt; &lt;value&gt;
		/// <br/> Example: features edit if token true &amp;&amp; group null set group "Custom Theme"
		/// </summary>
		/// <param name="commands">Parsed Commands</param>
		internal void Edit (IEnumerable <string> commands)
		{
			CollectConditionsAndFields (commands, out ConditionSet conditionSet);
			if (conditionSet != null)
			{
				CheckAndSet (conditionSet);
			}
		}
		
		private void CollectConditionsAndFields (IEnumerable <string> commands, out ConditionSet conditionSet)
		{
			string [] coms = commands.ToArray ();
			bool isSetMode = false;
			List <Condition> conditions = new List <Condition> ();
			List <Condition> fieldsToSet = new List <Condition> ();
			Condition currentCondition = new Condition ();

			foreach (string commad in coms)
			{
				string expandedCommand = commad;
				if (commad.StartsWith ("\"") && commad.EndsWith ("\"") && commad.Length > 2)
				{
					expandedCommand = commad.Substring (1, commad.Length - 2);
				}
				string command = expandedCommand.ToLower ();
 
				if (ConditionalSkip (command)) continue;

				if (!isSetMode)
				{
					if (IsAndOperator (command))
					{
						if (currentCondition.Equality != "") conditions.Add (currentCondition);
						currentCondition = new Condition ();
						continue;
					}
					
					if (IsSetMode (command))
					{
						isSetMode = true;
						if (currentCondition.Equality != "") conditions.Add (currentCondition);
						currentCondition = new Condition ();
						continue;
					}
				}

				if (isSetMode)
				{
					if (conditions.Count == 0)
					{
						ShowError (API.Current.Translate ("cli.errors.conditions.null"));
						conditionSet = null;
						return;
					}

					if (!currentCondition.CouldSetOneOfThem (command, expandedCommand))
					{
						fieldsToSet.Add (currentCondition);
						currentCondition = new Condition ();
					}
				} else
				{
					if (!currentCondition.CouldSetOneOfThem (command, expandedCommand))
					{
						conditions.Add (currentCondition);
						currentCondition = new Condition ();
					}
				}
			}
			fieldsToSet.Add (currentCondition);
			
			if (conditions.Count > 0 && fieldsToSet.Count > 0)
			{
				conditionSet = new ConditionSet () { conditions = conditions.ToArray (), fieldsToSet = fieldsToSet.ToArray () };
			} else
			{
				ShowError (API.Current.Translate (conditions.Count > 0 ? "cli.errors.setter.null" : "cli.errors.conditions.null"));
				conditionSet = null;
			}
		}

		
		private string [] CommandsToSkip
		{
			get => new [] { "edit", "if" };
		}

		private bool ConditionalSkip (string command)
		{
			return CommandsToSkip.Contains (command);
		}

		private bool IsSetMode (string command)
		{
			return command == "set";
		}

		private bool IsAndOperator (string command)
		{
			return command == "&&";
		}

		private void CheckAndSet (ConditionSet set)
		{
			Dictionary <string, ThemeInfo> update = new Dictionary <string, ThemeInfo> ();

			foreach (KeyValuePair <string, ThemeInfo> pair in API.Current.ThemeInfos)
			{
				if (pair.Value.location != ThemeLocation.Memory)
				{
					bool canOperate = true;
					Theme theme = null;
					string name = pair.Key;
					foreach (Condition condition in set.conditions)
					{
						LoadIfNecessary (name, condition, ref theme, true);
						if (condition != null) canOperate = canOperate && CheckCondition (name, pair.Value, condition);
					}

					if (canOperate)
					{
						foreach (Condition field in set.fieldsToSet)
						{
							LoadIfNecessary (name, field, ref theme, false);
							if (field != null)
							{
								ThemeInfo changedInfo = SetField (name, pair.Value, field);
								update.Add (pair.Key, changedInfo);
							}
						}
					}
				}
			}

			if (update.Count > 0)
			{
				foreach (KeyValuePair <string, ThemeInfo> pair in update)
				{
					API.Current.ThemeInfos [pair.Key] = pair.Value;
				}
			}
		}

		private void LoadIfNecessary (string name, Condition condition, ref Theme  theme, bool inCondition)
		{
			if (inCondition)
			{
				bool need = Condition.NeedToLoadThemeInConditions [condition.Target];
				if (need && theme == null)
				{
					theme = API.Current.GetTheme (name);
				}
			}
		}

		private bool CheckCondition (string name, ThemeInfo info, Condition condition)
		{
			bool result = info.CheckCondition (name, condition);
			return result;
		}

		private ThemeInfo SetField (string name, ThemeInfo info, Condition condition)
		{
			return info.SetField (name, condition);
		}

		private void ShowError (string text) => Program.program.ShowError (text);
	}
}