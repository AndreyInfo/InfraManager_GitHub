using System;

namespace InfraManager.Core.Threading
{
    /// <summary>
    /// Контекст потока операций, блокирующий одновременную работу с источником данных
    /// </summary>
    public sealed class ThreadContext : ThreadContextBase
    {
        #region Fields
        private static readonly object __lock = new object();
        #endregion

        #region Constructors
        public ThreadContext(params Tuple<string, object>[] tuples)
            : base()
        {
            if (tuples == null)
                throw new ArgumentNullException("tuples", "tuples is null.");
            //
            SetData(tuples);
        }
        #endregion

        #region protected override GetSync()
        protected override object GetSync()
        {
            return __lock;
        }
        #endregion
    }
}
