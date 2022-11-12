using System;

namespace Yuki_Theme.Core.Logger
{
	public class ConsoleLogger : BaseLogger
	{
		public static ConsoleLogger Instance = new ConsoleLogger();
		
		public override void Write(string text)
		{
			Console.WriteLine(text);
		}
	}
}