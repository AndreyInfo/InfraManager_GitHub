using AutoMapper;
using InfraManager.BLL.CrudWeb;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Calendar;
using InfraManager.DAL.ServiceDesk;
using System;

namespace InfraManager.BLL.Mapping;

public class DeleteModelProfile : Profile
{
    [Obsolete("DeleteModel используется для множественного удаления, переходить на единичное(не использовать DeleteModel")]
    public DeleteModelProfile()
    {
        CreateMap<DeleteModel<int>, SlotType>();
        CreateMap<DeleteModel<Guid>, PhoneType>();
        CreateMap<DeleteModel<Guid>, FileSystem>();
        CreateMap<DeleteModel<Guid>, Criticality>();
        CreateMap<DeleteModel<Guid>, CartridgeType>();
        CreateMap<DeleteModel<Guid>, InfrastructureSegment>();

        CreateMap<DeleteModel<Guid>, CallType>();

        CreateMap<DeleteModel<Guid>, CalendarHolidayItem>();
        CreateMap<DeleteModel<Guid>, Exclusion>();
    }
}
