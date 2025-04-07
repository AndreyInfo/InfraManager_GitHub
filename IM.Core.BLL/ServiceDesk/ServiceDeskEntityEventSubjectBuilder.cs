using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceDesk;
using System;

namespace InfraManager.BLL.ServiceDesk
{
    internal abstract class ServiceDeskEntityEventSubjectBuilder<TEntity> : EventSubjectBuilderBase<TEntity, TEntity>
        where TEntity : IServiceDeskEntity
    {
        protected ServiceDeskEntityEventSubjectBuilder(string name) : base(name)
        {
        }

        protected override Guid GetID(TEntity subject)
        {
            return subject.IMObjID;
        }

        protected override string GetSubjectValue(TEntity subject)
        {
            return subject.Number.ToString();
        }
    }
}
