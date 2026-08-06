using System;
using System.Windows.Forms;

namespace YukiTheme.Tools;

public class DelayedCall
{
	private readonly Timer _timer;
	private Action _onComplete;

	public DelayedCall(float delay, Action onComplete)
	{
		_onComplete = onComplete;
		_timer = new Timer { Interval = Convert.ToInt32(delay * 1000) };
		_timer.Tick += OnTick;
	}

	public void Start()
	{
		_timer.Start();
	}

	public void Cancel()
	{
		_timer.Stop();
		_timer.Dispose();
	}

	private void OnTick(object sender, EventArgs e)
	{
		_timer.Stop();
		_timer.Dispose();
		_onComplete?.Invoke();
		_onComplete = null;
	}

	public static DelayedCall Run(float delay, Action onComplete)
	{
		var call = new DelayedCall(delay, onComplete);
		call.Start();
		return call;
	}
}