IF NOT EXISTS (SELECT * FROM dbo.Operation WHERE  ID = 358 AND ClassID = 704)
	INSERT INTO dbo.Operation
		VALUES (358, 704, 'SD_General.Executor', 'Быть исполнителем', 'Операция дает возможность пользователю быть назначенным в качестве "Исполнителя" для "Заявок", "Заданий", "Проблем" Службы Поддержки.')
ELSE
	UPDATE dbo.Operation SET OperationName = 'Быть исполнителем', Description = 'Операция дает возможность пользователю быть назначенным в качестве "Исполнителя" для "Заявок", "Заданий", "Проблем" Службы Поддержки.' WHERE dbo.operation.ID = 358 AND dbo.operation.ClassID = 704