 
DECLARE @tmp  nvarchar(max)
DECLARE @reset nvarchar(max)
DECLARE @state int =0;
DECLARE @errorname nvarchar(max)
BEGIN TRAN --чтобы не повредить базу		
BEGIN TRY
    IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pk_manufacturers_seq]') AND type = 'SO')
        BEGIN
            create sequence pk_manufacturers_seq;
            SET @tmp = 'ALTER SEQUENCE pk_manufacturers_seq RESTART WITH <<const>>'
            SELECT @reset = ( REPLACE(@tmp,'<<const>>', (SELECT MAX([Идентификатор])+1 from [Производители])))
            alter table [Производители] add constraint DF_manufacturers_default default next value for
                pk_manufacturers_seq for [Идентификатор]
            exec sp_executesql @reset
        END
    PRINT N'Пройдено'
END TRY
BEGIN CATCH
     SET @state = 1;
    SET @errorname = ERROR_MESSAGE()
END CATCH
IF (@state = 1)
    BEGIN
        IF @@TRANCOUNT > 0
                ROLLBACK;
        THROW 51001,@errorname,1; -- в скрипте ошибка требующая внимания
    end
ELSE
    BEGIN
        IF @@TRANCOUNT > 0
            commit
    END


