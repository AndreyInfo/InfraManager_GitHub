using InfraManager.DAL.ServiceDesk.Quality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Quality
{
    public class QualityControlDetails
    {
        public Guid ID { get; init; }
        public DateTime UtcDate { get; init; }
        public QualityControlType Type { get; init; }
        public Guid CallID { get; init; }
        public int TimeSpanInMinutes { get; init; }
        public int TimeSpanInWorkMinutes { get; init; }

    }
}
