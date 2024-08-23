using System;
using YukiTheme.Engine;

namespace YukiTheme.Export;

public class ExternalExporter : Exporter
{
	protected override Exporter Next => null;

	protected override bool HasTheme(string name)
	{
		return false;
	}

	protected override void ExportTheme(string name)
	{
		throw new NotImplementedException("Not implemented yet");
	}
}