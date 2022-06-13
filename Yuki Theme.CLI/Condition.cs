namespace Yuki_Theme.CLI
{
	public class Condition
	{
		public string Target   = "";
		public string Equality = "";

		public bool CouldSetOneOfThem (string value)
		{
			if (Target == "") Target = value;
			else if (Equality == "") Equality = value;
			else return false;
			return true;
		}
	}
}