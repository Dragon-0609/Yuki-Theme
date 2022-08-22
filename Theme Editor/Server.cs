using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NamedPipeWrapper;
using Yuki_Theme.Core;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Communication;
using Yuki_Theme.Core.Interfaces;
using static Yuki_Theme.Core.Communication.MessageTypes;
using Message = Yuki_Theme.Core.Communication.Message;

namespace Theme_Editor
{
	public class Server : CommunicatorBase, Communicator.IServer
	{
		private       NamedPipeServer<Message> _server;
		
		public event MessageRecieved recieved;

		public bool IsConnected => _isConnected;

		private bool _isConnected = false;

		internal Server ()
		{
			Init ();
		}

		private void Init ()
		{
			_messages = new Queue<Message> ();
			SetAPI ();
			InitMessaging ();			
			// InitTimer ();
		}

		private void SetAPI ()
		{
			ServerAPI api = new ServerAPI ();
			Settings.ConnectAndGet ();
			CentralAPI.Current = api;
			Helper.mode = ProductMode.Plugin;
			api.Server = this;
			api.LoadSchemes ();
			api.AddEvents ();
		}

		private void InitMessaging ()
		{
			_server = new NamedPipeServer<Message> (Message.PATH);
			_server.ClientConnected += ClientConnected;
			_server.ClientDisconnected += ClientDisconnected;
			_server.ClientMessage += MessageReceived;
			_server.Error += ErrorRaised;
			_server.Start ();
			Console.WriteLine("Server started");
		}

		private void ClientConnected (NamedPipeConnection<Message, Message> connection)
		{
			SendMessage (TEST_CONNECTION);
		}

		private void ClientDisconnected (NamedPipeConnection<Message, Message> connection)
		{
			_timer.Stop ();
			_server.Stop ();
			Application.Exit ();
		}

		private void MessageReceived (NamedPipeConnection<Message, Message> connection, Message message)
		{
			Console.WriteLine ($"Received: {message}");
			ParseMessage (message);
			
			if (recieved != null)
				recieved (message);
		}

		private void ErrorRaised (Exception exception)
		{
			CentralAPI.Current.ShowError ($"ERROR: {exception} =>{exception.StackTrace}", "Error");
		}


		public override void SendMessage (Message message)
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
				case TEST_CONNECTION:
					_isConnected = true;
					SendMessage (TEST_CONNECTION_OK);
					break;
				
				case CLOSE_CLIENT:
					_server.Stop ();
					Application.Exit();
					break;
			}
		}
		
	}
}