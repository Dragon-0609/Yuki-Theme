using System.Windows;
using Microsoft.Win32;

namespace YukiTheme.Components;

public partial class CustomStickerWindow : Window
{
	public CustomStickerWindow()
	{
		InitializeComponent();
	}

	private void SelectImageClicked(object sender, RoutedEventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "PNG (*.png)|*.png";
		if (openFileDialog.ShowDialog() == true)
			ImagePath.Text = openFileDialog.FileName;
	}

	private void ClickedSave(object sender, RoutedEventArgs e)
	{
		DialogResult = true;
	}

	private void ClickedCancel(object sender, RoutedEventArgs e)
	{
		DialogResult = false;
	}
}