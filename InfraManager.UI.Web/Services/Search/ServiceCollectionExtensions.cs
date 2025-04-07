using System.Globalization;
using InfraManager.BLL.ServiceDesk.Search;
using InfraManager.BLL.Authentication;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.ServiceDesk;
using InfraManager.ResourcesArea;
using InfraManager.UI.Web.Services.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.UI.Web.Services.Search
{
    public static class ServiceCollectionExtensions
    {
        public static void AddViewLocalization(this IServiceCollection services)
        {
            services.AddControllersWithViews().AddDataAnnotationsLocalization(options => {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(Resources));
            }).AddViewLocalization();

            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(opt =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("ru-RU"),
                    new CultureInfo("en-US")
                };
                opt.DefaultRequestCulture = new RequestCulture("ru-RU");
                opt.SupportedCultures = supportedCultures;
                opt.SupportedUICultures = supportedCultures;
            });
        }


        public static void AddSearchService(this IServiceCollection services)
        {
            services.AddScoped<IServiceDeskSearchStrategy<SearchByTextParameters>, SearchByTextStrategy>();
            services.AddSingleton<ISearchService, SearchServiceProxy>();

            services
                .AddEntityWithNotesUpdater<Call>()
                .AddEntityWithNotesUpdater<WorkOrder>()
                .AddEntityWithNotesUpdater<Problem>()
                .AddEntityWithNotesUpdater<MassIncident>();
        }

        private static IServiceCollection AddEntityWithNotesUpdater<T>(this IServiceCollection services)
            where T : class, IGloballyIdentifiedEntity =>            
            services
                .AddScoped<IVisitNewEntity<T>, SearchServiceEntityUpdater<T, Note<T>>>()
                .AddScoped<IVisitModifiedEntity<T>, SearchServiceEntityUpdater<T, Note<T>>>()
                .AddScoped<IVisitDeletedEntity<T>, SearchServiceEntityUpdater<T, Note<T>>>()
                .AddScoped<IVisitNewEntity<Note<T>>, SearchServiceNoteUpdater<T>>()
                .AddScoped<IVisitModifiedEntity<Note<T>>, SearchServiceNoteUpdater<T>>()
                .AddScoped<IVisitDeletedEntity<Note<T>>, SearchServiceNoteUpdater<T>>();

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationByLDAP>();
        }
    }
}