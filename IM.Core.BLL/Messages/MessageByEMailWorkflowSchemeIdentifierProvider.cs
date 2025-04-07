using InfraManager.BLL.Workflow;
using InfraManager.DAL.Message;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Messages
{
    public class MessageByEMailWorkflowSchemeIdentifierProvider :
        ISelectWorkflowScheme<MessageByEmail>,
        ISelfRegisteredService<ISelectWorkflowScheme<MessageByEmail>>
    {
        public Task<string> SelectIdentifierAsync(MessageByEmail data, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(data.WorkflowSchemeIdentifier);
        }
    }
}
