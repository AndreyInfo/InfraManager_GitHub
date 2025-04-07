using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.ConnectionParameters;
using InfraManager.Core.Logging;
using InfraManager.UI.Web.Helpers;
using InfraManager.Web.SignalR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DevExpress.DashboardAspNetCore;
using DevExpress.DataAccess.Native;
using InfraManager.BLL;
using InfraManager.BLL.Dashboards;
using InfraManager.BLL.Localization;
using InfraManager.DAL;
using InfraManager.DAL.Settings;
using InfraManager.ResourcesArea;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfraManager.Web.Helpers
{
    public class DashboardHelper
    {
        public readonly IHttpContextAccessor _context;
        public static IWebHostEnvironment _webHostEnvironment;
        private readonly CustomDashboardFileStorage _storage;
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly string _dbType;
        private readonly IServiceProvider _services;
        
        public DashboardHelper(
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment,
            CustomDashboardFileStorage storage,
            IConnectionStringProvider connectionStringProvider,
            IConfiguration configuration,
            IServiceProvider services)
        {
            _context = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _storage = storage;
            _connectionStringProvider = connectionStringProvider;
            _services = services;
            _dbType = configuration["dbType"];
        }
        
        public class CustomDashboardStateService : IDashboardStateService
        {
            public DashboardState GetState(string dashboardId, System.Xml.Linq.XDocument dashboard)
            {
                var dashboardState = new DashboardState();
                //
                var nodes = dashboard.Descendants().ToList();
                var itemsParent = nodes.Find(p => p.Name.LocalName == "Items");
                if (itemsParent != null)
                {
                    var items = dashboard.Descendants().Where(x => x.Parent == itemsParent);
                    //
                    if (items != null)
                        foreach (var item in items)
                        {
                            if (item.Name.LocalName != "TreeView")
                                continue;
                            //
                            var a = item.Attribute("ComponentName");
                            //
                            var filterState = new DashboardItemState(a.Value);
                            filterState.MasterFilterValues.Add(new string[1] { null });
                            dashboardState.Items.Add(filterState);
                        }
                }
                //
                return dashboardState;
            }
        }

        public class CustomDashboardFileStorage : DashboardFileStorage
        {
            public readonly IHttpContextAccessor _context;
            public readonly IHubContext<EventHub> _hubContext;
            public static IWebHostEnvironment _webHostEnvironment;
            private readonly IConnectionStringProvider _connectionStringProvider;
            private readonly IServiceProvider _services;
            
            public CustomDashboardFileStorage(IHttpContextAccessor httpContextAccessor,
                                              IHubContext<EventHub> hubContext,
                                              IWebHostEnvironment webHostEnvironment,
                                              IConnectionStringProvider connectionStringProvider,
                                              IServiceProvider services)
                : base("/Data/Dashboard")
            {
                _context = httpContextAccessor;
                _hubContext = hubContext;
                _webHostEnvironment = webHostEnvironment;
                WorkingDirectory = Path.Join(_webHostEnvironment.WebRootPath, WorkingDirectory);
                _connectionStringProvider = connectionStringProvider;
                _services = services;
            }

            protected override void SaveDashboard(string dashboardID, XDocument dashboard, bool createNew)
            {
                using (var scope = _services.CreateScope())
                {
                    var dashboardBLL =
                        scope.ServiceProvider
                            .GetRequiredService<IDashboardBLL>();

                    dashboardBLL.UpdateDashboardData(Guid.Parse(dashboardID), dashboard.ToString()).GetAwaiter()
                            .GetResult();
                }
            }

            protected override XDocument LoadDashboard(string dashboardID)
            {
                var doc = base.LoadDashboard("dashboard_" + dashboardID);
                Dashboard dashboard = new Dashboard();
                dashboard.LoadFromXDocument(doc);
                //
                dashboard.DashboardLoading += Dashboard_DashboardLoading;
                //
                var auth = new AuthenticationHelper(_context.HttpContext, _hubContext);

                if (!auth.CurrentUserID.HasValue)
                {
                    return null;
                }

                var userIDGuid = auth.CurrentUserID.Value;


                var causerParameter = dashboard.Parameters.FirstOrDefault(x => x.Name == __causerIdParameterName);
                if (causerParameter == null)
                {
                    causerParameter = new DashboardParameter(__causerIdParameterName, typeof(Guid), userIDGuid);
                    dashboard.Parameters.Add(causerParameter);
                }
                else if (causerParameter.Type != typeof(Guid))
                {
                    Logger.Warning(string.Format("Недопустимый тип параметра {0}: {1}. Тип параметра {0} был автоматически преобразован в Guid.", __causerIdParameterName, causerParameter.Type));
                    causerParameter.Type = typeof(Guid);
                    causerParameter.Value = userIDGuid;
                }
                else
                    causerParameter.Value = userIDGuid;
                //
                causerParameter.Visible = false;
                //
                SetSqlCommandTimeout(dashboard);
                //
                return dashboard.SaveToXDocument();
            }

            
            
            private void Dashboard_DashboardLoading(object sender, EventArgs e)
            {

            }

            #region private method SetSqlCommandTimeout
            private void SetSqlCommandTimeout(Dashboard dashboard)
            {
                var currentConnectionObject = _connectionStringProvider.GetConnectionObject();

                foreach (var dataSourceDE in dashboard.DataSources)
                {
                    var sqlDS = dataSourceDE as DashboardSqlDataSource;
                    if (sqlDS != null)
                        sqlDS.ConnectionOptions.DbCommandTimeout = currentConnectionObject.CommandTimeout;
                }
            }
            #endregion
        }

        #region fields
        private static readonly DashboardInMemoryStorage __dashboardStorage = new DashboardInMemoryStorage();
        private static readonly string __causerIdParameterName = "causerID";

        #endregion

        #region fields method Initialize
        public void Initialize()
        {
            DashboardConfigurator.PassCredentials = true;
            DashboardConfigurator.Default.AllowExecutingCustomSql = true;
            //
            DashboardConfigurator.Default.SetDashboardStorage(_storage);
            DashboardConfigurator.Default.ConfigureDataConnection += Default_ConfigureDataConnection;
            DashboardConfigurator.Default.ConnectionError += Default_ConnectionError;
        }

        private void Default_ConnectionError(object sender, ConnectionErrorWebEventArgs e)
        {
            Logger.Error(e.Exception);
        }
        #endregion

        #region handle DashboardConfigurator

        private void Default_ConfigureDataConnection(object sender, ConfigureDataConnectionWebEventArgs e)
        {
            var mssqlType = new[] { "mssql", "ms" };
            var pgType = new[] { "pg" };
            DevExpress.DataAccess.Sql.SqlDataSource.DisableCustomQueryValidation = true;
            DevExpress.DataAccess.Sql.SqlDataSource.AllowCustomSqlQueries = true;

            //TODO: Костыль чтобы хоть как то работало подключение к БД. ВН одобрил. 
            if (e.ConnectionParameters.GetType() == typeof(MsSqlConnectionParameters) && mssqlType.Contains(_dbType))
            {
                SetMsSQLParameters(e);
                return;
            }

            if (e.ConnectionParameters.GetType() == typeof(PostgreSqlConnectionParameters) && pgType.Contains(_dbType))
            {
                SetPGParameters(e);
                return;
            }
            
            string errorText;
            using (var scope = _services.CreateScope())
            {
                var localize =
                    scope.ServiceProvider
                        .GetRequiredService<ILocalizeText>();

                errorText = localize.Localize(nameof(Resources.Dashboard_different_db));
            }

            throw new InvalidObjectException(errorText);
        }

        private void SetPGParameters(ConfigureDataConnectionWebEventArgs e)
        {
            var connectParams = new CustomStringConnectionParameters();
            connectParams.ConnectionString = "XpoProvider=Postgres;" + _connectionStringProvider.GetConnectionString();
            e.ConnectionParameters = connectParams;
        }

        private void SetMsSQLParameters(ConfigureDataConnectionWebEventArgs e)
        {
            var parameters = (MsSqlConnectionParameters)e.ConnectionParameters;

            var currentConnectionObject = _connectionStringProvider.GetConnectionObject();
            parameters.ServerName = currentConnectionObject.Server;
            parameters.DatabaseName = currentConnectionObject.Database;
            parameters.UserName = currentConnectionObject.Login;
            parameters.Password = currentConnectionObject.Password;
            parameters.AuthorizationType = MsSqlAuthorizationType.SqlServer;
        }
        #endregion

        #region method RegisterDashboard
        public void RegisterDashboard()
        {
            var request = _context.HttpContext.Request.Query;
            var dashboardID = request["dashboardID"].ToString(); //edited//
                                                                 //
            if (dashboardID != null)
            {
                string dashboardIDStr = string.Concat("dashboard_", dashboardID);
                __dashboardStorage.RegisterDashboard(dashboardIDStr, XDocument.Load(GetXMLPath(dashboardID)));
            }
        }
        #endregion

        #region public method ResetDashboard
        public void ResetDashboard(string dashboardID, string xml)
        {
            if (string.IsNullOrEmpty(xml))
                throw new InvalidOperationException("xml");
            //
            //var request = HttpContext.Current.Request;
            //
            string xmlDataPath = GetXMLPath(dashboardID);
            //
            DeleteOldFiles();
            //
            xml = xml.TrimEnd('\0');//DE v15.2 bug
                                    //
            if (!File.Exists(xmlDataPath))
                File.WriteAllText(xmlDataPath, xml);
            else
            {
                string oldXmlData = File.ReadAllText(xmlDataPath);
                if (string.Compare(oldXmlData, xml) != 0)
                    File.WriteAllText(xmlDataPath, xml);
            }
        }
        #endregion

        #region private method DeleteOldFiles
        private /*static*/ void DeleteOldFiles()
        {
            string xmlDataDirectory = GetMapPath("/Data/Dashboard");
            foreach (string filePath in Directory.GetFiles(xmlDataDirectory))
            {
                var fileAge = DateTime.Now - File.GetCreationTime(filePath);
                if (fileAge.TotalDays > 1)
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, String.Concat("Ошибка при попытке удаления файла. Путь к файлу: ", filePath ?? string.Empty));
                    }
            }
        }
        #endregion

        #region private method GetXMLPath
        private string GetXMLPath(string dashboardID)
        {
            return string.Concat(GetMapPath("/Data/Dashboard/"), "dashboard_", dashboardID, ".xml");
        }
        #endregion
        private string GetMapPath(string path)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            return Path.Join(webRootPath, path);
        }
    }
}