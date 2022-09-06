﻿using System.Drawing;
using System.Windows;
using Yuki_Theme.Core.WPF.Controls;
using Size = System.Drawing.Size;

namespace Yuki_Theme.Core.WPF.Interfaces
{
	public class Sticker
	{
		public interface IModel
		{
			void LoadImage (Image image);

			bool IsRenderImageNull ();
			bool IsOriginalImageNull ();
			void ReleaseRenderImage ();
			void PrepareRenderImage ();
			
			void LoadCustomSticker ();

			Image GetRenderImage ();
		}
		
		public interface IView
		{
			void ChangeVisibility (Visibility visibility);

			void ChangeImage (Image image);

			void SetSize (Size size);

			void ResetPosition ();
		}
		
		public interface IPresenter
		{
			
			void LoadSticker ();

			void LoadImage (Image image);
		}
	}
}