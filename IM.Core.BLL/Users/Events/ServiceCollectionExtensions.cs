using System;
using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Events;
using Microsoft.Extensions.DependencyInjection;

namespace InfraManager.BLL.Users.Events;

public static class ServiceCollectionExtensions
{
    private static string Subject = "Пользователь";

    public static IServiceCollection AddUserEvents(this IServiceCollection services)
    {
        services.AddScoped<UserSetRoleBuildEventMessage>();
        services.AddScoped<UserUnSetRoleBuildEventMessage>();
        
        return services
            .AddScoped<UserPropertyBuildersCollectionConfigurer>()
            .AddTransient<IConfigureEventWriter<User, User>,
                UserEventWriterConfigurer>()
            .WriteEventsOf<User>()
                .WhenAdded(OperationID.OperationalLevelAgreement_Add)
                    .WithMessage(user => $"Создан [{Subject}] '{user.ID}'")
                    .WithParamBuildersCollectionConfig<UserPropertyBuildersCollectionConfigurer>()
                    .AndSubjectBuilder<UserEventSubjectBuilder>()
            .WhenUpdated(OperationID.OperationalLevelAgreement_Update)
                .WithMessage(user => $"Сохранен [{Subject}] '{user.ID}'")
                    .WithParamBuildersCollectionConfig<UserPropertyBuildersCollectionConfigurer>()
                    .AndSubjectBuilder<UserEventSubjectBuilder>()
            .WhenDeleted(OperationID.OperationalLevelAgreement_Delete)
                .WithMessage(user => $"Удален [{Subject}] '{user.ID}'")
                    .AndSubjectBuilder<UserEventSubjectBuilder>()
            .Submit()
            .AddTransient<IConfigureEventWriter<UserRole, User>, UserRoleEventWriterConfigurer>()
            .AddScoped<ISubjectFinder<UserRole, User>, UserRoleSubjectFinder>()
            .WriteEventsOf<UserRole, User>()
                .WhenAdded(OperationID.Role_SetRole)
                    .WithMessageBuilder<UserSetRoleBuildEventMessage>()
                    .AndSubjectBuilder<UserRoleEventSubjectBuilder>()
                .WhenDeleted(OperationID.Role_UnSetRole)
                    .WithMessageBuilder<UserUnSetRoleBuildEventMessage>()
                    .AndSubjectBuilder<UserRoleEventSubjectBuilder>()
            .Submit();
    }

    private class UserRoleEventSubjectBuilder : IBuildEventSubject<UserRole, User>
    {
        public EventSubject Build(User subject)
        {
            return new EventSubject("Роль", subject.FullName,
                new InframanagerObject(subject.IMObjID, ObjectClass.User));
        }
    }
    
    
    private class UserEventSubjectBuilder : EventSubjectBuilderBase<User, User>
    {
        public UserEventSubjectBuilder() : base(Subject)
        {
        }

        protected override Guid GetID(User subject)
        {
            return subject.IMObjID;
        }

        protected override string GetSubjectValue(User subject)
        {
            return subject.ID.ToString();
        }
    }
}