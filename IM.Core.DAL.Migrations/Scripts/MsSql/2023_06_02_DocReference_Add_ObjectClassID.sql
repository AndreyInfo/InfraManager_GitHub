if 'ObjectClassID' not in (select [COLUMN_NAME] from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'DocumentReference')
	alter table [dbo].[DocumentReference] add ObjectClassID INT NULL
go

update [dbo].[DocumentReference] set ObjectClassID = 701 where [ObjectID] in (select [ID] from [dbo].[Call])
go
update [dbo].[DocumentReference] set ObjectClassID = 702 where [ObjectID] in (select [ID] from [dbo].[Problem])
go
update [dbo].[DocumentReference] set ObjectClassID = 703 where [ObjectID] in (select [ID] from [dbo].[RFC])
go
update [dbo].[DocumentReference] set ObjectClassID = 119 where [ObjectID] in (select [ID] from [dbo].[WorkOrder])
go
update [dbo].[DocumentReference] set ObjectClassID = 823 where [ObjectID] in (select [IMObjID] from [dbo].[MassIncident])
go
