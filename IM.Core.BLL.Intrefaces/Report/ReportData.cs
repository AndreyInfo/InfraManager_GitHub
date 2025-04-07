using System;

namespace InfraManager.BLL.Report
{
    public class ReportData
    {
        public string Name { get; init; }
        public string Note { get; init; }
        public string Data { get; set; }
        public Guid ReportFolderID { get; init; }
        public byte SecurityLevel { get; init; }
    }
}
