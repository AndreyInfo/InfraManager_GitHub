using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings
{
    public class SupportSettingsProblemsModel
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public bool UpdateComposition { get; set; }
        public bool Warn { get; set; }
        public string DefaultProblemWorkflowSchemeIdentifier { get; set; }
        public long Ticks { get; set; }
    }
}
