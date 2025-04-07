
	using System;
	using InfraManager;

	namespace Inframanager.DAL.ActiveDirectory.Import
	{
		//todo:раскрыть после релиза
		// [ObjectClassMapping(ObjectClass.UIADClass)]
		//    [OperationIdMapping(ObjectAction.Insert, OperationId.UIADClass_Insert)]
		//    [OperationIdMapping(ObjectAction.Update, OperationId.UIADClass_Update)]
		//    [OperationIdMapping(ObjectAction.Delete, OperationId.UIADClass_Delete)]
		//    [OperationIdMapping(ObjectAction.ViewDetails,OperationId.None)]
		//    [OperationIdMapping(ObjectAction.ViewDetailsArray,OperationId.None)]
		public class UIADClass
		{
			public Guid ID { get; init; }

			public string Name { get; init; }
		}
	}