DECLARE @state int =0;
DECLARE @errorname nvarchar(max)
DECLARE @UndifiendName nvarchar(500), @UndefinedAlterKey int
BEGIN TRAN --чтобы не повредить базу
BEGIN TRY
	
	select @UndifiendName = [Название] from [Производители] where [Идентификатор] = 0
	if (len(isnull(@UndifiendName, ''))>0)
	begin
		select top 1 @UndefinedAlterKey = [Идентификатор] from [Производители] where @UndifiendName = [Название] and [Идентификатор] > 0
		if(isnull(@UndefinedAlterKey,0)>0)
		begin
			-- здесь не должно быть ошибок
			update AdapterType set VendorID=0 where VendorID = @UndefinedAlterKey
			update PeripheralType set VendorID=0 where VendorID = @UndefinedAlterKey
			update [Типы активного оборудования] set [ИД производителя] = 0 where [ИД производителя] = @UndefinedAlterKey
			update [Типы оконечного оборудования] set [ИД производителя] = 0 where [ИД производителя] = @UndefinedAlterKey
			update SynValue set TrueID=0 where TrueID = @UndefinedAlterKey
			delete from [Производители] where [Идентификатор] = @UndefinedAlterKey;
		end
	end
    if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'ui_name' and object_id = OBJECT_ID('Производители')))
        BEGIN
            CREATE UNIQUE INDEX ui_name on [Производители] ([Название]);
        end;
END TRY
BEGIN CATCH
    SET @state = 1;
    SET @errorname = ERROR_MESSAGE()
end catch

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

