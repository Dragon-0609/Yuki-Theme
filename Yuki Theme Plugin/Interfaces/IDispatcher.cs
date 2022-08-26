using System;

namespace Yuki_Theme_Plugin.Interfaces
{
	public interface IDispatcher
	{
		void InvokeUI (Delegate method);
	}
}