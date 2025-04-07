using InfraManager.DAL.OrganizationStructure;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.AccessManagement;

public class AccessPermission
{
    public Guid ID { get; init; }

    public ObjectClass ObjectClassID { get; set; }

    public Guid ObjectID { get; set; }

    public bool? Properties { get; set; }
    public bool? Add { get; set; }
    public bool? Delete { get; set; }
    public bool? Update { get; set; }
    public bool? AccessManage { get; set; }

    public virtual ICollection<OrganizationItemGroup> OrganizationItems { get; init; }

}
