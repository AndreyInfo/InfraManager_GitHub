using System;

namespace InfraManager.Core
{
    public static class Types
    {
        #region InfraManager.Core
        public const string BusinessRoleTypeName = "InfraManager.Core.BusinessRole, InfraManager, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type BusinessRoleType = Type.GetType(BusinessRoleTypeName);

        public const string DataSourceTypeName = "InfraManager.Core.Data.DataSource, InfraManager.DAL, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type DataSourceType = Type.GetType(DataSourceTypeName);

        public const string ConfSysTypeName = "InfraManager.BLL.ConfSys, IM.Core.BLL, Version=1.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ConfSysType = Type.GetType(ConfSysTypeName);
        #endregion

        #region InfraManager.IM.BusinessLayer

        #region configuration
#if Configuration
        public const string AssetServiceTypeName = "InfraManager.IM.BusinessLayer.Asset.AssetService, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type AssetServiceType = Type.GetType(AssetServiceTypeName);

        public const string AdapterTypeName = "InfraManager.IM.BusinessLayer.Configuration.Adapter, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type AdapterType = Type.GetType(AdapterTypeName);

        public const string ProductCatalogTemplateTypeName = "InfraManager.IM.BusinessLayer.ProductCatalogue.ProductCatalogTemplate, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ProductCatalogTemplateType = Type.GetType(ProductCatalogTemplateTypeName);

        public const string NetworkDeviceTypeName = "InfraManager.IM.BusinessLayer.Configuration.NetworkDevice, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type NetworkDeviceType = Type.GetType(NetworkDeviceTypeName);

        public const string PeripheralTypeName = "InfraManager.IM.BusinessLayer.Configuration.Peripheral, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type PeripheralType = Type.GetType(PeripheralTypeName);

        public const string TerminalDeviceTypeName = "InfraManager.IM.BusinessLayer.Configuration.TerminalDevice, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type TerminalDeviceType = Type.GetType(TerminalDeviceTypeName);

        public const string AssetNumberTypeName = "InfraManager.IM.BusinessLayer.Asset.AssetNumber, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type AssetNumberType = Type.GetType(AssetNumberTypeName);

        public const string AssetFieldsTypeName = "InfraManager.IM.BusinessLayer.Asset.AssetFields, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type AssetFieldsType = Type.GetType(AssetFieldsTypeName);

        public const string ConfigurationUnitTypeName = "InfraManager.IM.BusinessLayer.Configuration.ConfigurationUnit, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ConfigurationUnitType = Type.GetType(ConfigurationUnitTypeName);

        public const string DataEntityTypeName = "InfraManager.IM.BusinessLayer.ConfigurationData.DataEntity, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type DataEntityType = Type.GetType(DataEntityTypeName);
#endif
#if Barcode
        public const string BarcodeWrapperTypeName = "InfraManager.Barcode.WinUI.BarcodeWrapper, InfraManager.Barcode.WinUI, Version=1.0.0.0, Culture=neutral, processorArchitecture=MSIL";
#endif
        #endregion

        public const string UserTypeName = "InfraManager.IM.BusinessLayer.User, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type UserType = Type.GetType(UserTypeName);

        public const string RoleTypeName = "InfraManager.IM.BusinessLayer.Role.Role, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type RoleType = Type.GetType(RoleTypeName);

        public const string DocumentTypeName = "InfraManager.IM.BusinessLayer.Repository.Document, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type DocumentType = Type.GetType(DocumentTypeName);

        public const string DocumentListTypeName = "InfraManager.IM.BusinessLayer.Repository.DocumentList, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type DocumentListType = Type.GetType(DocumentListTypeName);

        public const string OrganizationTypeName = "InfraManager.IM.BusinessLayer.Organization.Organization, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type OrganizationType = Type.GetType(OrganizationTypeName);

        public const string SubdivisionTypeName = "InfraManager.IM.BusinessLayer.OrganizationStructure.Subdivision, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type SubdivisionType = Type.GetType(SubdivisionTypeName);

        public const string CalendarHelperTypeName = "InfraManager.IM.BusinessLayer.Calendar.CalendarHelper, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type CalendarHelperType = Type.GetType(CalendarHelperTypeName);

        private const string CalendarTimeZoneTypeName = "InfraManager.IM.BusinessLayer.Calendar.CalendarTimeZone, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type CalendarTimeZoneType = Type.GetType(CalendarTimeZoneTypeName);

        #region locations
        public const string StorageLocationTypeName = "InfraManager.IM.BusinessLayer.Location.StorageLocation, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type StorageLocationType = Type.GetType(StorageLocationTypeName);

        public const string WorkplaceTypeName = "InfraManager.IM.BusinessLayer.Location.Workplace, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type WorkplaceType = Type.GetType(WorkplaceTypeName);

        public const string BuildingTypeName = "InfraManager.IM.BusinessLayer.Location.Building, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type BuildingType = Type.GetType(BuildingTypeName);

        public const string FloorTypeName = "InfraManager.IM.BusinessLayer.Location.Floor, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type FloorType = Type.GetType(FloorTypeName);

        public const string RackTypeName = "InfraManager.IM.BusinessLayer.Location.Rack, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type RackType = Type.GetType(RackTypeName);

        public const string RoomTypeName = "InfraManager.IM.BusinessLayer.Location.Room, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type RoomType = Type.GetType(RoomTypeName);
        #endregion

#if Software
        public const string SoftwareInstallationTypeName = "InfraManager.IM.BusinessLayer.Software.SoftwareInstallation, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type SoftwareInstallationType = Type.GetType(SoftwareInstallationTypeName);

        public const string SoftwareLicenceTypeName = "InfraManager.IM.BusinessLayer.Software.SoftwareLicence, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type SoftwareLicenceType = Type.GetType(SoftwareLicenceTypeName);

        public const string SoftwareModelTypeName = "InfraManager.IM.BusinessLayer.Software.SoftwareModel, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type SoftwareModelType = Type.GetType(SoftwareModelTypeName);

        public const string SoftwareModelSupportLineResponsibleTypeName = "InfraManager.IM.BusinessLayer.Software.SupportLineResponsible, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type SoftwareModelSupportLineResponsibleType = Type.GetType(SoftwareModelSupportLineResponsibleTypeName);
#endif
        public const string MessageByTypeName = "InfraManager.IM.BusinessLayer.Messages.Message, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type MessageByType = Type.GetType(MessageByTypeName);

        public const string MessageByEmailTypeName = "InfraManager.IM.BusinessLayer.Messages.MessageByEmail, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type MessageByEmailType = Type.GetType(MessageByEmailTypeName);
#if Configuration
        public const string MessageByInquiryTypeName = "InfraManager.IM.BusinessLayer.Messages.MessageByInquiry, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type MessageByInquiryType = Type.GetType(MessageByInquiryTypeName);

        public const string MessageByInquiryTaskForAssetTypeName = @"InfraManager.IM.BusinessLayer.Messages.MessageByInquiryTaskForAsset, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type MessageByInquiryTaskForAssetType = Type.GetType(MessageByInquiryTaskForAssetTypeName);

#if Monitoring
        public const string MessageByMonitoringTypeName = "InfraManager.IM.BusinessLayer.Messages.MessageByMonitoring, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type MessageByMonitoringType = Type.GetType(MessageByMonitoringTypeName);
#endif
#endif

        //public const string MessageByIntegrationTypeName = "InfraManager.IM.BusinessLayer.Messages.MessageByIntegration, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        //public static readonly Type MessageByIntegrationType = Type.GetType(MessageByIntegrationTypeName);

        public const string MessageByOrganizationStructureImportTypeName = "InfraManager.IM.BusinessLayer.Messages.MessageByOrganizationStructureImport, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type MessageByOrganizationStructureImportType = Type.GetType(MessageByOrganizationStructureImportTypeName);

        public const string MessageByInquiryTaskForUsersTypeName = @"InfraManager.IM.BusinessLayer.Messages.MessageByInquiryTaskForUsers, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type MessageByInquiryTaskForUsersType = Type.GetType(MessageByInquiryTaskForUsersTypeName);

        public static readonly string ServiceCenterTypeName = "InfraManager.IM.BusinessLayer.Asset.ServiceCenter, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ServiceCenterType = Type.GetType(ServiceCenterTypeName);

        public static readonly string ServiceContractTypeName = "InfraManager.IM.BusinessLayer.Asset.ServiceContract, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ServiceContractType = Type.GetType(ServiceContractTypeName);

        public static readonly string ServiceContractAgreementTypeName = "InfraManager.IM.BusinessLayer.Asset.ServiceContractAgreement, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ServiceContractAgreementType = Type.GetType(ServiceContractAgreementTypeName);

        public static readonly string SupplierTypeName = "InfraManager.IM.BusinessLayer.Asset.Supplier, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type SupplierType = Type.GetType(SupplierTypeName);

        public static readonly string MaterialTypeName = "InfraManager.IM.BusinessLayer.Materials.Material, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type MaterialType = Type.GetType(MaterialTypeName);
        #endregion

#if ServiceDesk
        #region InfraManager.SD.BusinessLayer
        public const string DependencyObjectTypeName = "InfraManager.SD.BusinessLayer.DependencyObject, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type DependencyObjectType = Type.GetType(DependencyObjectTypeName);

        public const string ExecutorAssignmentTypeTypeName = "InfraManager.SD.BusinessLayer.ExecutorAssignmentType, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ExecutorAssignmentTypeType = Type.GetType(ExecutorAssignmentTypeTypeName);

        public const string InfluenceTypeName = "InfraManager.SD.BusinessLayer.Influence, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type InfluenceType = Type.GetType(InfluenceTypeName);

        public const string ObjectNoteInfoTypeName = "InfraManager.SD.BusinessLayer.ObjectNoteInfo, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ObjectNoteInfoType = Type.GetType(ObjectNoteInfoTypeName);

        public const string PriorityTypeName = "InfraManager.SD.BusinessLayer.Priority, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type PriorityType = Type.GetType(PriorityTypeName);

        public const string PriorityMatrixTypeName = "InfraManager.SD.BusinessLayer.PriorityMatrix, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type PriorityMatrixType = Type.GetType(PriorityMatrixTypeName);

        public const string QueueTypeName = "InfraManager.SD.BusinessLayer.Queue, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type QueueType = Type.GetType(QueueTypeName);

        public const string UrgencyTypeName = "InfraManager.SD.BusinessLayer.Urgency, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type UrgencyType = Type.GetType(UrgencyTypeName);
        
        public const string TechnicalFailuresCategoryTypeName = "InfraManager.BLL.SD.EntityProviders.TechnicalFailuresCategory, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type TechnicalFailuresCategoryType = Type.GetType(TechnicalFailuresCategoryTypeName); 

        #region calls
        public const string CallTypeName = "InfraManager.SD.BusinessLayer.Calls.Call, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type CallType = Type.GetType(CallTypeName);

        public const string CallReceiptTypeTypeName = "InfraManager.SD.BusinessLayer.Calls.CallReceiptType, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type CallReceiptTypeType = Type.GetType(CallReceiptTypeTypeName);

        public const string CallSummaryTypeName = "InfraManager.SD.BusinessLayer.Calls.CallSummary, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type CallSummaryType = Type.GetType(CallSummaryTypeName);

        public const string CallTypeTypeName = "InfraManager.SD.BusinessLayer.Calls.CallType, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type CallTypeType = Type.GetType(CallTypeTypeName);

        public const string IncidentResultTypeName = "InfraManager.SD.BusinessLayer.Calls.IncidentResult, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type IncidentResultType = Type.GetType(IncidentResultTypeName);

        public const string RFSResultTypeName = "InfraManager.SD.BusinessLayer.Calls.RFSResult, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type RFSResultType = Type.GetType(RFSResultTypeName);

        public const string CallPromiseDateCalculationModeTypeName = "InfraManager.SD.BusinessLayer.Calls.CallPromiseDateCalculationMode, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type CallPromiseDateCalculationModeType = Type.GetType(CallPromiseDateCalculationModeTypeName);
        #endregion

        #region customcontrol
        public const string CustomControlTypeName = "InfraManager.SD.BusinessLayer.CustomController.CustomController, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type CustomControlType = Type.GetType(CustomControlTypeName);
        #endregion

        #region substitution
        public const string SubstitutionTypeName = "InfraManager.SD.BusinessLayer.Substitution.Substitution, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type SubstitutionType = Type.GetType(SubstitutionTypeName);
        #endregion

        #region negotiation
        public const string NegotiationTypeName = "InfraManager.SD.BusinessLayer.Negotiation, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type NegotiationType = Type.GetType(NegotiationTypeName);

        public const string NegotiationStatusTypeName = "InfraManager.SD.BusinessLayer.NegotiationStatus, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type NegotiationStatusType = Type.GetType(NegotiationStatusTypeName);

        public const string NegotiationUserTypeName = "InfraManager.SD.BusinessLayer.NegotiationUser, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type NegotiationUserType = Type.GetType(NegotiationUserTypeName);

        public const string VotingTypeTypeName = "InfraManager.SD.BusinessLayer.VotingType, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type VotingTypeType = Type.GetType(VotingTypeTypeName);
        #endregion

        #region parameters
        public const string ParameterValueTypeName = "InfraManager.IM.BusinessLayer.Parameters.ParameterValue, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ParameterValueType = Type.GetType(ParameterValueTypeName);

        public const string BinaryValueTypeName = "InfraManager.IM.BusinessLayer.Parameters.BinaryValue, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type BinaryValueType = Type.GetType(BinaryValueTypeName);

        public const string ParameterEnumValueTypeName = "InfraManager.IM.BusinessLayer.Parameters.ParameterEnumValue, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ParameterEnumValueType = Type.GetType(ParameterEnumValueTypeName);

        public const string ParameterEnumTypeName = "InfraManager.IM.BusinessLayer.Parameters.ParameterEnum, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ParameterEnumType = Type.GetType(ParameterEnumTypeName);

        #region filters
        public const string ParameterCustomDictionaryFilterTypeName = "InfraManager.IM.BusinessLayer.Parameters.Filters.CustomDictionaryFilter, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ParameterCustomDictionaryFilterType = Type.GetType(ParameterCustomDictionaryFilterTypeName);

        public const string ParameterConfigurationObjectFilterTypeName = "InfraManager.IM.BusinessLayer.Parameters.Filters.ConfigurationObjectFilter, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ParameterConfigurationObjectFilterType = Type.GetType(ParameterConfigurationObjectFilterTypeName);

        public const string ParameterDateTimeFilterTypeName = "InfraManager.IM.BusinessLayer.Parameters.Filters.DateTimeFilter, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ParameterDateTimeFilterType = Type.GetType(ParameterDateTimeFilterTypeName);

        public const string ParameterLocationFilterTypeName = "InfraManager.IM.BusinessLayer.Parameters.Filters.LocationFilter, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ParameterLocationFilterType = Type.GetType(ParameterLocationFilterTypeName);

        public const string ParameterModelFilterTypeName = "InfraManager.IM.BusinessLayer.Parameters.Filters.ModelFilter, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ParameterModelFilterType = Type.GetType(ParameterModelFilterTypeName);

        public const string ParameterNumberFilterTypeName = "InfraManager.IM.BusinessLayer.Parameters.Filters.NumberFilter, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ParameterNumberFilterType = Type.GetType(ParameterNumberFilterTypeName);

        public const string ParameterPositionFilterTypeName = "InfraManager.IM.BusinessLayer.Parameters.Filters.PositionFilter, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ParameterPositionFilterType = Type.GetType(ParameterPositionFilterTypeName);

        public const string ParameterStringFilterTypeName = "InfraManager.IM.BusinessLayer.Parameters.Filters.StringFilter, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ParameterStringFilterType = Type.GetType(ParameterStringFilterTypeName);

        public const string ParameterSubdivisionFilterTypeName = "InfraManager.IM.BusinessLayer.Parameters.Filters.SubdivisionFilter, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ParameterSubdivisionFilterType = Type.GetType(ParameterSubdivisionFilterTypeName);

        public const string ParameterUserFilterTypeName = "InfraManager.IM.BusinessLayer.Parameters.Filters.UserFilter, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ParameterUserFilterType = Type.GetType(ParameterUserFilterTypeName);

        public const string ParameterTableFilterTypeName = "InfraManager.IM.BusinessLayer.Parameters.Filters.TableFilter, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ParameterTableFilterType = Type.GetType(ParameterTableFilterTypeName);

        public const string ParameterDataTypeName = "InfraManager.IM.BusinessLayer.Parameters.ParameterData, InfraManager.BLL.IM, Version=6.0.0.0, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ParameterDataType = Type.GetType(ParameterDataTypeName);

        #endregion
        #endregion

        #region problems
        public const string ProblemTypeName = "InfraManager.SD.BusinessLayer.Problems.Problem, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ProblemType = Type.GetType(ProblemTypeName);

        public const string ProblemCauseTypeName = "InfraManager.SD.BusinessLayer.Problems.ProblemCause, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ProblemCauseType = Type.GetType(ProblemCauseTypeName);

        public const string ProblemTypeTypeName = "InfraManager.SD.BusinessLayer.Problems.ProblemType, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ProblemTypeType = Type.GetType(ProblemTypeTypeName);
        #endregion

        #region rfc
        public const string RFCTypeName = "InfraManager.SD.BusinessLayer.RFC.RFC, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type RFCType = Type.GetType(RFCTypeName);

        public const string RFCTypeTypeName = "InfraManager.SD.BusinessLayer.RFC.RFCType, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type RFCTypeType = Type.GetType(RFCTypeTypeName);
        #endregion

        #region serviceCatalogue
        public const string AttendanceTypeTypeName = "InfraManager.SD.BusinessLayer.ServiceCatalogue.AttendanceType, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type AttendanceTypeType = Type.GetType(AttendanceTypeTypeName);

        public const string ServiceTypeName = "InfraManager.SD.BusinessLayer.ServiceCatalogue.Service, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ServiceType = Type.GetType(ServiceTypeName);

        public const string ServiceAttendanceTypeName = "InfraManager.SD.BusinessLayer.ServiceCatalogue.ServiceAttendance, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ServiceAttendanceType = Type.GetType(ServiceAttendanceTypeName);

        public const string ServiceItemTypeName = "InfraManager.SD.BusinessLayer.ServiceCatalogue.ServiceItem, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ServiceItemType = Type.GetType(ServiceItemTypeName);

        public const string SLATypeName = "InfraManager.SD.BusinessLayer.ServiceCatalogue.SLA, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type SLAType = Type.GetType(SLATypeName);

        public const string SLAListTypeName = "InfraManager.SD.BusinessLayer.ServiceCatalogue.SLAList, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type SLAListType = Type.GetType(SLAListTypeName);

        public const string RuleTypeName = "InfraManager.SD.BusinessLayer.ServiceCatalogue.Rules.Rule, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type RuleType = Type.GetType(RuleTypeName);
        #endregion

        #region workOrders
        public const string WorkOrderTypeName = "InfraManager.SD.BusinessLayer.WorkOrders.WorkOrder, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type WorkOrderType = Type.GetType(WorkOrderTypeName);

        public const string WorkOrderPriorityTypeName = "InfraManager.SD.BusinessLayer.WorkOrders.WorkOrderPriority, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type WorkOrderPriorityType = Type.GetType(WorkOrderPriorityTypeName);

        public const string WorkOrderTemplateTypeName = "InfraManager.SD.BusinessLayer.WorkOrders.WorkOrderTemplate, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type WorkOrderTemplateType = Type.GetType(WorkOrderTemplateTypeName);

        public const string WorkOrderTypeTypeName = "InfraManager.SD.BusinessLayer.WorkOrders.WorkOrderType, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type WorkOrderTypeType = Type.GetType(WorkOrderTypeTypeName);

        public const string ActivesRequestSpecificationTypeName = "InfraManager.SD.BusinessLayer.WorkOrders.ActivesRequestSpecification, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type ActivesRequestSpecificationType = Type.GetType(ActivesRequestSpecificationTypeName);

        public const string GoodsInvoiceTypeName = "InfraManager.SD.BusinessLayer.WorkOrders.GoodsInvoice, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type GoodsInvoiceType = Type.GetType(GoodsInvoiceTypeName);

        public const string GoodsInvoiceSpecificationTypeName = "InfraManager.SD.BusinessLayer.WorkOrders.GoodsInvoiceSpecification, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type GoodsInvoiceSpecificationType = Type.GetType(GoodsInvoiceSpecificationTypeName);

        public const string GoodsInvoiceSpecificationDependencyPurchaseTypeName = "InfraManager.SD.BusinessLayer.WorkOrders.GoodsInvoiceSpecificationDependencyPurchase, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type GoodsInvoiceSpecificationDependencyPurchaseType = Type.GetType(GoodsInvoiceSpecificationDependencyPurchaseTypeName);

        public const string GoodsInvoiceSpecificationAssetReferenceTypeName = "InfraManager.SD.BusinessLayer.WorkOrders.GoodsInvoiceSpecificationAssetReference, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type GoodsInvoiceSpecificationAssetReferenceType = Type.GetType(GoodsInvoiceSpecificationAssetReferenceTypeName);

        public const string PurchaseSpecificationTypeName = "InfraManager.SD.BusinessLayer.WorkOrders.PurchaseSpecification, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type PurchaseSpecificationType = Type.GetType(PurchaseSpecificationTypeName);

        public const string PurchaseSpecificationDependencyActiveRequestsTypeName = "InfraManager.SD.BusinessLayer.WorkOrders.PurchaseSpecificationDependencyActiveRequests, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type PurchaseSpecificationDependencyActiveRequestsType = Type.GetType(PurchaseSpecificationDependencyActiveRequestsTypeName);

        public const string WorkOrderFinanceActivesRequestTypeName = "InfraManager.SD.BusinessLayer.WorkOrders.WorkOrderFinanceActivesRequest, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type WorkOrderFinanceActivesRequestType = Type.GetType(WorkOrderFinanceActivesRequestTypeName);

        public const string WorkOrderFinancePurchaseTypeName = "InfraManager.SD.BusinessLayer.WorkOrders.WorkOrderFinancePurchase, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type WorkOrderFinancePurchaseType = Type.GetType(WorkOrderFinancePurchaseTypeName);

        public const string PurchaseSpecificationDependencyFinanceBudgetRowTypeName = "InfraManager.SD.BusinessLayer.WorkOrders.PurchaseSpecificationDependencyFinanceBudgetRow, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type PurchaseSpecificationDependencyFinanceBudgetRowType = Type.GetType(PurchaseSpecificationDependencyFinanceBudgetRowTypeName);

        public const string FinanceBudgetRowTypeName = "InfraManager.SD.BusinessLayer.Finance.FinanceBudgetRow, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type FinanceBudgetRowType = Type.GetType(FinanceBudgetRowTypeName);

        public const string FinanceBudgetRowDependencyActiveRequestTypeName = "InfraManager.SD.BusinessLayer.Finance.FinanceBudgetRowDependencyActiveRequest, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type FinanceBudgetRowDependencyActiveRequestType = Type.GetType(FinanceBudgetRowDependencyActiveRequestTypeName);
        #endregion

        #region SDNote
        public const string SDNoteTypeName = "InfraManager.SD.BusinessLayer.SDNote, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type SDNoteType = Type.GetType(SDNoteTypeName);

        public const string SDNoteTypeTypeName = "InfraManager.SD.BusinessLayer.SDNoteType, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type SDNoteTypeType = Type.GetType(SDNoteTypeTypeName);
        #endregion

        #region MassIncident

        private const string MassIncidentTypeName = "InfraManager.BLL.SD.EntityProviders.EntityProviders.MassIncident, InfraManager.BLL.SD, Culture=neutral, processorArchitecture=MSIL";
        public static readonly Type MassIncidentType = Type.GetType(MassIncidentTypeName);

        #endregion

        #endregion
#endif
    }
}