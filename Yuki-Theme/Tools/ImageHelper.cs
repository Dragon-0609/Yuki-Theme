using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace YukiTheme.Tools
{
	public static class ImageHelper
	{
		public static Image Load(string icon, string nameSpace = "Icons.")
		{
			Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"YukiTheme.Resources.{nameSpace}{icon}");
			return Image.FromStream(stream);
		}

		public static void Save(string path, string name, string nameSpace)
		{
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"YukiTheme.Resources.{nameSpace}{name}"))
			{
				if (stream == null) throw new InvalidOperationException("File not found");
				if (File.Exists(path))
				{
					File.Delete(path);
				}

				using (FileStream file = File.Create(path))
				{
					file.Position = 0;
					stream.CopyTo(file);
				}
			}
		}

		public static Size CalculateDimension(Size value)
		{
			Size size = value;
			int capMax = 300;
			double aspect = 0;
			if (false)
			{
				if (true)
				{
					if (size.Height > capMax)
					{
						aspect = (size.Width + 0.0) / size.Height;
						size.Height = capMax;
						size.Width = Convert.ToInt32(Math.Round(aspect * capMax));
					}
				}
				else
				{
					if (size.Width > capMax)
					{
						aspect = (size.Height + 0.0) / size.Width;
						size.Width = capMax;
						size.Height = Convert.ToInt32(Math.Round(aspect * capMax));
					}
				}
			}

			// MessageBox.Show ( $"Dimension: {Settings.dimensionCapUnit}, max: {capMax}, aspect: {aspect}, Size: {size}, Original: {value}");
			return size;
		}
	}
}