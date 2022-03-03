using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Controls
{
	public class ReItem : ListViewItem
	{
		public const string THEME_WHITE_SPACE = "       ";
		
		public  bool          isGroup = false;
		private ReItem        parentGroup;
		public  List <ReItem> children = new List <ReItem> ();
		public  bool          isOld;
		public  bool          isHidden = false;

		public ReItem (string name, bool IsGroup = false) : base ()
		{
			Name = name;
			Text = IsGroup ? name : THEME_WHITE_SPACE + name;
			isGroup = IsGroup;
			Font = null;
			ParentGroup = null;
			isOld = false;
		}

		public ReItem (string name, bool IsGroup = false, ReItem rgroup = null) : base ()
		{
			Name = name;
			Text = IsGroup ? name : THEME_WHITE_SPACE + name;
			isGroup = IsGroup;
			Font = null;
			ParentGroup = rgroup;
			isOld = false;
		}

		public ReItem (string name, bool IsGroup = false, bool IsOld = false, ReItem rgroup = null) : base ()
		{
			Name = name;
			Text = IsGroup ? name : THEME_WHITE_SPACE + name;
			isGroup = IsGroup;
			Font = null;
			ParentGroup = rgroup;
			isOld = IsOld;
		}

		public ReItem ParentGroup
		{
			get
			{
				return parentGroup;
			}
			set
			{
				if (parentGroup != null)
					parentGroup.children.Remove (this);
				parentGroup = null;
				if(value != null)
				{
					parentGroup = value;
					parentGroup.children.Add (this);
				}
				
			}
		}

		public void SortChildren ()
		{
			children = children.OrderBy (i => i.Name).ToList ();
		}
		
	}
}