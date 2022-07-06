using System.Windows;
using Yuki_Theme.Core.WPF.Interfaces;
using Drawing = System.Drawing;

namespace Yuki_Theme.Core.WPF.Windows
{
	public class MainPresenter : Main.IPresenter
	{
		private Main.Model _model;
		private Main.IView _view;
		private Window     parent;

		public MainPresenter (Main.Model model, Main.IView view, Window parent)
		{
			_model = model;
			_view = view;
			this.parent = parent;
		}

		public void SetAPIActions ()
		{
			API_Events.ifHasImage = ifHasImage;
			API_Events.ifDoesntHave = ifDoesntHave;
			API_Events.ifHasSticker = ifHasSticker;
			API_Events.ifDoesntHaveSticker = ifDoesntHaveSticker;
			API_Events.AskChoice = AskActionChoice;
			API_Events.SaveInExport = SaveInExport;
			API_Events.showSuccess = FinishExport;
			API_Events.showError = ErrorExport;
			API_Events.hasProblem = hasProblem;
			API_Events.setPath = SetPath;
		}

		
		
		#region ApiEvents

		public void ifHasImage (Drawing.Image imgc)
		{
			_model.wallpaperOriginal = imgc;
			_model.calculatedWallpaperSize = Drawing.Rectangle.Empty;
		}

		public void ifDoesntHave ()
		{
			_model.wallpaperRender = null;
			_model.wallpaperOriginal = null;
		}

		public void ifHasSticker (Drawing.Image imgc)
		{
			_model.sticker = imgc;
		}

		public void ifDoesntHaveSticker ()
		{
			_model.sticker = null;
		}

		public void hasProblem (string content)
		{
			MessageBox.Show (
				content, API.Translate ("messages.theme.invalid.short"), MessageBoxButton.OK, MessageBoxImage.Error);
			_view.SelectDefaultTheme ();
		}

		public bool SaveInExport (string content, string title)
		{
			return MessageBox.Show (content, title,
			                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
		}

		public void FinishExport (string content, string title)
		{
			MessageBox.Show (content, title);
		}

		public void ErrorExport (string content, string title)
		{
			MessageBox.Show (content, title, MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private bool AskChoiceParser (string content, string title)
		{
			return MessageBox.Show (content,
			                        title,
			                        MessageBoxButton.YesNo) == MessageBoxResult.Yes;
		}
		
		private int AskActionChoice ()
		{
			return QuestionWindow.AskActionChoice (parent);
		}
		
		private void SetPath (string content, string title)
		{
			MessageBox.Show (content, title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			_view.OpenSettings ();
		}
		
		#endregion
		
	}
	
}