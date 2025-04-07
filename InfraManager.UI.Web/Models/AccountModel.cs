using System;

namespace InfraManager.UI.Web.Models
{
    public class AccountModel
    {
        public string UserFullName { get; set; }
        public string UserPositionName { get; set; }
        public string UserID { get; set; }
        public string UserAgent { get; set; }

        public string CultureName { get; set; }
        public bool IsReSignAvailable { get; set; }

        public string ViewNameSD { get; set; }
        public string ViewNameAsset { get; set; }
        public string ViewNameFinance { get; set; }

        public bool? IncomingCallProcessing { get; set; }
        public bool TimeManagementEnabled { get; set; }
        public bool BudgetEnabled { get; set; }
        public bool WebMobileEnabled { get; set; }

        public bool HasRoles { get; set; }
        public bool HasAdminRole { get; set; }
        public int[] GrantedOperations { get; set; }

        public string AssetFiltrationObjectID { get; set; }
        public int AssetFiltrationObjectClassID { get; set; }
        public string AssetFiltrationObjectName { get; set; }
        public byte AssetFiltrationTreeType { get; set; }
        public string AssetFiltrationField { get; set; }

        public bool UseCompactMenuOnly { get; set; }

        public Guid? FinanceBudgetID { get; set; }

        public bool ListView_GridLines { get; set; }
        public bool ListView_CompactMode { get; set; }
        public bool ListView_Multicolor { get; set; }
        
        public bool IsDemo { get; set; }
    }
}
