using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.Parameters
{
    public class ParameterEnumValue
    {
        public Guid ID { get; init; }
        public Guid ParameterEnumID { get; init; }
        public int OrderPosition { get; set; }
        public string Value { get; set; }
        public Guid? ParentID { get; init; }

        public virtual ParameterEnumValue Parent { get; init; }
        public virtual ICollection<ParameterEnumValue> ParameterEnums { get; private set; }
        public virtual ParameterEnum ParameterEnum { get; init; }
    }
}
