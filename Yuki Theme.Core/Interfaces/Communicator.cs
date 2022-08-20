using Yuki_Theme.Core.Communication;

namespace Yuki_Theme.Core.Interfaces;

public class Communicator
{
	
	public interface IServer
	{
		void SendMessage (Message message);
		void SendMessage (int id);
		void SendMessage (int id, string content);

		event MessageRecieved recieved;

		bool IsConnected { get; }
	}
	
	public interface IClient
	{
		void SendMessage (Message message);
		void SendMessage (int id);
		void SendMessage (int id, string content);
		
		event MessageRecieved recieved;
		
		bool IsConnected { get; }
	}
	
}

public delegate void MessageRecieved (Message message);