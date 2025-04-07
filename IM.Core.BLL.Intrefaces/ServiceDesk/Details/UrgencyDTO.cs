using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.DTOs
{
    public class UrgencyDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
