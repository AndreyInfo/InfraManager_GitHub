using System;

namespace InfraManager.DAL.Settings
{
    public class UserTreeSettings
    {
        public Guid? FiltrationObjectID { get; set; }
        public int FiltrationObjectClassID { get; set; }
        public string FiltrationObjectName { get; set; }
        public byte FiltrationTreeType { get; set; }
        public string FiltrationField { get; set; }
    }
}
