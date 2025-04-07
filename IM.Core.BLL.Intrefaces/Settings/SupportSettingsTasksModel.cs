using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings
{
    public class SupportSettingsTasksModel
    {
        public string DefaultWorkOrderWorkflowSchemeIdentifier { get; set; }
        public bool Warn { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        
    }
}
