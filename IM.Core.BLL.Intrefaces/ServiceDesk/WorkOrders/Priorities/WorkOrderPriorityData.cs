using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.WorkOrders.Priorities;

public class WorkOrderPriorityData
{
    public string Name { get; init; }
    
    public int Sequence { get; init; }
    
    public byte[] RowVersion { get; init; }
    
    public bool Default { get; init; }
    
    public string Color { get; init; }
}
