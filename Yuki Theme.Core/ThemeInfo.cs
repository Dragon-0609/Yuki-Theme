namespace Yuki_Theme.Core;

public class ThemeInfo
{
	public bool          isDefault;
	public bool          isOld;
	public ThemeLocation location;
	public bool          isTokenValid;
	public string        group;

	public ThemeInfo (bool isDefault, bool isOld, ThemeLocation location, string Group)
	{
		this.isDefault = isDefault;
		this.isOld = isOld;
		this.location = location;
		isTokenValid = false;
		group = Group;
	}

	public ThemeInfo (bool isDefault, bool isOld, ThemeLocation location, string Group, bool valid)
	{
		this.isDefault = isDefault;
		this.isOld = isOld;
		this.location = location;
		isTokenValid = valid;
		group = Group;
	}

	public override string ToString ()
	{
		return $"Default: {isDefault}, Old: {isOld}, Location: {location}";
	}
}