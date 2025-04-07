using System;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.LifeCycleCommands;
public interface IAssignValueForComponentsStrategy
{
    Task<LifeCycleCommandAlertDetails> AssignValueForComponentsByDeviceIDAsync(Guid id, LifeCycleCommandAlertData alertData);
}
