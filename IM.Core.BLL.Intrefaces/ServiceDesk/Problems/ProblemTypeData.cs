using System;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    /// <summary>
    /// Этот класс представляет входной контракт данных "Тип проблемы"
    /// </summary>
    public class ProblemTypeData : LookupData
    {
        public Guid? ParentID { get; init; }

        public byte[] Image { get; init; }

        public string WorkflowSchemeIdentifier { get; init; }


        public Guid? FormID { get; init; }


        public string ImageName { get; init; }
    }
}
