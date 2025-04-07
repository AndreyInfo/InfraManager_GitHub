using AutoMapper;
using InfraManager.DAL.Finance;

namespace InfraManager.BLL.Finance
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<Supplier, SupplierDetails>();
        }
    }
}
