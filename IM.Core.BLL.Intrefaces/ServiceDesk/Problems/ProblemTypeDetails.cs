using System;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    /// <summary>
    /// Этот класс описывает выходной контракт данных "Тип проблемы"
    /// </summary>
    public class ProblemTypeDetails : ProblemTypeData
    {
        public Guid ID { get; set; }
        
        public string FullName { get; init; }
    }
}
