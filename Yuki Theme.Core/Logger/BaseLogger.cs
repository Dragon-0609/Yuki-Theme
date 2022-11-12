using System;

namespace Yuki_Theme.Core.Logger
{
	public abstract class BaseLogger
	{
		public abstract void Write(string text);

		public void Write(string text, params object[] parameters)
		{
			string result = text;
			for (int i = 0; i < parameters.Length; i++)
			{
				if (result.Contains($"{{{i}}}"))
				{
					result = result.Replace($"{{{i}}}", parameters[i].ToString());
				}
			}

			Write(result);
		}
	}
}