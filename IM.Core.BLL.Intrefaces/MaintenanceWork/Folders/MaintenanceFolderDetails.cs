using System;

namespace InfraManager.BLL.MaintenanceWork.Folders;

public class MaintenanceFolderDetails
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public Guid? ParentID { get; init; }
    public byte[] RowVersion { get; init; }
    public ObjectClass ClassID => ObjectClass.MaintenanceFolder;
}
