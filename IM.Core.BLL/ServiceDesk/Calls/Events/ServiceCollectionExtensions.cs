using Inframanager.BLL.Events;
using InfraManager.BLL.ServiceDesk.Notes;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace InfraManager.BLL.ServiceDesk.Calls.Events
{
    internal static class ServiceCollectionExtensions
    { 
        /// <summary>
        /// Регистрирует сервисы, необходимые для записи истории изменений заявки
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCallEvents(this IServiceCollection services)
        {
            return services
                .AddScoped<CallPropertyEventParamsBuilderConfigurer>() // сервис конфигурирует построение параметров события для вставки и редактирования
                .AddTransient<IConfigureEventWriter<Call, Call>, CallEventWriterConfigurer>() // сервис который конфигурирует основной сервис управляющий построением событий
                .WriteEventsOf<Call>()
                    .WhenAdded(OperationID.Call_Add) // При добавлении сущности Call создаем событие с операцией Call_Add
                        .WithMessage(call => $"Создана [{Subject}] '{call.Number}'") // Текстом сообщения этого формата
                        .WithParamBuildersCollectionConfig<CallPropertyEventParamsBuilderConfigurer>() // По параметру на каждое заполненное свойство (используем стандартную коллекцию построителей параметров с кастомным конфигом
                        .AndSubjectBuilder<CallEventSubjectBuilder>() // И построителем EventSubject
                    .WhenUpdatedIf(OperationID.Call_Update, call => !call.Removed)
                        .WithMessage(call => $"Сохранена [{Subject}] '{call.Number}'") // интересно на кой в истории заявки писать ее номер (он же один и тот же во всех строках будет)
                        .WithParamBuildersCollectionConfig<CallPropertyEventParamsBuilderConfigurer>()
                        .AndSubjectBuilder<CallEventSubjectBuilder>()
                    .WhenUpdatedIf(OperationID.Call_Delete, call => call.Removed) // TODO: Логическое / физическое удаление рулится на стороне DAL, нужно доработать механизм визитеров, чтобы BLL генерил это событие на WhenDeleted
                        .WithMessage(call => $"Удалена [{Subject}] '{call.Number}'")
                        .AndSubjectBuilder<CallEventSubjectBuilder>()
                 .Submit()
                 .AddCallReferenceEvents<Problem>("Добавлена связь с проблемой", "Удалена связь с проблемой")
                 .AddCallReferenceEvents<ChangeRequest>("Добавлена связь с rfc", "Удалена связь с rfc")
                 .AddNoteEvents<Call>();
        }

        private static IServiceCollection AddCallReferenceEvents<TObject>(
            this IServiceCollection services,
            string insertAction,
            string deleteAction)
            where TObject : IServiceDeskEntity, IHaveUtcModifiedDate, IGloballyIdentifiedEntity
        {
            return services
                .AddTransient<IConfigureEventWriter<CallReference<TObject>, CallReference<TObject>>, ReferenceEventWriterConfigurer<CallReference<TObject>>>()
                .WriteEventsOf<CallReference<TObject>>()
                    .WhenAdded(OperationID.CallReference_Add)
                        .WithMessageBuilder(
                            serviceProvider => new CustomInjector()
                                .Inject(insertAction)
                                .GetService<ReferenceEventMessageBuilder<CallReference<TObject>, TObject>>(serviceProvider))
                        .AndNoSubject()
                    .WhenDeleted(OperationID.CallReference_Remove)
                        .WithMessageBuilder(
                            serviceProvider => new CustomInjector()
                                .Inject(deleteAction)
                                .GetService<ReferenceEventMessageBuilder<CallReference<TObject>, TObject>>(serviceProvider))
                        .AndNoSubject()
                 .Submit();
        }
        
        private const string Subject = "Заявка";

        private class CallEventSubjectBuilder : ServiceDeskEntityEventSubjectBuilder<Call>
        {
            public CallEventSubjectBuilder() : base(Subject)
            {
            }
        }
    }
}
