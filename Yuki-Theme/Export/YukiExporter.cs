using YukiTheme.Engine;

namespace YukiTheme.Export
{
	public class YukiExporter : Exporter
	{
		protected override Exporter Next { get; } = new PlainExporter();

		public YukiExporter()
		{
			
		}
		
		protected override bool HasTheme(string name)
		{
			
			return false;
		}

		protected override void ExportTheme(string name)
		{
			
		}
	}
}