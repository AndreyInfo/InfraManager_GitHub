using AutoMapper;
using SupplierContactPersonEntity = InfraManager.DAL.Suppliers.SupplierContactPerson;

namespace InfraManager.BLL.Suppliers.SupplierContactPerson;

internal class SupplierContactPersonProfile : Profile
{
    public SupplierContactPersonProfile()
    {
        CreateMap<SupplierContactPersonEntity, SupplierContactPersonDetails>();
        CreateMap<SupplierContactPersonData, SupplierContactPersonEntity>();
    }
}