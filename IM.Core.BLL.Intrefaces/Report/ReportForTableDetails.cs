using System;

namespace InfraManager.BLL.Report
{
    public class ReportForTableDetails
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public string Note { get; init; }
        public DateTime DateCreated { get; init; }
        public DateTime DateModified { get; init; }
        public string StringFolder { get; set; }
    }
}
