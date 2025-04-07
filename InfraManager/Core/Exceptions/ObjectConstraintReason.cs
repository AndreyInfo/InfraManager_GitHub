using System;

namespace InfraManager.Core.Exceptions
{
	//
	//TODO: hueta
	//
	[Serializable]
	public enum ObjectConstraintReason : byte
	{
		NotExists = 0,
		ChildExists = 1,
		ParentNotExists = 2,
		ParentExists = 3
	}
}
