﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using NamedPipeWrapper;
using Yuki_Theme.Core;
using Yuki_Theme.Core.API;
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

		private NamedPipeClient <Message> _client;

		private bool _isConnected = false;

		private bool stopClient = false;

		public event MessageRecieved recieved;


		private IConsole _console;

		private IDispatcher _dispatcher;

		internal Client (IConsole console, IDispatcher dispatcher)
		{
			_console = console;
			_dispatcher = dispatcher;
			Init ();
		}

		private void Init ()
		{
			_messages = new Queue <Message> ();
			RunServer ();
			if (!stopClient)
			{
				InitMessaging ();
				InitTimer ();
				StartTesting ();
			}
		}

		private void InitMessaging ()
		{
			_client = new NamedPipeClient <Message> (Message.PATH);
			_client.ServerMessage += MessageReceived;
			_client.Error += ErrorRaised;
			_client.Disconnected += ServerDisconnected;
			_client.Start ();
			_console.WriteToConsole ("Client Connected");
		}

		private void ServerDisconnected (NamedPipeConnection <Message, Message> connection)
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
			try
			{
				Process.Start (THEME_EDITOR);
			} catch
			{
				stopTimer = stopClient = true;
				ConnectionNotEstablished ();
			}
			
		}

		private void MessageReceived (NamedPipeConnection <Message, Message> connection, Message message)
		{
			_console.WriteToConsole ($"Received: {message.Id}");
			_dispatcher.InvokeUI (new MethodInvoker (delegate
			{
				if (recieved != null)
					recieved (message);
			}));

			ParseMessage (message);
		}

		protected override void ConnectionNotEstablished ()
		{
			MessageBox.Show ("Couldn't establish connection between Plugin and Theme Editor. Functionality will be limited.\nYou won't be able to export, edit and do anything that needs admin rights. If the IDE isn't in Program Files, it'll work, else it won't.", "Couldn't connect to Theme Editor app");
			CentralAPI.Current = new CommonAPI ();
			Settings.ConnectAndGet ();
		}

		public override void SendMessage (Message message)
		{
			if (_isConnected || message.Id == TEST_CONNECTION)
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
				case TEST_CONNECTION_OK:
					_isConnected = true;
					_console.WriteToConsole ("Theme editor is running");
					ConnectionEstablished ();
					SendMessage (TEST_CONNECTION_OK);
					break;
			}
		}
	}
}