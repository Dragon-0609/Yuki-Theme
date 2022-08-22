using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Yuki_Theme.Core.Interfaces;

namespace Yuki_Theme.Core.Communication
{
	public abstract class CommunicatorBase
	{
		protected Timer _timer;


		protected Queue<Message> _messages;
		

		protected void InitTimer ()
		{
			_timer = new Timer ();
			_timer.Interval = 100;
			_timer.Tick += TimerOnTick;
			_timer.Start ();
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