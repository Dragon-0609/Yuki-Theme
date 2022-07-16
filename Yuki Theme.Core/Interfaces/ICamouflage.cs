namespace Yuki_Theme.Core.Interfaces
{
	public interface ICamouflage
	{

		bool IsVisible (string item);
	
		bool IsRight (string   item);

		void SetVisible (string item, bool value);

		void SetRight (string item, bool value);

		void Reset ();
	
		void PopulateList ();
    
		void StartToHide ();

		void SaveData ();
	}
}