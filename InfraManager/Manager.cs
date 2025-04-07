using System;
using System.Threading;

namespace InfraManager.ComponentModel
{
    public abstract class Manager<T> : Singleton<T>, IManager where T : class
    {
        #region fields
        private static readonly object __lock = new object();
        protected readonly object _lock = new object();
        protected bool _isInitialized;
        private EventHandler _terminating;
        #endregion


        #region properties
        public static object StaticLock { get { return __lock; } }

        protected bool IsInitializedLockless { get { return _isInitialized; } }

        protected virtual bool IsRunningLockless { get { return false; } }
        #endregion


        #region events
        public event EventHandler Terminating
        {
            add { lock (_lock) _terminating = (EventHandler)Delegate.Combine(_terminating, value); }
            remove { lock (_lock) _terminating = (EventHandler)Delegate.Remove(_terminating, value); }
        }
        #endregion


        #region constructors
        static Manager() { }

        protected Manager() { }
        #endregion


        #region interface IManager
        public bool IsInitialized { get { lock (_lock) return IsInitializedLockless; } }

        public bool IsRunning { get { lock (_lock) return IsRunningLockless; } }

        protected abstract string ServiceName { get; }

        protected virtual bool InitializeLocked(params object[] arguments) { return true; }

        public virtual void Initialize(params object[] arguments)
        {
            if (arguments == null || arguments.Length != 0)
                throw new ArgumentException("arguments are not expected.", "arguments");
            //
            if (IsInitializedLockless)
                throw new InvalidOperationException("manager has been already initialized.");

            lock (_lock)
            {
                if (IsInitializedLockless)
                    throw new InvalidOperationException("manager has been already initialized.");
                //
                _isInitialized = InitializeLocked(arguments);
            }
        }

        public virtual void Terminate()
        {
            if (IsInitializedLockless)
                lock (_lock)
                    if (IsInitializedLockless)
                    {
                        _isInitialized = false;
                    }
        }

        public virtual void Start()
        {
            if (!IsRunningLockless)
                lock (_lock)
                    if (!IsRunningLockless)
                    {
                        if (!IsInitializedLockless)
                            throw new InvalidOperationException("manager has not been initialized.");
                    }
        }

        public virtual void Stop() { }
        #endregion

        #region method RestartServiceAfterDataSourceLocatorsChanged
        protected void RestartServiceAfterDataSourceLocatorsChanged()
        {
            if (string.IsNullOrWhiteSpace(this.ServiceName))
                return;
            //
            using (var process = new System.Diagnostics.Process()) //Will not work in containers
            {
                process.StartInfo.FileName = "cmd";
                process.StartInfo.Arguments = string.Format("/c net stop \"{0}\" & net start \"{0}\"", this.ServiceName);
                process.Start();
            }
        }
        #endregion

        #region protected method OnTerminating
        protected void OnTerminating()
        {
            var terminating = Interlocked.CompareExchange(ref _terminating, null, null);
            if (terminating != null)
                terminating(this, EventArgs.Empty);
        }
        #endregion
    }
}
