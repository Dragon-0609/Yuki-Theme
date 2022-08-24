using System;
using System.Collections.Generic;
using Timer = System.Timers.Timer;

namespace Yuki_Theme.Core.Communication
{
	public abstract class CommunicatorBase
	{
		protected System.Timers.Timer _timer;


		protected Queue<Message> _messages;
		

		protected void InitTimer ()
		{
			_timer = new Timer ();
			_timer.Interval = 100;
			_timer.Elapsed += TimerOnTick;
			// _timer.Start ();
		}

		private void TimerOnTick (object sender, EventArgs e)
		{
			lock (_messages)
			{
				if (_messages.Count > 0)
				{
					SendMessage (_messages.Dequeue ());
				}
			}
		}


		public abstract void SendMessage (Message message);
	}
}