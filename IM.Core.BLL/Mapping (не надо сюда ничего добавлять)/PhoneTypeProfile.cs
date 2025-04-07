using AutoMapper;
using InfraManager.BLL.Asset.dto;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.Mapping
{
    // TODO: Выпилить
    internal class PhoneTypeProfile : Profile
    {
        public PhoneTypeProfile()
        {
            CreateMap<PhoneType, PhoneTypeDTO>().ReverseMap();
        }
    }
}
