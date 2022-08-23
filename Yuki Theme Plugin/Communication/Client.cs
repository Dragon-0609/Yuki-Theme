using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using NamedPipeWrapper;
using Yuki_Theme.Core.Communication;
using Yuki_Theme.Core.Interfaces;
using Yuki_Theme_Plugin.Interfaces;
using static Yuki_Theme.Core.Communication.MessageTypes;
using Message = Yuki_Theme.Core.Communication.Message;

namespace Yuki_Theme_Plugin.Communication
{
	internal class Client : CommunicatorBase, Communicator.IClient
	{
		private const string THEME_EDITOR = "Theme_Editor.exe";
		
		private NamedPipeClient<Message> _client;

		private bool                 _isConnected = false;

		private EventWaitHandle _startedEvent;
		
		public event MessageRecieved recieved;
		

		private IConsole _console;
		
		internal Client (IConsole console)
		{
			_console = console;
			Init ();
		}
		
		private void Init ()
		{
			_messages = new Queue<Message> ();
			RunServer ();
			InitMessaging ();
		}

		private void InitMessaging ()
		{
			_client = new NamedPipeClient<Message> (Message.PATH);
			_client.ServerMessage += MessageReceived;
			_client.Error += ErrorRaised;
			_client.Disconnected += ServerDisconnected;
			_client.Start ();
			_console.WriteToConsole ("Client Connected");
		}

		private void ServerDisconnected (NamedPipeConnection<Message, Message> connection)
		{
			_client.Stop ();
			_console.WriteToConsole ("Client is disconnected");
		}

		private void ErrorRaised (Exception exception)
		{
			_console.WriteToConsole ($"ERROR: {exception}");
		}

		private void RunServer ()
		{
			Process.Start (THEME_EDITOR);
			
			_startedEvent = new EventWaitHandle(false, EventResetMode.ManualReset, @"Global\YukiThemeServerStarted");
			_startedEvent.WaitOne();
		}

		private void MessageReceived (NamedPipeConnection<Message, Message> connection, Message message)
		{
			_console.WriteToConsole ($"Received: {message.Id}");
			
			if (recieved != null)
				recieved (message);
			ParseMessage (message);
		}

		public override void SendMessage (Message message)
		{
			if (_isConnected)
			{
				_client.PushMessage (message);
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
				case TEST_CONNECTION:
					_isConnected = true;
					SendMessage (TEST_CONNECTION_OK);
					_console.WriteToConsole ("Theme editor is running");
					break;
			}
		}
	}
}