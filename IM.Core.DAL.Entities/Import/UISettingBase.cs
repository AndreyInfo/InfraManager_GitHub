using System;


namespace InfraManager.DAL.Import
{
    public class UISettingBase
    {
        public UISettingBase()
        {}
        
        public UISettingBase(Guid iD)
        {
            ID = iD;
        }

        public Guid ID { get; init; }

        public bool Removed { get; private set; }
        public void MarkForDelete()
        {
            Removed = true;
        }
    }
}
