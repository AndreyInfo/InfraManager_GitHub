/****** Object:  UserDefinedFunction [dbo].[func_AssetsAccessIsGranted]    Script Date: 25.10.2022 11:21:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[func_AssetsAccessIsGranted] (@classID int, @id uniqueidentifier, @ownerID uniqueidentifier, @ownerClassID int, @type tinyint, @propagate bit)
	returns bit
as
begin
	if @ownerID is null or @ownerClassID is null or @type is null or @classID is null or @propagate is null
		return 0
	--Admin
	if @ownerClassID = 9 and exists(select * from dbo.UserRole with(nolock) where UserID = @ownerID and RoleID = '00000000-0000-0000-0000-000000000001')
		return 1
	--не проверяем ТТ3
	if ((@type = 0 or @type = 1) and exists(select * from dbo.setting where id = 93 and cast(value as varbinary(max)) = 0x00))
		return 1
	--
	declare @tmpID uniqueidentifier
	declare @tmpID2 uniqueidentifier
	declare @int int
	declare @typeID uniqueidentifier				
	--	
	 if @classID = 6 --terminalDevice
	begin
		--
		select @tmpID = r.IMObjID, @tmpID2 = wp.IMObjID, @typeID = m.ProductCatalogTypeID
		from dbo.[Оконечное оборудование] d with(nolock)
		left join dbo.Комната r with(nolock) on r.Идентификатор = d.[ИД комнаты]
		left join dbo.[Рабочее место] wp with(nolock) on wp.Идентификатор = d.[ИД рабочего места] and wp.Идентификатор <> 0
		inner join dbo.[Типы оконечного оборудования] m with(nolock) on m.Идентификатор = d.[ИД типа ОО]		
		where d.IMObjID = @id
		--
		--
		if @tmpID2 is null
			return 0
		else
		return dbo.func_AccessIsGranted(22, @tmpID2, @ownerID, @ownerClassID, @type, 1) --workplace			
	end
	else if @classID = 5 --со = ттз; 
	begin
		--
		select @tmpID = r.IMObjID, @tmpID2 = rack.IMObjID, @typeID = m.ProductCatalogTypeID
		from dbo.[Активное устройство] d with(nolock)
		left join dbo.Комната r with(nolock) on r.Идентификатор = d.RoomID
		left join dbo.Шкаф rack with(nolock) on rack.Идентификатор = d.[ИД шкафа] and rack.Идентификатор <> 0
		inner join dbo.[Типы активного оборудования] m with(nolock) on m.Идентификатор = d.[ИД типа]
		where d.IMObjID = @id
		--
		if @tmpID2 is null
			return 0
		else
			return dbo.func_AccessIsGranted(4, @tmpID2, @ownerID, @ownerClassID, @type, 1) --rack		
	end
	else if @classID = 33 --адаптер = ттз;
	begin
		return 0	
	end
	else if @classID = 34 --пу = ттз; для логических - ресурсы только из ттз
	begin
		return 0
	end
	else
	return dbo.func_AccessIsGranted (@classID, @id, @ownerID, @ownerClassID, @type, @propagate ) --other class objects
	--
	return 1;
end
