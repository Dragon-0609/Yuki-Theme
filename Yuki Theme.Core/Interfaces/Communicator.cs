using Yuki_Theme.Core.Communication;

namespace Yuki_Theme.Core.Interfaces;

public class Communicator
{
	
	public interface IServer
	{
		void SendMessage (Message message);
		void SendMessage (int id);

		event MessageRecieved recieved;

		bool IsConnected { get; }
	}
	
	public interface IClient
	{
		event MessageRecieved recieved;
		
		bool IsConnected { get; }
	}
}

public delegate void MessageRecieved (Message message);