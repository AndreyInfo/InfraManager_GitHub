using System;

namespace InfraManager.BLL.KB
{
    public class FolderFilter
    {
        public Guid? ParentId { get; set; }

        public bool SeeInvisible { get; set; }
    }
}
