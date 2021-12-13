using System.Collections.Generic;
using CommandLine;

namespace Yuki_Theme.CLI
{
	
	[Verb("duplicate", HelpText = "Duplicate the specified themes\nduplicate <from> <to>")]
	public class DuplicateCommand
	{
		
		[Value( 0, HelpText = "Duplicate <from> <to>", Min = 2, Max = 2, MetaName = "Names")]
		public IEnumerable<string> Names {get; set;}
	}

	[Verb("list", HelpText = "Show list of themes")]
	public class ListCommand
	{
	}

	[Verb("export", HelpText = "Export selected theme\nexport <name>")]
	public class ExportCommand
	{
		[Value( 0, HelpText = "Export <name>", MetaName = "Theme")]
		public string Name {get; set;}
	}

	[Verb("import", HelpText = "Import theme by the path\nimport <path>")]
	public class ImportCommand
	{
		[Value( 0, HelpText = "Import <path>", MetaName = "Theme")]
		public string Path {get; set;}
	}

	[Verb("delete", HelpText = "Delete selected theme\ndelete <name>")]
	public class DeleteCommand
	{
		[Value( 0, HelpText = "Delete <name>", MetaName = "Theme")]
		public string Name {get; set;}
	}
	
	[Verb("rename", HelpText = "Rename the specified theme\nrename <from> <to>")]
	public class RenameCommand
	{
		
		[Value( 0, HelpText = "Rename <from> <to>", Min = 2, Max = 2, MetaName = "Theme")]
		public IEnumerable<string> Names {get; set;}
	}

	[Verb("settings", HelpText = "Change Setting")]
	public class SettingsCommand
	{
		[Option('p', "path", Required = false, HelpText = "Set path to pascal.")]
		public string Path { get; set; }
	
		[Option('q', "quiet", Required = false, HelpText = "Ask if there're other themes.")]
		public string Quiet { get; set; }
	
		[Option('m', "mode", Required = false, HelpText = "Setting mode. There're two options: Light and Advanced")]
		public string Mode { get; set; }
	}
}