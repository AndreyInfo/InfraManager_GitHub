using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Quality
{
    public class QualityControl
    {
        public Guid ID { get; init; }
        public DateTime UtcDate { get; set; }
        public QualityControlType Type { get; set; }
        public virtual Call Call { get; set; }
        public Guid CallID { get; set; }
        public int TimeSpanInMinutes { get; set; }
        public int TimeSpanInWorkMinutes { get; set; }
    }
}
