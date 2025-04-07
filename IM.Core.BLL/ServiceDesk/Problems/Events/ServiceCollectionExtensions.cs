using Inframanager.BLL.Events;
using InfraManager.BLL.ServiceDesk.Notes;
using InfraManager.DAL.ServiceDesk;
using Microsoft.Extensions.DependencyInjection;

namespace InfraManager.BLL.ServiceDesk.Problems.Events
{
    internal static class ServiceCollectionExtensions
    {
        private const string Subject = "Проблема";

        public static IServiceCollection AddProblemEvents(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigureEventWriter<Problem, Problem>, ProblemEventWriterConfigurer>()
                .AddScoped<ProblemEventParamsCollectionConfigurer>()
                .WriteEventsOf<Problem>()
                    .WhenAdded(OperationID.Problem_Add)
                        .WithMessage(problem => $"Создана [{Subject}] '{problem.Number}'")
                        .WithParamBuildersCollectionConfig<ProblemEventParamsCollectionConfigurer>()
                        .AndSubjectBuilder<ProblemEventSubjectBuilder>()
                    .WhenUpdated(OperationID.Problem_Update)
                        .WithMessage(problem => $"Сохранена [{Subject}] '{problem.Number}'")
                        .WithParamBuildersCollectionConfig<ProblemEventParamsCollectionConfigurer>()
                        .AndSubjectBuilder<ProblemEventSubjectBuilder>()
                    .WhenDeleted(OperationID.Problem_Delete)
                        .WithMessage(problem => $"Удалена [{Subject}] '{problem.Number}'")
                        .AndSubjectBuilder<ProblemEventSubjectBuilder>()
                    .Submit()
                    .AddNoteEvents<Problem>();
        }

        private class ProblemEventSubjectBuilder : ServiceDeskEntityEventSubjectBuilder<Problem>
        {
            public ProblemEventSubjectBuilder() : base(Subject)
            {
            }
        }
    }
}
