using InfraManager.BLL.Users;
using System;
using System.Collections.Generic;

namespace InfraManager.BLL.Asset;

public class GroupDetails
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public string ResponsibleName { get; init; }
    public string Used { get; init; } = "";
    public string Note { get; init; }
    public byte ByteType { get; init; }
    public GroupTypeData Type { get; init; }
    public Guid ResponsibleUserID { get; init; }
    public byte[] RowVersion { get; init; }
    public Guid[] PerformersId { get; init; }
    public IEnumerable<UserDetailsModel> QueueUserList { get; init; }
    public ObjectClass ClassID => ObjectClass.Group;
    public string QueueTypeName { get; init; }
}
