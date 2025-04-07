if 'ReferencedWorkOrderList' not in (select [ViewName] from [dbo].[WebFilters] where [Name] = '_ALL_')
	insert into [dbo].[WebFilters] ([ID], [Name], [Standart], [ViewName], [Others]) values (NEWID(), '_ALL_', 1, 'ReferencedWorkOrderList', 1)
go