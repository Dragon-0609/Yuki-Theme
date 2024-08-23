using System;

namespace YukiTheme.Tools;

public class ThemeLoadInfo
{
	public int CutNumber;
	public string Key;
	public Action<string> Value;

	public ThemeLoadInfo(string key, int cutNumber, Action<string> value)
	{
		Key = key;
		CutNumber = cutNumber;
		Value = value;
	}
}