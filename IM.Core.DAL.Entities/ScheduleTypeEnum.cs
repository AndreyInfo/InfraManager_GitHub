using System;
using System.Runtime.Serialization;

namespace InfraManager.Services.ScheduleService
{
    [DataContract]
	[Serializable]
	public enum ScheduleTypeEnum : byte
	{
		Once = 1,
		Daily = 2,
		Weekly = 3,
		Monthly = 4,
		Immediately = 5
	}
}
