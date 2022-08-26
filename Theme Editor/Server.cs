using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Windows;
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


		internal Server ()
		{
			Init ();
		}

		private void Init ()
		{
			_messages = new Queue<Message> ();
			InitMessaging ();
			SetAPI ();			
			InitTimer ();
		}

		private void SetAPI ()
		{
			ServerAPI api = new ServerAPI ();
			CentralAPI.Current = api;
			Helper.mode = ProductMode.Plugin;
			Settings.ConnectAndGet ();
			api.Server = this;
			api.LoadSchemes ();
			api.AddEvents ();
			api.AddEvent (OPEN_MAIN_WINDOW, message => App._manager.OpenMainWindow ());
		}

		private void InitMessaging ()
		{
			PipeSecurity pSecure = new PipeSecurity();
			pSecure.SetAccessRule(new PipeAccessRule(Environment.UserName, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow));
			
			_server = new NamedPipeServer<Message> (Message.PATH, pSecure);
			_server.ClientConnected += ClientConnected;
			_server.ClientDisconnected += ClientDisconnected;
			_server.ClientMessage += MessageReceived;
			_server.Error += ErrorRaised;
			_server.Start ();
			App._manager._server = this;
			Console.WriteLine("Server started");
		}

		private void ClientConnected (NamedPipeConnection<Message, Message> connection)
		{
			Console.WriteLine("Client Connected");
			SendMessage (TEST_CONNECTION);
		}

		private void ClientDisconnected (NamedPipeConnection<Message, Message> connection)
		{
			_timer.Stop ();
			_server.Stop ();
			Application.Current.Shutdown();
		}

		private void MessageReceived (NamedPipeConnection<Message, Message> connection, Message message)
		{
			Console.WriteLine ($"Received: {message.Id}");

			ParseMessage (message);
			Application.Current.Dispatcher.Invoke (() =>
			{

				if (recieved != null)
					recieved (message);
			});
		}

		private void ErrorRaised (Exception exception)
		{
			CentralAPI.Current.ShowError ($"ERROR: {exception} =>{exception.StackTrace}", "Error");
		}


		public override void SendMessage (Message message)
		{
			if (_server._connections.Count > 0)
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
				
				case CLOSE_SERVER:
					_server.Stop ();
					Application.Current.Shutdown();
					break;
			}
		}
		
	}
}