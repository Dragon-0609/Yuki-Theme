﻿using System;
using System.Collections.Generic;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Yuki_Theme.Core.Communication
{
	public abstract class CommunicatorBase
	{
		protected System.Timers.Timer _timer;

		protected int Interval = 100;

		private int tryCount;

		protected Queue<Message> _messages;

		protected bool stopTimer = false;

		protected bool ConsoleLogs = false;
		

		protected void InitTimer ()
		{
			_timer = new Timer ();
			_timer.Interval = Interval;
			_timer.Elapsed += SendMessages;
			_timer.Start ();
		}

		private void SendMessages (object sender, EventArgs e)
		{
			lock (_messages)
			{
				if (_messages.Count > 0)
				{
					SendMessage (_messages.Dequeue ());
				}
			}
		}

		protected void StartTesting ()
		{
			_timer.Stop ();
			tryCount = 0;
			_timer.Elapsed -= SendMessages;
			_timer.Elapsed += TestConnection;
			_timer.Interval = 1000;
			if (ConsoleLogs)
				Console.Write("Connection testing started");
			_timer.Start ();
		}
		private void TestConnection (object sender, ElapsedEventArgs e)
		{
			if (!stopTimer)
			{
				if (ConsoleLogs)
				{
					ClearCurrentConsoleLine ();
					Console.Write ("Testing connection: {0}", tryCount);
				}
				if (tryCount < 10)
				{
					SendMessage (new Message (MessageTypes.TEST_CONNECTION));
					tryCount++;
				} else if (tryCount < 20)
				{
					tryCount = 30;
					ConnectionNotEstablished ();
				}
			} else
			{
				_timer.Stop ();
			}
		}

		protected virtual void ConnectionEstablished ()
		{
			_timer.Stop ();
			_timer.Interval = Interval;
			_timer.Elapsed -= TestConnection;
			_timer.Elapsed += SendMessages;
			_timer.Start ();
		}

		protected abstract void ConnectionNotEstablished ();

		public abstract void SendMessage (Message message);
		
		private void ClearCurrentConsoleLine()
		{
			int currentLineCursor = Console.CursorTop;
			Console.SetCursorPosition(0, Console.CursorTop);
			Console.Write(new string(' ', Console.WindowWidth)); 
			Console.SetCursorPosition(0, currentLineCursor);
		}
	}
}