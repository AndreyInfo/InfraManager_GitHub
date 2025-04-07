using System.Collections.Generic;

namespace InfraManager.BLL.Settings
{

    public class MessageProcessingRule
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public string WorkflowSchemeIdentifier { get; set; }
        public List<Condition> Conditions { get; set; }
    }

    public class Condition
    {
        public string Key { get; set; }
        public string Parameter { get; set; }
    }
}
