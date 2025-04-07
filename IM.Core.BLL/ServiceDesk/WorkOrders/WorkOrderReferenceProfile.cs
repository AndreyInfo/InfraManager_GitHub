using AutoMapper;
using InfraManager.DAL.ServiceDesk.WorkOrders;

namespace InfraManager.BLL.ServiceDesk.WorkOrders;

public class WorkOrderReferenceProfile : Profile
{
    public WorkOrderReferenceProfile()
    {
        CreateMap<WorkOrderReference, InframanagerObject>()
            .ConstructUsing(reference => new InframanagerObject(reference.ObjectID, reference.ObjectClassID));
        this.CreateWorkOrderReferenceMap<WorkOrderReferenceListItem>();
    }
}