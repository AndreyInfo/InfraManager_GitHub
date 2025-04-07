using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Messages
{
    public class MessageData
    {
        public Guid ID { get; init; }
        public string UtcDateRegistered { get; init; }
        public string EntityStateID { get; init; }
        public Guid? WorkflowSchemeID { get; init; }
        public string WorkflowSchemeVersion { get; init; }
        public string WorkflowSchemeIdentifier { get; init; }
        public string EntityStateName { get; init; }
        public string UtcDateClosed { get; init; }

        public byte Type { get; init; }
        public int Count { get; init; }
        public byte SeverityType { get; init; }
    }
}
