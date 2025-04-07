using System;

namespace InfraManager.DAL.ServiceCatalogue
{
    /// <summary>
    /// Данная сущность определяет дерево сервисов (Родительский сервис-Дочерний сервис)
    /// <para>Данная сущность обязательно содержит как родительский так и дочерний сервисы.</para>
    /// </summary>
    public class ServiceDependency
    {
        public Guid ParentServiceID { get; set; }
        public Guid ChildServiceID { get; set; }

        public virtual Service ParentService { get; }
        public virtual Service ChildService { get;  }
    }
}
