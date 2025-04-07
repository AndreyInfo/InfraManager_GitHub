DO
$$
BEGIN
	INSERT INTO im.operation VALUES (2, 101, 'Organization.Properties', 'Открыть свойства', 'Операция позволяет просматривать свойства объекта Организация через форму свойств, но не позволяет изменять.')
	ON Conflict DO Nothing;

   	IF NOT EXISTS (
      SELECT FROM im.role_operation
      WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 2) 
	THEN
      INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 2)
	  ON Conflict DO Nothing;
   	END IF;
END
$$;