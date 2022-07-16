namespace Yuki_Theme.Core.Interfaces
{
	public interface IDatabase
	{
		void SetValue (string name, string value);

		string GetValue (string name);
		string GetValue (string name, string defaultValue);
		void DeleteValue (string name);
	}
}