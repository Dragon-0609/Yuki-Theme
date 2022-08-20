using System;
using System.Collections.Generic;
using Yuki_Theme.Core.Communication;

namespace Yuki_Theme.Core.API;

public abstract class NetworkAPI : API_Base
{
	protected Dictionary<int, Action<Message>> ActionsDictionary = new Dictionary<int, Action<Message>> ();

	public abstract void AddEvents ();

	

	protected void ParseMessage (Message message)
	{
		if (ActionsDictionary.ContainsKey (message.Id))
			ActionsDictionary[message.Id] (message);
	}

}