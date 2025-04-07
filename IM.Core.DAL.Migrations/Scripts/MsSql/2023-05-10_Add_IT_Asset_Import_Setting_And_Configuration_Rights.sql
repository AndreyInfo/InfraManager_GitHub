if not exists (select * from dbo.Class where ClassID = 193)
	insert into dbo.Class values(193, 'Задача импорта имущества');
go

if not 1356 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1356, 193, 'ITAssetImportSetting.Add', 'Создать', 'Операция дает возможность создавать новый объект Задача импорта имущества, но не дает возможности просмотра и изменения объекта.');
go

if not 1357 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1357, 193, 'ITAssetImportSetting.Update', 'Сохранить', 'Операция дает возможность изменять поля объекта Задача импорта имущества через форму свойств.');
go

if not 1358 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1358, 193, 'ITAssetImportSetting.Delete', 'Удалить', 'Операция дает возможность удалять объект Задача импорта имущества.');
go

if not 1359 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1359, 193, 'ITAssetImportSetting.Properties', 'Открыть свойства', 'Операция дает возможность просматривать поля объекта Задача импорта имущества через форму свойств, но не позволяет изменять их. На форме свойств объекта отсутствует кнопка "Сохранить".');
go

if not 1360 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1360, 193, 'ITAssetImportSetting.AddAs', 'Создать по аналогии', 'Операция дает возможность создавать по аналогии новый объект Задачу импорта имущества, но не дает возможности просмотра и изменения объекта.');
go

if not 1361 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1361, 193, 'ITAssetImportSetting.Plan', 'Запланировать', 'Операция дает возможность запланировать Задачу импорта имущества из списка задач импорта имущества. Дает возможность сделать активным пункт меню "Запланировать" в списке задач импорта портфеля сервисов.');
go

if not 1362 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1362, 193, 'ITAssetImportSetting.Execute', 'Выполнить', 'Операция дает возможность запускать Задачу импорта имущества из списка задач импорта имущества. Дает возможность сделать активным пункт меню "Выполнить" в списке задач импорта портфеля сервисов.');
go

if not exists (select * from dbo.Class where ClassID = 194)
	insert into dbo.Class values(194, 'Конфигурация CSV задачи импорта имущества');
go

if not 1363 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1363, 194, 'ITAssetImportCSVConfiguration.Add', 'Создать', 'Операция дает возможность создавать новый объект Конфигурация CSV задачи импорта имущества, но не дает возможности просмотра и изменения объекта.');
go

if not 1364 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1364, 194, 'ITAssetImportCSVConfiguration.Update', 'Сохранить', 'Операция дает возможность изменять поля объекта Конфигурация CSV задачи импорта имущества через форму свойств.');
go

if not 1365 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1365, 194, 'ITAssetImportCSVConfiguration.Delete', 'Удалить', 'Операция дает возможность удалять объект Конфигурация CSV задачи импорта имущества.');
go

if not 1366 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1366, 194, 'ITAssetImportCSVConfiguration.Properties', 'Открыть свойства', 'Операция дает возможность просматривать поля объекта Конфигурация CSV задачи импорта имущества через форму свойств, но не позволяет изменять их. На форме свойств объекта отсутствует кнопка "Сохранить".');
go

if not 1367 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1367, 194, 'ITAssetImportCSVConfiguration.AddAs', 'Создать по аналогии', 'Операция дает возможность создавать по аналогии новый объект Конфигурация CSV задачи импорта имущества, но не дает возможности просмотра и изменения объекта.');
go

insert into [dbo].[RoleOperation] ([RoleID], [OperationID])
select '00000000-0000-0000-0000-000000000001', t.[ID] 
from [dbo].[Operation] t
left join [dbo].[RoleOperation] x on x.OperationID = t.[ID] and x.[RoleID] = '00000000-0000-0000-0000-000000000001'
where (t.[ID] between 1356 and 1367) and (x.OperationID is null)
go