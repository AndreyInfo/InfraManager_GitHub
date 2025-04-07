using Inframanager.BLL.Events;
using InfraManager.BLL.ServiceDesk.Notes;
using InfraManager.DAL.Events;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InfraManager.BLL.ServiceDesk.WorkOrders.Events
{
    internal static class ServiceCollectionExtensions
    { 
        /// <summary>
        /// Регистрирует сервисы, необходимые для записи истории изменений заявки
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWorkOrderEvents(this IServiceCollection services)
        {
            return services
                .AddScoped<WorkOrderPropertyEventParamsBuilderConfigurer>()
                .AddTransient<IConfigureEventWriter<WorkOrder, WorkOrder>, WorkOrderEventWriterConfigurer>() 
                .WriteEventsOf<WorkOrder>()
                    .WhenAdded(OperationID.WorkOrder_Add) 
                        .WithMessage(wo => $"Создана [{Subject}] '{wo.Number}'") 
                        .WithParamBuildersCollectionConfig<WorkOrderPropertyEventParamsBuilderConfigurer>() 
                        .AndSubjectBuilder<WorkOrderEventSubjectBuilder>() 
                    .WhenAddedIf(OperationID.WorkOrderReference_Add, wo => wo.WorkOrderReferenceID != WorkOrderReference.NullID)
                        .WithMessage(wo => $"Связь с объектом '{wo.Number}' [{Subject}] добавлена.")
                        .AndSubjectBuilder<WorkOrderReferenceEventSubjectBuilder>()
                    .WhenUpdated(OperationID.WorkOrder_Update)
                        .WithMessage(wo => $"Сохранена [{Subject}] '{wo.Number}'") 
                        .WithParamBuildersCollectionConfig<WorkOrderPropertyEventParamsBuilderConfigurer>()
                        .AndSubjectBuilder<WorkOrderEventSubjectBuilder>()
                    .WhenDeleted(OperationID.Workplace_Delete) 
                        .WithMessage(wo => $"Удалена [{Subject}] '{wo.Number}'")
                        .AndSubjectBuilder<WorkOrderEventSubjectBuilder>()
                .Submit()
                .AddTransient<IConfigureEventWriter<WorkOrder, WorkOrderReference>, WorkOrderReferenceEventWriterConfigurer>()
                .AddScoped<ISubjectFinder<WorkOrder, WorkOrderReference>, WorkOrderReferenceSubjectFinder>()
                .WriteEventsOf<WorkOrder, WorkOrderReference>()
                    .WhenUpdatedIf(
                        OperationID.WorkOrderReference_Add,
                        (state, wo) =>
                            (long)state[nameof(WorkOrder.WorkOrderReferenceID)] == WorkOrderReference.NullID
                            && wo.WorkOrderReferenceID != WorkOrderReference.NullID)
                        .WithMessage(wo => $"Связь с объектом '{wo.Number}' [{Subject}] добавлена.")
                        .AndSubjectBuilder<WorkOrderReferenceEventSubjectBuilder>()
                    .WhenUpdatedIf(
                        OperationID.WorkOrderReference_Remove,
                        (state, wo) =>
                            (long)state[nameof(WorkOrder.WorkOrderReferenceID)] != WorkOrderReference.NullID
                            && wo.WorkOrderReferenceID == WorkOrderReference.NullID)
                        .WithMessage(wo => $"Связь с объектом '{wo.Number}' [{Subject}] удалена.")
                        .AndSubjectBuilder<WorkOrderReferenceEventSubjectBuilder>()
                .Submit()
                .AddNoteEvents<WorkOrder>(); 
        }

        private const string Subject = "Задача";

        private class WorkOrderEventSubjectBuilder : ServiceDeskEntityEventSubjectBuilder<WorkOrder>
        {
            public WorkOrderEventSubjectBuilder() : base(Subject)
            {
            }
        }

        private class WorkOrderReferenceEventSubjectBuilder : 
            IBuildEventSubject<WorkOrder, WorkOrderReference>,
            IBuildEventSubject<WorkOrder, WorkOrder>
        {
            public EventSubject Build(WorkOrderReference subject)
            {
                return new EventSubject(
                    "Связь с заданием",
                    subject.ReferenceName,
                    new InframanagerObject(subject.ObjectID, subject.ObjectClassID));
            }

            public EventSubject Build(WorkOrder subject)
            {
                return Build(subject.WorkOrderReference);
            }
        }
    }
}
