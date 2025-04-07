using System;

namespace InfraManager.BLL.Report
{
    /// <summary>
    /// Модель папки отчетов для получения из бд
    /// </summary>
    public class ReportFolderDetails
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public string Note { get; init; }
        public Guid ReportFolderID { get; init; }
        public byte SecurityLevel { get; init; }
    }
}
