namespace InfraManager.DAL.Events
{
	public enum EventType : int
	{
		EntityCreated = 0, //c->s
		EntityModified = 1, //c<->s
		EntityDeleted = 2, //c->s
		WorkflowSchemePublished = 3, //c->s
		WorkflowCreated = 4, //s->c [everybody matters]
		WorkflowDeleted = 5, //s->c [nobody matters]
		WorkflowStateChanging = 6, //s->c [requestor matters]
		WorkflowStateChanged = 7, //s->c [requestor matters]
		WorkflowTransitionFailed = 8, //s->c [requestor matters]
		EntityStateSet = 9, //c->s
		UserEntityEvent = 10,
		WorkflowSchemeModified = 11,
	}
}
