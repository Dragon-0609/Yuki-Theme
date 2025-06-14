using YukiTheme.Engine;

namespace YukiTheme.Tools
{
	public static class CompletionFontSizeFinder
	{
		public static int GetSize()
		{
			if (DataSaver.Load(SettingsConst.COMPLETION_FONT_AS_EDITOR, false))
			{
				int fontSize = (int)IDEAlterer.Instance.EditorFontSize - 3;
				fontSize = MiscHelper.Clamp(fontSize, 4, 24);
				return fontSize;
			}

			return DataSaver.Load(SettingsConst.COMPLETION_FONT, 8, 8);
		}
	}
}