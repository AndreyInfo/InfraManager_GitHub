using System;

namespace IMSystem
{
    public class IMObject
    {
        string _id = "0";
        int _classID = 0;
        string _name = string.Empty;
        public string ID { get; set; }
        public int ClassID { get; set; }
        public string Name { get; set; }
    }

    public static class Global
    {
        public struct IMObject
        {
            public string id;
            public string ClassID;
            public string Name;
        }

        public const string CurrentUserSlot = "CurrentSlot";
        public const string OBJ_ADMIN = "{00000000-0000-0000-0000-000000000001}";
        public const int NULL_User_ID = 1;
        public const string NULL_Organization_ID = "{00000000-0000-0000-0000-000000000001}";
        public const string NULL_Subdivision_ID = "{00000000-0000-0000-0000-000000000001}";
        public const string NULL_Urgency_ID = "{00000000-0000-0000-0000-000000000000}";
        public const string NULL_Influence_ID = "{00000000-0000-0000-0000-000000000000}";
        public const string NULL_Queue_ID = "{00000000-0000-0000-0000-000000000000}";
        public const int OBJ_VENDOR = 89;
        public const int OBJ_BUILDING = 1;
        public const int OBJ_FLOOR = 2;
        public const int OBJ_ROOM = 3;
        public const int OBJ_RACK = 4;
        public const int OBJ_NETWORKDEVICE = 5;
        public const int OBJ_TERMINALDEVICE = 6;
        public const int OBJ_PANEL = 7;
        public const int OBJ_OUTLET = 8;
        public const int OBJ_USER = 9;
        public const int OBJ_NetworkScheme = 10;
        public const int OBJ_AssetDeviation = 11;
        public const int OBJ_PORT = 13;
        public const int OBJ_PANELJACK = 14;
        public const int OBJ_OUTLETJACK = 15;
        public const int OBJ_CABEL = 16;
        public const int OBJ_CORD = 17;
        public const int OBJ_WORKPLACE = 22;
        public const int OBJ_SOFTWARE_DISTRIBUTION_CENTRE = 23;
        public const int OBJ_SOFTWARE_SUBLICENSE = 24;
        public const int OBJ_ROLE = 28;
        public const int OBJ_OWNER = 29;
        public const int OBJ_CATALOGUE = 30;
        public const int OBJ_ADAPTER = 33;
        public const int OBJ_PERIPHERAL = 34;
        public const int OBJ_SPLITTERJACK = 35;

        public const int OBJ_SoftwareLicenceModel = 38;

        public const int OBJ_DEVICEMODULE = 27;
        public const int OBJ_SPLICE = 21;
        public const int OBJ_INSTALLATIONSOFTWARE = 71;
        public const int OBJ_ASSET = 80;
        public const int OBJ_SOFTWARE = 81;
        public const int OBJ_SOFTWARE_LICENSE = 223;
        public const int OBJ_SOFTWARE_LICENSE_STANDALONE = 183;
        public const int OBJ_SOFTWARE_LICENSE_RENT = 184;
        public const int OBJ_SOFTWARE_LICENSE_UPGRADE = 185;
        public const int OBJ_SOFTWARE_LICENSE_SUBSCRIBE = 186;
        public const int OBJ_SOFTWARE_LICENSE_PROLONGATION = 187;
        public const int OBJ_SOFTWARE_LICENSE_OEM = 189;
        public const int OBJ_ORGANIZATION = 101;
        public const int OBJ_DIVISION = 102;

        public const int OBJ_DOCUMENT_TYPE = 108;
        public const int OBJ_DOCUMENT_FOLDER = 109;
        public const int OBJ_DOCUMENT = 110;

        public const int OBJ_SUPPLIER = 116;
        public const int OBJ_NOTIFICATION = 117;
        public const int OBJ_ACCESS = 118;
        public const int OBJ_ADAPTERMODEL = 95;

        public const int OBJ_SOFTWAREMODEL = 97;
        public const int OBJ_SOFTWARETYPE = 92;
        public const int OBJ_INSTALLATION = 71;

        public const int OBJ_NETWORKDEVICEMODEL = 93;
        public const int OBJ_SOFTWARELICENCESERIALNUMBER = 402;
        public const int OBJ_SOFTWARELICENCEREFERENCE = 403;

        public const int OBJ_TERMINALDEVICEMODEL = 94;

        public const int OBJ_PERIPHERALMODEL = 96;

        public const int OBJ_MATERIAL = 120;
        public const int OBJ_MATERIALMODEL = 107;
        public const int OBJ_MATERIAL_Cartridge = 36;

        public const int OBJ_CALL = 701;
        public const int OBJ_PROBLEM = 702;
        public const int OBJ_RFC = 703;
        public const int OBJ_SD_GENERAL = 704;
        public const int OBJ_WORKORDER = 119;
        public const int OBJ_QUEUE = 722;
        public const int OBJ_MESSAGE = 144;
        public const int OBJ_ServiceCatalog = 127;
        public const int OBJ_Rule = 129;
        public const int OBJ_SLA = 130;

        public const int OBJ_MCR = 804;
        public const int OBJ_PlanMCR = 805;
        public const int OBJ_AllMCR = 806;
        public const int OBJ_TechParamCompTD = 801;
        public const int OBJ_TechParamCompND = 802;
        public const int OBJ_CatDrive = 803;

        public const int CAT_NOTDEFINED = 0;
        public const int CAT_ROUTER = 1;
        public const int CAT_COMPUTER = 2;
        public const int CAT_WORKSTATION = 3;
        public const int CAT_SWITCH = 4;
        public const int CAT_TerminalDevicePrinter = 5;
        public const int CAT_SERVER = 6;
        public const int CAT_MODEM = 7;
        public const int CAT_BRIDGE = 8;
        public const int CAT_PHONE = 9;
        public const int CAT_FAX = 10;
        public const int CAT_StorageSystem = 11;
        public const int CAT_LogicalComponent = 12;
        public const int CAT_NetworkDevicePrinter = 13;
        public const int CAT_VirtualServer = 416;
        public const int CAT_VirtualComputer = 417;
        public const int CAT_Cluster = 420;

        public const int OBJ_Undisposed = 0;
        public const int OBJ_ServiceCenter = 114;
        public const int OBJ_ServiceContract = 115;

        public const int OBJ_DeviceMonitorTemplate = 145;
        public const int OBJ_DeviceMonitorParameterTemplate = 146;
        public const int OBJ_SNMPParameterEnum = 147;
        public const int OBJ_SNMPParameterType = 148;
        public const int OBJ_DeviceMonitorParameter = 149;
        public const int OBJ_SNMPSecurityParameters = 224;
        public const int OBJ_DashboardFolder = 153;
        public const int OBJ_Dashboard = 152;
        public const int OBJ_DashboardItem = 151;

        public const int OBJ_SnmpDeviceModel = 225;
        public const int OBJ_SnmpDeviceUnknown = 226;
        public const int OBJ_SnmpDeviceProfile = 227;
        public const int OBJ_SnmpDeviceSensor = 228;

        public const int OBJ_Adapter_Default = 329;
        public const int OBJ_MOTHERBOARD = 330;
        public const int OBJ_PROCESSOR = 331;
        public const int OBJ_MEMORY = 332;
        public const int OBJ_VIDEOADAPTER = 333;
        public const int OBJ_SOUNDCARD = 334;
        public const int OBJ_NETWORKADAPTER = 335;
        public const int OBJ_STORAGE = 336;
        public const int OBJ_CDANDDVDDRIVE = 337;
        public const int OBJ_FLOPPYDRIVE = 338;
        public const int OBJ_MONITOR = 340;
        public const int OBJ_KEYBOARD = 341;
        public const int OBJ_MOUSE = 342;
        public const int OBJ_Peripheral_Printer = 345;
        public const int OBJ_SCANER = 346;
        public const int OBJ_Peripheral_Modem = 347;
        public const int OBJ_STORAGECONTROLLER = 352;
        public const int OBJ_DRIVE = 400;
        public const int OBJ_UndisposedDevice = 666;

        public const int OBJ_SideOrganization = 721;
        public const int OBJ_ExternalUtility = 725;
        public const int OBJ_PURCHASE = 222;

        public const int OBJ_CreepingLine = 705;
        public const int OBJ_RFSResult = 706;
        public const int OBJ_IncidentResult = 707;
        public const int OBJ_ProblemType = 708;
        public const int OBJ_ProblemCause = 709;
        public const int OBJ_RFCType = 710;
        public const int OBJ_RFCCategory = 711;

        public const int OBJ_HtmlTagWorker = 121;
        public const int OBJ_EmailQuoteTrimmer = 122;

        public const int OBJ_CommonFilters = 744;
        public const int OBJ_Substitution = 745;

        public const int OBJ_CompositionFile = 746;
        public const int OBJ_RecognitionRules = 747;

        public const int OBJ_KBArticle = 137;
        public const int OBJ_KBArticleFolder = 138;
        public const int OBJ_KBArticleTag = 139;
        public const int OBJ_KBArticleStatus = 140;
        public const int OBJ_REPORTFOLDER = 727;
        public const int OBJ_REPORT = 728;
        public const int OBJ_USERIMPORT = 729;
        public const int OBJ_USERIMPORTADCONFIGURATION = 730;
        public const int OBJ_USERIMPORTCSVCONFIGURATION = 731;

        public const int OBJ_CallSummary = 132;
        public const int OBJ_Influence = 133;
        public const int OBJ_Urgency = 134;
        public const int OBJ_Priority = 135;
        public const int OBJ_CallType = 136;

        public const int OBJ_WorkOrderType = 142;
        public const int OBJ_WorkOrderPriority = 141;
        public const int OBJ_Budget = 143;
        public const int OBJ_ServiceCategory = 405;
        public const int OBJ_ServiceItem = 406;
        public const int OBJ_ServiceAttendance = 407;
        public const int OBJ_Service = 408;

        public const int OBJ_WorkOrderTemplateFolder = 154;
        public const int OBJ_WorkOrderTemplate = 155;

        public const int OBJ_MaintenanceFolder = 156;
        public const int OBJ_Maintenance = 157;

        public const int OBJ_WorkflowSchemeFolder = 158;
        public const int OBJ_WorkflowScheme = 159;

        public const int OBJ_Negotiation = 160;
        public const int OBJ_Workflow = 161;
        public const int OBJ_HypervisorModel = 162;
        public const int OBJ_LogicalPort = 163;
        public const int OBJ_DeviceApplication = 164;
        public const int OBJ_DataEntity = 165;
        public const int OBJ_CustomController = 188;

        public const int OBJ_FileSystem = 166;
        public const int OBJ_SchemeDependency = 167;
        public const int OBJ_DISCARRAY = 348;
        public const int OBJ_LOGICALUNIT = 349;
        public const int OBJ_VOLUME = 350;

        public const int OBJ_ComponentImportCSVConfiguration = 732;

        public const int OBJ_VirtualNetwork = 168;
        public const int OBJ_ParameterEnum = 169;
        public const int OBJ_CalendarWeekend = 170;
        public const int OBJ_CalendarHoliday = 171;
        public const int OBJ_Exclusion = 172;
        public const int OBJ_CalendarWorkSchedule = 173;
        public const int OBJ_AuditRuleSet = 174;
        public const int OBJ_ServiceUnit = 175;
        public const int OBJ_FixedAssetSettingConfiguration = 176;
        public const int OBJ_FixedAssetSetting = 177;
        public const int OBJ_FinanceAction = 178;
        public const int OBJ_FinanceBudget = 179;
        public const int OBJ_FinanceBudgetRow = 180;
        public const int OBJ_FinanceCenter = 181;
        public const int OBJ_ServiceContractModel = 182;
        public const int OBJ_ApplicationModule = 733;

        public const int OBJ_MessageBy = 734;
        public const int OBJ_MessageByEmail = 735;
        public const int OBJ_MessageByMonitoring = 736;
        public const int OBJ_MessageByInquiry = 737;

        public const int OBJ_Solution = 738;
        public const int OBJ_AgentFileInfo = 748;

        public const int OBJ_MessageByInquiryTaskForAsset = 739;
        public const int OBJ_MessageByIntegration = 740;
        public const int OBJ_MessageByOrganizationStructureImport = 741;
        public const int OBJ_MessageByInquiryTaskForUsers = 742;

        public const int OBJ_CostCategory = 353;
        public const int OBJ_CostCondition = 354;
        public const int OBJ_CostRule = 355;
        public const int OBJ_CostDistributionRule = 356;
        public const int OBJ_WorkCostRule = 357;
        public const int OBJ_Cost = 358;
        public const int OBJ_CostSetting = 359;
        public const int OBJ_ITSystem = 360;
        public const int OBJ_Work = 361;
        public const int OBJ_ServiceContractType = 362;
        public const int OBJ_CostSupplementObject = 363;
        public const int OBJ_DevExpressDashboard = 364;
        public const int OBJ_InfrastructureSegment = 366;
        public const int OBJ_Criticality = 367;
        public const int OBJ_DataEntityType = 368;
        public const int OBJ_Position = 90;
        public const int OBJ_UserActivityType = 12;
        public const int OBJ_ProjectState = 370;
        public const int OBJ_Project = 371;
        public const int OBJ_ManhoursWork = 18;
        public const int OBJ_TimeSheetNote = 19;
        public const int OBJ_ServiceTemplate = 372;
        public const int OBJ_ServiceTemplateRule = 373;
        public const int OBJ_ProductCatalogCategory = 374;
        public const int OBJ_LifeCycle = 375;
        public const int OBJ_Peripheral_Default = 376;
        public const int OBJ_Adapter_Modem = 377;
        public const int OBJ_ProductCatalogType = 378;
        public const int OBJ_AccessPermission = 365;
        public const int OBJ_FixedAsset = 379;
        public const int OBJ_ActivesRequestSpecification = 380;
        public const int OBJ_PurchaseSpecification = 381;
        public const int OBJ_GoodsInvoice = 382;
        public const int OBJ_GoodsInvoiceSpecification = 383;
        public const int OBJ_SupplierContactPerson = 384;
        public const int OBJ_SoftwareModelUsingType = 385;
        public const int OBJ_ServiceContractAgreement = 386;
        public const int OBJ_SupplierType = 387;
        public const int OBJ_ServiceContractMaintenance = 388;
        public const int OBJ_ServiceContractAssetMaintenance = 389;
        public const int OBJ_ServiceContractAgreementAssetMaintenance = 369;
        public const int OBJ_ServiceContractLicenceMaintenance = 390;
        public const int OBJ_ServiceContractAgreementLicenceMaintenance = 343;
        public const int OBJ_ServiceContractAgreementLicence = 339;
        public const int OBJ_ServiceContractLicence = 391;
        public const int OBJ_InventorySpecification = 392;
        public const int OBJ_ProductCatalogImportSetting = 393;
        public const int OBJ_ProductCatalogImportCSVConfiguration = 394;
        public const int OBJ_GoodsInvoiceImportSetting = 395;
        public const int OBJ_GoodsInvoiceImportConfiguration = 396;
        public const int OBJ_StorageLocation = 397;
        public const int OBJ_ServiceCatalogueImportSetting = 398;
        public const int OBJ_ServiceCatalogueImportCSVConfiguration = 399;
        public const int OBJ_ConfigurationUnit = 409;
        public const int OBJ_SwitchConfigurationUnit = 410;
        public const int OBJ_RouterConfigurationUnit = 411;
        public const int OBJ_PrinterConfigurationUnit = 412;
        public const int OBJ_StorageSystemConfigurationUnit = 413;
        public const int OBJ_ServerConfigurationUnit = 414;
        public const int OBJ_HostConfigurationUnit = 419;
        public const int OBJ_Cluster = 420;
        public const int OBJ_DataEntityImportSetting = 421;
        public const int OBJ_DataEntityImportCSVConfiguration = 422;

        public const int OBJ_LogicalObject = 415;
        public const int OBJ_LogicalServer = 416;
        public const int OBJ_LogicalComputer = 417;
        public const int OBJ_LogicalCommutator = 418;
        public const int OBJ_ELPTaskSetting = 820;
        public const int OBJ_SoftwareLicenceScheme = 750; //Схема лицензирования

        public const int OBJ_MassIncident = 823;

        public const int OPERATION_PROPERTIES_OWNER = 1;
        public const int OPERATION_PROPERTIES_ORGANIZATION = 2;
        public const int OPERATION_DELETE_ORGANIZATION = 3;
        public const int OPERATION_ADD_ORGANIZATION = 4;
        public const int OPERATION_PROPERTIES_SUBDIVISION = 5;
        public const int OPERATION_DELETE_SUBDIVISION = 6;
        public const int OPERATION_ADD_SUBDIVISION = 7;
        public const int OPERATION_PROPERTIES_BUILDING = 8;
        public const int OPERATION_DELETE_BUILDING = 9;
        public const int OPERATION_ADD_BUILDING = 10;
        public const int OPERATION_PROPERTIES_FLOOR = 11;
        public const int OPERATION_DELETE_FLOOR = 12;
        public const int OPERATION_ADD_FLOOR = 13;
        public const int OPERATION_SCHEME_FLOOR = 14;
        public const int OPERATION_PROPERTIES_ROOM = 15;
        public const int OPERATION_DELETE_ROOM = 16;
        public const int OPERATION_ADD_ROOM = 17;
        public const int OPERATION_PROPERTIES_RACK = 18;
        public const int OPERATION_DELETE_RACK = 19;
        public const int OPERATION_ADD_RACK = 20;
        public const int OPERATION_SCHEME_RACK = 22;
        public const int OPERATION_PROPERTIES_NETWORKDEVICE = 23;
        public const int OPERATION_DELETE_NETWORKDEVICE = 24;
        public const int OPERATION_ADD_NETWORKDEVICE = 25;

        public const int OPERATION_DIAGNISTIC_ASSET = 29;
        public const int OPERATION_Asset_WriteOff = 30;
        public const int OPERATION_HISTORY_ASSET = 31;
        public const int OPERATION_Asset_ToRepair = 32;
        public const int OPERATION_Asset_FromRepair = 33;

        public const int OPERATION_PROPERTIES_PANEL = 34;
        public const int OPERATION_DELETE_PANEL = 35;
        public const int OPERATION_ADD_PANEL = 36;
        public const int OPERATION_PROPERTIES_ACTIVEPORT = 37;
        public const int OPERATION_PATH_ACTIVEPORT = 38;
        public const int OPERATION_CONNECTION_CORD_ACTIVEPORT = 39;
        public const int OPERATION_DISCONNECT_CORD_ACTIVEPORT = 40;
        public const int OPERATION_PROPERTIES_PANELPORT = 41;
        public const int OPERATION_CONNECTION_CORD_PANELPORT = 42;
        public const int OPERATION_DISCONNECT_CORD_PANELPORT = 43;
        public const int OPERATION_CONNECT_CABEL_PANELPORT = 44;
        public const int OPERATION_DISCONNECT_CABEL_PANELPORT = 45;
        public const int OPERATION_PATH_PANELPORT = 46;
        public const int OPERATION_PROPERTIES_OUTLETPORT = 47;
        public const int OPERATION_CONNECT_CORD_OUTLETPORT = 48;
        public const int OPERATION_DISCONNECT_CORD_OUTLETPORT = 49;
        public const int OPERATION_CONNECT_CABEL_OUTLETPORT = 50;
        public const int OPERATION_DISCONNECT_CABEL_OUTLETPORT = 51;
        public const int OPERATION_PATH_OUTLETPORT = 52;
        public const int OPERATION_PROPERTIES_SPLITTER = 53;
        public const int OPERATION_CONNECT_CORD_SPLITTERPORT = 54;
        public const int OPERATION_DISCONNECT_CORD_SPLITTERPORT = 55;
        public const int OPERATION_PATH_SPLITTER = 58;
        public const int OPERATION_CONNECT_SPLITTER = 59;
        public const int OPERATION_DISCONNECT_SPLITTER = 60;
        public const int OPERATION_PROPERTIES_WORKPLACE = 61;
        public const int OPERATION_DELETE_WORKPLACE = 62;
        public const int OPERATION_ADD_WORKPLACE = 63;
        public const int OPERATION_PROPERTIES_TERMINALDEVICE = 65;
        public const int OPERATION_DELETE_TERMINALDEVICE = 66;
        public const int OPERATION_ADD_TERMINALDEVICE = 67;
        public const int OPERATION_CONNECT_CORD_TERMINALDEVICE = 68;
        public const int OPERATION_DISCONNECT_CORD_TERMINALDEVICE = 69;
        public const int OPERATION_PATH_TERMINALDEVICE = 70;
        public const int OPERATION_PROPERTIES_USER = 71;
        public const int OPERATION_DELETE_USER = 72;
        public const int OPERATION_ADD_USER = 73;
        public const int OPERATION_PROPERTIES_ADAPTER = 77;
        public const int OPERATION_DELETE_ADAPTER = 78;
        public const int OPERATION_PROPERTIES_PERIPHERAL = 79;
        public const int OPERATION_DELETE_PERIPHERAL = 80;
        public const int OPERATION_PROPERTIES_OUTLET = 81;
        public const int OPERATION_DELETE_OUTLET = 82;
        public const int OPERATION_ADD_OUTLET = 83;
        public const int OPERATION_ADD_ADAPTER = 84;
        public const int OPERATION_ADD_PERIPHERAL = 85;
        public const int OPERATION_ADD_INSTALLATION = 86;
        public const int OPERATION_PROPERTIES_INSTALLATION = 87;
        public const int OPERATION_DELETE_INSTALLATION = 88;
        public const int OPERATION_PROPERTIES_CONNECTORTYPE = 89;
        public const int OPERATION_ADD_CONNECTORTYPE = 90;
        public const int OPERATION_PROPERTIES_TECHNOLOGYTYPE = 91;
        public const int OPERATION_ADD_TECHNOLOGYTYPE = 92;
        public const int OPERATION_PROPERTIES_SLOTTYPE = 93;
        public const int OPERATION_ADD_SLOTTYPE = 94;
        public const int OPERATION_PROPERTIES_ROOMTYPE = 95;
        public const int OPERATION_ADD_ROOMTYPE = 96;
        public const int OPERATION_PROPERTIES_TELEPHONETYPE = 105;
        public const int OPERATION_ADD_TELEPHONETYPE = 106;

        public const int OPERATION_PROPERTIES_CARTRIDGETYPE = 110;
        public const int OPERATION_ADD_CARTRIDGETYPE = 111;
        public const int OPERATION_DELETE_CARTRIDGETYPE = 112;
        public const int OPERATION_PROPERTIES_VENDOR = 113;
        public const int OPERATION_ADD_VENDOR = 114;
        public const int OPERATION_DELETE_VENDOR = 115;
        public const int OPERATION_SYNONIM_VENDOR = 116;
        public const int OPERATION_PROPERTIES_POSTPOSITION = 117;
        public const int OPERATION_ADD_POSTPOSITION = 118;
        public const int OPERATION_DELETE_POSTPOSITION = 119;
        public const int OPERATION_SYNONIM_POSTPOSITION = 120;
        public const int OPERATION_PROPERTIES_MEASURE = 121;
        public const int OPERATION_ADD_MEASURE = 122;
        public const int OPERATION_DELETE_MEASURE = 123;
        public const int OPERATION_PROPERTIES_SOFTWARETYPE = 124;
        public const int OPERATION_ADD_SOFTWARETYPE = 125;
        public const int OPERATION_DELETE_SOFTWARETYPE = 126;
        public const int OPERATION_PROPERTIES_NETWORKDEVICEMODEL = 127;
        public const int OPERATION_ADD_NETWORKDEVICEMODEL = 128;
        public const int OPERATION_DELETE_NETWORKDEVICEMODEL = 129;
        public const int OPERATION_PORTS_NETWORKDEVICEMODEL = 130;
        public const int OPERATION_PROPERTIES_TERMINALDEVICEMODEL = 131;
        public const int OPERATION_ADD_TERMINALDEVICEMODEL = 132;
        public const int OPERATION_DELETE_TERMINALDEVICEMODEL = 133;
        public const int OPERATION_PROPERTIES_ADAPTERMODEL = 134;
        public const int OPERATION_ADD_ADAPTERMODEL = 135;
        public const int OPERATION_DELETE_ADAPTERMODEL = 136;
        public const int OPERATION_SYNONIM_ADAPTERMODEL = 137;
        public const int OPERATION_PROPERTIES_PERIPHERALMODEL = 138;
        public const int OPERATION_ADD_PERIPHERALMODEL = 139;
        public const int OPERATION_DELETE_PERIPHERALMODEL = 140;
        public const int OPERATION_SYNONIM_PERIPHERALMODEL = 141;
        public const int OPERATION_PROPERTIES_SOFTWAREMODEL = 142;
        public const int OPERATION_ADD_SOFTWAREMODEL = 143;
        public const int OPERATION_ADD_SOFTWAREMODELCOMMERCIAL = 890;
        public const int OPERATION_DELETE_SOFTWAREMODEL = 144;
        public const int OPERATION_PROPERTIES_RACKMODEL = 145;
        public const int OPERATION_ADD_RACKMODEL = 146;
        public const int OPERATION_DELETE_RACKMODEL = 147;
        public const int OPERATION_PROPERTIES_PANELMODEL = 148;
        public const int OPERATION_ADD_PANELMODEL = 149;
        public const int OPERATION_DELETE_PANELMODEL = 150;
        public const int OPERATION_PORTS_PANELMODEL = 151;
        public const int OPERATION_PROPERTIES_OUTLETMODEL = 152;
        public const int OPERATION_ADD_OUTLETMODEL = 153;
        public const int OPERATION_DELETE_OUTLETMODEL = 154;
        public const int OPERATION_PORTS_OUTLETMODEL = 155;
        public const int OPERATION_PROPERTIES_SPLITTERMODEL = 156;
        public const int OPERATION_ADD_SPLITTERMODEL = 157;
        public const int OPERATION_PROPERTIES_CORDMODEL = 158;
        public const int OPERATION_ADD_CORDMODEL = 159;
        public const int OPERATION_DELETE_CORDMODEL = 160;
        public const int OPERATION_PROPERTIES_CABLEMODEL = 161;
        public const int OPERATION_ADD_CABLEMODEL = 162;
        public const int OPERATION_DELETE_CABLEMODEL = 163;
        public const int OPERATION_PROPERTIES_CHANNELMODEL = 164;
        public const int OPERATION_ADD_CHANNELMODEL = 165;
        public const int OPERATION_DELETE_CHANNELMODEL = 166;
        public const int OPERATION_PROPERTIES_MATERIALMODEL = 167;
        public const int OPERATION_ADD_MATERIALMODEL = 168;
        public const int OPERATION_DELETE_MATERIALMODEL = 169;

        public const int OPERATION_PROPERTIES_DOCUMENTTYPE = 172;
        public const int OPERATION_ADD_DOCUMENTTYPE = 170;
        public const int OPERATION_UPDATE_DOCUMENTTYPE = 279;
        public const int OPERATION_DELETE_DOCUMENTTYPE = 171;

        public const int OPERATION_PROPERTIES_DOCUMENTFOLDER = 173;
        public const int OPERATION_ADD_DOCUMENTFOLDER = 174;
        public const int OPERATION_UPDATE_DOCUMENTFOLDER = 280;
        public const int OPERATION_DELETE_DOCUMENTFOLDER = 175;

        public const int OPERATION_NetworkScheme_Properties = 181;
        public const int OPERATION_NetworkScheme_Update = 223;
        public const int OPERATION_NetworkScheme_Delete = 224;
        public const int OPERATION_DashboardItem_Details = 225;

        public const int OPERATION_PROPERTIES_DOCUMENT = 184;
        public const int OPERATION_NEWDOCUMENT_DOCUMENT = 180;
        public const int OPERATION_ADDREFERENCE_DOCUMENT = 299;
        public const int OPERATION_UPDATE_DOCUMENT = 281;
        public const int OPERATION_DELETE_DOCUMENT = 179;
        public const int OPERATION_REMOVEREFERENCE_DOCUMENT = 439;
        public const int OPERATION_VIEW_DOCUMENT = 176;
        public const int OPERATION_EDIT_DOCUMENT = 177;
        public const int OPERATION_SAVE_DOCUMENT = 297;
        public const int OPERATION_SAVEAS_DOCUMENT = 178;
        public const int OPERATION_CHECKOUT_DOCUMENT = 182;
        public const int OPERATION_CHECKIN_DOCUMENT = 183;
        public const int OPERATION_RESETSTATE_DOCUMENT = 300;

        public const int OPERATION_PROPERTIES_MATERIALCONSUMPTION = 185;
        public const int OPERATION_ADD_MATERIALCONSUMPTION = 186;
        public const int OPERATION_DELETE_MATERIALCONSUMPTION = 187;
        public const int OPERATION_PROPERTIES_CORD = 188;
        public const int OPERATION_ADD_CORD = 189;
        public const int OPERATION_DELETE_CORD = 190;
        public const int OPERATION_PROPERTIES_CABLE = 191;
        public const int OPERATION_ADD_CABLE = 192;
        public const int OPERATION_DELETE_CABLE = 193;
        public const int OPERATION_PROPERTIES_CHANNEL = 194;
        public const int OPERATION_ADD_CHANNEL = 195;
        public const int OPERATION_DELETE_CHANNEL = 196;
        public const int OPERATION_PROPERTIES_SPLICE = 205;
        public const int OPERATION_ADD_SPLICE = 206;
        public const int OPERATION_DELETE_SPLICE = 207;
        public const int OPERATION_PROPERTIES_MATERIALPURCHASE = 208;
        public const int OPERATION_ADD_MATERIALPURCHASE = 209;
        public const int OPERATION_DELETE_MATERIALPURCHASE = 210;
        public const int OPERATION_PROPERTIES_SERVICECONTRACT = 211;
        public const int OPERATION_ADD_SERVICECONTRACT = 212;
        public const int OPERATION_DELETE_SERVICECONTRACT = 213;
        public const int OPERATION_PROPERTIES_SERVICECENTRE = 214;
        public const int OPERATION_ADD_SERVICECENTRE = 215;
        public const int OPERATION_DELETE_SERVICECENTRE = 216;
        public const int OPERATION_PROPERTIES_SUPPLIER = 217;
        public const int OPERATION_ADD_SUPPLIER = 218;
        public const int OPERATION_DELETE_SUPPLIER = 219;
        public const int OPERATION_PROPERTIES_NOTIFICATION = 220;
        public const int OPERATION_ADD_NOTIFICATION = 342;
        public const int OPERATION_DELETE_NOTIFICATION = 343;
        public const int OPERATION_UPDATE_OWNER = 226;
        public const int OPERATION_UPDATE_ORGANIZATION = 227;
        public const int OPERATION_UPDATE_SUBDIVISION = 228;
        public const int OPERATION_UPDATE_BUILDING = 229;
        public const int OPERATION_UPDATE_FLOOR = 230;
        public const int OPERATION_UPDATE_ROOM = 231;
        public const int OPERATION_UPDATE_RACK = 232;
        public const int OPERATION_UPDATE_NETWORKDEVICE = 233;
        public const int OPERATION_UPDATE_PANEL = 234;
        public const int OPERATION_UPDATE_ACTIVEPORT = 235;
        public const int OPERATION_UPDATE_PANELPORT = 236;
        public const int OPERATION_UPDATE_OUTLETPORT = 237;
        public const int OPERATION_UPDATE_SPLITTER = 238;
        public const int OPERATION_UPDATE_WORKPLACE = 239;
        public const int OPERATION_UPDATE_TERMINALDEVICE = 240;
        public const int OPERATION_UPDATE_USER = 241;
        public const int OPERATION_UPDATE_ADAPTER = 242;
        public const int OPERATION_UPDATE_PERIPHERAL = 243;
        public const int OPERATION_UPDATE_OUTLET = 244;
        public const int OPERATION_UPDATE_SOFTWARE = 245;
        public const int OPERATION_UPDATE_CONNECTORTYPE = 246;
        public const int OPERATION_UPDATE_TECHNOLOGYTYPE = 247;
        public const int OPERATION_UPDATE_SLOTTYPE = 248;
        public const int OPERATION_UPDATE_ROOMTYPE = 249;
        public const int OPERATION_UPDATE_TELEPHONETYPE = 254;

        public const int OPERATION_UPDATE_CARTRIDGETYPE = 256;
        public const int OPERATION_UPDATE_VENDOR = 257;
        public const int OPERATION_UPDATE_POSTPOSITION = 258;
        public const int OPERATION_UPDATE_MEASURE = 259;
        public const int OPERATION_UPDATE_SOFTWARETYPE = 260;
        public const int OPERATION_UPDATE_NETWORKDEVICEMODEL = 261;
        public const int OPERATION_UPDATE_TERMINALDEVICEMODEL = 262;
        public const int OPERATION_UPDATE_ADAPTERMODEL = 263;
        public const int OPERATION_UPDATE_PERIPHERALMODEL = 264;
        public const int OPERATION_UPDATE_SOFTWAREMODEL = 265;
        public const int OPERATION_UPDATE_RACKMODEL = 266;
        public const int OPERATION_UPDATE_PANELMODEL = 267;
        public const int OPERATION_UPDATE_OUTLETMODEL = 268;
        public const int OPERATION_UPDATE_SPLITTERMODEL = 269;
        public const int OPERATION_UPDATE_CORDMODEL = 270;
        public const int OPERATION_UPDATE_CABLEMODEL = 271;
        public const int OPERATION_UPDATE_CHANNELMODEL = 272;
        public const int OPERATION_UPDATE_MATERIALMODEL = 273;
        public const int OPERATION_UPDATE_MATERIALCONSUMPTION = 274;
        public const int OPERATION_UPDATE_CORD = 275;
        public const int OPERATION_UPDATE_CABLE = 276;
        public const int OPERATION_UPDATE_CHANNEL = 277;
        public const int OPERATION_UPDATE_SPLICE = 278;
        public const int OPERATION_UPDATE_MATERIALPURCHASE = 282;
        public const int OPERATION_UPDATE_SERVICECONTRACT = 283;
        public const int OPERATION_UPDATE_SERVICECENTRE = 284;
        public const int OPERATION_UPDATE_SUPPLIER = 285;
        public const int OPERATION_UPDATE_NOTIFICATION = 286;

        public const int OPERATION_PROPERTIES_ROLE = 288;
        public const int OPERATION_ADD_ROLE = 289;
        public const int OPERATION_ADD_ROLEAS = 305;
        public const int OPERATION_UPDATE_ROLE = 292;
        public const int OPERATION_CHANGE_ROLE = 291;
        public const int OPERATION_DELETE_ROLE = 290;

        public const int OPERATION_CHANGE_ACCESS = 298;

        public const int OPERATION_SD_General_VotingUser = 303;
        public const int OPERATION_EQUIPMENT_USER = 306;

        public const int OPERATION_Call_Add = 309;
        public const int OPERATION_Call_Delete = 310;
        public const int OPERATION_Call_Update = 313;
        public const int OPERATION_CALL_Properties = 518;
        public const int OPERATION_Call_AddAs = 315;
        public const int OPERATION_Call_ShowCallsForSubdivisionInWeb = 596;
        public const int OPERATION_Call_ShowCallsForITSubdivisionInWeb = 597;

        public const int OPERATION_Problem_Properties = 222;
        public const int OPERATION_Problem_Add = 319;
        public const int OPERATION_Problem_AddAs = 221;
        public const int OPERATION_Problem_Update = 323;
        public const int OPERATION_Problem_PowerfullAccess = 428;
        public const int OPERATION_Problem_Delete = 320;
        public const int OPERATION_Problem_ShowProblemsForITSubdivisionInWeb = 598;

        public const int OPERATION_WorkOrder_Properties = 302;
        public const int OPERATION_WorkOrder_Add = 301;
        public const int OPERATION_WorkOrder_AddAs = 331;
        public const int OPERATION_WorkOrder_Update = 333;
        public const int OPERATION_WorkOrder_Delete = 330;
        public const int OPERATION_WorkOrder_ShowWorkOrdersForITSubdivisionInWeb = 599;

        public const int OPERATION_RFC_Properties = 383;
        public const int OPERATION_RFC_Add = 384;
        public const int OPERATION_RFC_AddAs = 378;
        public const int OPERATION_RFC_Update = 386;
        public const int OPERATION_RFC_Delete = 385;
        public const int OPERATION_RFC_ShowRFCsForITSubdivisionInWeb = 377;

        public const int OPERATION_CHANGE_NOTIFICATION = 340;

        public const int OPERATION_PROPERTIES_QUEUE = 379;
        public const int OPERATION_ADD_QUEUE = 380;
        public const int OPERATION_UPDATE_QUEUE = 381;
        public const int OPERATION_DELETE_QUEUE = 382;

        public const int OPERATION_ADD_REPORTFOLDER = 446;
        public const int OPERATION_PROPERTIES_REPORTFOLDER = 447;
        public const int OPERATION_UPDATE_REPORTFOLDER = 448;
        public const int OPERATION_DELETE_REPORTFOLDER = 449;

        public const int OPERATION_ADD_REPORT = 450;
        public const int OPERATION_PROPERTIES_REPORT = 451;
        public const int OPERATION_UPDATE_REPORT = 452;
        public const int OPERATION_DELETE_REPORT = 453;
        public const int OPERATION_VIEW_REPORT = 454;
        public const int OPERATION_EDIT_REPORT = 455;

        public const int OPERATION_NAVIGATETOOBJECT = 350;

        public const int OPERATION_DEPENDENTS = 422;

        public const int OPERATION_Asset_MateriallyResponsible = 356;
        public const int OPERATION_Asset_Inventory = 895;
        public const int OPERATION_SD_General_Administrator = 357;
        public const int OPERATION_SD_General_Owner = 373;
        public const int OPERATION_SD_General_Executor = 358;
        public const int OPERATION_SLA_Add = 359;
        public const int OPERATION_SLA_Properties = 360;
        public const int OPERATION_SLA_Delete = 361;
        public const int OPERATION_SLA_Update = 362;
        public const int OPERATION_Rule_Add = 363;
        public const int OPERATION_Rule_Properties = 364;
        public const int OPERATION_Rule_Delete = 365;
        public const int OPERATION_Rule_Update = 366;
        public const int OPERATION_ServiceCatalog_Properties = 367;
        public const int OPERATION_ServiceCatalog_Update = 368;
        public const int OPERATION_ServiceCatalog_Service_Add = 780;
        public const int OPERATION_ServiceCatalog_Category_Add = 781;

        public const int OPERATION_Control_Add = 369;
        public const int OPERATION_Control_Remove = 370;

        public const int OPERATION_SYNONYM_SOFTWAREMODEL = 372;

        public const int OPERATION_CreepingLine_Properties = 21;
        public const int OPERATION_CreepingLine_Add = 56;
        public const int OPERATION_CreepingLine_Delete = 57;
        public const int OPERATION_CreepingLine_Update = 64;

        public const int OPERATION_RFSResult_Properties = 383;
        public const int OPERATION_RFSResult_Add = 384;
        public const int OPERATION_RFSResult_Delete = 385;
        public const int OPERATION_RFSResult_Update = 386;

        public const int OPERATION_IncidentResult_Properties = 387;
        public const int OPERATION_IncidentResult_Add = 388;
        public const int OPERATION_IncidentResult_Delete = 389;
        public const int OPERATION_IncidentResult_Update = 390;

        public const int OPERATION_ProblemType_Properties = 391;
        public const int OPERATION_ProblemType_Add = 392;
        public const int OPERATION_ProblemType_Delete = 393;
        public const int OPERATION_ProblemType_Update = 394;

        public const int OPERATION_ProblemCause_Properties = 395;
        public const int OPERATION_ProblemCause_Add = 396;
        public const int OPERATION_ProblemCause_Delete = 397;
        public const int OPERATION_ProblemCause_Update = 398;

        public const int OPERATION_Audit = 400;
        public const int OPERATION_TASKOFIMPORT_ABORT = 434;

        public const int OPERATION_DRIVE_ADD = 431;
        public const int OPERATION_DRIVE_UPDATE = 433;
        public const int OPERATION_DRIVE_DELETE = 432;
        public const int OPERATION_DRIVE_PROPERTIES = 457;

        public const int OPERATION_MODULE_ADD = 435;
        public const int OPERATION_MODULE_UPDATE = 436;
        public const int OPERATION_MODULE_DELETE = 437;
        public const int OPERATION_MODULE_PROPERTIES = 438;

        public const int OPERATION_SOFTWARELICENCE_ADD = 440;
        public const int OPERATION_SOFTWARELICENCE_PROPERTIES = 441;
        public const int OPERATION_SOFTWARELICENCE_UPDATE = 442;
        public const int OPERATION_SOFTWARELICENCE_DELETE = 443;

        public const int OPERATION_SetInvNumber_ASSET = 445;
        public const int OPERATION_Barcode_ASSET = 519;

        public const int OPERATION_Asset_ComputerInfo = 401;
        public const int OPERATION_Asset_ChangeDeviceComposition = 456;

        public const int OPERATION_InquirySubdevice_Create = 458;
        public const int OPERATION_InquirySubdevice_Cancel = 459;
        public const int OPERATION_InquirySubdevice_Block = 460;

        public const int SIMPLE_OPERATION_ADD = 0;
        public const int SIMPLE_OPERATION_CHANGE = 1;
        public const int SIMPLE_OPERATION_VIEW = 2;

        public const int OPERATION_USERIMPORT_ADD = 408;
        public const int OPERATION_USERIMPORT_ADDAS = 336;
        public const int OPERATION_USERIMPORT_UPDATE = 409;
        public const int OPERATION_USERIMPORT_DELETE = 410;
        public const int OPERATION_USERIMPORT_PROPERTIES = 411;
        public const int OPERATION_USERIMPORT_RUN = 412;
        public const int OPERATION_USERIMPORT_ABORT = 74;
        public const int OPERATION_USERIMPORT_SCHEDULE = 75;

        public const int OPERATION_USERIMPORTADCONFIGURATION_ADD = 201;
        public const int OPERATION_USERIMPORTADCONFIGURATION_UPDATE = 202;
        public const int OPERATION_USERIMPORTADCONFIGURATION_DELETE = 203;
        public const int OPERATION_USERIMPORTADCONFIGURATION_PROPERTIES = 204;
        public const int OPERATION_USERIMPORTADCONFIGURATION_ADDAS = 287;

        public const int OPERATION_CompositionFile_ADD = 746001;
        public const int OPERATION_CompositionFile_UPDATE = 746002;
        public const int OPERATION_CompositionFile_DELETE = 746003;
        public const int OPERATION_CompositionFile_PROPERTIES = 746004;

        public const int OPERATION_RecognitionRule_ADD = 747001;
        public const int OPERATION_RecognitionRule_UPDATE = 747002;
        public const int OPERATION_RecognitionRule_DELETE = 747003;
        public const int OPERATION_RecognitionRule_PROPERTIES = 747004;

        public const int OPERATION_USERIMPORTCSVCONFIGURATION_ADD = 307;
        public const int OPERATION_USERIMPORTCSVCONFIGURATION_UPDATE = 308;
        public const int OPERATION_USERIMPORTCSVCONFIGURATION_DELETE = 335;
        public const int OPERATION_USERIMPORTCSVCONFIGURATION_PROPERTIES = 337;
        public const int OPERATION_USERIMPORTCSVCONFIGURATION_ADDAS = 341;

        public const int OPERATION_CallSummary_Properties = 468;
        public const int OPERATION_CallSummary_Add = 469;
        public const int OPERATION_CallSummary_Delete = 470;
        public const int OPERATION_CallSummary_Update = 471;

        public const int OPERATION_CallSummary_Merge = 488;

        public const int OPERATION_Influence_Properties = 472;
        public const int OPERATION_Influence_Add = 473;
        public const int OPERATION_Influence_Delete = 474;
        public const int OPERATION_Influence_Update = 475;

        public const int OPERATION_Urgency_Properties = 476;
        public const int OPERATION_Urgency_Add = 477;
        public const int OPERATION_Urgency_Delete = 478;
        public const int OPERATION_Urgency_Update = 479;

        public const int OPERATION_Priority_Properties = 480;
        public const int OPERATION_Priority_Add = 481;
        public const int OPERATION_Priority_Delete = 482;
        public const int OPERATION_Priority_Update = 483;

        public const int OPERATION_CallType_Properties = 484;
        public const int OPERATION_CallType_Add = 485;
        public const int OPERATION_CallType_Delete = 486;
        public const int OPERATION_CallType_Update = 487;

        public const int OPERATION_KBArticle_Properties = 489;
        public const int OPERATION_KBArticle_Add = 490;
        public const int OPERATION_KBArticle_AddReference = 338;
        public const int OPERATION_KBArticle_Delete = 491;
        public const int OPERATION_KBArticle_DeleteReference = 339;
        public const int OPERATION_KBArticle_Update = 492;

        public const int OPERATION_KBArticle_Search = 519;

        public const int OPERATION_KBArticleFolder_Properties = 493;
        public const int OPERATION_KBArticleFolder_Add = 494;
        public const int OPERATION_KBArticleFolder_Delete = 495;
        public const int OPERATION_KBArticleFolder_Update = 496;

        public const int OPERATION_KBArticleTag_Properties = 497;
        public const int OPERATION_KBArticleTag_Add = 498;
        public const int OPERATION_KBArticleTag_Delete = 499;

        public const int OPERATION_Priority_EditTable = 500;

        public const int OPERATION_KBArticleStatus_Properties = 501;
        public const int OPERATION_KBArticleStatus_Add = 502;
        public const int OPERATION_KBArticleStatus_Delete = 503;
        public const int OPERATION_KBArticleStatus_Update = 504;

        public const int OPERATION_Default = 505;

        public const int OPERATION_WorkOrderPriority_Properties = 506;
        public const int OPERATION_WorkOrderPriority_Add = 507;
        public const int OPERATION_WorkOrderPriority_Delete = 508;
        public const int OPERATION_WorkOrderPriority_Update = 509;

        public const int OPERATION_WorkOrderType_Properties = 510;
        public const int OPERATION_WorkOrderType_Add = 511;
        public const int OPERATION_WorkOrderType_Delete = 512;
        public const int OPERATION_WorkOrderType_Update = 513;

        public const int OPERATION_Budget_Properties = 514;
        public const int OPERATION_Budget_Add = 515;
        public const int OPERATION_Budget_Delete = 516;
        public const int OPERATION_Budget_Update = 517;

        public const int OPERATION_SendMessage = 520;
        public const int OPERATION_Purchase_SendMessage = 521;
        public const int OPERATION_Call_PowerfullAccess = 522;
        public const int OPERATION_WorkOrder_PowerfullAccess = 524;

        public const int OPERATION_DeviceMonitorTemplate_Properties = 525;
        public const int OPERATION_DeviceMonitorTemplate_Add = 526;
        public const int OPERATION_DeviceMonitorTemplate_Delete = 527;
        public const int OPERATION_DeviceMonitorTemplate_Update = 528;

        public const int OPERATION_DeviceMonitorParameterTemplate_Properties = 529;
        public const int OPERATION_DeviceMonitorParameterTemplate_Add = 530;
        public const int OPERATION_DeviceMonitorParameterTemplate_Delete = 531;
        public const int OPERATION_DeviceMonitorParameterTemplate_Update = 532;

        public const int OPERATION_SNMPParameterEnum_Properties = 533;
        public const int OPERATION_SNMPParameterEnum_Add = 534;
        public const int OPERATION_SNMPParameterEnum_Delete = 535;
        public const int OPERATION_SNMPParameterEnum_Update = 536;

        public const int OPERATION_SNMPParameterType_Properties = 537;
        public const int OPERATION_SNMPParameterType_Add = 538;
        public const int OPERATION_SNMPParameterType_Delete = 539;
        public const int OPERATION_SNMPParameterType_Update = 540;

        public const int OPERATION_SNMPSecurityParameters_Properties = 324;
        public const int OPERATION_SNMPSecurityParameters_Add = 325;
        public const int OPERATION_SNMPSecurityParameters_Delete = 326;
        public const int OPERATION_SNMPSecurityParameters_Update = 321;

        public const int OPERATION_AuditRuleSet_Properties = 327;
        public const int OPERATION_AuditRuleSet_Add = 328;
        public const int OPERATION_AuditRuleSet_Delete = 329;
        public const int OPERATION_AuditRuleSet_Update = 332;

        public const int OPERATION_AssetDeviation_Apply = 345;
        public const int OPERATION_AssetDeviation_Close = 346;
        public const int OPERATION_AssetDeviation_Delete = 347;
        public const int OPERATION_AssetDeviation_FindAndReplace = 348;
        public const int OPERATION_AssetDeviation_Properties = 349;
        public const int OPERATION_AssetDeviation_Update = 371;

        public const int OPERATION_DeviceMonitorParameter_Properties = 541;
        public const int OPERATION_DeviceMonitorParameter_Add = 542;
        public const int OPERATION_DeviceMonitorParameter_Delete = 543;
        public const int OPERATION_DeviceMonitorParameter_Update = 544;
        public const int OPERATION_DeviceMonitorParameter_AddAs = 545;
        public const int OPERATION_DeviceMonitorParameter_View = 546;
        public const int OPERATION_SwitchOn = 547;

        public const int OPERATION_DashboardFolder_Properties = 558;
        public const int OPERATION_DashboardFolder_Add = 559;
        public const int OPERATION_DashboardFolder_Delete = 560;
        public const int OPERATION_DashboardFolder_Update = 561;
        public const int OPERATION_Dashboard_AddDevExpress = 757;
        public const int OPERATION_Dashboard_AccessManage = 762;

        public const int OPERATION_Dashboard_Properties = 554;
        public const int OPERATION_Dashboard_Add = 555;
        public const int OPERATION_Dashboard_Delete = 556;
        public const int OPERATION_Dashboard_Update = 557;

        public const int OPERATION_DashboardItem_Properties = 550;
        public const int OPERATION_DashboardItem_Add = 551;
        public const int OPERATION_DashboardItem_Delete = 552;
        public const int OPERATION_DashboardItem_Update = 553;

        public const int OPERATION_View = 562;
        public const int OPERATION_Calls = 563;
        public const int OPERATION_WorkOrders = 665;

        public const int OPERATION_WorkOrderTemplateFolder_Properties = 564;
        public const int OPERATION_WorkOrderTemplateFolder_Add = 565;
        public const int OPERATION_WorkOrderTemplateFolder_Delete = 566;
        public const int OPERATION_WorkOrderTemplateFolder_Update = 567;

        public const int OPERATION_WorkOrderTemplate_Properties = 568;
        public const int OPERATION_WorkOrderTemplate_Add = 569;
        public const int OPERATION_WorkOrderTemplate_Delete = 570;
        public const int OPERATION_WorkOrderTemplate_Update = 571;
        public const int OPERATION_WorkOrderTemplate_AddAs = 792;

        public const int OPERATION_WorkOrder_CreateByTemplate = 572;

        public const int OPERATION_MaintenanceFolder_Properties = 573;
        public const int OPERATION_MaintenanceFolder_Add = 574;
        public const int OPERATION_MaintenanceFolder_Delete = 575;

        public const int OPERATION_MaintenanceFolder_Update = 576;
        public const int OPERATION_Maintenance_Properties = 577;
        public const int OPERATION_Maintenance_Add = 578;
        public const int OPERATION_Maintenance_Delete = 579;
        public const int OPERATION_Maintenance_Update = 580;

        public const int OPERATION_WorkflowSchemeFolder_Properties = 581;
        public const int OPERATION_WorkflowSchemeFolder_Add = 582;
        public const int OPERATION_WorkflowSchemeFolder_Delete = 583;
        public const int OPERATION_WorkflowSchemeFolder_Update = 584;

        public const int OPERATION_WorkflowScheme_Properties = 585;
        public const int OPERATION_WorkflowScheme_Add = 586;
        public const int OPERATION_WorkflowScheme_Delete = 587;
        public const int OPERATION_WorkflowScheme_Update = 588;
        public const int OPERATION_WorkflowScheme_Publish = 589;
        public const int OPERATION_WorkflowScheme_AddAs = 590;

        public const int OPERATION_Workflow_Properties = 591;
        public const int OPERATION_Workflow_Restart = 592;
        public const int OPERATION_Workflow_Delete = 593;

        public const int OPERATION_WorkflowScheme_Import = 594;
        public const int OPERATION_WorkflowScheme_Export = 595;

        public const int OPERATION_HypervisorModel_Properties = 600;
        public const int OPERATION_HypervisorModel_Add = 601;
        public const int OPERATION_HypervisorModel_Delete = 602;
        public const int OPERATION_HypervisorModel_Update = 603;

        public const int OPERATION_LogicalPort_Properties = 604;
        public const int OPERATION_LogicalPort_Add = 605;
        public const int OPERATION_LogicalPort_Delete = 606;
        public const int OPERATION_LogicalPort_Update = 607;
        public const int OPERATION_LogicalPort_Connect = 608;
        public const int OPERATION_LogicalPort_Disconnect = 609;

        public const int OPERATION_DeviceApplication_Properties = 610;
        public const int OPERATION_DeviceApplication_Add = 611;
        public const int OPERATION_DeviceApplication_Delete = 612;
        public const int OPERATION_DeviceApplication_Update = 613;

        public const int OPERATION_DataEntity_Properties = 614;
        public const int OPERATION_DataEntity_Add = 615;
        public const int OPERATION_DataEntity_Delete = 616;
        public const int OPERATION_DataEntity_Update = 617;

        public const int OPERATION_FileSystem_Properties = 618;
        public const int OPERATION_FileSystem_Add = 619;
        public const int OPERATION_FileSystem_Delete = 620;
        public const int OPERATION_FileSystem_Update = 621;

        public const int OPERATION_InfrastructureSegment_Properties = 763;
        public const int OPERATION_InfrastructureSegment_Add = 764;
        public const int OPERATION_InfrastructureSegment_Delete = 765;
        public const int OPERATION_InfrastructureSegment_Update = 766;

        public const int OPERATION_Criticality_Properties = 767;
        public const int OPERATION_Criticality_Add = 768;
        public const int OPERATION_Criticality_Delete = 769;
        public const int OPERATION_Criticality_Update = 770;

        public const int OPERATION_DataEntityType_Properties = 771;
        public const int OPERATION_DataEntityType_Add = 772;
        public const int OPERATION_DataEntityType_Delete = 773;
        public const int OPERATION_DataEntityType_Update = 774;

        public const int OPERATION_User_Responsibility = 779;

        public const int OPERATION_DiscArray_Properties = 622;
        public const int OPERATION_DiscArray_Add = 623;
        public const int OPERATION_DiscArray_Delete = 624;
        public const int OPERATION_DiscArray_Update = 625;

        public const int OPERATION_LogicalUnit_Properties = 626;
        public const int OPERATION_LogicalUnit_Add = 627;
        public const int OPERATION_LogicalUnit_Delete = 628;
        public const int OPERATION_LogicalUnit_Update = 629;

        public const int OPERATION_Volume_Properties = 630;
        public const int OPERATION_Volume_Add = 631;
        public const int OPERATION_Volume_Delete = 632;
        public const int OPERATION_Volume_Update = 633;

        public const int OPERATION_SchemeDependency_Properties = 634;
        public const int OPERATION_SchemeDependency_AddAs = 635;
        public const int OPERATION_SchemeDependency_Delete = 636;
        public const int OPERATION_SchemeDependency_Update = 637;

        public const int OPERATION_ComponentImportCSVConfiguration_Properties = 638;
        public const int OPERATION_ComponentImportCSVConfiguration_Add = 639;
        public const int OPERATION_ComponentImportCSVConfiguration_Delete = 640;
        public const int OPERATION_ComponentImportCSVConfiguration_Update = 641;
        public const int OPERATION_ComponentImportCSVConfiguration_AddAs = 642;

        public const int OPERATION_VirtualNetwork_Properties = 643;
        public const int OPERATION_VirtualNetwork_Add = 644;
        public const int OPERATION_VirtualNetwork_Delete = 645;
        public const int OPERATION_VirtualNetwork_Update = 646;

        public const int OPERATION_SD_General_Calls_View = 647;
        public const int OPERATION_SD_General_Problems_View = 648;
        public const int OPERATION_SD_General_WorkOrders_View = 649;
        public const int OPERATION_SD_General_RFCs_View = 707;

        public const int OPERATION_ApplicationModule_ConfigurationManagment_View = 650;
        public const int OPERATION_ApplicationModule_ServiceDesk_View = 651;
        public const int OPERATION_ApplicationModule_SuppliesManagment_View = 652;
        public const int OPERATION_ApplicationModule_SoftwareManagment_View = 653;
        public const int OPERATION_ApplicationModule_Statistics = 654;

        public const int OPERATION_Document_AddFromFolder = 655;
        public const int OPERATION_SD_General_UnAssignedCalls = 656;

        public const int OPERATION_ParameterEnum_Properties = 657;
        public const int OPERATION_ParameterEnum_Add = 658;
        public const int OPERATION_ParameterEnum_Delete = 659;
        public const int OPERATION_ParameterEnum_Update = 660;

        public const int OPERATION_MessageByEmail_Properties = 735001;

        public const int OPERATION_MessageByEmail_Add = 735002;
        public const int OPERATION_MessageByEmail_Delete = 735003;

        public const int OPERATION_MessageByEmail_Update = 735004;

        public const int OPERATION_MessageByMonitoring_Properties = 736001;

        public const int OPERATION_MessageByMonitoring_Add = 736002;

        public const int OPERATION_MessageByMonitoring_Delete = 736003;

        public const int OPERATION_MessageByMonitoring_Update = 736004;

        public const int OPERATION_MessageByInquiry_Properties = 737001;

        public const int OPERATION_MessageByInquiry_Add = 737002;

        public const int OPERATION_MessageByInquiry_Delete = 737003;

        public const int OPERATION_MessageByInquiry_Update = 737004;

        public const int OPERATION_CommonFilters_EditForTasks = 744001;
        public const int OPERATION_CommonFilters_EditForCall = 744002;
        public const int OPERATION_CommonFilters_EditForWorkOrders = 744003;
        public const int OPERATION_CommonFilters_EditForProblems = 744004;
        public const int OPERATION_CommonFilters_EditForRFC = 744005;
        public const int OPERATION_CommonFilters_EditForMyCalls = 744006;
        public const int OPERATION_CommonFilters_EditForNegotiations = 744007;
        public const int OPERATION_CommonFilters_EditForControl = 744008;

        public const int OPERATION_LicenceScheme_Properties = 750001;
        public const int OPERATION_LicenceScheme_Create = 750002;
        public const int OPERATION_LicenceScheme_Edit = 750003;
        public const int OPERATION_LicenceScheme_Delete = 750004;

        public const int OPERATION_Dashboard_AddAs = 152005;

        public const int OPERATION_CalendarWeekend_Properties = 667;
        public const int OPERATION_CalendarWeekend_Add = 668;
        public const int OPERATION_CalendarWeekend_Delete = 669;
        public const int OPERATION_CalendarWeekend_Update = 670;

        public const int OPERATION_CalendarHoliday_Properties = 671;
        public const int OPERATION_CalendarHoliday_Add = 672;
        public const int OPERATION_CalendarHoliday_Delete = 673;
        public const int OPERATION_CalendarHoliday_Update = 674;

        public const int OPERATION_Exclusion_Properties = 675;
        public const int OPERATION_Exclusion_Add = 676;
        public const int OPERATION_Exclusion_Delete = 677;
        public const int OPERATION_Exclusion_Update = 678;

        public const int OPERATION_CalendarWorkSchedule_Properties = 679;
        public const int OPERATION_CalendarWorkSchedule_Add = 680;
        public const int OPERATION_CalendarWorkSchedule_Delete = 681;
        public const int OPERATION_CalendarWorkSchedule_Update = 682;
        public const int OPERATION_CalendarWorkSchedule_AddAs = 683;

        public const int OPERATION_Solution_Properties = 684;
        public const int OPERATION_Solution_Add = 685;
        public const int OPERATION_Solution_Delete = 686;
        public const int OPERATION_Solution_Update = 687;

        public const int OPERATION_Note_MarkAsReaded = 688;
        public const int OPERATION_Note_MarkAsUnread = 689;

        public const int OPERATION_MessageByInquiryTaskForAsset_Properties = 690;
        public const int OPERATION_MessageByInquiryTaskForAsset_Delete = 692;

        public const int OPERATION_MessageByIntegration_Properties = 694;
        public const int OPERATION_MessageByIntegration_Delete = 696;

        public const int OPERATION_MessageByOrganizationStructureImport_Properties = 698;
        public const int OPERATION_MessageByOrganizationStructureImport_Delete = 700;

        public const int OPERATION_MessageByInquiryTaskForUsers_Properties = 701;
        public const int OPERATION_MessageByInquiryTaskForUsers_Delete = 702;

        public const int OPERATION_Call_ViewWithAccessGranted = 703;
        public const int OPERATION_Call_ViewByMyService = 704;
        public const int OPERATION_Call_ViewAll = 705;

        public const int OPERATION_SD_General_ParticipateInAutoAssign = 708;

        #region AShtoppel
        public const int OBJ_TASK_OF_IMPORT = 126;

        public const int OPERATION_TASKOFIMPORT_ADD = 351;
        public const int OPERATION_TASKOFIMPORT_ADDAS = 334;
        public const int OPERATION_TASKOFIMPORT_PROPERTIES = 352;
        public const int OPERATION_TASKOFIMPORT_UPDATE = 197;
        public const int OPERATION_TASKOFIMPORT_DELETE = 353;
        public const int OPERATION_TASKOFIMPORT_EXECUTE = 354;
        public const int OPERATION_TASKOFIMPORT_SHEDULER = 355;

        public const int OBJ_NETWORK_TERMINAL_DEVICE = 126;
        public const int OPERATION_CHANGEOFSLOT = 402;
        public const int OPERATION_CHANGEOFSLOT2 = 403;
        public const int OBJ_DELETESLOT = 404;
        public const int OBJ_DELETEPORT = 405;

        public const int OPERATION_CHANGEOFPORTTECHNOLOGY = 406;
        public const int OPERATION_CHANGEOFPORTKINDCUTOFFPOINT = 407;

        public const int OPERATION_SOFTWARELICENCESERIALNUMBER_ADD = 461;
        public const int OPERATION_SOFTWARELICENCESERIALNUMBER_AddFromFile = 797;
        public const int OPERATION_SOFTWARELICENCESERIALNUMBER_DELETE = 462;
        public const int OPERATION_SOFTWARELICENCESERIALNUMBER_UPDATE = 466;
        public const int OPERATION_SOFTWARELICENCESERIALNUMBER_PROPERTIES = 523;

        public const int OPERATION_SOFTWARELICENCEREFERENCE_ADD = 463;
        public const int OPERATION_SOFTWARELICENCEREFERENCE_DELETE = 464;
        public const int OPERATION_SOFTWARELICENCEREFERENCE_PROPERTIES = 798;

        public const int OPERATION_UndisposedDevice_Delete = 465;

        public const int OPERATION_Compare = 467;

        #endregion

        public const int OPERATION_SideOrganization_Add = 413;
        public const int OPERATION_SideOrganization_Delete = 414;
        public const int OPERATION_SideOrganization_Properties = 415;
        public const int OPERATION_SideOrganization_Update = 416;

        public const int OPERATION_SnmpDeviceModel_Properties = 417;
        public const int OPERATION_SnmpDeviceModel_Add = 418;
        public const int OPERATION_SnmpDeviceModel_Delete = 419;
        public const int OPERATION_SnmpDeviceUnknown_Properties = 420;
        public const int OPERATION_SnmpDeviceUnknown_Delete = 421;

        public const int OPERATION_SnmpDeviceProfile_Properties = 691;
        public const int OPERATION_SnmpDeviceProfile_Add = 693;
        public const int OPERATION_SnmpDeviceProfile_Delete = 695;

        public const int OPERATION_SnmpDeviceSensor_Properties = 697;
        public const int OPERATION_SnmpDeviceSensor_Add = 699;
        public const int OPERATION_SnmpDeviceSensor_Delete = 709;

        public const int OPERATION_ExternalUtility_Add = 423;
        public const int OPERATION_ExternalUtility_Delete = 424;
        public const int OPERATION_ExternalUtility_Properties = 425;
        public const int OPERATION_ExternalUtility_Update = 426;
        public const int OPERATION_ExternalUtility_Execute = 427;


        public const int OPERATION_PURCHASE_PROPERTIES = 22201;
        public const int OPERATION_PURCHASE_DELETE = 22202;
        public const int OPERATION_PURCHASE_ADD = 22203;
        public const int OPERATION_PURCHASE_UPDATE = 22204;
        public const int OPERATION_PURCHASE_CLONE = 22205;
        public const int OPERATION_PURCHASE_PRINT = 22206;

        public const int OPERATION_Call_Pick = 701009;
        public const int OPERATION_WorkOrder_Pick = 119009;

        public const int OPERATION_Call_Transmit = 701010;
        public const int OPERATION_Problem_Transmit = 702008;
        public const int OPERATION_WorkOrder_Transmit = 119010;
        public const int OPERATION_RFC_Transmit = 703010;

        public const int OPERATION_CostCategory_Properties = 710;
        public const int OPERATION_CostCategory_Add = 711;
        public const int OPERATION_CostCategory_Delete = 712;
        public const int OPERATION_CostCategory_Update = 713;

        public const int OPERATION_CostCondition_Properties = 714;
        public const int OPERATION_CostCondition_Add = 715;
        public const int OPERATION_CostCondition_Delete = 716;
        public const int OPERATION_CostCondition_Update = 717;

        public const int OPERATION_CostRule_Properties = 725;
        public const int OPERATION_CostRule_Add = 726;
        public const int OPERATION_CostRule_Delete = 727;
        public const int OPERATION_CostRule_Update = 728;

        public const int OPERATION_CostDistributionRule_Properties = 729;
        public const int OPERATION_CostDistributionRule_Add = 730;
        public const int OPERATION_CostDistributionRule_Delete = 731;
        public const int OPERATION_CostDistributionRule_Update = 732;

        public const int OPERATION_WorkCostRule_Properties = 733;
        public const int OPERATION_WorkCostRule_Add = 734;
        public const int OPERATION_WorkCostRule_Delete = 735;
        public const int OPERATION_WorkCostRule_Update = 736;

        public const int OPERATION_Cost_Properties = 737;
        public const int OPERATION_Cost_Add = 738;
        public const int OPERATION_Cost_Delete = 739;
        public const int OPERATION_Cost_Update = 740;

        public const int OPERATION_CostSetting_Properties = 741;
        public const int OPERATION_CostSetting_Add = 742;
        public const int OPERATION_CostSetting_Delete = 743;
        public const int OPERATION_CostSetting_Update = 744;
        public const int OPERATION_CostSetting_Execute = 745;
        public const int OPERATION_CostSetting_Sheduler = 746;
        public const int OPERATION_CostSetting_Abort = 747;

        public const int OPERATION_ITSystem_Properties = 748;
        public const int OPERATION_ITSystem_Add = 749;
        public const int OPERATION_ITSystem_Delete = 750;
        public const int OPERATION_ITSystem_Update = 751;

        public const int OPERATION_Work_Properties = 752;
        public const int OPERATION_Work_Add = 753;
        public const int OPERATION_Work_Delete = 754;
        public const int OPERATION_Work_Update = 755;

        public const int OPERATION_ServiceContractType_Properties = 756;

        public const int OPERATION_ProjectState_Properties = 782;
        public const int OPERATION_ProjectState_Add = 783;
        public const int OPERATION_ProjectState_Delete = 784;
        public const int OPERATION_ProjectState_Update = 785;

        public const int OPERATION_Project_Properties = 786;
        public const int OPERATION_Project_Add = 787;
        public const int OPERATION_Project_Delete = 788;
        public const int OPERATION_Project_Update = 789;
        public const int OPERATION_Project_AddSubProject = 790;
        public const int OPERATION_Project_DeleteSubProject = 791;

        public const int OPERATION_EmailQuoteTrimmer_Properties = 198;
        public const int OPERATION_EmailQuoteTrimmer_Add = 199;
        public const int OPERATION_EmailQuoteTrimmer_Delete = 200;

        public const int OPERATION_HtmlTagWorker_Properties = 718;
        public const int OPERATION_HtmlTagWorker_Add = 719;
        public const int OPERATION_HtmlTagWorker_Delete = 720;
        public const int OPERATION_HtmlTagWorker_AddAs = 721;

        public const int OPERATION_ADD_ACTIVEPORT = 722;
        public const int OPERATION_DELETE_ACTIVEPORT = 723;

        public const int OPERATION_UserActivityType_Add = 76;
        public const int OPERATION_UserActivityType_Delete = 304;
        public const int OPERATION_UserActivityType_Properties = 311;
        public const int OPERATION_UserActivityType_Update = 312;

        public const int OPERATION_ManhoursWork_Add = 314;
        public const int OPERATION_ManhoursWork_Delete = 316;
        public const int OPERATION_ManhoursWork_Properties = 317;
        public const int OPERATION_ManhoursWork_Update = 318;

        public const int OPERATION_ServiceUnit_Add = 794;
        public const int OPERATION_ServiceUnit_Delete = 795;
        public const int OPERATION_ServiceUnit_Properties = 793;
        public const int OPERATION_ServiceUnit_Update = 796;

        public const int OPERATION_ProductCatalogCategory_Add = 800;
        public const int OPERATION_ProductCatalogCategory_Delete = 801;
        public const int OPERATION_ProductCatalogCategory_Properties = 799;
        public const int OPERATION_ProductCatalogCategory_Update = 802;

        public const int OPERATION_LifeCycle_Add = 804;
        public const int OPERATION_LifeCycle_AddAs = 805;
        public const int OPERATION_LifeCycle_Delete = 806;
        public const int OPERATION_LifeCycle_Properties = 803;
        public const int OPERATION_LifeCycle_Update = 807;

        public const int OPERATION_ProductCatalogType_Add = 809;
        public const int OPERATION_ProductCatalogType_Delete = 810;
        public const int OPERATION_ProductCatalogType_Properties = 808;
        public const int OPERATION_ProductCatalogType_Update = 811;
        public const int OPERATION_ProductCatalogType_CopyParameters = 775;

        public const int OPERATION_FixedAsset_Properties = 812;
        public const int OPERATION_FixedAsset_Bind = 813;
        public const int OPERATION_FixedAsset_Delete = 814;
        public const int OPERATION_FixedAsset_Update = 815;

        public const int OPERATION_ActivesRequestSpecification_Add = 817;
        public const int OPERATION_ActivesRequestSpecification_Delete = 818;
        public const int OPERATION_ActivesRequestSpecification_Properties = 816;
        public const int OPERATION_ActivesRequestSpecification_Update = 819;

        public const int OPERATION_FixedAssetSettingConfiguration_Add = 821;
        public const int OPERATION_FixedAssetSettingConfiguration_Delete = 822;
        public const int OPERATION_FixedAssetSettingConfiguration_Properties = 820;
        public const int OPERATION_FixedAssetSettingConfiguration_Update = 823;

        public const int OPERATION_FixedAssetSetting_Add = 825;
        public const int OPERATION_FixedAssetSetting_Delete = 826;
        public const int OPERATION_FixedAssetSetting_Properties = 824;
        public const int OPERATION_FixedAssetSetting_Update = 827;
        public const int OPERATION_FixedAssetSetting_Run = 830;
        public const int OPERATION_FixedAssetSetting_Abort = 831;
        public const int OPERATION_FixedAssetSetting_Schedule = 828;
        public const int OPERATION_FixedAssetSetting_AddAs = 829;

        public const int OPERATION_PurchaseSpecification_Add = 833;
        public const int OPERATION_PurchaseSpecification_Delete = 834;
        public const int OPERATION_PurchaseSpecification_Properties = 832;
        public const int OPERATION_PurchaseSpecification_Update = 835;

        public const int OPERATION_GoodsInvoice_Properties = 836;
        public const int OPERATION_GoodsInvoice_Add = 837;
        public const int OPERATION_GoodsInvoice_Delete = 838;
        public const int OPERATION_GoodsInvoice_Update = 839;

        public const int OPERATION_GoodsInvoiceSpecification_Properties = 840;
        public const int OPERATION_GoodsInvoiceSpecification_Add = 841;
        public const int OPERATION_GoodsInvoiceSpecification_Delete = 842;
        public const int OPERATION_GoodsInvoiceSpecification_Update = 843;

        public const int OPERATION_SoftwareLicenceModel_Properties = 844;
        public const int OPERATION_SoftwareLicenceModel_Add = 845;
        public const int OPERATION_SoftwareLicenceModel_Delete = 846;
        public const int OPERATION_SoftwareLicenceModel_Update = 847;

        public const int OPERATION_FinanceAction_Properties = 848;
        public const int OPERATION_FinanceAction_Add = 849;
        public const int OPERATION_FinanceAction_Delete = 850;
        public const int OPERATION_FinanceAction_Update = 851;

        public const int OPERATION_FinanceBudget_Properties = 852;
        public const int OPERATION_FinanceBudget_Add = 853;
        public const int OPERATION_FinanceBudget_Delete = 854;
        public const int OPERATION_FinanceBudget_Update = 855;

        public const int OPERATION_FinanceBudgetRow_Properties = 856;
        public const int OPERATION_FinanceBudgetRow_Add = 857;
        public const int OPERATION_FinanceBudgetRow_Delete = 858;
        public const int OPERATION_FinanceBudgetRow_Update = 859;

        public const int OPERATION_FinanceCenter_Properties = 860;
        public const int OPERATION_FinanceCenter_Add = 861;
        public const int OPERATION_FinanceCenter_Delete = 862;
        public const int OPERATION_FinanceCenter_Update = 863;

        public const int OPERATION_SupplierContactPerson_Properties = 864;
        public const int OPERATION_SupplierContactPerson_Add = 865;
        public const int OPERATION_SupplierContactPerson_Delete = 866;
        public const int OPERATION_SupplierContactPerson_Update = 867;

        public const int OPERATION_SoftwareModelUsingType_Properties = 868;
        public const int OPERATION_SoftwareModelUsingType_Add = 869;
        public const int OPERATION_SoftwareModelUsingType_Delete = 870;
        public const int OPERATION_SoftwareModelUsingType_Update = 871;

        public const int OPERATION_ServiceContractAgreement_Properties = 872;
        public const int OPERATION_ServiceContractAgreement_Add = 873;
        public const int OPERATION_ServiceContractAgreement_Delete = 874;
        public const int OPERATION_ServiceContractAgreement_Update = 875;

        public const int OPERATION_SupplierType_Properties = 876;
        public const int OPERATION_SupplierType_Add = 877;
        public const int OPERATION_SupplierType_Delete = 878;
        public const int OPERATION_SupplierType_Update = 879;

        public const int OPERATION_ServiceContractMaintenance_Properties = 880;
        public const int OPERATION_ServiceContractMaintenance_Add = 881;
        public const int OPERATION_ServiceContractMaintenance_Delete = 882;
        public const int OPERATION_ServiceContractMaintenance_Update = 883;

        public const int OPERATION_Asset_ViewRepairList = 884;
        public const int OPERATION_Asset_ViewWriteOffList = 885;

        public const int OPERATION_ServiceContractModel_Properties = 886;
        public const int OPERATION_ServiceContractModel_Add = 887;
        public const int OPERATION_ServiceContractModel_Delete = 888;
        public const int OPERATION_ServiceContractModel_Update = 889;

        public const int OPERATION_ServiceContractLicence_Properties = 891;
        public const int OPERATION_ServiceContractLicence_Add = 892;
        public const int OPERATION_ServiceContractLicence_Delete = 893;
        public const int OPERATION_ServiceContractLicence_Update = 894;

        public const int OPERATION_InventorySpecification_Properties = 896;
        public const int OPERATION_InventorySpecification_Add = 897;
        public const int OPERATION_InventorySpecification_Delete = 898;
        public const int OPERATION_InventorySpecification_Update = 899;

        public const int OPERATION_ProductCatalogImportSetting_Add = 900;
        public const int OPERATION_ProductCatalogImportSetting_AddAs = 915;
        public const int OPERATION_ProductCatalogImportSetting_Update = 901;
        public const int OPERATION_ProductCatalogImportSetting_Delete = 902;
        public const int OPERATION_ProductCatalogImportSetting_Properties = 903;
        public const int OPERATION_ProductCatalogImportSetting_Run = 904;
        public const int OPERATION_ProductCatalogImportSetting_Abort = 905;
        public const int OPERATION_ProductCatalogImportSetting_Schedule = 906;
        public const int OPERATION_ProductCatalogImportSetting_Complete = 907;
        public const int OPERATION_ProductCatalogImportSetting_Import = 908;
        public const int OPERATION_ProductCatalogImportSetting_Audit = 909;

        public const int OPERATION_ProductCatalogImportCSVConfiguration_Add = 910;
        public const int OPERATION_ProductCatalogImportCSVConfiguration_Update = 911;
        public const int OPERATION_ProductCatalogImportCSVConfiguration_Delete = 912;
        public const int OPERATION_ProductCatalogImportCSVConfiguration_Properties = 913;
        public const int OPERATION_ProductCatalogImportCSVConfiguration_AddAs = 914;

        public const int OPERATION_GoodsInvoiceImportSetting_Add = 916;
        public const int OPERATION_GoodsInvoiceImportSetting_AddAs = 917;
        public const int OPERATION_GoodsInvoiceImportSetting_Update = 918;
        public const int OPERATION_GoodsInvoiceImportSetting_Delete = 919;
        public const int OPERATION_GoodsInvoiceImportSetting_Properties = 920;
        public const int OPERATION_GoodsInvoiceImportSetting_Run = 921;
        public const int OPERATION_GoodsInvoiceImportSetting_Abort = 922;
        public const int OPERATION_GoodsInvoiceImportSetting_Schedule = 923;
        public const int OPERATION_GoodsInvoiceImportSetting_Complete = 924;
        public const int OPERATION_GoodsInvoiceImportSetting_Import = 925;
        public const int OPERATION_GoodsInvoiceImportSetting_Audit = 926;

        public const int OPERATION_GoodsInvoiceImportConfiguration_Add = 927;
        public const int OPERATION_GoodsInvoiceImportConfiguration_Update = 928;
        public const int OPERATION_GoodsInvoiceImportConfiguration_Delete = 929;
        public const int OPERATION_GoodsInvoiceImportConfiguration_Properties = 930;
        public const int OPERATION_GoodsInvoiceImportConfiguration_AddAs = 931;

        public const int OPERATION_StorageLocation_Add = 932;
        public const int OPERATION_StorageLocation_Update = 933;
        public const int OPERATION_StorageLocation_Delete = 934;
        public const int OPERATION_StorageLocation_Properties = 935;

        public const int OPERATION_ServiceCatalogueImportSetting_Add = 936;
        public const int OPERATION_ServiceCatalogueImportSetting_AddAs = 951;
        public const int OPERATION_ServiceCatalogueImportSetting_Update = 937;
        public const int OPERATION_ServiceCatalogueImportSetting_Delete = 938;
        public const int OPERATION_ServiceCatalogueImportSetting_Properties = 939;
        public const int OPERATION_ServiceCatalogueImportSetting_Run = 940;
        public const int OPERATION_ServiceCatalogueImportSetting_Abort = 941;
        public const int OPERATION_ServiceCatalogueImportSetting_Schedule = 942;
        public const int OPERATION_ServiceCatalogueImportSetting_Complete = 943;
        public const int OPERATION_ServiceCatalogueImportSetting_Import = 944;
        public const int OPERATION_ServiceCatalogueImportSetting_Audit = 945;

        public const int OPERATION_ServiceCatalogueImportCSVConfiguration_Add = 946;
        public const int OPERATION_ServiceCatalogueImportCSVConfiguration_Update = 947;
        public const int OPERATION_ServiceCatalogueImportCSVConfiguration_Delete = 948;
        public const int OPERATION_ServiceCatalogueImportCSVConfiguration_Properties = 949;
        public const int OPERATION_ServiceCatalogueImportCSVConfiguration_AddAs = 950;

        public const int OPERATION_ConfigurationUnit_Add = 952;
        public const int OPERATION_ConfigurationUnit_Update = 953;
        public const int OPERATION_ConfigurationUnit_Delete = 954;
        public const int OPERATION_ConfigurationUnit_Properties = 955;

        public const int OPERATION_Cluster_Add = 956;
        public const int OPERATION_Cluster_Update = 957;
        public const int OPERATION_Cluster_Delete = 958;
        public const int OPERATION_Cluster_Properties = 959;

        public const int OPERATION_AgentFileInfo_Add = 748001;
        public const int OPERATION_AgentFileInfo_Create = 748002;
        public const int OPERATION_AgentFileInfo_Delete = 748003;
        public const int OPERATION_AgentFileInfo_Open = 748004;

        public const int OPERATION_SoftwareDistributionCentre_Add = 961;
        public const int OPERATION_SoftwareDistributionCentre_Update = 963;
        public const int OPERATION_SoftwareDistributionCentre_Delete = 962;
        public const int OPERATION_SoftwareDistributionCentre_Properties = 960;

        public const int OPERATION_DataEntityImportSetting_Add = 964;
        public const int OPERATION_DataEntityImportSetting_AddAs = 979;
        public const int OPERATION_DataEntityImportSetting_Update = 965;
        public const int OPERATION_DataEntityImportSetting_Delete = 966;
        public const int OPERATION_DataEntityImportSetting_Properties = 967;
        public const int OPERATION_DataEntityImportSetting_Run = 968;
        public const int OPERATION_DataEntityImportSetting_Abort = 969;
        public const int OPERATION_DataEntityImportSetting_Schedule = 970;
        public const int OPERATION_DataEntityImportSetting_Complete = 971;
        public const int OPERATION_DataEntityImportSetting_Import = 972;
        public const int OPERATION_DataEntityImportSetting_Audit = 973;

        public const int OPERATION_DataEntityImportCSVConfiguration_Add = 974;
        public const int OPERATION_DataEntityImportCSVConfiguration_Update = 975;
        public const int OPERATION_DataEntityImportCSVConfiguration_Delete = 976;
        public const int OPERATION_DataEntityImportCSVConfiguration_Properties = 977;
        public const int OPERATION_DataEntityImportCSVConfiguration_AddAs = 978;
        
        public const int LDAP_PATH = 11000;

        public const string UserField1Name = "Пользовательское поле 1";
        public const string UserField2Name = "Пользовательское поле 2";
        public const string UserField3Name = "Пользовательское поле 3";
        public const string UserField4Name = "Пользовательское поле 4";
        public const string UserField5Name = "Пользовательское поле 5";

        public enum IMClass
        {
            User = OBJ_USER,

            #region AShtoppel
            ChangeOfSlot = OPERATION_CHANGEOFSLOT,
            ChangeOfSlot2 = OPERATION_CHANGEOFSLOT2,
            DeleteSlot = OBJ_DELETESLOT,
            DeletePort = OBJ_DELETEPORT,
            ChangePortTypeTechnology = OPERATION_CHANGEOFPORTTECHNOLOGY,
            ChangePortKindcutOffPoint = OPERATION_CHANGEOFPORTKINDCUTOFFPOINT,
            #endregion

            Purchase = OBJ_PURCHASE,
            SoftWareLicense = OBJ_SOFTWARE_LICENSE
        }

        public enum ConnectedItem
        {
            Disconnected = 0,
            Cord = 1,
            Cable = 2
        }

        public enum PortState
        {
            Free = 0,
            Busy = 1,
            Faulty = 2,
            Splitted = 3
        }

        public enum SubDeviceState
        {
            Committed = 0,
            Found = 1,
            Removed = 2,
            Nonexistent = 3
        }

        public enum LineState
        {
            Free = 0,
            Engaded = 1,
            Faulty = 2
        }

        public enum AssetStatus
        {
            OK = 0,
            Defect = 1,
            UnderRepair = 2,
            WrittenOff = 3
        }

        public enum PortStatus
        {
            Enabled = 0,
            Disabled = 1
        }

        public enum TelephoneLineType
        {
            Internal = 0,
            External = 1
        }

        public enum MaterialOperation
        {
            Purchase = 1,
            Consume = 2
        }

        public class FastNavigatorTag
        {
            public int oldID;
            public Guid IMObjID;
            public int ClassID;
            public string Name;
            public int IconID;
            public int SubClassID;
            public FastNavigatorTag()
            {
                this.oldID = 0;
                this.IMObjID = Guid.Empty;
                this.ClassID = 0;
                this.Name = string.Empty;
                this.IconID = 0;
                this.SubClassID = 0;
            }
        }

        public enum NavigatorType
        {
            SKSStruct = 1,
            ORGStruct = 2
        }

        public enum SKSNavigator
        {
            Organization = 1,
            Building = 2,
            Floor = 4,
            Room = 8,
            Workplace = 16,
            Rack = 32,
            Outlet = 64,
            User = 128,
            TerminalDevice = 256,
            NetworkDevice = 512,
            Panel = 1024,
            NetworkDevicePort = 2048,
            PanelPort = 4096,
            OutletPort = 8192,
            SplitterPort = 16384,
            Adapter = 32768,
            Peripheral = 65536,
            Installation = 131072,
            Null = 666,
            Owner = 1073741824,
        }

        public enum ORGNavigator
        {
            Organization = 1,
            Division = 2,
            User = 4,
            Owner = 1073741824
        }

        public enum SelectionListMode
        {
            DefaultMode,
            RackInRoom,
            RackInRoom_NoWorkplaces,
            RackInRoom_NoOutlets,
            NoRacks
        }

        public static string GetNormalizedGuid(string guid)
        {
            var leftCurlyBracket = "{";
            if (guid == string.Empty)
            {
                return guid;
            }
            if (guid.Substring(0, 1) == leftCurlyBracket)
            {
                guid = guid.Substring(1, guid.Length - 2);
            }
            return guid.ToLower();
        }

        public static string GetDBGuid(string guid)
        {
            var leftCurlyBracket = "{";
            if (guid == string.Empty)
            {
                return guid;
            }
            if (guid.Substring(0, 1) != leftCurlyBracket)
            {
                guid = leftCurlyBracket + guid + "}";
            }
            return guid.ToUpper();
        }

        public static bool IsPort(long ID)
        {
            return ID / 1000000 == OBJ_PORT ? true : false;
        }

        public static bool IsTerminalDevice(long ID)
        {
            return ID / 1000000 == OBJ_TERMINALDEVICE ? true : false;
        }

        public static bool IsJack(long ID)
        {
            return ID / 1000000 == OBJ_PANELJACK || ID / 1000000 == OBJ_OUTLETJACK || ID / 1000000 == OBJ_SPLITTERJACK ? true : false;
        }

        public static bool IsSplitterJack(long ID)
        {
            return ID / 1000000 == OBJ_SPLITTERJACK ? true : false;
        }

        public static bool IsSplice(long ID)
        {
            return ID / 1000000 == OBJ_SPLICE ? true : false;
        }

        public static string GetLocalApplicationDataDir()
        {
            var path = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\InfraManager";
            if (!System.IO.Directory.Exists(path))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                catch (Exception)
                {

                }
            }

            return path;
        }

        public static string GetCommonProgramFilesDir()
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + "\\InfraManager";
        }

        public static string GetCommonApplicationDataDir()
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\InfraManager";
        }

        public static string GetLocalMyDocumentsDir()
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        public enum SimpleOperation
        {
            Add = SIMPLE_OPERATION_ADD,
            Change = SIMPLE_OPERATION_CHANGE,
            View = SIMPLE_OPERATION_VIEW
        }
    }
}
