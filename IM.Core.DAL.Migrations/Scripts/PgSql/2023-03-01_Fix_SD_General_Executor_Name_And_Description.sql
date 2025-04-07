DO
$$
BEGIN
   	IF NOT EXISTS (
      SELECT FROM im.operation
      WHERE im.operation.ID = 358 AND im.operation.class_id = 704)
	THEN
      INSERT INTO im.operation VALUES (358, 704, 'SD_General.Executor', 'Быть исполнителем', 'Операция дает возможность пользователю быть назначенным в качестве "Исполнителя" для "Заявок", "Заданий", "Проблем" Службы Поддержки.')
	  ON Conflict DO Nothing;
	ELSE
	  UPDATE im.operation SET operation_name = 'Быть исполнителем', description = 'Операция дает возможность пользователю быть назначенным в качестве "Исполнителя" для "Заявок", "Заданий", "Проблем" Службы Поддержки.' WHERE im.operation.ID = 358 AND im.operation.class_id = 704;
   	END IF;
END
$$;