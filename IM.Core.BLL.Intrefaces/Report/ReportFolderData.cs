using System;

namespace InfraManager.BLL.Report
{
    /// <summary>
    /// Модель папки отчетов для добаления/изменения в бд
    /// </summary>
    public class ReportFolderData
    {
        public string Name { get; init; }
        public string Note { get; init; }
        public Guid ReportFolderID { get; init; }
        public byte SecurityLevel { get; init; }
    }
}
