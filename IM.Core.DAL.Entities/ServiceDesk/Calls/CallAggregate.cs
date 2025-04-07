using System;

namespace InfraManager.DAL.ServiceDesk.Calls
{
    /// <summary>
    /// Этот класс описывает агрегат с рассчитываемыми данными заявок (денормализация)
    /// </summary>
    public class CallAggregate
    {
        protected CallAggregate()
        {
        }

        public CallAggregate(Guid callID)
        {
            CallID = callID;
        }

        public long ID { get; init; }
        public Guid CallID { get; }
        public int DocumentCount { get; set; }
        public int ProblemCount { get; set; }
        public int WorkOrderCount { get; set; }
        public string QueueName { get; set; }
    }
}
