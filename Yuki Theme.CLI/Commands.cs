using System.Collections.Generic;
using CommandLine;

namespace Yuki_Theme.CLI
{
	
	[Verb("copy", HelpText = "Copy the specified themes\ncopy <from> <to>")]
	public class CopyCommand
	{
		
		[Value( 0, HelpText = "Copy <from> <to>", Min = 2, Max = 2, MetaName = "Names")]
		public IEnumerable<string> Names {get; set;}
	}

	[Verb("list", HelpText = "Show list of themes")]
	public class ListCommand
	{
	}

	[Verb("clear", HelpText = "Clear the CLI")]
	public class ClearCommand
	{
	}

	[Verb("fields", HelpText = "Show main editable fields")]
	public class FieldsCommand
	{
	}

	[Verb("allfields", HelpText = "Show all editable fields")]
	public class AllFieldsCommand
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

	[Verb ("settings", HelpText = "Change Setting")]
	public class SettingsCommand
	{
		[Option ('p', "path", Required = false, HelpText = "Set path to pascal.")]
		public string Path { get; set; }

		[Option ('q', "quiet", Required = false, HelpText = "Ask if there're other themes.")]
		public string Quiet { get; set; }

		[Option ('m', "mode", Required = false, HelpText = "Setting mode. There're two options: Light and Advanced")]
		public string Mode { get; set; }

		[Option ('a', "action", Required = false,
		         HelpText = "Setting mode. There're three options: Delete, Import, Ignore")]
		public string Action { get; set; }
	}

	[Verb ("edit", HelpText =
		       "Edit the theme\nedit <THEME NAME>  [ -d | --definition { Default | LineNumber | FoldLine | FoldMarker | SelectedFoldLine | Digit | Comment | String | KeyWord | BeginEnd | Punctuation | Operator | Constant | Image | Sticker } ] <VALUES>\n" +
		       "Type \"yuki help edit\" if you need more explanation, how to use the command")]
	public class EditCommand
	{
		[Value( 0, HelpText = "Theme name", MetaName = "Theme")]
		public string Name {get; set;}

		[Option ('d', "definition", HelpText =
			         "[ -d | --definition { Default | LineNumber | FoldLine | FoldMarker | SelectedFoldLine | Digit | Comment | String | KeyWord | BeginEnd | Punctuation | Operator | Constant | Image | Sticker } ] <VALUES>:\n\n" +
			         "For colors:\nBackground:	{ -b | --bg }\nText: 	{ -t | --text }\n\n" +
			         "If you want to change just text color:\n\n" +

			         "yuki edit Test -d Default -t #DDDDDD\n\n" +

			         "If you want to change just background color:\n\n" +

			         "yuki edit Test -d Default -b #323232\n\n\n" +
			         "For images:\n\n{ -o | --opacity } [ -a | --align { left | center | right } ] { -p | --path }\nFor example:\n\nyuki edit Test -d Image -p \"C:\\Test\\wallpaper.png\" -a center -o 15",
		         Required = true)]
		public string Definition { get; set; }

		[Option ('b', "bg", Required = false,
		         HelpText = "Opacity of the image.\n\n For example:\n\n yuki edit Test -d Image -o 15")]
		public string Background { get; set; }

		[Option ('t', "opacity", Required = false,
		         HelpText = "Opacity of the image.\n\n For example:\n\n yuki edit Test -d Image -o 15")]
		public string Text { get; set; }

		[Option ('o', "opacity", Required = false,
		         HelpText = "Opacity of the image.\n\n For example:\n\n yuki edit Test -d Image -o 15")]
		public string Opacity { get; set; }

		[Option ('a', "align", Required = false,
		         HelpText = "Alignment of the image.\n\n For example:\n\n yuki edit Test -d Image -a left")]
		public string Align { get; set; }

		[Option ('p', "path", Required = false,
		         HelpText =
			         "Path to the image.\n\n For example:\n\n yuki edit Test -d Image -p \"C:\\Test\\wallpaper.png\"")]
		public string Path { get; set; }

	}
}