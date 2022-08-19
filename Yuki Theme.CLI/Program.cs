using System;
using CLITools;
using CommandLine;
using Yuki_Theme.Core;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Utils;

namespace Yuki_Theme.CLI
{
	public class Program
	{
		internal static MainClass program = new MainClass ();
		
		public static void Main (string [] args)
		{
			Settings.translation.LoadLocalization ();
			
			AdminTools adminTools = new AdminTools();
			if (!adminTools.CurrentUserIsAdmin ())
				program.ShowError(API.Current.Translate ("messages.warnings.adminprivileges.cli.content"));
			
			var parser = new Parser (parserSettings =>
			{
				parserSettings.AutoHelp = true;
				parserSettings.AutoVersion = true;
				parserSettings.CaseInsensitiveEnumValues = false;
				parserSettings.CaseSensitive = true;
				parserSettings.EnableDashDash = true;
				parserSettings.IgnoreUnknownArguments = false;
			});
			
			if (args != null && args.Length > 0)
			{
				program.quit = true;
				program.Parse (parser, args);
			} else
			{
				program.CheckUpdateInslattaion ();
				program.loop = true;
				program.ShowLoopMessage ();
				program.completion = new Completion ();
				ReadLine.AutoCompletionHandler = program.completion;
				ReadLine.HistoryEnabled = true;

				program.LoadCLI (true);
				while (!program.quit)
				{
					string command = ReadLine.Read ("yuki> ");
					if (command.ToLower ().Contains ("quit")) break;
					if (command.ToLower ().StartsWith ("yuki "))
					{
						command = command.Substring (5);
					}
					if (command.StartsWith (" ")) command = command.TrimStart ();
					if (command.ToLower().StartsWith ("exit")) break;
					program.Parse (parser, program.ParseArguments (command));
				}
			}
		}

	}
}