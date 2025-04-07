using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    public interface IHaveUtcModifiedDate
    {
        public DateTime UtcDateModified { get; set; }
    }
}
