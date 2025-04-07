update [dbo].[Call_Aggregate]
	set [WorkOrderCount] = (select count(1) from [dbo].[WorkOrderReference] x where x.ClassID = 701 and x.ObjectID = CallID)
go

update [dbo].[Call_Aggregate]
	set [QueueName] = (select q.[Name] from [dbo].[Queue] q join [dbo].[Call] c on c.QueueID = q.ID and c.ID = CallID)
go