using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Technologies
{
    public class TechnologyCompatibilityNodeData
    {
        public int TechIDFrom { get; set; }
        public virtual TechnologyTypeDetails TechnologyTypeFrom { get; init; }

        public int TechIDTo { get; set; }
        public virtual TechnologyTypeDetails TechnologyTypeTo { get; init; }
    }
}
