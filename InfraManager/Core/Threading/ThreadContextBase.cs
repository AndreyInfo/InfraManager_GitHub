using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using InfraManager.Core.Helpers;
using System.Collections.Concurrent;

namespace InfraManager.Core.Threading
{
    /// <summary>
    /// Базовый абстрактный класс для сущностей, реализующих модель контекста потока операций
    /// </summary>
    public abstract class ThreadContextBase : IDisposable
    {
        #region private nested class ManagedThreadServiceInfo
        private sealed class ManagedThreadServiceInfo
        {
            #region Fields
            /// <summary>
            /// Управляемый поток
            /// </summary>
            private readonly Thread _managedThread;
            /// <summary>
            /// Список идентификаторов контекстов, работающих с текущим потоком, и маркеры их активности
            /// </summary>
            private readonly ConcurrentDictionary<Guid, bool> _threadContextIDs;
            /// <summary>
            /// Список наименований слотов, заполненных контекстами
            /// </summary>
            private readonly HashSet<string> _slots;
            #endregion

            #region Properties
            #region ManagedThread
            /// <summary>
            /// Управляемый поток
            /// </summary>
            public Thread ManagedThread
            {
                get
                {
                    return _managedThread;
                }
            }
            #endregion

            #region ThreadContextIDsCount
            /// <summary>
            /// Возвращает число контекстов, в текущий момент работающих с потоком
            /// </summary>
            public int ThreadContextIDsCount
            {
                get
                {
                    var tmp = _threadContextIDs.ToArray();
                    return tmp.Where(tc => tc.Value != false).Count();
                }
            }
            #endregion

            #region Slots
            /// <summary>
            /// Список наименований слотов, заполненных контекстами
            /// </summary>
            public IEnumerable<string> Slots
            {
                get
                {
                    return _slots;
                }
            }
            #endregion

            #region SlotsLock
            /// <summary>
            /// Объект синхронизации операций с <see cref="P:Slots"/>
            /// </summary>
            public object SlotsLock
            {
                get
                {
                    return this.Slots;
                }
            }
            #endregion
            #endregion

            #region Constructors
            /// <summary>
            /// 
            /// </summary>
            /// <param name="managedThreadID">Управляемый поток</param>
            public ManagedThreadServiceInfo(Thread managedThread)
            {
                _managedThread = managedThread;
                _threadContextIDs = new ConcurrentDictionary<Guid, bool>();
                _slots = new HashSet<string>();
            }
            #endregion

            #region method AddThreadContextID
            /// <summary>
            /// 
            /// </summary>
            /// <param name="threadContextID">Уникальный идентификатор контекста</param>
            public void AddThreadContextID(Guid threadContextID)
            {
                _threadContextIDs.TryAdd(threadContextID, true);
            }
            #endregion

            #region method RemoveThreadContextID
            /// <summary>
            /// 
            /// </summary>
            /// <param name="threadContextID">Уникальный идентификатор контекста</param>
            public void RemoveThreadContextID(Guid threadContextID)
            {
                bool oldValue;
                if (this.ManagedThread == Thread.CurrentThread)
                    _threadContextIDs.TryRemove(threadContextID, out oldValue);
                else
                    _threadContextIDs.TryUpdate(threadContextID, false, true);
            }
            #endregion

            #region method AddSlot
            /// <summary>
            /// 
            /// </summary>
            /// <param name="slot">Наименование слота</param>
            public void AddSlot(string slot)
            {
                if (slot == null)
                    return;

                lock (this.SlotsLock)
                    if (!_slots.Contains(slot))
                        _slots.Add(slot);
            }
            #endregion

            #region method RemoveSlot
            /// <summary>
            /// 
            /// </summary>
            /// <param name="slot">Уникальный идентификатор контекста</param>
            public void RemoveSlot(string slot)
            {
                if (slot == null)
                    return;

                lock (this.SlotsLock)
                    _slots.Remove(slot);
            }
            #endregion

            #region method IsAffectedByThreadContext
            /// <summary>
            /// Метод, определяющий, был ли данный поток изменён контекстом с указанным идентификатором
            /// </summary>
            /// <param name="threadContextID">Уникальный идентификатор контекста</param>
            /// <returns>True, если бы л изменён, в противном случае - False</returns>
            public bool IsAffectedByThreadContext(Guid threadContextID)
            {
                return _threadContextIDs.ContainsKey(threadContextID);
            }
            #endregion
        }
        #endregion

        #region Constants
        public const string AuthenticationTokenSlot = "authenticationToken";
        public const string DataSourceLocatorSlot = "dataSourceLocator";
        public const string DataSourceSlot = "dataSource";
        public const string ConnectionTokenSlot = "connectionToken";
        #endregion

        #region Fields
        /// <summary>
        /// Словарь, содержащий информацию о том, сколько контекстов работает в рамках указанного потока
        /// и какие слоты в них настроены
        /// </summary>
        private static readonly Dictionary<Thread, ManagedThreadServiceInfo> __syncs = new Dictionary<Thread, ManagedThreadServiceInfo>();
        /// <summary>
        /// Уникальный идентификатор экземпляра контекста
        /// </summary>
        private readonly Guid _ID;
        /// <summary>
        /// Флаг, указывющий, были ли уже освобождены ресурсы объекта
        /// </summary>
        private bool _isDisposed;
        /// <summary>
        /// Объект синхронизации доступа к <see cref="M:_isDisposed"/>
        /// </summary>
        private readonly object _isDisposedSync;

        private readonly Thread _thread;
        #endregion

        #region Properties
        #region IsDisposed
        /// <summary>
        /// Флаг, указывющий, были ли уже освобождены ресурсы объекта
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return _isDisposed;
            }
            private set
            {
                if (_isDisposed)
                    return;

                _isDisposed = value;
                RaiseIsDisposedChanged();
            }
        }
        #endregion

        #region ID
        /// <summary>
        /// Уникальный идентификатор экземпляра контекста
        /// </summary>
        public Guid ID
        {
            get
            {
                return _ID;
            }
        }
        #endregion
        #endregion

        #region Events
        #region IsDisposedChanged
        public event EventHandler IsDisposedChanged;

        private void RaiseIsDisposedChanged()
        {
            if (IsDisposedChanged != null)
                IsDisposedChanged(this, System.EventArgs.Empty);
        }
        #endregion
        #endregion

        #region Constructors
        public ThreadContextBase()
        {
            _isDisposedSync = new object();
            _isDisposed = false;
            _ID = Guid.NewGuid();
            _thread = System.Threading.Thread.CurrentThread;
        }
        #endregion

        #region Destructor
        ~ThreadContextBase()
        {
            Dispose(false);
        }
        #endregion

        #region public method Dispose
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="T:System.Threading.SynchronizationLockException"/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isDisposing">Флаг, указывающий, необходимо ли освободить выделенные под объект ресурсы</param>
        /// <exception cref="T:System.Threading.SynchronizationLockException"/>
        protected virtual void Dispose(bool isDisposing)
        {
            if (!isDisposing)
                return;

            lock (_isDisposedSync)
            {
                lock (__syncs)
                {
                    var currentManagedThread = System.Threading.Thread.CurrentThread;
                    //Было ли оказано воздействие на текущий поток?
                    if (__syncs.ContainsKey(currentManagedThread))
                    {
                        var currentManagedThreadServiceInfo = __syncs[currentManagedThread];
                        //Было ли воздействие оказано текущим контекстом?
                        if (currentManagedThreadServiceInfo.IsAffectedByThreadContext(this.ID))
                        {
                            //Если да, то убираем учёт текущего контекста
                            currentManagedThreadServiceInfo.RemoveThreadContextID(this.ID);
                            //Если активного контекста больше нет (для данного потока), то очищаем слоты данных
                            if (currentManagedThreadServiceInfo.ThreadContextIDsCount == 0)
                            {
                                __syncs.Remove(currentManagedThreadServiceInfo.ManagedThread);

                                if (currentManagedThread == currentManagedThreadServiceInfo.ManagedThread)
                                    foreach (var slot in currentManagedThreadServiceInfo.Slots)
                                        ThreadHelper.RemoveData(slot);
                            }
                        }
                    }

                    //Удаляем текущий контекст из других потоков
                    var managedThreads = __syncs.Where(s => s.Value.IsAffectedByThreadContext(this.ID)).Select(s => s.Key);
                    foreach (var managedThread in managedThreads)
                    {
                        var managedThreadServiceInfo = __syncs[managedThread];
                        managedThreadServiceInfo.RemoveThreadContextID(this.ID);

                        if (managedThreadServiceInfo.ThreadContextIDsCount == 0)
                            __syncs.Remove(managedThreadServiceInfo.ManagedThread);
                    }
                }

                var sync = GetSync();
                if (sync != null)
                    Monitor.Exit(sync);

                this.IsDisposed = true;
            }
        }
        #endregion

        #region abstract method GetSync
        /// <summary>
        /// Возвращает объект синхронизации 
        /// </summary>
        /// <returns></returns>
        protected abstract object GetSync();
        #endregion

        #region protected method SetData
        /// <summary>
        /// Производит запись данных в слоты текущего потока
        /// </summary>
        /// <param name="tuples">Пары наименования слота - значение</param>
        /// <exception cref="T:System.InvalidOperationException"/>
        /// <exception cref="T:System.ArgumentException"/>
        protected void SetData(params Tuple<string, object>[] tuples)
        {
            lock (_isDisposedSync)
            {
                if (this.IsDisposed)
                    throw new InvalidOperationException("Object is already disposed!");

                #region Проверка параметров
                if (tuples == null || tuples.Length == 0)
                    return;
                if (tuples.Any(t => t.Item1 == null))
                    throw new ArgumentException("tuples", "Tuples must not contain items with identifiers equals null.");
                #endregion

                var currentThread = System.Threading.Thread.CurrentThread;
                ManagedThreadServiceInfo managedThreadServiceInfo;
                lock (__syncs)
                    if (__syncs.ContainsKey(currentThread))
                        managedThreadServiceInfo = __syncs[currentThread];
                    else
                    {
                        managedThreadServiceInfo = new ManagedThreadServiceInfo(currentThread);
                        __syncs.Add(currentThread, managedThreadServiceInfo);
                    }
                managedThreadServiceInfo.AddThreadContextID(this.ID);

                lock (managedThreadServiceInfo.SlotsLock)
                    try
                    {
                        var sync = GetSync();
                        if (sync != null)
                            Monitor.Enter(sync);

                        foreach (var tuple in tuples)
                        {
                            ThreadHelper.SetData(tuple.Item1, tuple.Item2);
                            managedThreadServiceInfo.AddSlot(tuple.Item1);
                        }
                    }
                    catch (Exception exc)
                    {
                        OnSetDataException(exc);
                        throw;
                    }
            }
        }
        #endregion

        #region protected virtual void OnSetDataException
        /// <summary>
        /// Обработчик, позволяющий потомкам внедрить 
        /// дополнительную логику в процесс обработки исключения при выполнении
        /// метода <see cref="SetData"/>
        /// <param name="exc">Описание исключения</param>
        /// </summary>
        /// <exception cref="T:System.NullReferenceException"/>
        /// <exception cref="T:System.Threading.SynchronizationLockException"/>
        protected virtual void OnSetDataException(Exception exc)
        {
            Dispose(true);
        }
        #endregion
    }
}
