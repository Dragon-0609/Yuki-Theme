using Yuki_Theme.Core.Logger;
using Yuki_Theme_Plugin.Interfaces;

namespace Yuki_Theme_Plugin
{
	public class OutputConsoleLogger:BaseLogger
	{
		public static IConsole Console;

		public static OutputConsoleLogger Instance = new OutputConsoleLogger();
		
		public override void Write(string text)
		{
			Console.WriteToConsole(text);
		}
	}
}