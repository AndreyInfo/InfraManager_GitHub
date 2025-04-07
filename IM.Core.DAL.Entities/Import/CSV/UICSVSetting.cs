using System;

namespace InfraManager.DAL.Import.CSV
{
    public class UICSVSetting : UISettingBase, IMarkableForDelete
    {
        public UICSVSetting(Guid iD, Guid? cSVConfigurationID, string path = "") : base(iD)
        {
            ID = iD;
            CSVConfigurationID = cSVConfigurationID;
            Path = path;
        }


        public Guid? CSVConfigurationID { get; set; }
        public string Path { get; set; }

        public virtual UICSVConfiguration Configuration { get; set; }
    }
}
