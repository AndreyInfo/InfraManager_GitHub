using System;

namespace InfraManager.DAL.AccessManagement;

public class ObjectAccess
{
    public ObjectAccess()
    {

    }

    public ObjectAccess(Guid ownerID, AccessTypes type, ObjectClass classID, Guid? objectID, bool propagate = true)
    {
        ID = Guid.NewGuid();
        OwnerID = ownerID;
        Type = type;
        Propagate = propagate;
        ClassID = classID;
        ObjectID = objectID;
    }

    public Guid ID { get; init; }
    public Guid OwnerID { get; init; }
    public AccessTypes Type { get; init; }
    public bool Propagate { get; set; }
    public ObjectClass ClassID { get; init; }
    public Guid? ObjectID { get; init; }
}
