if object_id('[dbo].[Document_GetObjectIDByClassIDAndStrID]') is null
	exec('create procedure [dbo].[Document_GetObjectIDByClassIDAndStrID] 	@ClassID int,
	@ObjectID varchar(40)
	AS 
	begin
		return 0;
	end
')
go


alter  PROCEDURE [dbo].[Document_GetObjectIDByClassIDAndStrID]
	@ClassID int,
	@ObjectID varchar(40)
AS 
	if (@ClassID = 29) --OWNER
	begin
		SELECT '{00000000-0000-0000-0000-000000000001}' as StrID
	end
	else
	if (@ObjectID ='0')
	begin
		select top 0 Version as StrID from dbo.DBInfo
	end
	else
	if (@ClassID = 101) --ORG
	begin
		SELECT [идентификатор] as StrID from dbo.[организация] where [Идентификатор] = @ObjectID
	end
	else
	if (@ClassID = 1) --BUILDING
	begin
		SELECT IMObjID as StrID from dbo.[Здание] 
		WHERE  [Идентификатор] = @ObjectID
	end
	else
	if (@ClassID = 2) --FLOOR
	begin
		SELECT IMObjID as StrID from dbo.[Этаж] 
		WHERE  [Идентификатор] = @ObjectID
	end
	else
	if (@ClassID = 3) --ROOM
	begin
		SELECT IMObjID as StrID from dbo.[Комната] 
		WHERE  [Идентификатор] = @ObjectID
	end
	else 
	if (@ClassID = 4) --RACK
	begin
		SELECT IMObjID as StrID from dbo.[Шкаф] 
		WHERE  [Идентификатор] = @ObjectID
	end
	else 
	if (@ClassID = 22) --WORKPLACE
	begin
		SELECT IMObjID as StrID from dbo.[Рабочее место] 
		WHERE  [Идентификатор] = @ObjectID
	end
	else
	if (@ClassID = 5) --NETWORKDEVICE
	begin
		SELECT IMObjID as StrID from dbo.[Активное устройство] 
		WHERE  [Идентификатор] = @ObjectID and [Название] is not null
	end
	else
	if (@ClassID = 6) --TERMINALDEVICE
	begin
		SELECT IMObjID as StrID from dbo.[Оконечное оборудование] 
		WHERE  [Идентификатор] = @ObjectID and [Название] is not null
	end
	else 
	if (@ClassID = 7) --PANEL
	begin
		SELECT IMObjID as StrID from dbo.[Панель] 
		WHERE  [Идентификатор] = @ObjectID and [Название] is not null
	end
	else
	if (@ClassID = 8) --OUTLET
	begin
		SELECT IMObjID as StrID from dbo.[Розетка] 
		WHERE  [Идентификатор] = @ObjectID
	end
	else
	if (@ClassID = 13) --ACTIVE PORT
	begin
		SELECT IMObjID as StrID from dbo.[Порт активный] 
		WHERE  [Идентификатор] = @ObjectID
	end
	else
	if (@ClassID = 15) --PANEL PORT
	begin
		SELECT IMObjID as StrID from dbo.[Порт пассивный] 
		WHERE  [Идентификатор] = @ObjectID
	end
	else
	if (@ClassID = 14) --OUTLET PORT  
	begin
		SELECT IMObjID as StrID from dbo.[Порт пассивный] 
		WHERE  [Идентификатор] = @ObjectID
	end
	else
	if (@ClassID = 71) --Installation
	begin
		SELECT IMObjID as StrID from dbo._ConfigurationItemInstallation
		WHERE  (ClassID = 71)  and OldID = @ObjectID
	end
	else
	if (@ClassID = 9) --User
	begin
		SELECT IMObjID as StrID from dbo.[Пользователи] 
		WHERE  [Идентификатор] = @ObjectID
	end
	else
	if (@ClassID = 33) --Adapter
	begin
		SELECT AdapterID as StrID from dbo.[Adapter] where AdapterID = @ObjectID
	end
	else
	if (@ClassID = 34) --Peripheral
	begin
		SELECT PeripheralID as StrID from dbo.[Peripheral] where PeripheralID = @ObjectID
	end
	else
	if (@ClassID = 35) --Splitter
	begin
		SELECT SplitterID as StrID from dbo.[Splitter] where SplitterID = @ObjectID
	end
	else
	if (@ClassID = 102) --Subdivision
	begin
		SELECT идентификатор as StrID from dbo.[Подразделение] where [Идентификатор] = @ObjectID
	end
	else
	if (@ClassID = 93) --networkDeviceModel
	begin
		select IMObjID as StrID from dbo.[Типы активного оборудования] where Идентификатор = @ObjectID
	end
	else
	if (@ClassID = 94) --terminalDeviceModel
	begin
		select IMObjID as StrID from dbo.[Типы оконечного оборудования] where Идентификатор = @ObjectID
	end
	else
	if (@ClassID = 98) --rackModel
	begin
		select IMObjID as StrID from dbo.[Типы шкафов] where Идентификатор = @ObjectID
	end

