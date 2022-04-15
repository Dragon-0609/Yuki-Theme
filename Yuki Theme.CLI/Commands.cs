using System.Collections.Generic;
using CommandLine;

namespace Yuki_Theme.CLI
{
	
	[Verb("copy", HelpText = "cli.help.copy")]
	public class CopyCommand
	{
		
		[Value( 0, HelpText = "cli.help.copy.names", Min = 2, Max = 2, MetaName = "Names")]
		public IEnumerable<string> Names {get; set;}
	}

	[Verb("list", HelpText = "cli.help.list")]
	public class ListCommand
	{
	}

	[Verb("clear", HelpText = "cli.help.clear")]
	public class ClearCommand
	{
	}

	[Verb("fields", HelpText = "cli.help.fields")]
	public class FieldsCommand
	{
	}

	[Verb("allfields", HelpText = "cli.help.allfields")]
	public class AllFieldsCommand
	{
	}

	[Verb("export", HelpText = "cli.help.export")]
	public class ExportCommand
	{
		[Value( 0, HelpText = "cli.help.export.name", MetaName = "Theme")]
		public string Name {get; set;}
	}

	[Verb("import", HelpText = "cli.help.import")]
	public class ImportCommand
	{
		[Value( 0, HelpText = "cli.help.import.path", MetaName = "Theme")]
		public string Path {get; set;}
	}

	[Verb("delete", HelpText = "cli.help.delete")]
	public class DeleteCommand
	{
		[Value( 0, HelpText = "cli.help.delete.name", MetaName = "Theme")]
		public string Name {get; set;}
	}
	
	[Verb("rename", HelpText = "cli.help.rename")]
	public class RenameCommand
	{
		
		[Value( 0, HelpText = "cli.help.rename.names", Min = 2, Max = 2, MetaName = "Theme")]
		public IEnumerable<string> Names {get; set;}
	}

	[Verb ("settings", HelpText = "Change Setting")]
	public class SettingsCommand
	{
		[Option ('p', "path", Required = false, HelpText = "cli.help.settings.path")]
		public string Path { get; set; }

		[Option ('q', "quiet", Required = false, HelpText = "cli.help.settings.quiet")]
		public string Quiet { get; set; }

		[Option ('m', "mode", Required = false, HelpText = "cli.help.settings.mode")]
		public string Mode { get; set; }

		[Option ('a', "action", Required = false, HelpText = "cli.help.settings.action")]
		public string Action { get; set; }
	}

	[Verb ("edit", HelpText = "cli.help.edit")]
	public class EditCommand
	{
		[Value( 0, HelpText = "cli.help.edit.name", MetaName = "Theme")]
		public string Name {get; set;}

		[Option ('d', "definition", HelpText = "cli.help.edit.definition", Required = true)]
		public string Definition { get; set; }

		[Option ('b', "bg", Required = false, HelpText = "cli.help.edit.background")]
		public string Background { get; set; }

		[Option ('t', "opacity", Required = false, HelpText = "cli.help.edit.text")]
		public string Text { get; set; }

		[Option ('o', "opacity", Required = false, HelpText = "cli.help.edit.opacity")]
		public string Opacity { get; set; }

		[Option ('a', "align", Required = false, HelpText = "cli.help.edit.align")]
		public string Align { get; set; }

		[Option ('p', "path", Required = false, HelpText = "cli.help.edit.path")]
		public string Path { get; set; }

	}
}