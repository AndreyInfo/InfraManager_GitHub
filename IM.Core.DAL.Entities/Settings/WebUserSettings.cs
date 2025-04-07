using Newtonsoft.Json;
using System;

namespace InfraManager.DAL.Settings
{
    /// <summary>
    /// Этот класс представляет настройки пользователя в Web-приложении
    /// </summary>
    public class WebUserSettings
    {
        protected WebUserSettings()
        {
        }

        public WebUserSettings(Guid userID)
        {
            UserID = userID;
            AssetFiltrationData = string.Empty;
        }

        public Guid UserID { get; private set; }
        public string CultureName { get; set; }
        public string ViewNameSD { get; set; }
        public bool IncomingCallProcessing { get; set; }
        public byte TimeSheetFilter { get; set; }
        public string ViewNameAsset { get; set; }
        public string AssetFiltrationData { get; set; }
        public bool UseCompactMenuOnly { get; set; }
        public string ViewNameFinance { get; set; }
        public Guid? FinanceBudgetID { get; set; }
        public bool ListViewGridLines { get; set; }
        public bool ListViewCompactMode { get; set; }
        public bool ListViewMulticolor { get; set; }

        #region Tree settings

        private UserTreeSettings _treeSettings = null;
        public UserTreeSettings GetTreeSettings() => _treeSettings =
            _treeSettings 
                ?? (string.IsNullOrWhiteSpace(AssetFiltrationData) || AssetFiltrationData.Equals("null") //TODO Empty AssetFiltrationData should be null not "null"
            ? new UserTreeSettings()
            : JsonConvert.DeserializeObject<UserTreeSettings>(AssetFiltrationData));

        public void SetTreeSettings(UserTreeSettings treeSettings)
        {
            AssetFiltrationData = treeSettings == null
                ? string.Empty
                : JsonConvert.SerializeObject(treeSettings);
            _treeSettings = treeSettings ?? new UserTreeSettings();
        }

        public void SetTreeSettings(
            Guid? objectID, 
            int objectClassID,
            string objectName, 
            byte type, 
            string field)
        {
            SetTreeSettings(
                new UserTreeSettings
                {
                    FiltrationField = field,
                    FiltrationObjectClassID = objectClassID,
                    FiltrationObjectID = objectID,
                    FiltrationObjectName = objectName,
                    FiltrationTreeType = type
                });
        }

        #endregion
    }
}
