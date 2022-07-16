using System.Linq;

namespace Yuki_Theme.Core.Utils;

public class HighlitherUtil
{
	public static string[] names =
	{
		"linebigcomment", "linecomment", "blockcomment", "blockcomment2", "string", "digits", "beginend", "keywords", "programsections",
		"punctuation", "nonreserved1", "async","operatorkeywords", "selectionstatements", "iterationstatements", "exceptionhandlingstatements",
		"raisestatement", "jumpstatements", "jumpprocedures", "internalconstant", "internaltypes", "referencetypes", "modifiers",
		"accessmodifiers", "accesskeywords1", "errorwords", "warningwords", "direcivenames", "specialdirecivenames", "direcivevalues",
		"markprevious"
	};

	public static bool IsInColors (string str, bool forceAdvanced = false)
	{
		return IsInColors (str, SettingMode.Advanced, forceAdvanced);
	}

	public static bool IsInColors (string str, SettingMode mode, bool forceAdvanced)
	{
		if (Settings.settingMode == mode || forceAdvanced)
			return HighlitherUtil.names.Contains (str.ToLower ());

		return str != "Special Character" && ShadowNames.PascalFields_raw.ContainsKey (str);
	}
}