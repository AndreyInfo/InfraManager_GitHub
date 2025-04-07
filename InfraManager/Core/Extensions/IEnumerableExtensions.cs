using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace InfraManager.Core.Extensions
{
	public static class IEnumerableExtensions
	{
		#region Contains
		public static bool Contains<T>(this IEnumerable<T> source, Func<T, bool> func)
		{
			if (source == null)
				return false;
			return source.Count(func) > 0;
		}
		#endregion

		#region FirstOrDefault
		public static T FirstOrDefault<T>(this IEnumerable<T> source, Func<T, bool> func, T @default)
		{
			if (source.Contains(func))
				return source.First(func);
			else
				return @default;
		}
		#endregion

		#region ForEach
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			if (source == null)
				return;
			foreach (T item in source)
				action(item);
		}
		#endregion

        #region IndexOf
        public static int IndexOf<T>(this IEnumerable<T> source, T item)
        {
            var itemIndex = 0;
            foreach (var s in source)
                if (!object.Equals(s, item))
                    itemIndex++;
                else
                    return itemIndex;
            return -1;
        }
        #endregion

        #region ParallelForEach
        public static void ParallelForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			if (source == null)
				return;
			using (var counter = new _Counter(source.Count()))
			{
				foreach (var item in source)
				{
					var captured = item;
                    while (!ThreadPool.QueueUserWorkItem(x => { action(captured); counter.Signal(); }))
                        Thread.Sleep(0);
				}
				counter.Wait();
			}
		}


		#region classes
		private sealed class _Counter : IDisposable
		{
			private readonly ManualResetEvent _manualResetEvent;
			private volatile int _count;
			private readonly object _sync = new object();


			public _Counter(int count)
			{
				_count = count;
				_manualResetEvent = new ManualResetEvent(count == 0);
			}


			#region method Signal
			public void Signal()
			{
				lock (_sync)
					if (_count > 0 && --_count == 0)
						_manualResetEvent.Set();
			}
			#endregion

			#region method Wait
			public void Wait()
			{
				_manualResetEvent.WaitOne();
			}
			#endregion


			#region interface IDisposable
			public void Dispose()
			{
				((IDisposable)_manualResetEvent).Dispose();
			}
			#endregion
		}
        #endregion
        #endregion

        #region IsEqualValues
        public static bool IsEqualValues<T>(this IEnumerable<T> source)
        {
            T firstValue = default(T);
            bool isFirstValue = true;
            //
            foreach (var val in source)
            {
                if (isFirstValue)
                {
                    firstValue = val;
                    isFirstValue = false;
                }
                else if (firstValue != null)
                {
                    if (!firstValue.Equals(val))
                        return false;
                }
                else if (val != null)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}
