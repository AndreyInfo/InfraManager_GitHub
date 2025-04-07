/****** Object:  UserDefinedFunction [dbo].[func_AccessIsGranted]    Script Date: 25.10.2022 11:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER function [dbo].[func_AccessIsGranted] (@classID int, @id uniqueidentifier, @ownerID uniqueidentifier, @ownerClassID int, @type tinyint, @propagate bit)
	returns bit
as
begin
	if @ownerID is null or @ownerClassID is null or @type is null or @classID is null or @propagate is null
		return 0
	--Admin
	if @ownerClassID = 9 and exists(select * from dbo.UserRole with(nolock) where UserID = @ownerID and RoleID = '00000000-0000-0000-0000-000000000001')
		return 1
	--не проверяем ТТ3
	if ((@type = 0 or @type = 1) and exists(select * from dbo.setting with(nolock) where id = 93 and cast(value as varbinary(max)) = 0x00))
		return 1
	--
	declare @tmpID uniqueidentifier
	declare @tmpID2 uniqueidentifier
	declare @int int
	declare @typeID uniqueidentifier
	--
	if @classID = 29 --owner
	begin
		if exists(select * from dbo.ObjectAccess with(nolock) where OwnerID = @ownerID and [Type] = @type and (@propagate = 0 or Propagate = 1) and ClassID = @classID) --owner
			return 1
		--
		return 0
	end
	else if @classID = 101 --organization
	begin
		if exists(select * from dbo.ObjectAccess with(nolock) where OwnerID = @ownerID and [Type] = @type and (@propagate = 0 or Propagate = 1) and ClassID = @classID and ObjectID = @id) --organization
			return 1
		--owner
		return dbo.func_AccessIsGranted(29, '00000000-0000-0000-0000-000000000000', @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 102 --subdivision (recursive)
	begin
		if exists(select * from dbo.ObjectAccess with(nolock) where OwnerID = @ownerID and [Type] = @type and (@propagate = 0 or Propagate = 1) and ClassID = @classID and ObjectID = @id) --subdivision
			return 1
		--
		select @tmpID = [ИД организации], @tmpID2 = [ИД подразделения]
		from dbo.Подразделение with(nolock)
		where Идентификатор = @id
		--
		while not(@tmpID2 is null)
		begin
			if exists(select * from dbo.ObjectAccess with(nolock) where OwnerID = @ownerID and [Type] = @type and Propagate = 1 and ClassID = @classID and ObjectID = @tmpID2) --parent subdivision
				return 1
			--
			select @tmpID = [ИД организации], @tmpID2 = [ИД подразделения]
			from dbo.Подразделение with(nolock)
			where Идентификатор = @tmpID2
		end
		--organization
		return dbo.func_AccessIsGranted(101, @tmpID, @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 9 --user
	begin
		if @id = '00000000-0000-0000-0000-000000000000'
			return 1
		--
		if @type = 1 or @type = 2 --sks
		begin
			select @tmpID = wp.IMObjID, @int = wp.Идентификатор
			from dbo.Пользователи u with(nolock)
			inner join dbo.[Рабочее место] wp with(nolock) on wp.Идентификатор = u.[ИД рабочего места]
			where u.IMObjID = @id
			--
			if @int = 0
				return 1 --no workplace - access granted
			else
				set @int = 22
		end
		else
			select @tmpID = [ИД подразделения], @int = 102
			from dbo.Пользователи with(nolock)
			where IMObjID = @id
		--workplace/subdivision
		return dbo.func_AccessIsGranted(@int, @tmpID, @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 1 --building
	begin
		if exists(select * from dbo.ObjectAccess with(nolock) where OwnerID = @ownerID and [Type] = @type and (@propagate = 0 or Propagate = 1) and ClassID = @classID and ObjectID = @id) --building
			return 1
		--
		select @tmpID = [ИД организации], @int = Идентификатор
		from dbo.Здание with(nolock)
		where IMObjID = @id
		--
		if @int = 0
			return 1 --no building - access granted
		--organization
		return dbo.func_AccessIsGranted(101, @tmpID, @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 2 --floor
	begin
		if exists(select * from dbo.ObjectAccess with(nolock) where OwnerID = @ownerID and [Type] = @type and (@propagate = 0 or Propagate = 1) and ClassID = @classID and ObjectID = @id) --floor
			return 1
		--
		select @tmpID = b.IMObjID, @int = f.Идентификатор
		from dbo.Этаж f with(nolock)
		inner join dbo.Здание b with(nolock) on b.Идентификатор = f.[ИД здания]
		where f.IMObjID = @id
		--
		if @int = 0
			return 1 --no floor - access granted
		--building
		return dbo.func_AccessIsGranted(1, @tmpID, @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 3 --room
	begin
		if exists(select * from dbo.ObjectAccess with(nolock) where OwnerID = @ownerID and [Type] = @type and (@propagate = 0 or Propagate = 1) and ClassID = @classID and ObjectID = @id) --room
			return 1
		--
		select @tmpID = f.IMObjID, @int = r.Идентификатор
		from dbo.Комната r with(nolock)
		inner join dbo.Этаж f with(nolock) on f.Идентификатор = r.[ИД этажа]
		where r.IMObjID = @id
		--
		if @int = 0
			return 1 --no room - access granted
		--floor
		return dbo.func_AccessIsGranted(2, @tmpID, @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 22 --workplace
	begin
		select @tmpID = r.IMObjID, @int = wp.Идентификатор
		from dbo.[Рабочее место] wp with(nolock)
		inner join dbo.Комната r with(nolock) on r.Идентификатор = wp.[ИД комнаты]
		where wp.IMObjID = @id
		--
		if @int = 0
			return 1 --no workplace - access granted
		--room
		return dbo.func_AccessIsGranted(3, @tmpID, @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 4 --rack
	begin
		if exists(select * from dbo.ObjectAccess with(nolock) where OwnerID = @ownerID and [Type] = @type and (@propagate = 0 or Propagate = 1) and ClassID = @classID and ObjectID = @id) --rack
			return 1
		--
		select @tmpID = r.IMObjID
		from dbo.Шкаф rack with(nolock)
		inner join dbo.Комната r with(nolock) on r.Идентификатор = rack.[ИД комнаты]
		where rack.IMObjID = @id
		--room
		return dbo.func_AccessIsGranted(3, @tmpID, @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 6 --terminalDevice
	begin
		if exists(select *
				from dbo.[Оконечное оборудование] d with(nolock)
				inner join dbo.Asset a with(nolock) on a.DeviceID = d.Идентификатор
				left join dbo.Пользователи u with(nolock) on u.Идентификатор = a.UserID
				where d.IMObjID = @id and (a.UtilizerID = @ownerID or u.IMObjID = @ownerID))
			return 1
		--
		select @tmpID = r.IMObjID, @tmpID2 = wp.IMObjID, @typeID = m.ProductCatalogTypeID
		from dbo.[Оконечное оборудование] d with(nolock)
		left join dbo.Комната r with(nolock) on r.Идентификатор = d.[ИД комнаты]
		left join dbo.[Рабочее место] wp with(nolock) on wp.Идентификатор = d.[ИД рабочего места] and wp.Идентификатор <> 0
		inner join dbo.[Типы оконечного оборудования] m with(nolock) on m.Идентификатор = d.[ИД типа ОО]		
		where d.IMObjID = @id
		--
		if dbo.func_AccessIsGranted(378, @typeID, @ownerID, @ownerClassID, 0, 1) = 0
			return 0
		--
		if @tmpID2 is null
			return dbo.func_AccessIsGranted(3, @tmpID, @ownerID, @ownerClassID, @type, 1) --room
		else
			return dbo.func_AccessIsGranted(22, @tmpID2, @ownerID, @ownerClassID, @type, 1) --workplace
	end
	else if @classID = 5 --со = ттз; 
	begin
		if exists(select *
				from dbo.[Активное устройство] d with(nolock)
				inner join dbo.Asset a with(nolock) on a.DeviceID = d.Идентификатор
				left join dbo.Пользователи u with(nolock) on u.Идентификатор = a.UserID
				where d.IMObjID = @id and (a.UtilizerID = @ownerID or u.IMObjID = @ownerID or d.OrganizationItemID = @ownerID))
			return 1
		--
		select @tmpID = r.IMObjID, @tmpID2 = rack.IMObjID, @typeID = m.ProductCatalogTypeID
		from dbo.[Активное устройство] d with(nolock)
		left join dbo.Комната r with(nolock) on r.Идентификатор = d.RoomID
		left join dbo.Шкаф rack with(nolock) on rack.Идентификатор = d.[ИД шкафа] and rack.Идентификатор <> 0
		inner join dbo.[Типы активного оборудования] m with(nolock) on m.Идентификатор = d.[ИД типа]
		where d.IMObjID = @id
		--
		if dbo.func_AccessIsGranted(378, @typeID, @ownerID, @ownerClassID, 0, 1) = 0
			return 0
		--
		if @tmpID2 is null
			return dbo.func_AccessIsGranted(3, @tmpID, @ownerID, @ownerClassID, @type, 1) --room
		else
			return dbo.func_AccessIsGranted(4, @tmpID2, @ownerID, @ownerClassID, @type, 1) --rack		
	end
	else if @classID = 33 --адаптер = ттз;
	begin
		if exists(select *
				from dbo.Adapter d with(nolock)
				inner join dbo.Asset a with(nolock) on a.DeviceID = d.IntID
				left join dbo.Пользователи u with(nolock) on u.Идентификатор = a.UserID
				where d.AdapterID = @id and (a.UtilizerID = @ownerID or u.IMObjID = @ownerID))
			return 1
		--
		select @tmpID = (case when a.RoomID<>0 then r.IMObjID
								when a.NetworkDeviceID<>0 then rnd.IMObjID
								when a.TerminalDeviceID<>0 then rtd.IMObjID
								else CAST(null as uniqueidentifier) end),
				@int = (case when a.RoomID<>0 then 3
								when a.NetworkDeviceID<>0 then 3
								when a.TerminalDeviceID<>0 then 3
								else 0 end),				
				@typeID = m.ProductCatalogTypeID
		from dbo.Adapter a with(nolock)
		inner join dbo.AdapterType m with(nolock) on m.AdapterTypeID = a.AdapterTypeID
		left join dbo.Комната r with(nolock) on r.Идентификатор = a.RoomID and a.RoomID <> 0
		left join dbo.[Оконечное оборудование] td with(nolock) on td.Идентификатор = a.TerminalDeviceID and a.TerminalDeviceID <> 0
		left join dbo.Комната rtd with(nolock) on rtd.Идентификатор = td.[ИД комнаты] and td.[ИД комнаты] <> 0
		left join dbo.[Активное устройство] nd with(nolock) on nd.Идентификатор = a.NetworkDeviceID and a.NetworkDeviceID <> 0
		left join dbo.Комната rnd with(nolock) on rnd.Идентификатор = nd.RoomID and nd.RoomID <> 0
		where a.AdapterID =  @id
		--
		if dbo.func_AccessIsGranted(378, @typeID, @ownerID, @ownerClassID, 0, 1) = 0
			return 0
		--room/networkDevice/teminalDevice
		return dbo.func_AccessIsGranted(@int, @tmpID, @ownerID, @ownerClassID, @type, 1)		
	end
	else if @classID = 34 --пу = ттз; для логических - ресурсы только из ттз
	begin
		if exists(select *
				from dbo.Peripheral d with(nolock)
				inner join dbo.Asset a with(nolock) on a.DeviceID = d.IntID
				left join dbo.Пользователи u with(nolock) on u.Идентификатор = a.UserID
				where d.PeripheralID = @id and (a.UtilizerID = @ownerID or u.IMObjID = @ownerID))
			return 1
		--
		select @tmpID = (case when p.RoomID<>0 then r.IMObjID
								when p.NetworkDeviceID<>0 then rnd.IMObjID
								when p.TerminalDeviceID<>0 then rtd.IMObjID
								else CAST(null as uniqueidentifier) end),
				@int = (case when p.RoomID<>0 then 3
								when p.NetworkDeviceID<>0 then 3
								when p.TerminalDeviceID<>0 then 3
								else 0 end),
				@typeID = m.ProductCatalogTypeID
		from dbo.Peripheral p with(nolock)
		inner join dbo.PeripheralType m with(nolock) on m.PeripheralTypeID = p.PeripheralTypeID
		left join dbo.Комната r with(nolock) on r.Идентификатор = p.RoomID and p.RoomID <> 0
		left join dbo.[Оконечное оборудование] td with(nolock) on td.Идентификатор = p.TerminalDeviceID and p.TerminalDeviceID <> 0
		left join dbo.Комната rtd with(nolock) on rtd.Идентификатор = td.[ИД комнаты] and td.[ИД комнаты] <> 0
		left join dbo.[Активное устройство] nd with(nolock) on nd.Идентификатор = p.NetworkDeviceID and p.NetworkDeviceID <> 0
		left join dbo.Комната rnd with(nolock) on rnd.Идентификатор = nd.RoomID and nd.RoomID <> 0
		where p.PeripheralID = @id
		--
		if dbo.func_AccessIsGranted(378, @typeID, @ownerID, @ownerClassID, 0, 1) = 0
			return 0
		--room/networkDevice/teminalDevice
		return dbo.func_AccessIsGranted(@int, @tmpID, @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 71 --SoftwareInstallation
	begin
		select @tmpID = DeviceID, @int = DeviceClassID
		from dbo.SoftwareInstallation with(nolock)
		where ID = @id
		--
		return dbo.func_AccessIsGranted(@int, @tmpID, @ownerID, @ownerClassID, @type, 0) --terminalDevice/networkDevice
	end
	else if @classID = 348 --DiscArray
	begin
		select @tmpID = DeviceID, @int = DeviceClassID
		from dbo.DiscArray with(nolock)
		where ID = @id
		--
		return dbo.func_AccessIsGranted(@int, @tmpID, @ownerID, @ownerClassID, @type, 0) --terminalDevice/networkDevice
	end
	else if @classID = 13 --ActivePort
	begin
		select @tmpID = d.IMObjID
		from dbo.[Порт активный] p with(nolock)
		inner join dbo.[Активное устройство] d with(nolock) on d.Идентификатор = p.[ИД активного устройства]
		where p.IMObjID = @id
		--
		return dbo.func_AccessIsGranted(5, @tmpID, @ownerID, @ownerClassID, @type, 0) --networkDevice
	end
	else if @classID = 7 --Panel
	begin
		select @tmpID = r.IMObjID
		from dbo.Панель p with(nolock)
		inner join dbo.Шкаф r with(nolock) on r.Идентификатор = p.[ИД шкафа]
		where p.IMObjID = @id
		--rack
		return dbo.func_AccessIsGranted(4, @tmpID, @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 8 --Outlet
	begin
		select @tmpID = r.IMObjID
		from dbo.Розетка o with(nolock)
		inner join dbo.Комната r with(nolock) on r.Идентификатор = o.[ИД комнаты]
		where o.IMObjID = @id
		--room
		return dbo.func_AccessIsGranted(3, @tmpID, @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 14 or @classID = 15 --PanelPort / OutletPort
	begin
		select @tmpID = t.ID, @int = t.ClassID
		from dbo.[Порт пассивный] p with(nolock)
		inner join
		(
			select Идентификатор as IntID, IMObjID as ID, 8 as ClassID from dbo.Розетка with(nolock)
			union all
			select Идентификатор as IntID, IMObjID as ID, 7 as ClassID from dbo.Панель with(nolock)
		) t on t.IntID = p.[ИД розетки/панели]
		where p.IMObjID = @id
		--
		return dbo.func_AccessIsGranted(@int, @tmpID, @ownerID, @ownerClassID, @type, 0) --penel / outlet
	end
	else if @classID = 35 --SplitterPort
	begin
		select @int = [ИД розетки/панели] from dbo.[Порт пассивный] with(nolock) where IMObjID = @id
		select @id = IMObjID from dbo.[Порт пассивный] with(nolock) where Идентификатор = @int
		if @id is null
			return 0
		--
		return dbo.func_AccessIsGranted(@int / 1000000, @id, @ownerID, @ownerClassID, @type, 0) --penel / outlet
	end
	else if @classID = 119 --workOrder
	begin
	declare @WorkOrderInitiatorID uniqueidentifier
	declare @WorkOrderExecutorID uniqueidentifier
	declare @WorkOrderAssignorID uniqueidentifier
	declare @WorkOrderQueueID uniqueidentifier
	select TOP 1 @WorkOrderInitiatorID = InitiatorID, @WorkOrderExecutorID = ExecutorID, @WorkOrderAssignorID = AssignorID, @WorkOrderQueueID = QueueID from dbo.WorkOrder with(nolock) where ID = @id
		--if exists(select * from dbo.WorkOrder with(nolock) where ID = @id and (InitiatorID = @ownerID or ExecutorID = @ownerID or AssignorID = @ownerID or QueueID = @ownerID))
		if (@WorkOrderInitiatorID = @ownerID or @WorkOrderExecutorID = @ownerID or @WorkOrderAssignorID = @ownerID or @WorkOrderQueueID = @ownerID)
			return 1
		if @ownerClassID = 9
		begin
			if @WorkOrderQueueID is not null and exists(select 1 from dbo.QueueUser qu with(nolock) where qu.QueueID = @WorkOrderQueueID and qu.UserID = @ownerID)
			--if exists(select 1 from dbo.WorkOrder w with(nolock) where w.ID = @id and exists(select 1 from dbo.QueueUser qu with(nolock) where qu.QueueID = w.QueueID and qu.UserID = @ownerID))
				return 1
			--
			--599 Видеть задания ИТ-сотрудников
			if @ownerClassID = 9 and exists(select ro.OperationID
						from dbo.UserRole ur with(nolock)
						inner join dbo.RoleOperation ro with(nolock) on ro.RoleID = ur.RoleID
						where ur.UserID = @ownerID and ro.OperationID = 599)
				begin
					declare @UserDeptID uniqueidentifier
					declare @InitiatorDeptID uniqueidentifier
					declare @AssignorDeptID uniqueidentifier
					declare @ExecutorDeptID uniqueidentifier
					declare @UserOrgID uniqueidentifier
					declare @InitiatorOrgID uniqueidentifier
					declare @AssignorOrgID uniqueidentifier
					declare @ExecutorOrgID uniqueidentifier
					select TOP 1    @InitiatorDeptID = uO.[DivisionID], @InitiatorOrgID = uO.[OrganizationID] from [dbo].[view_User] uO with(nolock) where uO.ID = @WorkOrderInitiatorID
					select TOP 1    @UserDeptID = uO.[DivisionID], @UserOrgID = uO.[OrganizationID] from [dbo].[view_User] uO with(nolock) where uO.ID = @ownerID
					--
					if (@UserOrgID is not null)
						begin
							if(@UserOrgID is not null and @InitiatorOrgID is not null and @UserOrgID = @InitiatorOrgID and 
							dbo.func_SubdivisionIsSibling(@UserDeptID, @InitiatorDeptID) = 1)
								return 1
							select TOP 1    @AssignorDeptID = uO.[DivisionID], @AssignorOrgID = uO.[OrganizationID] from [dbo].[view_User] uO with(nolock) where uO.ID = @WorkOrderAssignorID
							if(@UserOrgID is not null and @AssignorOrgID is not null and @UserOrgID = @AssignorOrgID and 
							dbo.func_SubdivisionIsSibling(@UserDeptID, @AssignorDeptID) = 1)
								return 1
							select TOP 1    @ExecutorDeptID = uO.[DivisionID], @ExecutorOrgID = uO.[OrganizationID] from [dbo].[view_User] uO with(nolock) where uO.ID = @WorkOrderExecutorID
							if(@UserOrgID is not null and @ExecutorOrgID is not null and @UserOrgID = @ExecutorOrgID and 
							dbo.func_SubdivisionIsSibling(@UserDeptID, @ExecutorDeptID) = 1)
								return 1
						end
				end
			declare @TOZQueueResponsible bit
			declare @WorkorderQueueResponsibleID uniqueidentifier
			if(@WorkOrderQueueID is not null)
				begin
					select top 1 @WorkorderQueueResponsibleID = q.[ResponsibleID] from [dbo].[Queue] q where q.[ID]=@WorkOrderQueueID
						if (@WorkorderQueueResponsibleID is not null)
							begin
								if (dbo.func_AccessIsGranted(9, @WorkorderQueueResponsibleID, @ownerID, @ownerClassID, 2, 0) = 0)
									set @TOZQueueResponsible = 0
								else if (dbo.func_AccessIsGranted(9, @WorkorderQueueResponsibleID, @ownerID, @ownerClassID, 3, 0) = 0)
									set @TOZQueueResponsible = 0
								else
									set @TOZQueueResponsible = 1
									if 	@TOZQueueResponsible = 1
										return 1
							end
				end
			declare @TOZInitiator bit
			declare @TOZAssignor bit
			declare @TOZExecutor bit
			if (@ownerClassID = 9 and @WorkOrderExecutorID is null) and
				exists(select 1 from dbo.UserRole with(nolock) where UserID = @ownerID and 
				RoleID in (select distinct RoleID from dbo.RoleOperation with(nolock) where OperationID = 358)) --executor
					begin
						if (@WorkOrderInitiatorID is not null)
							begin
								if (dbo.func_AccessIsGranted(9, @WorkOrderInitiatorID, @ownerID, @ownerClassID, 2, 0) = 0)
									set @TOZInitiator = 0
								else if (dbo.func_AccessIsGranted(9, @WorkOrderInitiatorID, @ownerID, @ownerClassID, 3, 0) = 0)
									set @TOZInitiator = 0
								else
									set @TOZInitiator = 1
							end
						if 	@TOZInitiator = 1
								return 1
						if (@WorkOrderAssignorID is not null)
							begin
								if (dbo.func_AccessIsGranted(9, @WorkOrderAssignorID, @ownerID, @ownerClassID, 2, 0) = 0)
									set @TOZAssignor = 0
								else if (dbo.func_AccessIsGranted(9, @WorkOrderAssignorID, @ownerID, @ownerClassID, 3, 0) = 0)
									set @TOZAssignor = 0
								else
									set @TOZAssignor = 1
							end
						if 	@TOZAssignor = 1
								return 1
					end
			if (@ownerClassID = 9 and 
				exists(select 1 from dbo.UserRole with(nolock) where UserID = @ownerID and 
				RoleID in (select distinct RoleID from dbo.RoleOperation with(nolock) where OperationID = 357)) --SDadmin
				)
					begin
						if (@WorkOrderInitiatorID is not null and @TOZInitiator is null)
							begin
								if (dbo.func_AccessIsGranted(9, @WorkOrderInitiatorID, @ownerID, @ownerClassID, 2, 0) = 0)
									set @TOZInitiator = 0
								else if (dbo.func_AccessIsGranted(9, @WorkOrderInitiatorID, @ownerID, @ownerClassID, 3, 0) = 0)
									set @TOZInitiator = 0
								else
									set @TOZInitiator = 1
							end
						if 	@TOZInitiator = 1
								return 1
						if (@WorkOrderAssignorID is not null and @TOZAssignor is null)
							begin
								if (dbo.func_AccessIsGranted(9, @WorkOrderAssignorID, @ownerID, @ownerClassID, 2, 0) = 0)
									set @TOZAssignor = 0
								else if (dbo.func_AccessIsGranted(9, @WorkOrderAssignorID, @ownerID, @ownerClassID, 3, 0) = 0)
									set @TOZAssignor = 0
								else
									set @TOZAssignor = 1
							end
						if 	@TOZAssignor = 1
								return 1
						if (@WorkOrderExecutorID is not null and
							dbo.func_AccessIsGranted(9, @WorkOrderExecutorID, @ownerID, @ownerClassID, 2, 0) = 1 and 
							dbo.func_AccessIsGranted(9, @WorkOrderExecutorID, @ownerID, @ownerClassID, 3, 0) = 1
						)
							return 1
				end
		end
		--
		return 0
	end
	else if @classID = 701 --call
	begin
	    declare @CallOwnerID uniqueidentifier
	    declare @CallExecutorID uniqueidentifier
	    declare @CallClientID uniqueidentifier
		declare @CallInitiatorID uniqueidentifier
        declare @CallServiceItemID uniqueidentifier
        declare @CallServiceAttendanceID uniqueidentifier
		declare @CallWorkflowSchemeID uniqueidentifier
		declare @CallQueueID uniqueidentifier
		declare @CallServicePlaceID uniqueidentifier
		declare @CallServicePlaceClassID int
	    select  TOP 1 @CallQueueID = QueueID,  @CallServiceAttendanceID = cs.ServiceAttendanceID,  @CallServiceItemID = cs.ServiceItemID, @CallOwnerID = OwnerID, 
		@CallClientID = ClientID, @CallInitiatorID = InitiatorID,  @CallExecutorID = ExecutorID, @CallWorkflowSchemeID = WorkflowSchemeID,
		@CallServicePlaceID = ServicePlaceID,  @CallServicePlaceClassID = ServicePlaceClassID
		from dbo.[Call] c with(nolock) 
		join dbo.[CallService] cs with(nolock) on c.CallServiceID = cs.ID
		where c.ID=@id
		if ( @CallClientID = @ownerID or @CallOwnerID = @ownerID or @CallExecutorID = @ownerID or @CallInitiatorID = @ownerID or @CallQueueID = @ownerID)
			return 1
		if  @CallQueueID is not null and exists(select TOP 1 qu.QueueID from dbo.QueueUser qu with(nolock) where qu.QueueID = @CallQueueID and qu.UserID = @ownerID)
			return 1
		declare @TOZSKS bit
		declare @TOZORG bit
		declare @TOZServiceItem bit
		declare @TOZServiceAttendance bit
		if (@CallOwnerID is null)
		begin
			if @ownerClassID = 9 and (@CallWorkflowSchemeID is null) and ((@CallServiceItemID is null) and (@CallServiceAttendanceID is null)) 
				and not exists(select * from dbo.UserRole with(nolock) where UserID = @ownerID and RoleID in (select distinct RoleID from dbo.RoleOperation with(nolock) where OperationID = 656)) --UnClassified
				return 0
			-- service place toz sks
			if ((@CallServicePlaceClassID is not null) and (@CallServicePlaceID is not null))
				begin
					if (@CallServicePlaceClassID = 3)
						set @TOZSKS = dbo.func_AccessIsGranted(3, @CallServicePlaceID, @ownerID, @ownerClassID, 2, 0)
					else if (@CallServicePlaceClassID = 22)
						set @TOZSKS = dbo.func_AccessIsGranted(22, @CallServicePlaceID, @ownerID, @ownerClassID, 2, 0)
					else 
						set @TOZSKS = 0
				end
			else
			set @TOZSKS = dbo.func_AccessIsGranted(9, @CallClientID, @ownerID, @ownerClassID, 2, 0)
			---
			if (@TOZSKS = 0)
				return 0
			set @TOZORG = dbo.func_AccessIsGranted(9, @CallClientID, @ownerID, @ownerClassID, 3, 0)
			if (@TOZORG = 0)
				return 0
				--if  @CallQueueID is not null and exists(select TOP 1 qu.QueueID from dbo.QueueUser qu with(nolock) where qu.QueueID = @CallQueueID and qu.UserID = @ownerID)
			--	return 1
			--if dbo.func_AccessIsGranted(9, @CallClientID, @ownerID, @ownerClassID, 2, 0) = 0 --toz sks
			--	return 0
			--if dbo.func_AccessIsGranted(9, @CallClientID, @ownerID, @ownerClassID, 3, 0) = 0 --toz org
			--	return 0
		--
		--	select  TOP 1 @tmpID = c.ServiceItemID, @tmpID2 = c.ServiceAttendanceID
		--	from dbo.[Call] c with(nolock) 
		--	where c.ID = @id 
			--
			if ( @CallServiceItemID is not null)
				set @TOZServiceItem = dbo.func_AccessIsGranted(406, @CallServiceItemID, @ownerID, @ownerClassID, 4, 0)
			else
			if ( @CallServiceAttendanceID is not null)
				set @TOZServiceAttendance = dbo.func_AccessIsGranted(407, @CallServiceAttendanceID, @ownerID, @ownerClassID, 4, 0)
			--if (not @CallServiceItemID is null) or (not @CallServiceAttendanceID is null)
			if (@TOZServiceItem is not null and @TOZServiceItem = 0)
				return 0
			if (@TOZServiceAttendance is not null and @TOZServiceAttendance = 0)
				return 0
			--begin
				--set @TOZServiceItem = dbo.func_AccessIsGranted(406, @CallServiceItemID, @ownerID, @ownerClassID, 4, 0)
				--if (not @CallServiceItemID is null) and dbo.func_AccessIsGranted(406, @CallServiceItemID, @ownerID, @ownerClassID, 4, 0) = 0
				--if (@TOZServiceItem = 0)
				--	return 0
				--else if (not @CallServiceAttendanceID is null) and dbo.func_AccessIsGranted(407, @CallServiceAttendanceID, @ownerID, @ownerClassID, 4, 0) = 0
				--	return 0
			--end
--			else if @ownerClassID = 9 and (@CallWorkflowSchemeID is null) 
--					and not exists(select * from dbo.UserRole with(nolock) where UserID = @ownerID and RoleID in (select distinct RoleID from dbo.RoleOperation with(nolock) where OperationID = 656)) --UnClassified
--				return 0   --move up
			if @ownerClassID = 9 and 
			--@CallOwnerID is null and
						exists(select * from dbo.UserRole with(nolock) where UserID = @ownerID and RoleID in (select distinct RoleID from dbo.RoleOperation with(nolock) where OperationID = 373)) --owner
				return 1
		end
		--
		--if ( @CallOwnerID = @ownerID or @CallExecutorID = @ownerID or @CallInitiatorID = @ownerID or @CallQueueID = @ownerID)  --go first
		--	return 1
		--
		if @ownerClassID != 9 or exists(select * from dbo.UserRole with(nolock) where UserID = @ownerID and RoleID in (select distinct RoleID from dbo.RoleOperation with(nolock) where OperationID = 357)) --SDadmin
		begin
			-- select  TOP 1 @tmpID = c.ClientID from dbo.[Call] c with(nolock) where c.ID = @id  
		--	if dbo.func_AccessIsGranted(9, @CallClientID, @ownerID, @ownerClassID, 2, 0) = 1 --toz sks
		--		and 
		--		dbo.func_AccessIsGranted(9, @CallClientID, @ownerID, @ownerClassID, 3, 0) = 1 --toz org
		if (@TOZSKS is null)
		begin
					if ((@CallServicePlaceClassID is not null) and (@CallServicePlaceID is not null))
						begin
							if (@CallServicePlaceClassID = 3)
								set @TOZSKS = dbo.func_AccessIsGranted(3, @CallServicePlaceID, @ownerID, @ownerClassID, 2, 0)
							else if (@CallServicePlaceClassID = 22)
								set @TOZSKS = dbo.func_AccessIsGranted(22, @CallServicePlaceID, @ownerID, @ownerClassID, 2, 0)
							else 
								set @TOZSKS = 0
						end
					else
						set @TOZSKS = dbo.func_AccessIsGranted(9, @CallClientID, @ownerID, @ownerClassID, 2, 0)
		end
			--set @TOZSKS = dbo.func_AccessIsGranted(9, @CallClientID, @ownerID, @ownerClassID, 2, 0)
		if (@TOZORG is null)
			set @TOZORG = dbo.func_AccessIsGranted(9, @CallClientID, @ownerID, @ownerClassID, 3, 0)
			if 	(@TOZSKS = 1 and @TOZORG = 1)
			begin
				--select  TOP 1 @tmpID = c.ServiceItemID, @tmpID2 = c.ServiceAttendanceID
				--from dbo.[Call] c with(nolock)  
				--where c.ID = @id
				--
				if ( @CallServiceItemID is not null)
					begin
						if(@TOZServiceItem is null)
							set @TOZServiceItem = dbo.func_AccessIsGranted(406, @CallServiceItemID, @ownerID, @ownerClassID, 4, 0)
						if(@TOZServiceItem = 1)
							return 1
					end
				else if ( @CallServiceAttendanceID is not null)
					begin
						if(@TOZServiceAttendance is null)
							set @TOZServiceAttendance = dbo.func_AccessIsGranted(407, @CallServiceAttendanceID, @ownerID, @ownerClassID, 4, 0)
						if(@TOZServiceAttendance = 1)
							return 1
					end
				else
				  return 1
				--if ( @CallServiceItemID is not null)
				--begin
				--	if dbo.func_AccessIsGranted(406, @CallServiceItemID, @ownerID, @ownerClassID, 4, 0) = 1
				--		return 1
				--end
				--else if (@CallServiceAttendanceID is not null)
				--begin
				--	if dbo.func_AccessIsGranted(407, @CallServiceAttendanceID, @ownerID, @ownerClassID, 4, 0) = 1
				--		return 1
				--end
				--else
				--	return 1
			end
		end
		--
		if @ownerClassID = 9   --user, not group
		begin
		    -- go second
			--if  @CallQueueID is not null and exists(select TOP 1 qu.QueueID from dbo.QueueUser qu with(nolock) where qu.QueueID = @CallQueueID and qu.UserID = @ownerID)
			--	return 1
			--
			--if @ownerClassID = 9 and 
			--@CallOwnerID is null and
			--			exists(select TOP 1  1 from dbo.UserRole with(nolock) where UserID = @ownerID and RoleID in (select distinct RoleID from dbo.RoleOperation with(nolock) where OperationID = 373)) --owner
			--	return 1
			--
			declare @CLUserDeptID uniqueidentifier
			declare @CLUserOrgID uniqueidentifier
			--597 Видеть заявки ИТ-сотрудников
			if exists(select   1
						from dbo.UserRole ur with(nolock)
						inner join dbo.RoleOperation ro with(nolock) on ro.RoleID = ur.RoleID    --inner
						where ur.UserID = @ownerID and ro.OperationID = 597) 
			begin
			    declare @CLOwnerDeptID uniqueidentifier
				declare @CLExecutorDeptID uniqueidentifier
				declare @CLOwnerOrgID uniqueidentifier
				declare @CLExecutorOrgID uniqueidentifier
				select TOP 1    @CLOwnerDeptID = uO.[DivisionID], @CLOwnerOrgID = uO.[OrganizationID] from [dbo].[view_User] uO with(nolock) where uO.ID = @CallOwnerID
				select TOP 1    @CLExecutorDeptID = uE.[DivisionID], @CLExecutorOrgID = uE.[OrganizationID] from [dbo].[view_User] uE with(nolock) where uE.ID = @CallExecutorID
				--
				--select @typeID = [ИД подразделения]
				--from dbo.Пользователи with(nolock)
				--where IMObjID = @ownerID
				select TOP 1 @CLUserDeptID = uO.[DivisionID], @CLUserOrgID = uO.[OrganizationID] from [dbo].[view_User] uO with(nolock) where uO.ID = @ownerID
				if (@CLUserOrgID is not null and @CLOwnerOrgID is not null and @CLUserOrgID = @CLOwnerOrgID 
				and dbo.func_SubdivisionIsSibling(@CLUserDeptID, @CLOwnerDeptID) = 1)
					return 1
				if	(@CLUserOrgID is not null and @CLExecutorOrgID is not null and @CLUserOrgID = @CLExecutorOrgID 
				and dbo.func_SubdivisionIsSibling(@CLUserDeptID, @CLExecutorDeptID) = 1)
					return 1
			end
			--
			--596 Видеть заявки сотрудников
			if exists(select ro.OperationID
						from dbo.UserRole ur with(nolock)
						inner join dbo.RoleOperation ro with(nolock) on ro.RoleID = ur.RoleID
						where ur.UserID = @ownerID and ro.OperationID = 596)     --inner
			begin
			    declare @ClientDeptID uniqueidentifier
				declare @ClientOrgID uniqueidentifier
				select TOP 1    @ClientDeptID = uO.[DivisionID], @ClientOrgID = uO.[OrganizationID] from [dbo].[view_User] uO with(nolock) where uO.ID = @CallClientID
				select TOP 1    @UserDeptID = uO.[DivisionID], @UserOrgID = uO.[OrganizationID] from [dbo].[view_User] uO with(nolock) where uO.ID = @ownerID
				--set @tmpID = null
				--select  TOP 1  @tmpID = u.[ИД подразделения]
				--from  dbo.Пользователи u with(nolock) where u.IMObjID = @CallClientID   --inner
				--
				--set @tmpID2 = null
				--select @tmpID2 = [ИД подразделения]
				--from dbo.Пользователи with(nolock)
				--where IMObjID = @ownerID
				----
				--if dbo.func_SubdivisionIsSibling(@tmpID2, @tmpID) = 1
				--	return 1
				if
				(@UserOrgID is not null and @ClientOrgID is not null and @UserOrgID = @ClientOrgID 
				and dbo.func_SubdivisionIsSibling(@UserDeptID, @ClientDeptID) = 1)
			--	if dbo.func_SubdivisionIsSibling(@UserDeptID, @OwnerDeptID) = 1 or
			--		dbo.func_SubdivisionIsSibling(@UserDeptID, @ExecutorDeptID) = 1
					return 1
			end
		end
		--
		return 0
	end
	else if @classID = 702 --problem
	begin
		if @ownerClassID = 9
		begin
			if exists(select * from dbo.UserRole with(nolock) where UserID = @ownerID and RoleID in (select distinct RoleID from dbo.RoleOperation with(nolock) where OperationID = 357)) --SDadmin
				return 1
			if exists(select * from dbo.Problem with(nolock) where ID = @id and OwnerID = @ownerID)
				return 1
			if exists(select * from dbo.Problem with(nolock) where ID = @id and OwnerID is null) and
				exists(select * from dbo.UserRole with(nolock) where UserID = @ownerID and RoleID in (select distinct RoleID from dbo.RoleOperation with(nolock) where OperationID = 373)) --owner
				return 1
			--
			--598 Видеть проблемы ИТ-сотрудников
			if not exists(select ro.OperationID
						from dbo.UserRole ur with(nolock)
						inner join dbo.RoleOperation ro with(nolock) on ro.RoleID = ur.RoleID
						where ur.UserID = @ownerID and ro.OperationID = 598)
				return 0
			--
			select @tmpID = uO.[ИД подразделения]
			from dbo.Problem p with(nolock)
			left join dbo.Пользователи uO with(nolock) on uO.IMObjID = p.OwnerID
			where p.ID = @id
			--
			select @tmpID2 = [ИД подразделения]
			from dbo.Пользователи with(nolock)
			where IMObjID = @ownerID
			--
			if dbo.func_SubdivisionIsSibling(@tmpID2, @tmpID) = 1
				return 1
		end
		--
		return 0
	end
	else if @classID = 703 --RFC
	begin
		if @ownerClassID = 9
		begin
			if exists(select * from dbo.UserRole with(nolock) where UserID = @ownerID and RoleID in (select distinct RoleID from dbo.RoleOperation with(nolock) where OperationID = 357)) --SDadmin
				return 1
			if exists(select * from dbo.RFC with(nolock) where ID = @id and OwnerID = @ownerID)
				return 1
			if exists(select * from dbo.RFC with(nolock) where ID = @id and OwnerID is null) and
				exists(select * from dbo.UserRole with(nolock) where UserID = @ownerID and RoleID in (select distinct RoleID from dbo.RoleOperation with(nolock) where OperationID = 373)) --owner
				return 1
			if exists(select * from dbo.RFC with(nolock) where ID = @id and InitiatorID = @ownerID)
				return 1
			--
			--377 Видеть rfc ИТ-сотрудников
			if not exists(select ro.OperationID
						from dbo.UserRole ur with(nolock)
						inner join dbo.RoleOperation ro with(nolock) on ro.RoleID = ur.RoleID
						where ur.UserID = @ownerID and ro.OperationID = 377)
				return 0
			--
			select @tmpID = uO.[ИД подразделения]
			from dbo.RFC p with(nolock)
			left join dbo.Пользователи uO with(nolock) on uO.IMObjID = p.OwnerID
			where p.ID = @id
			--
			select @tmpID2 = [ИД подразделения]
			from dbo.Пользователи with(nolock)
			where IMObjID = @ownerID
			--
			if dbo.func_SubdivisionIsSibling(@tmpID2, @tmpID) = 1
				return 1
		end
		--
		return 0
	end
	else if @classID = 164 --deviceApplication
	begin
		select @tmpID = DeviceID, @int = DeviceClassID
		from dbo.DeviceApplication with(nolock)
		where ID = @id
		--
		return dbo.func_AccessIsGranted(@int, @tmpID, @ownerID, @ownerClassID, @type, 1) --terminalDevice/networkDevice
	end
	else if @classID = 165 --dataEntity
	begin
		select @tmpID = DeviceApplicationID
		from dbo.DataEntity with(nolock)
		where ID = @id
		--
		return dbo.func_AccessIsGranted(164, @tmpID, @ownerID, @ownerClassID, @type, 1) --deviceApplication
	end
	else if @classID = 130 --sla
	begin
		if exists(select *
				from dbo.OrganizationItemGroup with(nolock)
				where ID = @id and 
					dbo.func_AccessIsGranted(OrganizationItemClassID, OrganizationItemID, @ownerID, @ownerClassID, @type, 1) = 1)--organization/subdivision/user/queue
			return 1
		--
		return 0
	end
	else if @classID = 127 --serviceCatalogue
	begin
		if exists(select * from dbo.ObjectAccess with(nolock) where OwnerID = @ownerID and [Type] = @type and (@propagate = 0 or Propagate = 1) and ClassID = @classID) --serviceCatalogue
			return 1
		--
		return 0
	end
	else if @classID = 405 --serviceCategory
	begin
		if exists(select * from dbo.ObjectAccess with(nolock) where OwnerID = @ownerID and [Type] = @type and (@propagate = 0 or Propagate = 1) and ClassID = @classID and ObjectID = @id) --serviceCategory
			return 1
		--serviceCatalogue
		return dbo.func_AccessIsGranted(127, '00000000-0000-0000-0000-000000000001', @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 408 --service
	begin
		if exists(select * from dbo.ObjectAccess with(nolock) where OwnerID = @ownerID and [Type] = @type and (@propagate = 0 or Propagate = 1) and ClassID = @classID and ObjectID = @id) --service
			return 1
		select @tmpID = ServiceCategoryID from dbo.[Service] with(nolock) where ID = @id
		--serviceCategory
		return dbo.func_AccessIsGranted(405, @tmpID, @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 406 --serviceItem
	begin
		if exists(select * from dbo.ObjectAccess with(nolock) where OwnerID = @ownerID and [Type] = @type and (@propagate = 0 or Propagate = 1) and ClassID = @classID and ObjectID = @id) --serviceItem
			return 1
		--
		select @tmpID = ServiceID from dbo.ServiceItem with(nolock) where ID = @id
		--service
		return dbo.func_AccessIsGranted(408, @tmpID, @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 407 --serviceAttendance
	begin
		if exists(select * from dbo.ObjectAccess with(nolock) where OwnerID = @ownerID and [Type] = @type and (@propagate = 0 or Propagate = 1) and ClassID = @classID and ObjectID = @id) --serviceAttendance
			return 1
		--
		select @tmpID = ServiceID from dbo.ServiceAttendance with(nolock) where ID = @id
		--service
		return dbo.func_AccessIsGranted(408, @tmpID, @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 30 --deviceCatalogue
	begin
		if exists(select * from dbo.ObjectAccess with(nolock) where OwnerID = @ownerID and [Type] = @type and (@propagate = 0 or Propagate = 1) and ClassID = @classID) --deviceCatalogue
			return 1
		--
		return 0
	end
	else if @classID = 374 --productCatalogCategory
	begin
	if exists(select * from dbo.ObjectAccess with(nolock) where OwnerID = @ownerID and [Type] = @type and (@propagate = 0 or Propagate = 1) and ClassID = @classID and ObjectID = @id) --current category
			return 1
		--
		select @tmpID = ParentProductCatalogCategoryID
		from dbo.ProductCatalogCategory with(nolock)
		where ID = @id
		--
		--look at parent actegory
		if not (@tmpID is null)
			return dbo.func_AccessIsGranted(374, @tmpID, @ownerID, @ownerClassID, @type, 1)
		--
		--deviceCatalogue
		return dbo.func_AccessIsGranted(30, '00000000-0000-0000-0000-000000000001', @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 378 --productCatalogType
	begin		
		if exists(select * from dbo.ObjectAccess with(nolock) where OwnerID = @ownerID and [Type] = @type and (@propagate = 0 or Propagate = 1) and ClassID = @classID and ObjectID = @id) --current type
			return 1
		--
		select @tmpID = ProductCatalogCategoryID
		from dbo.ProductCatalogType with(nolock)
		where ID = @id
		--productCatalogCategory
		return dbo.func_AccessIsGranted(374, @tmpID, @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 95 --adapterModel
	begin
		select @tmpID = ProductCatalogTypeID
		from dbo.AdapterType with(nolock)
		where AdapterTypeID = @id
		--adapterType
		return dbo.func_AccessIsGranted(378, @tmpID, @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 96 --peripheralModel
	begin
		select @tmpID = ProductCatalogTypeID
		from dbo.PeripheralType with(nolock)
		where PeripheralTypeID = @id
		--peripheralType
		return dbo.func_AccessIsGranted(378, @tmpID, @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 93 --networkDeviceModel
	begin
		select @tmpID = m.ProductCatalogTypeID
		from dbo.[Типы активного оборудования] m with(nolock)
		where m.IMObjID = @id
		--networkDeviceType
		return dbo.func_AccessIsGranted(378, @tmpID, @ownerID, @ownerClassID, @type, 1)
	end
	else if @classID = 94 --terminalDeviceModel
	begin
		select @tmpID = m.ProductCatalogTypeID
		from dbo.[Типы оконечного оборудования] m with(nolock)
		where m.IMObjID = @id
		--terminalDeviceType
		return dbo.func_AccessIsGranted(378, @tmpID, @ownerID, @ownerClassID, @type, 1)
	end
	--
	return 1 --other class objects
end
