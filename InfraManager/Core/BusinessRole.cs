using System;
using System.Runtime.Serialization;

namespace InfraManager.Core
{
	[DataContract]
	[Serializable]
	public enum BusinessRole : int
	{
		[EnumMember]
		[FriendlyName("Нет")]
		None = 0,                           //0
		[EnumMember]
		[FriendlyName("Владелец заявки")]
		CallOwner = 1,                      //1
		[EnumMember]
		[FriendlyName("Клиент заявки")]
		CallClient = 1 << 1,                //2
		[EnumMember]
		[FriendlyName("Инициатор заявки")]
		CallInitiator = 1 << 2,             //4
		[EnumMember]
		[FriendlyName("Выполнивший заявку")]
		CallAccomplisher = 1 << 3,          //8
		[EnumMember]
		[FriendlyName("Исполнитель заявки")]
		CallExecutor = 1 << 4,              //16
		[EnumMember]
		[FriendlyName("Исполнитель задания")]
		WorkOrderExecutor = 1 << 5,         //32
		[EnumMember]
		[FriendlyName("Назначивший задание")]
		WorkOrderAssignor = 1 << 6,         //64
		[EnumMember]
		[FriendlyName("Инициатор задания")]
		WorkOrderInitiator = 1 << 7,        //128
		[EnumMember]
		[FriendlyName("Материально ответственный")]
		MateriallyLiablePerson = 1 << 8,    //256
		[EnumMember]
		[FriendlyName("Использующий")]
		Utilizer = 1 << 9,                  //512
		[EnumMember]
		[FriendlyName("Администратор")]
		SDAdministrator = 1 << 10,          //1024
		[EnumMember]
		[FriendlyName("Владелец проблемы")]
		ProblemOwner = 1 << 11,             //2048
		[EnumMember]
		[FriendlyName("Участник согласования")]
		NegotiationParticipant = 1 << 12,   //4096
		[EnumMember]
		[FriendlyName("Система")]
		System = 1 << 13,                   //8192
        [EnumMember]
        [FriendlyName("Контролер")]
        ControllerParticipant = 1 << 14,     //16384
		[EnumMember]
		[FriendlyName("Владелец запроса на изменения")]
		RFCOwner = 1 << 15,                  //32768
		[EnumMember]
		[FriendlyName("Инициатор запроса на изменения")]
		RFCInitiator = 1 << 16,               //65536
        [EnumMember]
        [FriendlyName("Заместитель")]
        DeputyUser = 1 << 17,               //131072
        [EnumMember]
        [FriendlyName("Замещаемый")]
        ReplacedUser = 1 << 18,             //262144
        
        [EnumMember]
        [FriendlyName("Инициатор массового инцидента")]
        MassIncidentInitiator = 1 << 19, 
        
        [EnumMember]
        [FriendlyName("Владелец массового инцидента")]
        MassIncidentOwner = 1 << 20, 
        
        [EnumMember]
        [FriendlyName("Исполнитель массового инцидента")]
        MassIncidentExecutor = 1 << 21,
        
        [EnumMember]
        [FriendlyName("Инициатор проблемы")]
        ProblemInitiator = 1 << 22, 
        
        [EnumMember]
        [FriendlyName("Исполнитель проблемы")]
        ProblemExecutor = 1 << 23,
    }
}
