using YukiTheme.Engine;

namespace YukiTheme.Export;

public class ExportListener
{
    internal static ExportListener Instance;

    private Exporter _exporter;

    public ExportListener()
    {
        Instance = this;
        PluginEvents.Instance.ThemeChanged += ThemeChanged;
    }

    internal void SetExporter(Exporter exporter)
    {
        _exporter = exporter;
    }

    private void ThemeChanged(string name)
    {
        _exporter.Export(name);
    }
}