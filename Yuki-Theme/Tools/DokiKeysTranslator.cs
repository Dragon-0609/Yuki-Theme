namespace YukiTheme.Tools;

public static class DokiKeysTranslator
{
	public static string GetTranslation(string key)
	{
		return key switch
		{
			"foregroundColor" => "FOREGROUND",
			"textEditorBackground" => "BACKGROUND",
			"selectionBackground" => "SELECTION",
			"accentColor" => "ACCENT",
			"lineNumberColor" => "LINE_COLOR",
			"identifierHighlight" => "MARKER",
			"constantColor" => "DIGITS",
			"comments" => "COMMENT",
			"stringColor" => "STRING",
			"classNameColor" => "METHOD",
			"keywordColor" => "KEYWORD",
			"keyColor" => "KEYS",
			_ => key
		};
	}
}