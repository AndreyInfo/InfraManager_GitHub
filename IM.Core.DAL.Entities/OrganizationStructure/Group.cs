using Inframanager;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;
using System.Linq;

[ObjectClassMapping(ObjectClass.Group)]
[OperationIdMapping(ObjectAction.Insert, OperationID.Group_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.Group_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.Group_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.Group_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Group_Properties)]
public class Group : IGloballyIdentifiedEntity
{   
    public Group()
    {

    }

    public Group(string note, string name, Guid responsibleID)
    {
        IMObjID = Guid.NewGuid();
        Name = name;
        Note = note;
        Type = GroupType.None;
        ResponsibleID = responsibleID;
        ResponsibleUser = null;
        QueueUsers = null;
    }

    public Guid IMObjID { get; init; }

    public string Name { get; init; }

    public string Note { get; set; }

    public byte[] RowVersion { get; set; }

    public GroupType Type { get; set; }

    public Guid ResponsibleID { get; set; }

    public virtual User ResponsibleUser { get; set; }

    public virtual IEnumerable<GroupUser> QueueUsers { get; private set; }

    public static bool UserInOrganizationItem(
        ObjectClass organizationItemClassID,
        Guid? organizationItemID,
        Guid? userID) => throw new NotSupportedException();

    public static readonly Guid NullGroupID = Guid.Empty;
    public static Expression<Func<Group, bool>> IsNotNullObject =>
        group => group.IMObjID != NullGroupID;
}
