namespace YukiTheme.Tools;

internal interface IDatabase
{
	void SetValue(string name, string value);
	void SetValueWithSave(string name, string value);
	string GetValue(string name);
	string GetValue(string name, string defaultValue);
	void DeleteValue(string name);
	void Wipe();
	void SaveAll();
}