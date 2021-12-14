using System.Drawing;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Forms
{
	public partial class TesterForm : Form
	{
		public TesterForm ()
		{
			InitializeComponent ();
			tabPage1.BackColor = Color.Gray;
			tabPage2.BackColor = Color.Red;
			tabPage3.BackColor = Color.Red;
		}
	}
}