using Yuki_Theme.Core.WPF.Controls;

namespace Yuki_Theme.Core.WPF.Interfaces
{
	public interface IToolBarController
	{
		void FillList ();
		void UpdateInfo (ToolBarListItem item);
		void SetIconContainer ();

		bool IsVisible (string item);
		bool IsRight (string item);

		void SetVisible (string item, bool val);
		void SetRight (string item, bool val);
		void ResetToolBar ();
		void ReloadToolBar ();
	}
}