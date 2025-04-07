using System;

namespace InfraManager.BLL.MaintenanceWork.Folders;

public class MaintenanceFolderData
{
    public string Name { get; init; }
    public Guid? ParentID { get; init; }
    public byte[] RowVersion { get; init; }
}
