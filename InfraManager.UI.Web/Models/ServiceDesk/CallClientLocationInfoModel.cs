using System;

namespace InfraManager.UI.Web.Models.ServiceDesk
{
    public class CallClientLocationInfoModel
    {
        public Guid? PlaceID { get; init; }

        public string PlaceName { get; init; }

        public string PlaceFullName { get; set; }

        public int? PlaceIntID { get; init; }

        public int? PlaceClassID { get; init; }
    }
}
