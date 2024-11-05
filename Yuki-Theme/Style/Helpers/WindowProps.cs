namespace YukiTheme.Style.Helpers;

public struct WindowProps
{
	public int Left;
	public int Top;
	public int Width;
	public int Height;
	public bool Maximized;

	public static WindowProps Parse(string target)
	{
		var spp = target.Split(':');

		if (spp.Length < 2) return new WindowProps { Left = 0, Top = 0 };

		var props = new WindowProps
		{
			Left = int.Parse(spp[0]),
			Top = int.Parse(spp[1])
		};
		try
		{
			if (spp.Length > 2)
			{
				props.Width = int.Parse(spp[2]);
				if (spp.Length > 3)
				{
					props.Height = int.Parse(spp[3]);
					if (spp.Length == 5)
						props.Maximized = bool.Parse(spp[4]);
				}
			}
		}
		catch
		{
			// ignored
		}

		return props;
	}

	public override string ToString()
	{
		return $"{Left}:{Top}:{Width}:{Height}:{Maximized}";
	}
}