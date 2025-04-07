using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Parameters
{
    public class ParameterEnumValuesData
    {
        public ParameterEnumValueData Parent { get; set; }
        public List<ParameterEnumValuesData> Childrens { get; set; }
    }
}
