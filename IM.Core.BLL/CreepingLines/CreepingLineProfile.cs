using AutoMapper;
using InfraManager.DAL;

namespace InfraManager.BLL.CreepingLines
{
    public class CreepingLineProfile : Profile
    {
        public CreepingLineProfile()
        {
            CreateMap<CreepingLineData, CreepingLine>();

            CreateMap<CreepingLine, CreepingLineDetails>();
        }
    }
}
