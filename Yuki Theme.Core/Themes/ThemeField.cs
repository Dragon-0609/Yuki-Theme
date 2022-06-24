using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yuki_Theme.Core.Themes;

public class ThemeField
{
	private const string FOREGROUND_ATTRIBUTE = "color";
	private const string BACKGROUND_ATTRIBUTE = "bgcolor";
	private const string BOLD_ATTRIBUTE       = "bold";
	private const string ITALIC_ATTRIBUTE     = "italic";

	[JsonProperty ("background", Required = Required.AllowNull)]
	public string Background { get; set; }

	[JsonProperty ("foreground", Required = Required.AllowNull)]
	public string Foreground { get; set; }

	[JsonProperty (BOLD_ATTRIBUTE, Required = Required.AllowNull)]
	public bool? Bold { get; set; }

	[JsonProperty (ITALIC_ATTRIBUTE, Required = Required.AllowNull)]
	public bool? Italic { get; set; }

	public static ThemeField GetFieldFromDictionary (Dictionary <string, string> field)
	{
		ThemeField themeField = new ThemeField { Bold = null, Italic = null };
		foreach (KeyValuePair <string, string> pair in field)
		{
			if (pair.Key == FOREGROUND_ATTRIBUTE)
				themeField.Foreground = pair.Value;
			else if (pair.Key == BACKGROUND_ATTRIBUTE)
				themeField.Background = pair.Value;
			else if (pair.Key == BOLD_ATTRIBUTE)
				themeField.Bold = bool.Parse (pair.Value);
			else if (pair.Key == ITALIC_ATTRIBUTE)
				themeField.Italic = bool.Parse (pair.Value);
		}

		return themeField;
	}

	public Dictionary <string, string> ConvertToDictionary ()
	{
		Dictionary <string, string> dictionary = new Dictionary <string, string> ();

		if (Background != null)
			dictionary.Add (BACKGROUND_ATTRIBUTE, Background);

		if (Foreground != null)
			dictionary.Add (FOREGROUND_ATTRIBUTE, Foreground);

		if (Bold != null)
			dictionary.Add (BOLD_ATTRIBUTE, Bold.ToString ().ToLower ());

		if (Italic != null)
			dictionary.Add (ITALIC_ATTRIBUTE, Italic.ToString ().ToLower ());

		return dictionary;
	}

	public void SetAttributeByName (string name, string value)
	{
		if (name == FOREGROUND_ATTRIBUTE)
			Foreground = value;
		else if (name == BACKGROUND_ATTRIBUTE)
			Background = value;
		else if (name is BOLD_ATTRIBUTE or ITALIC_ATTRIBUTE)
		{
			Bold = bool.Parse (value);
			Italic = bool.Parse (value);
		}
	}

	public Dictionary <string, string> GetAttributes ()
	{
		Dictionary <string, string> dictionary = new Dictionary <string, string> ();

		if (Background != null) dictionary.Add (BACKGROUND_ATTRIBUTE, Background);
		if (Foreground != null) dictionary.Add (FOREGROUND_ATTRIBUTE, Foreground);
		if (Bold != null) dictionary.Add (BOLD_ATTRIBUTE, Bold.ToString ().ToLower ());
		if (Italic != null) dictionary.Add (ITALIC_ATTRIBUTE, Italic.ToString ().ToLower ());
		return dictionary;
	}

	public static Dictionary <string, ThemeField> GetThemeFieldsWithRealNames (SyntaxType syntax, Theme theme)
	{
		return GetThemeFieldsWithRealNames (syntax, theme.Fields);
	}

	public static Dictionary <string, ThemeField> GetThemeFieldsWithRealNames (SyntaxType                      syntax,
	                                                                           Dictionary <string, ThemeField> themeFields)
	{
		Dictionary <string, ThemeField> localDic = new Dictionary <string, ThemeField> ();
		List <string> shadowNames = new List <string> (); // This is necessary not to repeat fields
		foreach (KeyValuePair <string, ThemeField> pair in themeFields)
		{
			string shadowName = ShadowNames.GetShadowName (pair.Key, syntax, true);
			// Console.WriteLine (shadowName);
			if (shadowName != null && !shadowNames.Contains (shadowName) && ShadowNames.HasRealName (shadowName, syntax))
			{
				string [] realName = ShadowNames.GetRealName (shadowName, syntax);
				if (realName != null)
					foreach (string st in realName)
						localDic.Add (st, pair.Value);

				shadowNames.Add (shadowName);
			}
		}

		return localDic;
	}

	public bool IsAttributeNull (string name)
	{
		bool res = false;
		if (name == BACKGROUND_ATTRIBUTE)
			res = Background == null;
		else if (name == FOREGROUND_ATTRIBUTE)
			res = Foreground == null;
		else if (name == BOLD_ATTRIBUTE)
			res = Bold == null;

		return res;
	}

	public bool IsNull ()
	{
		return Background == null && Foreground == null;
	}

	public override string ToString ()
	{
		string bg = Background ?? "null";
		string fg = Foreground ?? "null";
		string bd = Bold == null ? "null" : Bold.ToString ();
		string it = Italic == null ? "null" : Italic.ToString ();
		return $"Background: {bg}, Foreground: {fg}, Bold: {bd}, Italic: {it}";
	}

	public ThemeField CopyField ()
	{
		ThemeField field = new ThemeField
		{
			Background = Background,
			Foreground = Foreground,
			Bold = Bold,
			Italic = Italic
		};
		return field;
	}

	public void SetValues (ThemeField fiel)
	{
		if (fiel.Background != null)
			Background = fiel.Background;
		if (fiel.Foreground != null)
			Foreground = fiel.Foreground;
		if (fiel.Bold != null)
			Bold = fiel.Bold;
		if (fiel.Italic != null)
			Italic = fiel.Italic;
	}

	public ThemeField MergeWithAnother (ThemeField target)
	{
		if (this.Background == null && target.Background != null)
		{
			this.Background = target.Background;
		}

		if (this.Foreground == null && target.Foreground != null)
		{
			this.Foreground = target.Foreground;
		}

		if (this.Bold == null && target.Bold != null)
		{
			this.Bold = target.Bold;
		}

		if (this.Italic == null && target.Italic != null)
		{
			this.Italic = target.Italic;
		}

		return this;
	}

	public bool IsEqual (ThemeField field)
	{
		bool equal = true;
		if (Background != null)
		{
			equal = field.Background != null && Background == field.Background;
		} else if (field.Background != null)
		{
			equal = false;
		}

		if (equal)
		{
			if (Foreground != null)
			{
				equal = field.Foreground != null && Foreground == field.Foreground;
			} else if (field.Foreground != null)
			{
				equal = false;
			}
		}

		if (equal)
		{
			if (Bold != null)
			{
				equal = field.Bold != null && Bold == field.Bold;
			} else if (field.Bold != null)
			{
				equal = false;
			}
		}

		if (equal)
		{
			if (Italic != null)
			{
				equal = field.Italic != null && Italic == field.Italic;
			} else if (field.Italic != null)
			{
				equal = false;
			}
		}

		return equal;
	}
}