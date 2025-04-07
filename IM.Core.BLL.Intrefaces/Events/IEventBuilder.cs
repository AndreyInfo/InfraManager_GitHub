using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Attributes;
using InfraManager.DAL.Events;
using System;
using System.Threading.Tasks;

namespace InfraManager.BLL.Events
{
    [Obsolete("Use Inframanager.BLL.Events.IConfigureEventBuilder<T>")]
    public interface IEventBuilder
    {
        Task<BaseResult<Event, EventFaults>> CreateEvent(object oldValue, object newValue, Func<FieldCompareAttribute, string, Func<object, Task<string>>> formatter = null);
    }
}
