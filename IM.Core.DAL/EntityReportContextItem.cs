using System;

namespace InfraManager.DAL.ServiceDesk
{
    internal class EntityReportContextItem<TEntity>
    {
        public TEntity Entity { get; init; }
        public Guid UserID { get; init; }
    }
}
