using AutoMapper;
using InfraManager.DAL.Software;

namespace InfraManager.BLL.Software.SoftwareLicenseModelAdditionFields
{
    public class LicenseModelAdditionFieldsDataProfile : Profile
    {
        public LicenseModelAdditionFieldsDataProfile()
        {
            CreateMap<LicenseModelAdditionFieldsData, LicenseModelAdditionFields>();
        }
    }
}
