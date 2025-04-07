using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.Parameters
{
    public class ParameterEnum
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public byte[] RowVersion { get; init; }
        public bool IsTree { get; init; }

        public virtual ICollection<ParameterEnumValue> ParameterEnumValues { get; private set; }
    }
}
