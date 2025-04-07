using System;

namespace InfraManager.DAL.ServiceDesk
{
    public interface IDefault
    {
        Guid ID { get; }
        bool Default { get; }
    }
}
