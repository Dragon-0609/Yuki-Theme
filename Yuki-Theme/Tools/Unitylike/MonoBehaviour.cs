using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using YukiTheme.Engine;

namespace YukiTheme.Tools;

// MonoBehaviour-like lifecycle: Start runs once, Update runs every tick with a delta time.
public abstract class MonoBehaviour
{
	private const int TICK_INTERVAL_MS = 16;

	private readonly Timer _timer;
	private readonly Stopwatch _stopwatch = new Stopwatch();
	private TimeSpan _lastElapsed;
	private Dictionary<string, MethodInfo> _methods = new Dictionary<string, MethodInfo>();

	protected TimeInfo Time;
	protected Input Input;
	private bool _started;

	protected MonoBehaviour()
	{
		Time = new TimeInfo();
		Input = new Input();

		_stopwatch.Start();
		_timer = new Timer { Interval = TICK_INTERVAL_MS };
		_timer.Tick += OnTick;
		_timer.Start();
	}

	public void Destroy()
	{
		_timer.Stop();
		_timer.Tick -= OnTick;
		_timer.Dispose();
		_stopwatch.Stop();
	}

	private void OnTick(object sender, EventArgs e)
	{
		TimeSpan elapsed = _stopwatch.Elapsed;
		Time.deltaTime = (float)(elapsed - _lastElapsed).TotalSeconds;
		Time.time = (float)elapsed.TotalSeconds;
		_lastElapsed = elapsed;

		Input.Poll();

		if (!_started)
		{
			_started = true;
			TryRun("Start");
		}
		
		TryRun("Update");
	}

	private void TryRun(string name)
	{
		if (!_methods.ContainsKey(name))
			_methods[name] = GetType().GetMethod(name,
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

		try
		{
			_methods[name]?.Invoke(this, null);
		}
		catch (TargetInvocationException e)
		{
			IDEConsole.Log($"[{GetType().Name}.{name}] {e.InnerException}");
		}

	}
	
	// ReSharper disable twice InconsistentNaming
	public class TimeInfo
	{
		public float time;
		public float deltaTime;
	}
}