using System;
using System.Drawing;
using System.Reflection;
using VisualPascalABC.OptionsContent;

namespace YukiTheme.Tools;

public static class MiscHelper
{
	public static T GetByReflection<T>(this object instance, string key)
	{
		FieldInfo getopt = instance.GetType().GetField(key, BindingFlags.NonPublic | BindingFlags.Instance);
		return (T)getopt?.GetValue(instance);
	}

	public static void SetByReflection<T>(this T instance, string key, object value)
	{
		FieldInfo getopt = instance.GetType().GetField(key, BindingFlags.NonPublic | BindingFlags.Instance);
		getopt?.SetValue(instance, value);
	}

	public static void CallByReflection<T>(this T instance, string key, object value)
	{
		MethodInfo methodInfo = instance.GetType().GetMethod(key, BindingFlags.NonPublic | BindingFlags.Instance);
		methodInfo.Invoke(instance, new[] { value });
	}

	public static Rectangle GetSizes(Size ima, int mWidth, int mHeight, Alignment align)
	{
		Rectangle res = new Rectangle();
		double rY = (double)mHeight / ima.Height;
		res.Width = (int)(ima.Width * rY);
		res.Height = (int)(ima.Height * rY);
		res.X = (mWidth - res.Width) / (int)align;

		// If image's drawing rectangle's width is smaller than mWidth.

		if (res.Width < mWidth)
			res = GetSizesHorizontal(ima, mWidth, mHeight);

		return res;
	}

	private static Rectangle GetSizesHorizontal(Size ima, int mWidth, int mHeight)
	{
		Rectangle res = new Rectangle();
		double rY = (double)mWidth / ima.Width;
		res.Width = (int)(ima.Width * rY);
		res.Height = (int)(ima.Height * rY);
		res.Y = (mHeight - res.Height) / 2;

		return res;
	}

	public static int ToInt(this bool value) => value ? 1 : 0;
	public static bool ToBool(this int value) => value == 1;
}