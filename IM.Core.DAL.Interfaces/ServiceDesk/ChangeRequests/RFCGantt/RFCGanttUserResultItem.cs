using System;

namespace InfraManager.DAL.ServiceDesk.ChangeRequests.RFCGantt;

public sealed class RFCGanttUserResultItem
{
	public Guid ID { get; init; }
	public String FullName { get; init; }
	public String PositionName { get; init; }
	public String Phone { get; init; }
	public String PhoneInternal { get; init; }
	public String SubdivisionFullName { get; init; }
	public String Email { get; init; }
}
