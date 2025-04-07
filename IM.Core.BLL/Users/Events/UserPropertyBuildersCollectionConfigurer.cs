using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.Users.Events;

public class UserPropertyBuildersCollectionConfigurer : IConfigureDefaultEventParamsBuilderCollection<User>
{
    private readonly IFinder<JobTitle> _jobTitles;
    private readonly ISubdivisionFullNameQuery _fullSubdivisionPath;

    public UserPropertyBuildersCollectionConfigurer(IFinder<JobTitle> jobTitles,
        ISubdivisionFullNameQuery fullSubdivisionPath)
    {
        _jobTitles = jobTitles;
        _fullSubdivisionPath = fullSubdivisionPath;
    }

    public void Configure(IDefaultEventParamsBuildersCollection<User> collection)
    {
        collection
            .HasProperty(x => x.LoginName)
            .HasName("Логин"); 
        
        collection
            .HasProperty(x => x.Note)
            .HasName("Примечание");
            
        collection
            .HasProperty(x => x.Pager)
            .HasName("Прочее");
        
        collection
            .HasProperty(x => x.Email)
            .HasName("Email");

        collection
            .HasProperty(x => x.Number)
            .HasName("Табельный номер");
            
        collection
            .HasProperty(x => x.Name)
            .HasName("Имя");
        
        collection
            .HasProperty(x => x.Surname)
            .HasName("Фамилия");
        
        collection
            .HasProperty(x => x.Patronymic)
            .HasName("Отчество");
        
        collection
            .HasProperty(x => x.PositionID)
            .HasConverter(x => _jobTitles.Find(x)?.Name)
            .HasName("Должность");
            
        collection
            .HasProperty(x => x.SubdivisionID)
            .HasConverter(x => (_fullSubdivisionPath.QueryAsync(x.Value).Result))
            .HasName("Подразделение");
    }
}