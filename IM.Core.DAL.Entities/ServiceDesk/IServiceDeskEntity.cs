using System;

namespace InfraManager.DAL.ServiceDesk
{
    public interface IServiceDeskEntity : IWorkflowEntity
    {
        public int Number { get; }
        public Guid PriorityID { get; set; }
        public string Description { get; }
    }
}
