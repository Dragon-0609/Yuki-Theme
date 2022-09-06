using System;
using System.Drawing;
using Yuki_Theme.Core.Utils;

namespace Yuki_Theme.Core.API
{
	public static class API_Events
	{
		public static Func <string, string, bool> SaveInExport;
		public static Action <string, string>     setPath;
		public static Action <string, string>     showSuccess;
		public static Action <string, string>     showError;
		public static Func <int>                  AskChoice;
		public static Action <string>             hasProblem           = null;
		public static Action                      onBGIMAGEChange      = null;
		public static Action                      onSTICKERChange      = null;
		public static Action                      onSTATUSChange       = null;
		public static Action <Image>              ifHasImage           = null;
		public static Action                      ifDoesntHave         = null;
		public static Action <Image>              ifHasSticker         = null;
		public static Action                      ifDoesntHaveSticker  = null;
		public static Action <string, string>     onRename;
	}

	[Serializable]
	public class PreviewOptions
	{
		public SyntaxType Syntax;
		public bool       NeedToDelete;
		public bool       HasAction;

		public PreviewOptions (SyntaxType type, bool delete)
		{
			Syntax = type;
			NeedToDelete = delete;
			HasAction = false;
		}

		public PreviewOptions (SyntaxType type, bool delete, bool action)
		{
			Syntax = type;
			NeedToDelete = delete;
			HasAction = action;
		}

	}
	
}