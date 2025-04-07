using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InfraManager.BLL.Settings
{
    public class SupportSettingsGeneralModel
    {
        public int NumberSupportLines { get; set; }
        public int NumberSearchResults { get; set; }
        public bool UseDB { get; set; }
        public bool UseTTZ { get; set; }
        public bool AllowAdd { get; set; }
        public bool LaborCosts { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PeriodType ReportingPeriod { get; set; }
        public bool AutoChange { get; set; }
    }
}
