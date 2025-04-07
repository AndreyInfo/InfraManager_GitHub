using DevExpress.AspNetCore;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardWeb;
using IM.Core.DM.BLL;
using IM.Core.HttpInfrastructure;
using IM.Core.WF.BLL;
using InfraManager.BLL;
using InfraManager.BLL.Scheduler;
using InfraManager.BLL.ServiceDesk.CustomValues;
using InfraManager.BLL.ServiceDesk.Search;
using InfraManager.BLL.Settings;
using InfraManager.BLL.Web.Settings;
using InfraManager.Core;
using InfraManager.Core.Data;
using InfraManager.CrossPlatform.BLL.Intrefaces.Schedule;
using InfraManager.CrossPlatform.WebApi.Contracts.Auth;
using InfraManager.DAL;
using InfraManager.DAL.DbConfiguration;
using InfraManager.ServiceBase;
using InfraManager.Services;
using InfraManager.Services.MailService;
using InfraManager.UI.Web.Configuration;
using InfraManager.UI.Web.Filters;
using InfraManager.UI.Web.Helpers;
using InfraManager.UI.Web.Middleware;
using InfraManager.UI.Web.ModelBinding;
using InfraManager.UI.Web.PathBuilders;
using InfraManager.UI.Web.Services.Search;
using InfraManager.UI.Web.SignalR;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using InfraManager.WebAPIClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.IO;
using System.Linq;
using InfraManager.UI.Web.CookieEvents;
using InfraManager.UI.Web.Services;
using Newtonsoft.Json;
using WebOptimizer.Sass;
using static InfraManager.Web.Helpers.DashboardHelper;
using DevExpress.XtraReports.Web.Extensions;
using DevExpress.AspNetCore.Reporting;
using DevExpress.DashboardCommon;
using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.DevExpress;
using InfraManager.DAL.Settings;

namespace InfraManager.UI.Web
{
    public class Startup
    {
        //TODO для чего?
        public static IConfiguration CofigurationCostylie { get; set; }

        public IWebHostEnvironment Environment { get; set; }

        IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            CofigurationCostylie = configuration;
            Environment = env;

            var alternativeSettingsFilePath = Path.Combine(env.ContentRootPath, _configuration["SettingFileAlt"]);
            ResourcesArea.Global.ExeConfigurationPath =
                File.Exists(alternativeSettingsFilePath)
                    ? alternativeSettingsFilePath
                    : Path.Combine(env.ContentRootPath, _configuration["SettingFile"]);
            var alternativeLocatorFile = Path.Combine("settings", "old-datasource-locator.xml");
            DataSourceManager.Instance.Initialize(
                new[] { File.Exists(alternativeLocatorFile) ? alternativeLocatorFile : "old-datasource-locator.xml" });
            DataSourceManager.Instance.Start();
            ServiceManager.Instance = new HttpServiceManager();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            ApplicationManager.Instance.ApplicationVersion = configuration["ApplicationVersion"];
            ApplicationManager.Instance.WorkflowServiceBaseURL = configuration["Settings:WorkflowServiceBaseURL"];
            ApplicationManager.Instance.SearchServiceBaseURL = configuration["Settings:SearchServiceBaseURL"];
            ApplicationManager.Instance.MailServiceBaseURL = configuration["Settings:MailServiceBaseURL"];
            ApplicationManager.Instance.TelephonyServiceBaseURL = configuration["Settings:TelephonyServiceBaseURL"];
            ApplicationManager.Instance.WebAPISecret = configuration["TokenAuthenticationOptions:TokenPassword"];
            ApplicationManager.Instance.IsWebApplication = true;
            ApplicationManager.Instance.ImportServiceBaseURL = configuration["Settings:ImportServiceBaseURL"];

            DashboardExportSettings.CompatibilityMode = DashboardExportCompatibilityMode.Restricted;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            services.AddAutoMapper(
                typeof(BLL.ServiceCollectionExtensions).Assembly,
                typeof(DAL.ServiceCollectionExtensions).Assembly,
                GetType().Assembly);

            var dbType = _configuration.GetValue<string>("dbType")?.ToLower();
            var supportedTypes = new[] { "pg", "ms", "mssql" };
           
            if (!supportedTypes.Contains(dbType))
            {
                throw new ApplicationException($"Database engine type {dbType} is not supported or specified");
            }

            if (dbType == "pg")
            {
                services.UsePgSql();
            }

            if (dbType == "mssql" || dbType == "ms")
            {
                services.UseMsSql();
            }

            services.AddDAL();
            services.AddBLL();
            services.AddLookupQueries();
            services.AddScoped<IUserContextProvider, UserContextProvider>(); // TODO: Should be replaced by lightweight ICurrentUser
            services.AddScoped<ICurrentUser, HttpContextCurrentUser>();
            services.AddScoped<IDefaultClientCultureProvider, ClientCultureProvider>();
            services.AddScoped<IUserLanguageChecker, UserLanguageChecker>();
            services.AddSingleton<IAppSettingsEditor, AppSettingsEditor>();
            services.AddSingleton<IOldDataSourceLocatorEditor, OldDataSourceLocatorEditor>();
            services.AddScoped<CustomValueFactory>();
            services.AddPathBuilders();

            services.AddWfBLL();
            services.AddDMBLL();

            services.AddSearchService();
            services.AddServices();
            var maxSearchCacheSize = (int)Math.Pow(10d, 6d);
            services.AddSingleton<IServiceDeskSearchCache>(new ServiceDeskSearchCache(maxSearchCacheSize));
            services.AddScoped<ServiceDeskSearchService>();

            // Настройка Swagger

            if (SwaggerUIEnabled())
            {
                services.AddSwaggerGen(swaggerOptions =>
                {
                    swaggerOptions.CustomSchemaIds(x => x.FullName.Replace("+", "."));
                    swaggerOptions.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                });
            }

            services
                .AddSingleton<IScheduleBLL>(x =>
                {
                    var config = x.GetService<IConfiguration>();
                    return new ScheduleBLL(config.GetConnectionString("scheduler-service"), x.GetService<ILogger<ScheduleBLL>>());
                });

            services.AddServices(_configuration);

            services.AddAuthentication("JWT_OR_COOKIE")
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new PathString("/Account/Authenticate");
                    options.EventsType = typeof(OnSignInEvent);
                })
                .AddScheme<AuthenticationSchemeOptions, TokenAuthenticationHandler>(TokenAuthenticationScheme.SchemeName, null)
                .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", option =>
                {
                    option.ForwardDefaultSelector = cotext =>
                    {
                        string auth = cotext.Request.Headers[HeaderNames.Authorization];
                        if (!string.IsNullOrEmpty(auth) && auth.StartsWith(TokenAuthenticationScheme.SchemeName))
                            return TokenAuthenticationScheme.SchemeName;
                        return CookieAuthenticationDefaults.AuthenticationScheme;
                    };
                });
            services.AddScoped<OnSignInEvent>();
            services.AddScoped<LocationProvider>();

            services.Configure<TokenAuthenticationOptions>(option => _configuration.GetSection("TokenAuthenticationOptions").Bind(option));
            services.Configure<DocumentsSettingsOptions>(option => _configuration.GetSection("FileOptions").Bind(option));

            
            services.AddCors();

            services.AddControllers(options =>
            {
                options.ModelBinderProviders.InsertBodyOrFormBinding();
                options.Filters.Add<ExceptionFilter>();
            });

            services
                .AddControllersWithViews()
                .AddNewtonsoftJson(
                    jo =>
                    {
                        jo.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        jo.SerializerSettings.ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = null,
                        };
                        jo.SerializerSettings.Converters.Add(new NullableGuidPropertyConverter());
                        jo.SerializerSettings.Converters.Add(new NullableIntPropertyConverter());
                        jo.SerializerSettings.Converters.Add(new FieldTypesPropertyConverter());
                        jo.SerializerSettings.MaxDepth = int.MaxValue;
                    });
            services.AddSignalR();
            services.AddResponseCompression(
                options =>
                {
                    options.Providers.Add<DeflateCompressionProvider>();
                    options.Providers.Add<GzipCompressionProvider>();
                });

            services.AddHostedService<HubWorker>();

            // Temporary solution for PostgreSQL support DataManagers
            services.AddHostedService<PgSqlDataManagerActivator>();

            //NEW
            services.AddHttpContextAccessor();
            services.AddTransient<CustomDashboardFileStorage>();
            services.AddSingleton<DashboardHelper>();

            //local
            services.AddViewLocalization();

            services.AddDevExpressControls();
            
            services.AddScoped(serviceProvider =>
            {
                var connectionStringProvider = serviceProvider.GetService<IConnectionStringProvider>();
                return new CustomDevExpressConnectionStringProvider(connectionStringProvider, dbType);
            });

            services.AddScoped(serviceProvider =>
            {
                var defaultConfigurator = DashboardConfigurator.Default;
                defaultConfigurator.SetConnectionStringsProvider(serviceProvider
                    .GetRequiredService<CustomDevExpressConnectionStringProvider>());
                
                return defaultConfigurator;
            });

            services.AddWebOptimizer(pipeline =>
                {
                    WebOptimizerScssOptions options = new WebOptimizerScssOptions()
                    {
                        OutputStyle = DartSassHost.OutputStyle.Compressed,
                    };
                    pipeline.AddScssBundle(options, "/Styles/redesign.min.css", "/Styles/redesign.scss");
                }
            );

            services.AddScoped<IMailService>(provider => new MailServiceClient(_configuration["Settings:MailServiceBaseURL"]));

            services.AddHostedService<SessionWatcherService>();
            services.AddScoped<ReportStorageWebExtension, CustomReportStorageWebExtension>();
            services.ConfigureReportingServices(configurator =>
            {
                configurator.ConfigureReportDesigner(designerConfigurator =>
                {
                    designerConfigurator.EnableCustomSql();
                });
                configurator.UseAsyncEngine();
            });
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<FileUploadFilter>();
            });

#if Demo
            services.AddDemoWatcherService();
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DashboardHelper dh)
        {
            if (string.IsNullOrWhiteSpace(env.ContentRootPath))
                env.ContentRootPath = Directory.GetCurrentDirectory();
            if (string.IsNullOrWhiteSpace(env.WebRootPath))
                env.WebRootPath = Path.Combine(env.ContentRootPath, "wwwroot");

            JsPluginHelper.Initialize(env);

            if (SwaggerUIEnabled())
            {
                app.UseSwagger();

                app.UseSwaggerUI(swaggerOptions =>
                {
                    swaggerOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            }

            app.UseWebOptimizer();

            app.UseRequestLocalization();


            app.UseStaticFiles();

            app.UseDevExpressControls();
            dh.Initialize();

            app.UseRouting();

            var corsHosts = _configuration.GetSection("AllowCors").Get<string[]>();
            corsHosts ??= new[] { "http://localhost:8080" };

            app.UseCors(options => options.WithOrigins(corsHosts)
                .AllowAnyMethod()
                .AllowCredentials()
                .AllowAnyHeader());

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<AquireRequestStateMiddleware>();


            app.UseEndpoints(endpoints =>
            {
                EndpointRouteBuilderExtension.MapDashboardRoute(endpoints, "api/DevExpressCore", "DevExpressCore");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=SD}/{action=Table}/{id?}");
                endpoints.MapHub<EventHub>("/events");
            });
            SettingsPath.Value = ResourcesArea.Global.ExeConfigurationPath;
        }

        private bool SwaggerUIEnabled()
        {
            return bool.TryParse(_configuration["enableSwaggerUI"], out var swaggerEnabled) && swaggerEnabled;
        }
    }
}
