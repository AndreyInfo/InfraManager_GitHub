using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.CalendarWorkSchedules;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements.Events;

public class OperationalLevelAgreementPropertyBuildersCollectionConfigurer :
    IConfigureDefaultEventParamsBuilderCollection<OperationalLevelAgreement>
{
    private readonly IFinder<Form> _forms;
    private readonly IFinder<TimeZone> _timeZones;
    private readonly IFinder<CalendarWorkSchedule> _calendarWorkSchedules;
    
    public OperationalLevelAgreementPropertyBuildersCollectionConfigurer(
        IFinder<Form> forms, 
        IFinder<TimeZone> timeZones,
        IFinder<CalendarWorkSchedule> calendarWorkSchedules)
    {
        _forms = forms;
        _timeZones = timeZones;
        _calendarWorkSchedules = calendarWorkSchedules;
    }
    
    public void Configure(IDefaultEventParamsBuildersCollection<OperationalLevelAgreement> collection)
    {
        collection
            .HasProperty(x => x.Name)
            .HasName("Название");
        
        collection
            .HasProperty(x => x.UtcFinishDate)
            .HasName("Действует по");
        
        collection
            .HasProperty(x => x.UtcStartDate)
            .HasName("Действует с");
            
        collection
            .HasProperty(x => x.Note)
            .HasName("Описание");
        
        collection
            .HasProperty(x => x.Number)
            .HasName("Номер");
        
        collection
            .HasProperty(x => x.FormID)
            .HasConverter(x => _forms.Find(x)?.Name)
            .HasName("Форма");
        
        collection
            .HasProperty(x => x.TimeZoneID)
            .HasConverter(x => _timeZones.Find(x)?.Name)
            .HasName("Временная зона");
        
        collection
            .HasProperty(x => x.CalendarWorkScheduleID)
            .HasConverter(x => _calendarWorkSchedules.Find(x)?.Name)
            .HasName("График");
    }
}