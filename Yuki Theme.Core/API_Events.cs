using System;
using System.Drawing;

namespace Yuki_Theme.Core;

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
	public static Action <Image>              ifHasImage2          = null;
	public static Action                      ifDoesntHave2        = null;
	public static Action <Image>              ifHasSticker2        = null;
	public static Action                      ifDoesntHaveSticker2 = null;
	public static Action <string, string>     onRename;
}