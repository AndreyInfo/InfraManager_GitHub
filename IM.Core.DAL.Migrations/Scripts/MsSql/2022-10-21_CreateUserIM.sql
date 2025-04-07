DECLARE @srvVersion INT
SET @srvVersion = @@microsoftversion / 0x01000000

DECLARE @cmd VARCHAR(8000)

IF @srvVersion = 8
BEGIN
	IF NOT EXISTS (SELECT * FROM master.dbo.sysxlogins WHERE name = N'im')
		SET @cmd = '
		EXEC sp_addlogin ''im'', ''jd89$32#90JHgwjn%MLwhb3b''
		'
	ELSE
	    SET @cmd = '
	    EXEC sp_password null, ''jd89$32#90JHgwjn%MLwhb3b'', ''im''
	    '
	EXEC(@cmd)
	RETURN
END
ELSE IF @srvVersion > 8
BEGIN
	IF NOT EXISTS (SELECT * FROM master.sys.server_principals WHERE name = N'im')
	    SET @cmd = '
	    CREATE LOGIN im
	    WITH PASSWORD = ''jd89$32#90JHgwjn%MLwhb3b'', 
	    CHECK_POLICY = OFF
	    '
	ELSE
	    SET @cmd = '
	    ALTER LOGIN im
	    WITH PASSWORD = ''jd89$32#90JHgwjn%MLwhb3b'',
	    CHECK_POLICY = OFF
	    '
	EXEC(@cmd)
END

--Create user
IF @srvVersion = 8
BEGIN
    SET @cmd = '  
    IF EXISTS (SELECT * FROM dbo.sysusers WHERE name = N''im'')
        EXEC sp_revokedbaccess N''im''

    EXEC sp_grantdbaccess N''im'', N''im''
	EXEC sp_addrolemember N''db_owner'', N''im''
	
    USE master
    '
    EXEC (@cmd)
    RETURN
END
ELSE IF @srvVersion > 8
BEGIN
    SET @cmd = '
    IF EXISTS (SELECT * FROM dbo.sysusers WHERE name = N''im'')
    BEGIN
		DECLARE @cmd varchar(8000)
		DECLARE @schemaName varchar(8000)
		SELECT @schemaName = sys.schemas.name FROM sys.schemas 
		WHERE sys.schemas.principal_id = (SELECT uid FROM dbo.sysusers WHERE name = N''im'')
		
        IF NOT @schemaName is NULL
		BEGIN
            SET @cmd = ''DROP SCHEMA '' + @schemaName
            EXEC(@cmd)
        END
        DROP USER im
    END

	CREATE USER im FOR LOGIN im WITH DEFAULT_SCHEMA = dbo
	EXEC sp_addrolemember N''db_owner'', N''im''

    USE master
    '
    EXEC (@cmd)
    RETURN
END