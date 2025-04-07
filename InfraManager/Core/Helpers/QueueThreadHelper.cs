using System;
using System.Collections.Generic;
using System.Threading;
using InfraManager.Core.Logging;

namespace InfraManager.Core.Helpers
{
    public static class QueueThreadHelper
    {
        #region fields
        private static Dictionary<object, Queue<Action>> __data;
        #endregion

        #region constructors
        static QueueThreadHelper()
        {
            __data = new Dictionary<object, Queue<Action>>();
        }
        #endregion

        #region static method ProcessQueue
        private static void ProcessQueue(object sourceObject)
        {
            Queue<Action> sourceQueue = null;
            Action sourceNextAction = null;
            lock (__data)
            {
                if (!__data.TryGetValue(sourceObject, out sourceQueue))
                {
                    Logger.Critical("QueueThreadHelper: sourceQueue is null");
                    throw new NotImplementedException("sourceQueue is null");
                }
                //
                if (sourceQueue.Count > 0)
                    sourceNextAction = sourceQueue.Dequeue();
                else if (!__data.Remove(sourceObject))
                {
                    Logger.Critical("QueueThreadHelper: sourceQueue not exists, first run");
                    throw new NotImplementedException("sourceQueue not exists, first run");
                }
            }
            //
            while (sourceNextAction != null)
            {
                try
                {
                    sourceNextAction();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
                //                
                lock (__data)
                    if (sourceQueue.Count > 0)
                        sourceNextAction = sourceQueue.Dequeue();
                    else
                    {
                        sourceNextAction = null;
                        if (!__data.Remove(sourceObject))
                        {
                            Logger.Critical("QueueThreadHelper: sourceQueue not exists");
                            throw new NotImplementedException("sourceQueue not exists");
                        }
                    }
            }
        }
        #endregion

        #region static method EnqueueToSourceThread
        public static void EnqueueToSourceThread(Object source, Action action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");
            //          
            bool needToRun = false;
            lock (__data)
            {
                Queue<Action> queue = null;
                if (!__data.TryGetValue(source, out queue))
                {
                    needToRun = true;
                    queue = new Queue<Action>();
                    __data.Add(source, queue);
                }
                queue.Enqueue(action);
            }
            //
            if (needToRun)
                while (!ThreadPool.QueueUserWorkItem(_ =>
                {
                    ProcessQueue(source);
                }))
                    Thread.Sleep(0);
        }
        #endregion
    }
}
