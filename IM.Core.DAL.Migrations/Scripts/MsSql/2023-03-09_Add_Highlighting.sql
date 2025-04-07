if 'Highlighting' not in (select [TABLE_NAME] from [INFORMATION_SCHEMA].[TABLES] where [TABLE_SCHEMA] = 'dbo')
    create table [dbo].[Highlighting] (
        [ID] uniqueidentifier not null default(newid()),
        [Name] nvarchar(255) not null,
		[Sequence] int not null,
        constraint PK_Highlighting primary key clustered ([ID]),
		constraint UK_Highlighting unique nonclustered ([Name]));	
go

if 'HighlightingCondition' not in (select [TABLE_NAME] from [INFORMATION_SCHEMA].[TABLES] where [TABLE_SCHEMA] = 'dbo')
    create table [dbo].[HighlightingCondition] (
        [ID] uniqueidentifier not null default(newid()),
		[HighlightingID] uniqueidentifier not null,
        [DirectoryParameter] int null,
        [EnumParameter] tinyint null,
        [Condition] tinyint not null,
        [BackgroundColor] nvarchar(50) not null,
        [FontColor] nvarchar(50) not null,
        constraint PK_HighlightingCondition primary key clustered ([ID]),
		constraint FK_HighlightingCondition_Highlighting foreign key ([HighlightingID]) references [dbo].[Highlighting]([ID]) on delete cascade);	
go

if 'HighlightingConditionValue' not in (select [TABLE_NAME] from [INFORMATION_SCHEMA].[TABLES] where [TABLE_SCHEMA] = 'dbo')
    create table [dbo].[HighlightingConditionValue] (
        [ID] uniqueidentifier not null default(newid()),
		[HighlightingConditionID] uniqueidentifier not null,
        [PriorityID] uniqueidentifier null,
        [UrgencyID] uniqueidentifier null,
        [InfluenceID] uniqueidentifier null,
        [SlaID] uniqueidentifier null,
        [IntValue1] int null,
		[IntValue2] int null,
		[StringValue] nvarchar(255) null,
        constraint PK_HighlightingConditionValue primary key clustered ([ID]),
		constraint FK_HighlightingConditionValue_HighlightingCondition foreign key ([HighlightingConditionID]) references [dbo].[HighlightingCondition]([ID]) on delete cascade,	
		constraint FK_HighlightingConditionValue_Priority foreign key ([PriorityID]) references [dbo].[Priority]([ID]) on delete cascade,		
		constraint FK_HighlightingConditionValue_Urgency foreign key ([UrgencyID]) references [dbo].[Urgency]([ID]) on delete cascade,		
		constraint FK_HighlightingConditionValue_Influence foreign key ([InfluenceID]) references [dbo].[Influence]([ID]) on delete cascade,		
		constraint FK_HighlightingConditionValue_Sla foreign key ([SlaID]) references [dbo].[Sla]([ID]) on delete cascade);		
go

if not 1344 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1344, 190, 'Highlighting.Add', 'Создать', 'Операция дает возможность создавать новый объект Правила выделения строк в списке.');
go

if not 1345 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1345, 190, 'Highlighting.Update', 'Обновить', 'Операция дает возможность изменять объект Правила выделения строк в списке.');
go

if not 1346 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1346, 190, 'Highlighting.Delete', 'Удалить', 'Операция дает возможность удалять объект Правила выделения строк в списке.');
go

if not 1347 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1347, 190, 'Highlighting.Properties', 'Открыть свойства', 'Операция дает возможность просмотра объекта Правила выделения строк в списке.');
go

if not 1348 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1348, 191, 'HighlightingCondition.Add', 'Создать', 'Операция дает возможность создавать новый объект Условие правила выделения строк в списке.');
go

if not 1349 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1349, 191, 'HighlightingCondition.Update', 'Обновить', 'Операция дает возможность изменять объект Условие правила выделения строк в списке.');
go

if not 1350 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1350, 191, 'HighlightingCondition.Delete', 'Удалить', 'Операция дает возможность удалять объект Условие правила выделения строк в списке.');
go

if not 1351 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1351, 191, 'HighlightingCondition.Properties', 'Открыть свойства', 'Операция дает возможность просмотра объекта Условие правила выделения строк в списке.');
go

if not 1352 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1352, 192, 'HighlightingConditionValue.Add', 'Создать', 'Операция дает возможность создавать новый объект Значение условий правила выделения строк в списке.');
go

if not 1353 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1353, 192, 'HighlightingConditionValue.Update', 'Обновить', 'Операция дает возможность изменять объект Значение условий правила выделения строк в списке.');
go

if not 1354 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1354, 192, 'HighlightingConditionValue.Delete', 'Удалить', 'Операция дает возможность удалять объект Значение условий правила выделения строк в списке.');
go

if not 1355 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1355, 192, 'HighlightingConditionValue.Properties', 'Открыть свойства', 'Операция дает возможность просмотра объекта Значение условий правила выделения строк в списке.');
go

insert into [dbo].[RoleOperation] ([RoleID], [OperationID])
select '00000000-0000-0000-0000-000000000001', t.[ID] 
from [dbo].[Operation] t
left join [dbo].[RoleOperation] x on x.OperationID = t.[ID] and x.[RoleID] = '00000000-0000-0000-0000-000000000001'
where (t.[ID] between 1344 and 1355) and (x.OperationID is null)
go