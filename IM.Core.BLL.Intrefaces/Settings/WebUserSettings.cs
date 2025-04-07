using System;

namespace InfraManager.BLL.Settings
{
    public class WebUserSettings
    {
        public string CultureName { get; set; }
        public string ViewNameSD { get; set; }
        public string ViewNameAsset { get; set; }
        public string ViewNameFinance { get; set; }
        public bool? IncomingCallProcessing { get; set; } 
        public string AssetFiltrationObjectID { get; set; }
        public int? AssetFiltrationObjectClassID { get; set; }
        public string AssetFiltrationObjectName { get; set; }
        public byte? AssetFiltrationTreeType { get; set; }
        public string AssetFiltrationField { get; set; }
        public bool? UseCompactMenuOnly { get; set; }
        public Guid? FinanceBudgetID { get; set; }
        public ListViewUserSettings ListView { get; set; }
        public bool IsDefault { get; set; }
    }
}
