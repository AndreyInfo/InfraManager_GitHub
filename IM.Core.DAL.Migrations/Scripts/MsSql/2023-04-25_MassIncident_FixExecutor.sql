update [dbo].[MassIncident] set [ExecutedByUserID] = 1 where [ExecutedByUserID] is null
go
alter table [dbo].[MassIncident] alter column [ExecutedByUserID] int not null
go