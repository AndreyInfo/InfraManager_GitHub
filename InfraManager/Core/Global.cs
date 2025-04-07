using System;
using System.IO;
using System.Security.Principal;

namespace InfraManager.Core
{
    public static class Global
    {

        //IMPORTANT. Recompile everything on any changes
        public const string SupportedDBVersion = "5.6.1";
        public const string CompatibleDBVersion = "6.7.128";

        public const string CompatibleReportsDBVersion = "5.2.7";
        public const string ReportsDBName = "IMReports";

        public const string CompatibleGUISettingsVersion = "6.0.1";
        public const string CompatibleFilterSettingsVersion = "6.0.1";

        //
        //TODO: myth -- very bad. no need in overriding system settings.
        //
        public const string DateFormat = "dd.MM.yyyy";
        public const string DateTimeFormat = "dd.MM.yyyy HH:mm:ss";
        public const string DateTimeWithoutSecondsFormat = "dd.MM.yyyy HH:mm";
        public const string TimeFormat = "HH':'mm':'ss";
        public const string TimeWithoutSecondsFormat = "HH':'mm";
        public const string TimeSpanWithoutSecondsFormat = "hh':'mm";

        public const string DecimalFormat = "0.00";
        public const string DecimalFormatWithThousandSeparator = "N";
        public const string NumberFormat = "0";

        public const int MaxItemsToShowOnDeleting = 3;
        public const int MaxItemNameLengthToShowOnDeleting = 50;

#if Demo
        public const int IMDemoMaxTerminalDeviceCount = 200;
        public const int IMDemoMaxAdapterCount = 7000;
        public const int IMDemoMaxNetworkDeviceCount = 100;
        public const int IMDemoMaxSoftwareInstallationCount = 40000;
        public const int IMDemoMaxSoftwareLicenceCount = 100;
        public const int IMDemoMaxCallCount = 500;
        public const int IMDemoMaxWorkOrderCount = 500;
		public const int IMDemoMaxProblemCount = 100;
		public const int IMDemoMaxDeviceMonitorCount = 10;
#endif


        private static readonly string __localApplicationDataDir = string.Concat(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            Path.DirectorySeparatorChar,
            "inframanager",
            Path.DirectorySeparatorChar);
        private static readonly string __commonApplicationDataDir = string.Concat(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            Path.DirectorySeparatorChar,
            "inframanager",
            Path.DirectorySeparatorChar);
        private static readonly string __temporaryFilesDir = string.Concat(
            Path.GetTempPath(),
            "inframanager",
            Path.DirectorySeparatorChar);
        private static readonly string __dbBackupsDir = string.Concat(
            AppDomain.CurrentDomain.BaseDirectory,
            @"dbbackups\");
        private static readonly string __reportsDir = string.Concat(
            AppDomain.CurrentDomain.BaseDirectory,
            @"reports\");
        private static readonly string __schemasDir = string.Concat(
            AppDomain.CurrentDomain.BaseDirectory,
            @"schemas\");
        private static readonly string __tasksDir = string.Concat(
            AppDomain.CurrentDomain.BaseDirectory,
            @"tasks\");
        private static readonly string __mibsDir = string.Concat(
            AppDomain.CurrentDomain.BaseDirectory,
            @"mibs\");
        private static readonly string __templatesDir = string.Concat(
            AppDomain.CurrentDomain.BaseDirectory,
            @"templates\");
        private static readonly string __factorsDir = string.Concat(
            AppDomain.CurrentDomain.BaseDirectory,
            @"factors\");


        #region properties
        /// <summary>
        /// Используется для хранения логов.
        /// </summary>
        public static string LocalApplicationDataDir
        {
            get { return __localApplicationDataDir; }
        }

        /// <summary>
        /// Используется для хранения информации о подключении к БД.
        /// </summary>
        public static string CommonApplicationDataDir
        {
            get { return __commonApplicationDataDir; }
        }

        /// <summary>
        /// Используется для хранения временных файлов.
        /// </summary>
        public static string TemporaryFilesDir
        {
            get { return __temporaryFilesDir; }
        }

        public static string DBBackupsDir
        {
            get { return __dbBackupsDir; }
        }

        public static string ReportsDir
        {
            get { return __reportsDir; }
        }

        public static string SchemasDir
        {
            get { return __schemasDir; }
        }

        //
        //TODO: na hui
        //
        public static string TasksDir
        {
            get { return __tasksDir; }
        }

        public static string MibsDir
        {
            get { return __mibsDir; }
        }

        public static string TemplatesDir
        {
            get { return __templatesDir; }
        }

        public static string FactorsDir
        {
            get { return __factorsDir; }
        }

        public static string ConfigurationFilePath
        {
            get
            {
                return ResourcesArea.Global.ExeConfigurationPath;
            }
        }

        public static string CurrentUserLogin
        {
            get
            {
                using (var windowsIdentity = WindowsIdentity.GetCurrent())
                    return windowsIdentity.Name;
            }
        }

        public static string CurrentUserSID
        {
            get;
            set;
        }

        #endregion

        #region im features

        #region static method GetProtectionValue
        private static T GetProtectionValue<T>(string propertyName) where T : struct
        {
            try
            {
                System.Reflection.Assembly a = null;
                if (ApplicationManager.Instance.IsWebApplication)
                    a = System.Reflection.Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin\\InfraManager.Data.dll"));
                else
                    a = System.Reflection.Assembly.Load("InfraManager.Data");
                //
                if (a == null)
                    return default(T);
                var t = a.GetType("InfraManager.Data.VersionInfo");
                if (t == null)
                    return default(T);
                var mList = t.GetMethods(System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                foreach (System.Reflection.MethodInfo mInfo in mList)
                    if (mInfo.Name == "Get" && mInfo.GetParameters().Length == 0)
                    {

                        var vInfo = mInfo.Invoke(null, new object[] { });
                        if (vInfo == null)
                            return default(T);
                        //
                        var pInfo = vInfo.GetType().GetProperty(propertyName);
                        if (pInfo == null)
                            return default(T);
                        //
                        var retval = (T)pInfo.GetValue(vInfo, null);
                        return retval;
                    }
            }
            catch
            { }
            //
            return default(T);
        }
#endregion

        #region CostEnabled
        public static bool CostEnabled { get; }
        #endregion

        #region BudgetEnabled
        public static bool BudgetEnabled { get; }
        #endregion

        #region WebMobileEnabled
        public static bool WebMobileEnabled { get; }
        #endregion

        #region TimeManagement
        public static bool TimeManagementEnabled { get; }
        #endregion

        #region TelephonyEnabled
        public static bool TelephonyEnabled { get; }
        #endregion

        #region ConcurrentWebUsers
        public static ushort ConcurrentWebUsers { get { return 2; } }
        #endregion

        #region WebPersonalLicenceCount
        public static ushort WebPersonalLicenceCount { get { return 0; } }
        #endregion
#endregion

        #region constructors
        static Global()
        { }
        #endregion
    }
}
