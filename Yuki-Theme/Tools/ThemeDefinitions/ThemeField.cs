using Newtonsoft.Json;

namespace YukiTheme.Tools.ThemeDefinitions;

public class ThemeField
{
	[JsonProperty("background", Required = Required.AllowNull)]
	public string Background { get; set; }

	[JsonProperty("foreground", Required = Required.AllowNull)]
	public string Foreground { get; set; }

	[JsonProperty("bold", Required = Required.AllowNull)]
	public bool? Bold { get; set; }

	[JsonProperty("italic", Required = Required.AllowNull)]
	public bool? Italic { get; set; }
}