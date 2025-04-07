using InfraManager.DAL.AccessManagement;
using System;

namespace InfraManager.BLL.AccessManagement;

public class AccessElementsData
{
    public Guid OwnerID { get; set; }

    public ObjectClass ObjectClassID { get; set; }

    public Guid? ObjectID { get; init; }

    public AccessTypes AccessType { get; init; }

    public bool IsSelectFull { get; init; } // Propagate true

    public bool IsSelectPart { get; init; }// Propagate false
}
