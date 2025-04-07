
if not exists (select ID from [dbo].[Operation] where ID = 1326)  
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description]) values (1326, 704, 'MassiveIncident.BeOwner', 'Быть владельцем массового инцидента', 'Операция дает возможность пользователю быть назначенным в качестве "Владельца" для объектов Массовый инцидент.')
go

if not exists (select ID from [dbo].[Operation] where ID = 1327)  
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description]) values (1327, 704, 'ChangeRequest.ViewAll', 'Вкладка Запросы на изменение.', 'Операция предоставляет доступ к списку "Все запросы на изменение".') 
go

if not exists (select ID from [dbo].[Operation] where ID = 1328)  
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description]) values (1328, 823, 'MassiveIncident.Clone', 'Создать по аналогии', 'Операция позволяет добавлять новый объект Массовый инцидент, поля которого будут идентичны полям Массового инцидента, взятого за основу.')  
go

if not exists (select ID from [dbo].[Operation] where ID = 1329)  
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description]) values (1329, 823, 'MassiveIncident.EditServiceFields', 'Редактировать служебные поля массового инцидента', 'Операция позволяет изменять даты: Зарегистрирован, Открыт, Закрыть до, Выполнен, но не дает возможность открывать свойства массового инцидента.')  
go

if not exists (select ID from [dbo].[Operation] where ID = 1330)  
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description]) values (1330, 823, 'MassiveIncident.ChangeOwner', 'Передать', 'Операция позволяет назначить массовому инциденту другого владельца.')  
go

if not exists (select ID from [dbo].[Operation] where ID = 1331)  
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description]) values (1331, 823, 'MassiveIncident.SeeInherited', 'Видеть массовые инциденты ИТ-сотрудников', 'Позволяет увидеть пользователю в списке массовых инцидентов («Мое рабочее место», «Все массовые инциденты») массовые инциденты, которые не завершены и где владельцем или исполнителем, или инициатором являются сотрудники из того же подразделения что и этот пользователь или из подразделений ниже по иерархии')  
go

insert into [dbo].[RoleOperation] ([OperationID], [RoleID])
select t.[ID], '00000000-0000-0000-0000-000000000001'
from [dbo].[Operation] t
left join [dbo].[RoleOperation] x on x.OperationID = t.[ID] and x.[RoleID] = '00000000-0000-0000-0000-000000000001'
where (t.[ID] between 1326 and 1331) and (x.OperationID is null)
go