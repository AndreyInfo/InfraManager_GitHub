IF NOT EXISTS (SELECT * FROM dbo.Operation WHERE ClassID = 101 AND ID = 2)
	INSERT INTO dbo.Operation
		VALUES (2, 101, 'Organization.Properties', 'Открыть свойства', 'Операция позволяет просматривать свойства объекта Организация через форму свойств, но не позволяет изменять.');
		
IF NOT EXISTS (SELECT * FROM dbo.RoleOperation WHERE RoleID = '00000000-0000-0000-0000-000000000001' AND OperationID = 2)
	INSERT INTO dbo.RoleOperation
		VALUES ('00000000-0000-0000-0000-000000000001', 2);