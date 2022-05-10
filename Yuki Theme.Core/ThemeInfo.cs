namespace Yuki_Theme.Core;

public class ThemeInfo
{
	public bool          isDefault;
	public bool          isOld;
	public ThemeLocation location;

	public ThemeInfo (bool isDefault, bool isOld, ThemeLocation location)
	{
		this.isDefault = isDefault;
		this.isOld = isOld;
		this.location = location;
	}
}