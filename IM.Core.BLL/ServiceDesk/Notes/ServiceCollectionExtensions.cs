using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace InfraManager.BLL.ServiceDesk.Notes
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNoteEvents<TNote>(this IServiceCollection services)
        {
            var customInjector = new CustomInjector().Inject(typeof(TNote).GetObjectClassOrRaiseError());

            return services
                .AddTransient<IConfigureEventWriter<Note<TNote>, Note<TNote>>>(
                    serviceProvider => customInjector.GetService<NoteEventWriterConfigurer<Note<TNote>>>(serviceProvider))
                .AddScoped<NoteEventMessageBuilder<Note<TNote>>>()
                .WriteEventsOf<Note<TNote>>()
                    .WhenAdded(OperationID.Note_Add)
                        .WithMessageBuilder<NoteEventMessageBuilder<Note<TNote>>>()
                        .AndNoSubject()
                .Submit();
        }
    }
}
