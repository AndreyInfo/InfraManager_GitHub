using System;

namespace InfraManager.BLL
{
    public class ListTreeFilter
    {
        public Guid? FiltrationObjectID { get; set; }
        public int FiltrationObjectClassID { get; set; }
        public string FiltrationObjectName { get; set; }
        public byte FiltrationTreeType { get; set; }
        public string FiltrationField { get; set; }
    }
}
