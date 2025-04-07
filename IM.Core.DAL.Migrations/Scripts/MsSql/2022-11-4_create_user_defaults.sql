DECLARE @tmp   nvarchar(max)
DECLARE @reset nvarchar(max)
DECLARE @state int =0;
DECLARE @errorname nvarchar(max)
BEGIN TRAN --чтобы не повредить базу		
BEGIN TRY
    IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pk_users_seq]') AND type = 'SO')
        BEGIN
            create sequence pk_users_seq;
            SET @tmp = 'ALTER SEQUENCE pk_users_seq RESTART WITH <<const>>'
            SELECT @reset = (REPLACE(@tmp, '<<const>>', (SELECT MAX([Идентификатор]) + 1 from [Пользователи])))
            alter table [Пользователи]
                add constraint DF_users_default default next value for
                    pk_users_seq for [Идентификатор]
            exec sp_executesql @reset
        END
    if not exists(
            select top 1 0
            from sys.all_columns c
                     join sys.tables t on t.object_id = c.object_id
                     join sys.schemas s on s.schema_id = t.schema_id
                     join sys.default_constraints d on c.default_object_id = d.object_id
            where t.name = 'Пользователи'
              and c.name = 'IMObjID'
              and s.name = 'dbo')
        begin
            alter table [Пользователи]
                add default NEWID() FOR IMObjID;
        end

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


