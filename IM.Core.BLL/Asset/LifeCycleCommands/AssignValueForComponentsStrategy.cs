using System;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.LifeCycleCommands;
internal class AssignValueForComponentsStrategy : IAssignValueForComponentsStrategy
{
    public Task<LifeCycleCommandAlertDetails> AssignValueForComponentsByDeviceIDAsync(Guid id, LifeCycleCommandAlertData alertData)
    {
        throw new NotImplementedException();
    }
}
