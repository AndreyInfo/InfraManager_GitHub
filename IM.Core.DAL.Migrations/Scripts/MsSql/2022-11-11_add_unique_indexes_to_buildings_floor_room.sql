if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Building_Name_Into_Organization' and object_id = OBJECT_ID('[Здание]')))
BEGIN
    CREATE UNIQUE INDEX UI_Building_Name_Into_Organization on [Здание] (Название, [ИД организации]);
END;

if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Floor_Name_Into_Building' and object_id = OBJECT_ID('[Этаж]')))
BEGIN
    CREATE UNIQUE INDEX UI_Floor_Name_Into_Building on [Этаж] (Название, [ИД здания]);
END;

if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Room_Name_Into_Floor' and object_id = OBJECT_ID('[Комната]')))
BEGIN
    CREATE UNIQUE INDEX UI_Room_Name_Into_Floor on [Комната] (Название, [ИД этажа]);
END;