using System;

namespace Yuki_Theme.Core.Communication
{
	[Serializable]
	public class Message
	{
		public const string PATH = "YukiTheme";
		
		public int    Id;
		public string Content;
		public object OtherContent;

		public Message (int id)
		{
			Id = id;
			Content = "";
			OtherContent = null;
		}

		public Message (int id, string content)
		{
			Id = id;
			Content = content;
			OtherContent = null;
		}

		public Message (int id, object content)
		{
			Id = id;
			Content = "";
			OtherContent = content;
		}

		public Message (int id, string content, object otherContent)
		{
			Id = id;
			Content = content;
			OtherContent = otherContent;
		}

		public override string ToString ()
		{
			return Id + Content.Length > 0 ? $" - {Content}" : "";
		}
	}

	public class MessageTypes
	{
		public const int TEST_CONNECTION    = 1;
		public const int TEST_CONNECTION_OK = 2;
		public const int EXPORT_THEME       = 3;
		public const int RELEASE_RESOURCES  = 4;
		public const int APPLY_THEME        = 5;
		public const int APPLY_THEME_LIGHT  = 6;
		public const int SET_PASCAL_PATH    = 7;
		public const int SELECT_THEME       = 8;
		public const int OPEN_MAIN_WINDOW   = 9;
		public const int RELOAD_THEME   = 10;


		public const int CLOSE_SERVER = 66;
	}
	
	
}