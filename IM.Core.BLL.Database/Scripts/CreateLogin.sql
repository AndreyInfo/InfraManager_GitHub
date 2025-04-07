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
	RETURN
END