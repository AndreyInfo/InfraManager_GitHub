IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DBInfo' and COLUMN_NAME = 'Version')
    SELECT Version FROM dbo.DBInfo
ELSE
    SELECT null