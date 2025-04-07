DECLARE @srvVersion INT
SET @srvVersion = @@microsoftversion / 0x01000000

DECLARE @cmd VARCHAR(8000)

SET @cmd = '
RESTORE FILELISTONLY FROM DISK = ''' + @File + '''
'

CREATE TABLE #files (
	    lname nvarchar(128), 
	    pname nvarchar(260),	
	    type char(1),
	    fgroup nvarchar(128),
	    size numeric(20,0),   
	    maxsize numeric(20,0))

IF @srvVersion > 8
BEGIN
    ALTER TABLE #files
    ADD field bigint			
    ALTER TABLE #files
    ADD createLSN numeric(25,0)
    ALTER TABLE #files
    ADD dropLSN numeric(25,0)			
    ALTER TABLE #files
    ADD uniqueId uniqueidentifier
    ALTER TABLE #files
    ADD readOnlyLSN numeric(25,0)
    ALTER TABLE #files
    ADD readWriteLSN numeric(25,0)
    ALTER TABLE #files
    ADD backUpSizeInBytes bigint
    ALTER TABLE #files
    ADD sourceBlockSize int
    ALTER TABLE #files
    ADD fileGroupId int	
    ALTER TABLE #files
    ADD logGroupGUID uniqueidentifier
    ALTER TABLE #files
    ADD diff numeric(25,0)
    ALTER TABLE #files
    ADD diffGUID uniqueidentifier
    ALTER TABLE #files
    ADD isReadOnly bit
    ALTER TABLE #files
    ADD isPresent bit
END

IF @srvVersion > 9
BEGIN
    ALTER TABLE #files
    ADD thumbrint varbinary(32)
end

IF @srvVersion > 12
BEGIN
    ALTER TABLE #files
    ADD snapshotURL nvarchar(360)
end


INSERT #files 
EXEC(@cmd)

DECLARE @lnameData NVARCHAR(128)
DECLARE @lnameLog NVARCHAR(128)
SELECT @lnameData = lname FROM #files WHERE type = 'D'
SELECT @lnameLog = lname FROM #files WHERE type = 'L'

DROP TABLE #files

SET @cmd = '
RESTORE DATABASE [' + @Database + ']
FROM DISK = ''' + @File + '''
WITH 
MOVE ''' + @lnameData + ''' TO ''' + @MDFFile + ''',
MOVE ''' + @lnameLog + ''' TO ''' + @LDFFile + ''',
REPLACE,
STATS = 10
'
EXEC(@cmd)

USE master

IF (@lnameData = 'IMConverter_dat')
BEGIN
	SET @cmd = '
	ALTER DATABASE [' + @Database + '] MODIFY FILE (NAME = ''IMConverter_dat'', NEWNAME = ''IMDatabase_dat'')
	'
	EXEC(@cmd)
	SET @cmd = '
	ALTER DATABASE [' + @Database + '] MODIFY FILE (NAME = ''IMConverter_log'', NEWNAME = ''IMDatabase_log'')
	'
	EXEC(@cmd)
END

SET @cmd = '
IF EXISTS(SELECT * FROM [' + @Database + '].INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = ''DBInfo'' AND COLUMN_NAME = ''ID'') AND
	EXISTS(SELECT * FROM [' + @Database + '].INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = ''DBInfo'' AND COLUMN_NAME = ''UtcDateIDSet'')
	EXEC(''UPDATE [' + @Database + '].[dbo].[DBInfo] SET ID = NEWID(), UtcDateIDSet = GETUTCDATE()'')

IF EXISTS(SELECT * FROM [' + @Database + '].INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = ''DataSourceInfo'')
	EXEC(''DELETE FROM [' + @Database + '].[dbo].[DataSourceInfo]'')

IF EXISTS(SELECT * FROM [' + @Database + '].INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = ''ConnectUser'')
	EXEC(''DELETE FROM [' + @Database + '].[dbo].[ConnectUser]'')
'
EXEC(@cmd)

USE master