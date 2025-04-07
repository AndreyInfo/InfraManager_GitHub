if 'TechnicalFailuresCategory' not in (select[TABLE_NAME] from[INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
    create table [dbo].[TechnicalFailuresCategory] (
        [ID] int not null identity(1,1),
        [Name] nvarchar(100) not null,
		constraint PK_TechnicalFailuresCategory primary key clustered ([ID]),
		constraint UK_TechnicalFailuresCategory_Name unique([Name]))		
go
if not exists (select * from dbo.Class where ClassID = 907)
	INSERT into dbo.Class values(907, 'Категории технических сбоев')
go

if not 1119 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1119, 907, 'TechnicalFailuresCategory.Properties', 'Открыть свойства', 'Операция позволяет просматривать свойства объекта Категория технических сбоев через форму свойств, но не позволяет изменять.')
go

if not 1120 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1120, 907, 'TechnicalFailuresCategory.Add', 'Создать', 'Операция дает возможность создавать новый объект Категория технических сбоев, но не дает возможности просмотра и изменения объекта Категория технических сбоев.')
go

if not 1122 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1122, 907, 'TechnicalFailuresCategory.Update', 'Редактировать', 'Операция дает возможность изменять параметры Категории технических сбоев и сохранять внесенные изменения.')
go

if not 1121 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1121, 907, 'TechnicalFailuresCategory.Delete', 'Удалить', 'Операция дает возможность удалять объект Категория технических сбоев.')
go

insert into [dbo].[RoleOperation] ([OperationID], [RoleID])
select t.[ID], '00000000-0000-0000-0000-000000000001'
from [dbo].[Operation] t
left join [dbo].[RoleOperation] x on x.OperationID = t.[ID] and x.[RoleID] = '00000000-0000-0000-0000-000000000001'
where (t.[ID] between 1119 and 1121) and (x.OperationID is null)
go
