using System;
using System.Windows.Forms;
using NamedPipeWrapper;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Interfaces;
using static Yuki_Theme.Core.Communication.MessageTypes;
using Message = Yuki_Theme.Core.Communication.Message;

namespace Theme_Editor
{
	internal class Client : Communicator.IClient
	{
		private NamedPipeClient<Message> _client;

		private bool isConnected = false;

		public event MessageRecieved recieved;

		public bool IsConnected => isConnected;
		
		[STAThread]
		public static void Main (string[] args)
		{
			Client client = new Client ();
		}
		
		private Client ()
		{
			Init ();
			Application.Run ();
		}
		
		private void Init ()
		{
			SetAPI ();
			InitMessaging ();
		}

		private void SetAPI ()
		{
			API.Current = new ClientAPI ();
		}

		private void InitMessaging ()
		{
			_client = new NamedPipeClient<Message> (Message.PATH);
			_client.ServerMessage += MessageReceived;
			_client.Error += ErrorRaised;
			_client.Disconnected += ServerDisconnected;
			_client.Start ();
		}

		private void ServerDisconnected (NamedPipeConnection<Message, Message> connection)
		{
			_client.Stop ();
			Application.Exit ();
		}

		private void ErrorRaised (Exception exception)
		{
			API.Current.ShowError ($"ERROR: {exception}", "Error");
		}

		private void MessageReceived (NamedPipeConnection<Message, Message> connection, Message message)
		{
			Console.WriteLine ($"Received: {message}");
			
			if (recieved != null)
				recieved (message);
			ParseMessage (message);
		}

		private void SendMessage (int id)
		{
			_client.PushMessage (new Message (id));
		}

		private void ParseMessage (Message message)
		{
			switch (message.Id)
			{
				case TEST_CONNECTION:
					isConnected = true;
					SendMessage (TEST_CONNECTION_OK);
					break;
				
				case CLOSE_CLIENT:
					_client.Stop ();
					Application.Exit();
					break;
			}
		}
	}
}