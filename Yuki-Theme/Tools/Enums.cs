namespace YukiTheme.Tools
{
	public enum SettingMode : int
	{
		Light    = 0,
		Advanced = 1
	}

	public enum RelativeUnit : int
	{
		Pixel   = 0,
		Percent = 1
	}

	public enum ProductMode : int
	{
		Program = 0,
		Plugin  = 1,
		CLI     = 2
	}

	public enum Alignment : int
	{
		Left   = 1000,
		Center = 2,
		Right  = 1
	}

	public enum ImageType : int
	{
		None = 0,
		Wallpaper = 1,
		Sticker = 2
	}

	public enum ThemeFormat
	{
		Old,
		New,
		Null
	}

	public enum ThemeLocation
	{
		File,
		Memory
	}

	public enum DimensionCapUnit : int
	{
		Height = 0,
		Width = 1
	}
	
}