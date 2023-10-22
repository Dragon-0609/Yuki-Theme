using System;
using System.Windows.Forms;
using System.Windows.Media;

namespace YukiTheme.Components;

public class AlignmentHelper
{
	private SnapWindow _window;
	private AnchorStyles _context;

	public AlignmentHelper(SnapWindow window)
	{
		_window = window;
	}

	internal void SetAlign(AnchorStyles align)
	{
		_context = align;
		SetAlignX();
		SetAlignY();
	}

	private void SetAlignX()
	{
		AlignConditions conditions = new AlignConditions(AnchorStyles.Left, AnchorStyles.Right);
		AlignResults results = new AlignResults(AlignmentX.Left, AlignmentX.Center, AlignmentX.Right);

		ConditionalAlignSet(conditions, results, val => _window.AlignX = (AlignmentX)val);
	}

	private void SetAlignY()
	{
		AlignConditions conditions = new AlignConditions(AnchorStyles.Top, AnchorStyles.Bottom);
		AlignResults results = new AlignResults(AlignmentY.Top, AlignmentY.Center, AlignmentY.Bottom);

		ConditionalAlignSet(conditions, results, val => _window.AlignY = (AlignmentY)val);
	}

	private void ConditionalAlignSet(AlignConditions conditions, AlignResults results, Action<Enum> setTarget)
	{
		bool hasLeft = _context.HasFlag(conditions.Left);
		bool hasRight = _context.HasFlag(conditions.Right);

		if (hasLeft)
			setTarget(hasRight ? results.Center : results.First);
		else if (hasRight)
			setTarget(results.Last);
		else
			setTarget(results.Last);
	}

	private struct AlignConditions
	{
		internal Enum Left;
		internal Enum Right;

		public AlignConditions(Enum left, Enum right)
		{
			Left = left;
			Right = right;
		}
	}

	private struct AlignResults
	{
		internal readonly Enum First;
		internal readonly Enum Center;
		internal readonly Enum Last;

		public AlignResults(Enum first, Enum center, Enum last)
		{
			First = first;
			Center = center;
			Last = last;
		}
	}
}