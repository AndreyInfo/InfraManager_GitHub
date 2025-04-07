namespace InfraManager.CrossPlatform.WebApi.Contracts
{
    public static class Constants
    {
        public const string UserGuidHeader = "x-user-guid";

        public const string UserGuidClaim = "UserGuidClaim";

        public const string UserDeviceIDHeader = "x-device-fingerprint";

        public const string CorrelationIDHeader = "x-correlation-id";

        public const int WebApiRequestEventId = 1000;

        public const int WebApiErrorEventId = 1001;

        public const int WebApiResponseEventId = 1002;

        // TODO: Дублирует enum ObjectClass
        #region ClassIDs

        public const int UndisposedDevice = 666;
        public const int ActiveDevice = 5;
        public const int TerminalDevice = 6;
        public const int LicenceSchema = 750;
        public const int ELPSetting = 820;
        public const int Installation = 71;

        #endregion
    }
}
