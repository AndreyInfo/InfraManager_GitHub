if not exists (select 1 from operation where ClassID = 450)
	insert into operation
		values(997, 450, 'ConfigurationUnitBase.Add ', 'Создать', 'Операция дает возможность создавать новый объект Конфигурационная единица, но не дает возможности просмотра и изменения объекта Конфигурационная единица.'),
			  (998, 450, 'ConfigurationUnitBase.Update', 'Сохранить', 'Операция позволяет изменять поля объекта Конфигурационная единица через форму свойств.'),
			  (999, 450, 'ConfigurationUnitBase.Delete', 'Удалить', 'Операция дает возможность удалять объект Конфигурационная единица.'),
			  (1000, 450, 'ConfigurationUnitBase.Properties', 'Открыть свойства', 'Операция позволяет просматривать свойства объекта Конфигурационная единица через форму свойств, но не позволяет изменять.')

if not exists (select 1 from dbo.Class where ClassID = 450)
	insert into dbo.Class values(450, 'Конфигурационная единица')

if not exists (select 1 from dbo.RoleOperation where RoleID = '00000000-0000-0000-0000-000000000001' and 
														(OperationID = 997
														OR OperationID = 998
														OR OperationID = 999
														OR OperationID = 1000))
	insert into dbo.RoleOperation values ('00000000-0000-0000-0000-000000000001',997),
										('00000000-0000-0000-0000-000000000001',998),
										('00000000-0000-0000-0000-000000000001',999),
										('00000000-0000-0000-0000-000000000001',1000)