using System;

namespace InfraManager.Core.Threading
{
    /// <summary>
    /// Контекст потока операций, не блокирующий одновременную работу с источником данных
    /// </summary>
    public sealed class NonBlockingThreadContext : ThreadContextBase
    {
        #region Fields
        private readonly object _sync;
        #endregion

        #region Constructors
        public NonBlockingThreadContext(params Tuple<string, object>[] tuples)
        {
            _sync = new object();

            SetData(tuples);
        }
        #endregion

        #region protected override method GetSync
        protected override object GetSync()
        {
            return _sync;
        }
        #endregion
    }
}
