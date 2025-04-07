if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Workplace_Name_Into_Room' and object_id = OBJECT_ID('[Рабочее место]')))
        BEGIN
            CREATE UNIQUE INDEX UI_Workplace_Name_Into_Room on [Рабочее место] (Название, [ИД комнаты]);
        end;