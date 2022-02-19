using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Controls
{
	public class ReItem : ListViewItem
	{
		public  bool          isGroup = false;
		private ReItem        rgroupIt;
		public  List <ReItem> childs = new List <ReItem> ();
		public  bool          isOld;

		public ReItem (string nm, bool isG = false) : base ()
		{
			Name = nm;
			Text = isG ? nm : $"       {nm}";
			isGroup = isG;
			Font = null;
			rgroupItem = null;
			isOld = false;
		}

		public ReItem (string nm, bool isG = false, ReItem rgroup = null) : base ()
		{
			Name = nm;
			Text = isG ? nm : $"       {nm}";
			isGroup = isG;
			Font = null;
			rgroupItem = rgroup;
			isOld = false;
		}

		public ReItem (string nm, bool isG = false, bool isO = false, ReItem rgroup = null) : base ()
		{
			Name = nm;
			Text = isG ? nm : $"       {nm}";
			isGroup = isG;
			Font = null;
			rgroupItem = rgroup;
			isOld = isO;
		}

		public ReItem rgroupItem
		{
			get
			{
				return rgroupIt;
			}
			set
			{
				if (rgroupIt != null)
					rgroupIt.childs.Remove (this);
				rgroupIt = null;
				if(value != null)
				{
					rgroupIt = value;
					rgroupIt.childs.Add (this);
				}
				
			}
		}

		public void SortChildren ()
		{
			childs = childs.OrderBy (i => i.Text).ToList ();
		}
		
	}
}