using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

namespace YukiTheme.Tools;

public static class MiscHelper
{
	public static T GetByReflection<T>(this object instance, string key, bool isPublic = false)
	{
		return GetByReflection<T>(instance.GetType(), instance, key, isPublic);
	}

	private static T GetByReflection<T>(Type type, object instance, string key, bool isPublic)
	{
		var getopt = type.GetField(key, (isPublic ? BindingFlags.Public : BindingFlags.NonPublic) | BindingFlags.Instance);
		if (getopt == null)
		{
			if (type.BaseType != null)
				return GetByReflection<T>(type.BaseType, instance, key, isPublic);
			throw new NullReferenceException("Field not found");
		}

		return (T)getopt?.GetValue(instance);
	}

	public static T GetPropertyByReflection<T>(this object instance, string key)
	{
		return GetPropertyByReflection<T>(instance.GetType(), instance, key);
	}

	private static T GetPropertyByReflection<T>(Type type, object instance, string key)
	{
		var getopt = type.GetProperty(key, BindingFlags.NonPublic | BindingFlags.Instance);
		if (getopt == null)
		{
			if (type.BaseType != null)
				return GetPropertyByReflection<T>(type.BaseType, instance, key);
			throw new NullReferenceException("Property not found");
		}

		return (T)getopt?.GetValue(instance);
	}

	public static void SetByReflection<T>(this T instance, string key, object value)
	{
		var getopt = instance.GetType().GetField(key, BindingFlags.NonPublic | BindingFlags.Instance);
		getopt?.SetValue(instance, value);
	}

	public static void CallByReflection<T>(this T instance, string key, object value)
	{
		var methodInfo = instance.GetType().GetMethod(key, BindingFlags.NonPublic | BindingFlags.Instance);
		methodInfo.Invoke(instance, new[] { value });
	}

	public static T CallFunctionByReflection<T>(this object instance, string key)
	{
		var methodInfo = instance.GetType().GetMethod(key, BindingFlags.NonPublic | BindingFlags.Instance);
		return (T)methodInfo.Invoke(instance, null);
	}

	public static Rectangle GetSizes(Size ima, int mWidth, int mHeight, Alignment align)
	{
		var res = new Rectangle();
		var rY = (double)mHeight / ima.Height;
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
		var res = new Rectangle();
		var rY = (double)mWidth / ima.Width;
		res.Width = (int)(ima.Width * rY);
		res.Height = (int)(ima.Height * rY);
		res.Y = (mHeight - res.Height) / 2;

		return res;
	}

	public static int ToInt(this bool value)
	{
		return value ? 1 : 0;
	}

	public static bool ToBool(this int value)
	{
		return value == 1;
	}


	public static string Replace(this string origin, Dictionary<string, string> toReplace)
	{
		foreach (var pair in toReplace) origin = origin.Replace(pair.Key, pair.Value);

		return origin;
	}
}