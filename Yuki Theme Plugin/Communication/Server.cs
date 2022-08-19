using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using NamedPipeWrapper;
using Yuki_Theme.Core.Interfaces;
using Yuki_Theme_Plugin.Interfaces;
using static Yuki_Theme.Core.Communication.MessageTypes;
using Message = Yuki_Theme.Core.Communication.Message;

namespace Yuki_Theme_Plugin.Communication
{
	internal class Server : Communicator.IServer
	{
		private const string                   THEME_INSTALLER = "Theme_Installer.exe";
		private       NamedPipeServer<Message> _server;
		
		public event MessageRecieved recieved;

		public bool IsConnected => _isConnected;

		private Queue<Message> _messages;

		private Timer _timer;

		private bool _isConnected = false;

		private IConsole _console;

		internal Server (IConsole console)
		{
			_console = console;
			Init ();
		}

		private void Init ()
		{
			_messages = new Queue<Message> ();
			InitMessaging ();
			RunClient ();
			InitTimer ();
		}

		private void InitMessaging ()
		{
			_server = new NamedPipeServer<Message> (Message.PATH);
			_server.ClientConnected += ClientConnected;
			_server.ClientDisconnected += ClientDisconnected;
			_server.ClientMessage += MessageReceived;
			_server.Error += ErrorRaised;
			_server.Start ();
		}

		private void ClientConnected (NamedPipeConnection<Message, Message> connection)
		{
			SendMessage (TEST_CONNECTION);
		}

		private void ClientDisconnected (NamedPipeConnection<Message, Message> connection)
		{
			_timer.Stop ();
			_server.Stop ();
			_console.WriteToConsole ("Client is disconnected");
		}

		private void MessageReceived (NamedPipeConnection<Message, Message> connection, Message message)
		{
			_console.WriteToConsole ($"Received: {message}");
			ParseMessage (message);
			
			if (recieved != null)
				recieved (message);
		}

		private void ErrorRaised (Exception exception)
		{
			_console.WriteToConsole ($"ERROR: {exception}");
		}


		private void RunClient ()
		{
			Process.Start (THEME_INSTALLER);
		}

		public void SendMessage (Message message)
		{
			if (_isConnected && _server._connections.Count > 0)
			{
				foreach (NamedPipeConnection<Message, Message> connection in _server._connections)
				{
					connection.PushMessage (message);
				}
			} else
			{
				_messages.Enqueue (message);
			}
		}

		public void SendMessage (int id)
		{
			SendMessage (new Message (id));
		}

		public void SendMessage (int id, string content)
		{
			SendMessage (new Message (id, content));
		}

		private void ParseMessage (Message message)
		{
			switch (message.Id)
			{
				case TEST_CONNECTION_OK:
					_isConnected = true;
					_console.WriteToConsole ("Theme applier is running");
					break;
			}
		}
		

		private void InitTimer ()
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
	}
}