DECLARE @srvVersion INT
SET @srvVersion = @@microsoftversion / 0x01000000

DECLARE @cmd VARCHAR(8000)

IF @srvVersion = 8
BEGIN
    SET @cmd = '
    USE [' + @Database + ']
    
    IF EXISTS (SELECT * FROM dbo.sysusers WHERE name = N''' + @User + ''')
        EXEC sp_revokedbaccess N''' + @User + '''

    EXEC sp_grantdbaccess N''' + @Login + ''', N''' + @User + '''
	EXEC sp_addrolemember N''db_owner'', N''' + @User + '''
	
    USE master
    '
    EXEC (@cmd)
    RETURN
END
ELSE IF @srvVersion > 8
BEGIN
    SET @cmd = '
    USE [' + @Database + ']
    
    IF EXISTS (SELECT * FROM dbo.sysusers WHERE name = N''' + @User + ''')
    BEGIN
		DECLARE @cmd varchar(8000)
		DECLARE @schemaName varchar(8000)
		SELECT @schemaName = sys.schemas.name FROM sys.schemas 
		WHERE sys.schemas.principal_id = (SELECT uid FROM dbo.sysusers WHERE name = N''' + @User + ''')
		
        IF NOT @schemaName is NULL
		BEGIN
            SET @cmd = ''DROP SCHEMA '' + @schemaName
            EXEC(@cmd)
        END
        DROP USER ' + @User + '
    END

	CREATE USER ' + @User + ' FOR LOGIN ' + @Login + ' WITH DEFAULT_SCHEMA = dbo
	EXEC sp_addrolemember N''db_owner'', N''' + @User + '''

    USE master
    '
    EXEC (@cmd)
    RETURN
END