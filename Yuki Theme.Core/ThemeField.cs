using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yuki_Theme.Core
{
	public class ThemeField
	{
		[JsonProperty ("background", Required = Required.AllowNull)]
		public string Background { get; set; }

		[JsonProperty ("foreground", Required = Required.AllowNull)]
		public string Foreground { get; set; }

		[JsonProperty ("bold", Required = Required.AllowNull)]
		public bool? Bold { get; set; }

		[JsonProperty ("italic", Required = Required.AllowNull)]
		public bool? Italic { get; set; }
		
		public static ThemeField GetFieldFromDictionary (Dictionary <string, string> field)
		{
			ThemeField themeField = new ThemeField {Bold = null, Italic = null};
			foreach (KeyValuePair <string, string> pair in field)
			{
				switch (pair.Key)
				{
					case "color" :
					{
						themeField.Foreground = pair.Value;
					}
						break;
					case "bgcolor" :
					{
						themeField.Background = pair.Value;
					}
						break;
					case "bold" :
					{
						themeField.Bold = bool.Parse (pair.Value);
					}
						break;
					case "italic" :
					{
						themeField.Italic = bool.Parse (pair.Value);
					}
						break;
				}
			}

			return themeField;
		}

		public Dictionary <string, string> ConvertToDictionary ()
		{
			Dictionary <string, string> dictionary = new Dictionary <string, string> ();

			if (Background != null)
				dictionary.Add ("bgcolor", Background);
			
			if (Foreground != null)
				dictionary.Add ("color", Foreground);
			
			if (Bold != null)
				dictionary.Add ("bold", Bold.ToString().ToLower());
			
			if (Italic != null)
				dictionary.Add ("italic", Italic.ToString().ToLower());
			
			return dictionary;
		}

		public void SetAttributeByName (string name, string value)
		{
			if (name == "color")
				Foreground = value;
			else
				Background = value;
		}

		public Dictionary <string, string> GetAttributes ()
		{
			Dictionary <string, string> dictionay = new Dictionary <string, string> ();

			if(Background!= null) dictionay.Add ("bgcolor", Background);
			if(Foreground!= null) dictionay.Add ("color", Foreground);
			if(Bold!= null) dictionay.Add ("bold", Bold.ToString().ToLower());
			if(Italic!= null) dictionay.Add ("italic", Italic.ToString().ToLower());
			
			return dictionay;
		}
		
	}
}