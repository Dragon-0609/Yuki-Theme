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
	
	[Serializable]
	public class MessageContainer
	{
		public object [] Objects;
	}

	public class MessageTypes
	{
		public const int TEST_CONNECTION      = 1;
		public const int TEST_CONNECTION_OK   = 2;
		public const int EXPORT_THEME         = 3;
		public const int RELEASE_RESOURCES    = 4;
		public const int RELEASE_RESOURCES_OK = 5;
		public const int APPLY_THEME          = 6;
		public const int APPLY_THEME_LIGHT    = 7;
		public const int SELECT_THEME         = 8;
		public const int OPEN_MAIN_WINDOW     = 9;
		public const int RELOAD_THEME         = 10;
		public const int PREVIEW_THEME        = 11;
		public const int RELOAD_SETTINGS      = 12;
		public const int ADD_THEME            = 13;
		public const int THEME_ADDED          = 14;
		public const int GET_TOOL_BAR_ITEMS = 15;
		public const int RESET_TOOL_BAR = 16;
		public const int RELOAD_TOOL_BAR = 17;
		public const int SAVE_TOOL_BAR_DATA = 18;
		public const int GET_ASSEMBLY_NAME = 19;
		
		public const int SET_PASCAL_PATH  = 30;	
		public const int SET_CURRENT_NAME = 31;
		public const int SET_TOOLBAR_ITEMS = 32;
		public const int SET_ASSEMBLY_NAME = 33;
		public const int SET_TOOL_BAR_VISIBILITY = 34;
		public const int SET_TOOL_BAR_ALIGN = 35;


		public const int CLOSE_SERVER = 66;
	}
	
	
}