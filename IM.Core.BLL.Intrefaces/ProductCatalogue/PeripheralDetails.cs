using System;

namespace InfraManager.BLL.ProductCatalogue
{
    public class PeripheralDetails
    {
        public Guid ID { get; init; }
        public Guid PeripheralTypeID { get; init; }
        public int? TerminalDeviceID { get; init; }
        public int? NetworkDeviceID { get; init; }
        public string Name { get; init; }
        public string Serial_no { get; init; }
        public string Note { get; init; }
        public int IntID { get; init; }
        public int StateID { get; init; }
        public int RoomID { get; init; }
        public decimal? BwLoad { get; init; }
        public decimal? ColorLoad { get; init; }
        public decimal? FotoLoad { get; init; }
        public ObjectClass? ClassID { get; private init; }
        public byte[] RowVersion { get; init; }
        public Guid? PeripheralDatabaseID { get; init; }
        public Guid? ComplementaryID { get; init; }
        public int? ComplementaryIntID { get; init; }
        public string Code { get; init; }
        public string ExternalID { get; init; }
    }
}
