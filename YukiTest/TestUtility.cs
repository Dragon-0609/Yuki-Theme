using System;
using Newtonsoft.Json.Linq;

namespace YukiTest
{
	public static class TestUtility
	{
		public static void Validate(this JToken token, string message)
		{
			if (token == null) throw new NullReferenceException(message);
		}
	}
}