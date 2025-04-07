using System;
using System.Threading;

namespace InfraManager.Core.Threading
{
	public class ThreadManager
	{
		#region fields
		private readonly object _lock = new object();
		private EventHandler _started;
		private EventHandler _stopped;
		private EventHandler _finished;
		private EventHandler<EventArgs<Exception>> _faulted;
		private bool _isRunning;
		private bool _isStopping;
		private Thread _thread;
		private Action<object> _runner;
		private object _state;
		#endregion


		#region properties
		public bool IsRunning
		{
			get { lock (_lock) return _isRunning; }
		}

		public bool IsStopping
		{
			get { lock (_lock) return _isStopping; }
		}

		public Thread Thread
		{
			get { lock (_lock) return _thread; }
		}
		#endregion


		#region events
		public event EventHandler Stopped
		{
			add { lock (_lock) _stopped = (EventHandler)Delegate.Combine(_stopped, value); }
			remove { lock (_lock) _stopped = (EventHandler)Delegate.Remove(_stopped, value); }
		}

		public event EventHandler Started
		{
			add { lock (_lock) _started = (EventHandler)Delegate.Combine(_started, value); }
			remove { lock (_lock) _started = (EventHandler)Delegate.Remove(_started, value); }
		}

		public event EventHandler Finished
		{
			add { lock (_lock) _finished = (EventHandler)Delegate.Combine(_finished, value); }
			remove { lock (_lock) _finished = (EventHandler)Delegate.Remove(_finished, value); }
		}

		public event EventHandler<EventArgs<Exception>> Faulted
		{
			add { lock (_lock) _faulted = (EventHandler<EventArgs<Exception>>)Delegate.Combine(_faulted, value); }
			remove { lock (_lock) _faulted = (EventHandler<EventArgs<Exception>>)Delegate.Remove(_faulted, value); }
		}
		#endregion


		#region constructors
		public ThreadManager(Action<object> runner, object state)
		{
			if (runner == null)
			{
				throw new ArgumentNullException("runner", "runner is null.");
			}
			_runner = runner;
			_state = state;
		}

		public ThreadManager(Action<object> runner)
			: this(runner, null)
		{ }
		#endregion


		#region method CreateThread
		public void CreateThread()
		{
			lock (_lock)
			{
				if (_thread != null)
				{
					throw new InvalidOperationException("Thread has been already created.");
				}
				_thread = new Thread(() => Run()) { IsBackground = true };
			}
		}
		#endregion

		#region method Start
		public void Start()
		{
			lock (_lock)
			{
				if (_isRunning)
				{
					throw new InvalidOperationException("Thread has been already started.");
				}
				if (_thread == null)
				{
					_thread = new Thread(() => Run()) { IsBackground = true };
				}
				_thread.Start();
				_isRunning = true;
			}
			//
			OnStarted();
		}
		#endregion

		#region method Stop
		public void Stop()
		{
			lock (_lock)
			{
				_isStopping = true;
			}
			//
			OnStopped();
		}
		#endregion

		#region protected virtual method OnStarted
		protected virtual void OnStarted()
		{
			var started = Interlocked.CompareExchange(ref _started, null, null);
			if (started != null)
				started(this, System.EventArgs.Empty);
		}
		#endregion

		#region protected virtual method OnStopped
		protected virtual void OnStopped()
		{
			var stopped = Interlocked.CompareExchange(ref _stopped, null, null);
			if (stopped != null)
				stopped(this, System.EventArgs.Empty);
		}
		#endregion

		#region protected virtual method OnFinished
		protected virtual void OnFinished()
		{
			var finished = Interlocked.CompareExchange(ref _finished, null, null);
			if (finished != null)
				finished(this, System.EventArgs.Empty);
		}
		#endregion

		#region protected virtual method OnFaulted
		protected virtual void OnFaulted(Exception e)
		{
			var faulted = Interlocked.CompareExchange(ref _faulted, null, null);
			if (faulted != null)
				faulted(this, new EventArgs<Exception>(e));
		}
		#endregion

		#region private method Run
		private void Run()
		{
			try
			{
				var state = _state;
				_state = null;
				_runner(state);
			}
			catch (Exception e)
			{
				OnFaulted(e);
			}
			finally
			{
				_thread = null;
				_isRunning = false;
				_isStopping = false;
				OnFinished();
			}
		}
		#endregion
	}
}
