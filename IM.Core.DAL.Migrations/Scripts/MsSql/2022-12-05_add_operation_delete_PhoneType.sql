if NOT EXISTS (select 1 from Operation where ID = 1206)
	INSERT into Operation 
		VALUES (1206, 86, N'TelephoneType.Delete', N'Удалить', N'Операция дает возможность удалять объект Тип телефона.');
else
	UPDATE Operation  SET ClassID = 86
					  , Name = N'TelephoneType.Delete'
					  , OperationName = N'Удалить'
					  , Description = N'Операция дает возможность удалять объект Тип телефона.'
	where ID = 1206