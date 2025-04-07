create or replace function im.func_user_in_negotiation(_object_id uuid, _object_class_id int, _user_id uuid)
returns boolean
language 'plpgsql'
as $$
declare
	_now timestamp(3);
begin

	_now := now() at time zone 'utc';

	if 303 not in (select ro.operation_id from im.user_role ur join im.role_operation ro on ur.role_id = ro.role_id where ur.user_id = _user_id) then
		return false;
	end if;

	if exists(
		select 1
		from im.negotiation_user nu
		join im.negotiation n on n.ID = nu.negotiation_id		
		where n.object_id = _object_id
			and n.object_class_id = _object_class_id
			and (
				nu.user_id = _user_id
					or _user_id in (select du.child_user_id from im.deputy_user du where du.parent_user_id = nu.user_id 
						and du.utc_data_deputy_with <= _now
						and du.utc_data_deputy_by >= _now))) then
		return true;
	end if;

	return false;

end;
$$;

CREATE OR REPLACE FUNCTION im.func_access_is_granted(
	_classid integer,
	_id uuid,
	_ownerid uuid,
	_ownerclassid integer,
	_type integer,
	_propagate boolean)
    RETURNS boolean
    LANGUAGE 'plpgsql'
AS $$
declare 
	_tmpID UUID;
	_tmpID2 UUID;
	tmpint int = 0;
	_typeID UUID;
	_WorkOrderInitiatorID UUID;
	_WorkOrderExecutorID UUID;
	_WorkOrderAssignorID UUID;
	_WorkOrderQueueID UUID;
	_TOZInitiator boolean;
	_TOZAssignor boolean;
	_UserDeptID UUID;
	_InitiatorDeptID UUID;
	_AssignorDeptID UUID;
	_ExecutorDeptID UUID;
	_UserOrgID UUID;
	_InitiatorOrgID UUID;
	_AssignorOrgID UUID;
	_ExecutorOrgID UUID;
	_TOZQueueResponsible boolean;
	_WorkorderQueueResponsibleID UUID;
	_CLUserDeptID UUID;
	_CLUserOrgID UUID;
	_CLOwnerDeptID UUID;
	_CLExecutorDeptID UUID;
	_CLOwnerOrgID UUID;
	_CLExecutorOrgID UUID;
	_ClientDeptID UUID;
	_ClientOrgID UUID;
	_CallOwnerID UUID;
	_CallExecutorID UUID;
	_CallClientID UUID;
	_CallInitiatorID UUID;
    _CallServiceItemID UUID;
    _CallServiceAttendanceID UUID;
	_CallWorkflowSchemeID UUID;
	_CallQueueID UUID;
	_CallServicePlaceID UUID;
	_CallServicePlaceClassID int;
	_TOZSKS boolean;
	_TOZORG boolean;
	_TOZServiceItem boolean;
	_TOZServiceAttendance boolean;
	
begin
	if _ownerid is null or _ownerclassid is null or _type is null or _classid is null or _propagate is null then
		return FALSE;
	--Admin
	elseif _ownerclassid = 9 and exists(select * from user_role where user_id = _ownerid and role_id = '00000000-0000-0000-0000-000000000001') then
		return TRUE;
	--Р Р…Р Вµ Р С—РЎР‚Р С•Р Р†Р ВµРЎР‚РЎРЏР ВµР С Р СћР Сћ3
	elseif ((_type = 0 or _type = 1) and exists(select * from setting where id = 93 and cast(value AS bytea) = '\x00')) then
		return TRUE;
	--
	--
	elseif _classid = 29 then --owner
		if exists(select * from object_access where owner_id = _ownerid and type = _type and (not _propagate or propagate) and class_id = _classid) then --owner
			return TRUE;
		end if;
		--
		return FALSE;
	elseif _classid = 101 then --organization
		if exists(select * from object_access where owner_id = _ownerid and type = _type and (not _propagate or propagate) and class_id = _classid and object_id = _id) then --organization
			return TRUE;
		end if;
		--owner
		return func_access_is_granted(29, '00000000-0000-0000-0000-000000000000', _ownerid, _ownerclassid, _type, true);
	elseif _classid = 102 then --subdivision (recursive)
		if exists(select * from object_access where owner_id = _ownerid and type = _type and (not _propagate or propagate) and class_id = _classid and object_id = _id) then --subdivision
			return TRUE;
		end if;
		--
		select into _tmpID, _tmpID2 organization_id, department_id
		from department
		where identificator = _id;
		--
		while not(_tmpID2 is null) loop
		
			if exists(select * from object_access where owner_id = _ownerid and type = _type and propagate and class_id = _classid and object_id = _tmpID2) then --parent subdivision
				return TRUE;
			end if;
			--
			select into _tmpID, _tmpID2 organization_id, department_id
			from department
			where identificator = _tmpID2;
		end loop;
		--organization
		return func_access_is_granted(101, _tmpID, _ownerid, _ownerclassid, _type, true);
	elseif _classid = 9 then --user
		if _id = '00000000-0000-0000-0000-000000000000' then
			return TRUE;
		end if;
		--
		if _type = 1 or _type = 2 then--sks
			select into _tmpID, tmpint wp.im_obj_id, wp.identificator
			from users u
			inner join workplace wp on wp.identificator = u.workplace_id
			where u.im_obj_id = _id;
			--
			if (tmpint = 0) then
				return TRUE; --no workplace - access granted
			else
				tmpint = 22;
			end if;
		else
			select into _tmpID, tmpint department_id, 102
			from users
			where im_obj_id = _id;
		end if;
		--workplace/subdivision
		return func_access_is_granted(tmpint, _tmpID, _ownerid, _ownerclassid, _type, true);
	elseif _classid = 1 then --building
		if exists(select * from object_access where owner_id = _ownerid and type = _type and (not _propagate or propagate) and class_id = _classid and object_id = _id) then --building
			return TRUE;
		end if;
		--
		select into _tmpID, tmpint organization_id, identificator
		from building
		where im_obj_id = _id;
		--
		if tmpint = 0 then
			return TRUE; --no building - access granted
		end if;
		--organization
		return func_access_is_granted(101, _tmpID, _ownerid, _ownerclassid, _type, true);
	elseif _classid = 2 then --floor
		if exists(select * from object_access where owner_id = _ownerid and type = _type and (not _propagate or propagate) and class_id = _classid and object_id = _id) then --floor
			return TRUE;
		end if;
		--
		select into _tmpID, tmpint b.im_obj_id, f.identificator
		from floor f
		inner join building b on b.identificator = f.building_id
		where f.im_obj_id = _id;
		--
		if tmpint = 0 then
			return TRUE; --no floor - access granted
		end if;
		--building
		return func_access_is_granted(1, _tmpID, _ownerid, _ownerclassid, _type, true);
	elseif _classid = 3 then --room
		if exists(select * from object_access where owner_id = _ownerid and type = _type and (not _propagate or propagate) and class_id = _classid and object_id = _id) then --room
			return TRUE;
		end if;
		--
		select into _tmpID, tmpint f.im_obj_id, r.identificator
		from room r
		inner join floor f on f.identificator = r.floor_id
		where r.im_obj_id = _id;
		--
		if tmpint = 0 then
			return TRUE; --no room - access granted
		end if;
		--floor
		return func_access_is_granted(2, _tmpID, _ownerid, _ownerclassid, _type, true);
	elseif _classid = 22 then --workplace
		select into _tmpID, tmpint r.im_obj_id, wp.identificator
		from workplace wp
		inner join room r on r.identificator = wp.room_id
		where wp.im_obj_id = _id;
		--
		if tmpint = 0 then
			return TRUE; --no workplace - access granted
		end if;
		--room
		return func_access_is_granted(3, _tmpID, _ownerid, _ownerclassid, _type, true);
	elseif _classid = 4 then --rack
		if exists(select * from object_access where owner_id = _ownerid and type = _type and (not _propagate or propagate) and class_id = _classid and object_id = _id) then --rack
			return TRUE;
		end if;
		--
		select into _tmpID r.im_obj_id
		from cabinet rack
		inner join room r on r.identificator = rack.room_id
		where rack.im_obj_id = _id;
		--room
		return func_access_is_granted(3, _tmpID, _ownerid, _ownerclassid, _type, true);
	elseif _classid = 6 then --terminalDevice
		if exists(select *
				from terminal_equipment d
				inner join asset a on a.device_id = d.identificator
				left join users u on u.identificator = a.user_id
				where d.im_obj_id = _id and (a.utilizer_id = _ownerid or u.im_obj_id = _ownerid)) then
			return TRUE;
		end if;
		--
		select into _tmpID, _tmpID2, _typeID r.im_obj_id, wp.im_obj_id, m.product_catalog_type_id
		from terminal_equipment d
		left join room r on r.identificator = d.room_id
		left join workplace wp on wp.identificator = d.workplace_id and wp.identificator <> 0
		inner join terminal_equipment_types m on m.identificator = d.terminal_equipment_type_id		
		where d.im_obj_id = _id;
		--
		if not func_access_is_granted(378, _typeID, _ownerid, _ownerclassid, 0, true) then
			return FALSE;
		end if;
		--
		if _tmpID2 is null then
			return func_access_is_granted(3, _tmpID, _ownerid, _ownerclassid, _type, true); --room
		else
			return func_access_is_granted(22, _tmpID2, _ownerid, _ownerclassid, _type, true); --workplace
		end if;
	elseif _classid = 5 then --РЎРѓР С• = РЎвЂљРЎвЂљР В·; 
		if exists(select *
				from active_equipment d
				inner join asset a on a.device_id = d.identificator
				left join users u on u.identificator = a.user_id
				where d.im_obj_id = _id and (a.utilizer_id = _ownerid or u.im_obj_id = _ownerid or d.organization_item_id = _ownerid)) then
			return TRUE;
		end if;
		--
		select into _tmpID, _tmpID2, _typeID r.im_obj_id, rack.im_obj_id, m.product_catalog_type_id
		from active_equipment d
		left join room r on r.identificator = d.room_id
		left join cabinet rack on rack.identificator = d.cabinet_id and rack.identificator <> 0
		inner join active_equipment_types m on m.identificator = d.type_id
		where d.im_obj_id = _id;
		--
		if not func_access_is_granted(378, _typeID, _ownerid, _ownerclassid, 0, true) then 
			return FALSE;
		end if;
		--
		if _tmpID2 is null then
			return func_access_is_granted(3, _tmpID, _ownerid, _ownerclassid, _type, true); --room
		else
			return func_access_is_granted(4, _tmpID2, _ownerid, _ownerclassid, _type, true); --rack		
		end if;
	elseif _classid = 33 then --Р В°Р Т‘Р В°Р С—РЎвЂљР ВµРЎР‚ = РЎвЂљРЎвЂљР В·;
		if exists(select *
				from adapter d
				inner join asset a on a.device_id = d.tmpintid
				left join users u on u.identificator = a.user_id
				where d.adapter_id = _id and (a.utilizer_id = _ownerid or u.im_obj_id = _ownerid)) then
			return TRUE;
		end if;
		--
		select into _tmpID, tmpint, _typeID 
				(case when a.RoomID<>0 then r.im_obj_id
								when a.NetworkDeviceID<>0 then rnd.im_obj_id
								when a.TerminalDeviceID<>0 then rtd.im_obj_id
								else CAST(null AS UUID) end),
				(case when a.RoomID<>0 then 3
								when a.NetworkDeviceID<>0 then 3
								when a.TerminalDeviceID<>0 then 3
								else 0 end),				
				m.product_catalog_type_id
		from adapter a
		inner join adapter_type m on m.adapter_type_id = a.adapter_type_id
		left join room r on r.identificator = a.room_id and a.room_id <> 0
		left join terminal_equipment td on td.identificator = a.terminal_device_id and a.terminal_device_id <> 0
		left join room rtd on rtd.identificator = td.room_id and td.room_id <> 0
		left join active_equipment nd on nd.identificator = a.network_device_id and a.network_device_id <> 0
		left join room rnd on rnd.identificator = nd.room_id and nd.room_id <> 0
		where a.adapter_id =  _id;
		--
		if not func_access_is_granted(378, _typeID, _ownerid, _ownerclassid, 0, true) then
			return FALSE;
		end if;
		--room/networkDevice/teminalDevice
		return func_access_is_granted(tmpint, _tmpID, _ownerid, _ownerclassid, _type, true);		
	elseif _classid = 34 then --Р С—РЎС“ = РЎвЂљРЎвЂљР В·; Р Т‘Р В»РЎРЏ Р В»Р С•Р С–Р С‘РЎвЂЎР ВµРЎРѓР С”Р С‘РЎвЂ¦ - РЎР‚Р ВµРЎРѓРЎС“РЎР‚РЎРѓРЎвЂ№ РЎвЂљР С•Р В»РЎРЉР С”Р С• Р С‘Р В· РЎвЂљРЎвЂљР В·
		if exists(select *
				from peripheral d
				inner join asset a on a.device_id = d.tmpintid
				left join users u on u.identificator = a.user_id
				where d.peripheral_id = _id and (a.utilizer_id = _ownerid or u.im_obj_id = _ownerid)) then
			return TRUE;
		end if;
		--
		select into _tmpID, tmpint, _typeID 
				(case when p.RoomID<>0 then r.im_obj_id
								when p.NetworkDeviceID<>0 then rnd.im_obj_id
								when p.TerminalDeviceID<>0 then rtd.im_obj_id
								else CAST(null AS UUID) end),
				(case when p.RoomID<>0 then 3
								when p.NetworkDeviceID<>0 then 3
								when p.TerminalDeviceID<>0 then 3
								else 0 end),
				m.product_catalog_type_id
		from peripheral p
		inner join peripheral_type m on m.peripheral_type_id = p.peripheral_type_id
		left join room r on r.identificator = p.room_id and p.room_id <> 0
		left join terminal_equipment td on td.identificator = p.terminal_device_id and p.terminal_device_id <> 0
		left join room rtd on rtd.identificator = td.room_id and td.room_id <> 0
		left join active_equipment nd on nd.identificator = p.network_device_id and p.network_device_id <> 0
		left join room rnd on rnd.identificator = nd.room_id and nd.room_id <> 0
		where p.peripheral_id = _id;
		--
		if not func_access_is_granted(378, _typeID, _ownerid, _ownerclassid, 0, true) then
			return FALSE;
		end if;
		--room/networkDevice/teminalDevice
		return func_access_is_granted(tmpint, _tmpID, _ownerid, _ownerclassid, _type, true);
	elseif _classid = 71 then --SoftwareInstallation
		select into _tmpID, tmpint device_id, device_class_id
		from software_installation
		where id = _id;
		--
		return func_access_is_granted(tmpint, _tmpID, _ownerid, _ownerclassid, _type, false); --terminalDevice/networkDevice
	elseif _classid = 348 then --DiscArray
		select into _tmpID, tmpint device_id, device_class_id
		from disc_array
		where id = _id;
		--
		return func_access_is_granted(tmpint, _tmpID, _ownerid, _ownerclassid, _type, false); --terminalDevice/networkDevice
	elseif _classid = 13 then --ActivePort
		select into _tmpID d.im_obj_id
		from active_port p
		inner join active_equipment d on d.identificator = p.active_equipment_id
		where p.im_obj_id = _id;
		--
		return func_access_is_granted(5, _tmpID, _ownerid, _ownerclassid, _type, false); --networkDevice
	elseif _classid = 7 then --Panel
		select into _tmpID r.im_obj_id
		from panel p
		inner join cabinet r on r.identificator = p.cabinet_id
		where p.im_obj_id = _id;
		--rack
		return func_access_is_granted(4, _tmpID, _ownerid, _ownerclassid, _type, true);
	elseif _classid = 8 then --Outlet
		select into _tmpID r.im_obj_id
		from socket o
		inner join room r on r.identificator = o.room_id
		where o.im_obj_id = _id;
		--room
		return func_access_is_granted(3, _tmpID, _ownerid, _ownerclassid, _type, true);
	elseif _classid = 14 or _classid = 15 then --PanelPort / OutletPort
		select into _tmpID, tmpint t.id, t.class_id
		from passive_port p
		inner join
		(
			select identificator AS tmpintid, im_obj_id AS id, 8 AS class_id from socket
			union all
			select identificator AS tmpintid, im_obj_id AS id, 7 AS class_id from panel
		) t on t.tmpintid = p.socket_id
		where p.im_obj_id = _id;
		--
		return func_access_is_granted(tmpint, _tmpID, _ownerid, _ownerclassid, _type, false); --penel / outlet
	elseif _classid = 35 then --SplitterPort
		select into tmpint socket_id from passive_port where im_obj_id = _id;
		select into _id im_obj_id from passive_port where identificator = tmpint;
		if _id is null then
			return FALSE;
		end if;
		--
		return func_access_is_granted(tmpint / 1000000, _id, _ownerid, _ownerclassid, _type, false); --penel / outlet
	elseif _classid = 119 then --workOrder
		select  into _WorkOrderInitiatorID, _WorkOrderExecutorID, _WorkOrderAssignorID, _WorkOrderQueueID initiator_id, executor_id, assignor_id, queue_id from work_order where id = _id fetch first 1 rows only;
		--if exists(select * from work_order where id = _id and (initiator_id = _ownerid or executor_id = _ownerid or assignor_id = _ownerid or queue_id = _ownerid))
		if (_WorkOrderInitiatorID = _ownerid or _WorkOrderExecutorID = _ownerid or _WorkOrderAssignorID = _ownerid or _WorkOrderQueueID = _ownerid) then
			return TRUE;
		end if;
		if _ownerclassid = 9 then
			if _WorkOrderQueueID is not null and exists(select 1 from queue_user qu where qu.queue_id = _WorkOrderQueueID and qu.user_id = _ownerid) then
			--if exists(select 1 from work_order w where w.id = _id and exists(select 1 from queue_user qu where qu.queue_id = w.queue_id and qu.user_id = _ownerid))
				return TRUE;
			end if;
			--
			--599 Р вЂ™Р С‘Р Т‘Р ВµРЎвЂљРЎРЉ Р В·Р В°Р Т‘Р В°Р Р…Р С‘РЎРЏ Р ВР Сћ-РЎРѓР С•РЎвЂљРЎР‚РЎС“Р Т‘Р Р…Р С‘Р С”Р С•Р Р†
			if (_ownerclassid = 9 and exists(select ro.operation_id
						from user_role ur
						inner join role_operation ro on ro.role_id = ur.role_id
						where ur.user_id = _ownerid and ro.operation_id = 599)) then
					select into _InitiatorDeptID, _InitiatorOrgID uO.division_id, uO.organization_id from view_user uO where uO.id = _WorkOrderInitiatorID fetch first 1 rows only;
					select into _UserDeptID, _UserOrgID uO.division_id, uO.organization_id from view_user uO where uO.id = _ownerid fetch first 1 rows only;
					--
					if (_UserOrgID is not null) then
							if(_UserOrgID is not null and _InitiatorOrgID is not null and _UserOrgID = _InitiatorOrgID and 
							func_subdivision_is_sibling(_UserDeptID, _InitiatorDeptID)) then
								return TRUE;
							end if;
							select into _AssignorDeptID, _AssignorOrgID uO.division_id, uO.organization_id from view_user uO where uO.id = _WorkOrderAssignorID fetch first 1 rows only;
							if(_UserOrgID is not null and _AssignorOrgID is not null and _UserOrgID = _AssignorOrgID and 
							func_subdivision_is_sibling(_UserDeptID, _AssignorDeptID)) then
								return TRUE;
							end if;
							select into _ExecutorDeptID, _ExecutorOrgID uO.division_id, uO.organization_id from view_user uO where uO.id = _WorkOrderExecutorID fetch first 1 rows only;
							if(_UserOrgID is not null and _ExecutorOrgID is not null and _UserOrgID = _ExecutorOrgID and 
							func_subdivision_is_sibling(_UserDeptID, _ExecutorDeptID)) then
								return TRUE;
							end if;
						end if;
				end if;
			if(_WorkOrderQueueID is not null) then
					select into _WorkorderQueueResponsibleID q.responsible_id from queue q where q.id=_WorkOrderQueueID fetch first 1 rows only;
						if (_WorkorderQueueResponsibleID is not null) then
								if (not func_access_is_granted(9, _WorkorderQueueResponsibleID, _ownerid, _ownerclassid, 2, false)) then
									_TOZQueueResponsible = FALSE;
								elseif (not func_access_is_granted(9, _WorkorderQueueResponsibleID, _ownerid, _ownerclassid, 3, false)) then
									_TOZQueueResponsible = FALSE;
								else
									_TOZQueueResponsible = TRUE;
								end if;
								if 	_TOZQueueResponsible then
									return TRUE;
								end if;
						end if;
			end if;
			if (_ownerclassid = 9 and _WorkOrderExecutorID is null) and
				exists(select 1 from user_role where user_id = _ownerid and 
				role_id in (select distinct role_id from role_operation where operation_id = 358)) then --executor
						if (_WorkOrderInitiatorID is not null) then
							if (not func_access_is_granted(9, _WorkOrderInitiatorID, _ownerid, _ownerclassid, 2, false)) then
								_TOZInitiator = FALSE;
							elseif (not func_access_is_granted(9, _WorkOrderInitiatorID, _ownerid, _ownerclassid, 3, false)) then 
								_TOZInitiator = FALSE;
							else
								_TOZInitiator = TRUE;
							end if;
							if 	_TOZInitiator then
								return TRUE;
						end if;
						if (_WorkOrderAssignorID is not null) then
							if (not func_access_is_granted(9, _WorkOrderAssignorID, _ownerid, _ownerclassid, 2, false)) then
								 _TOZAssignor = FALSE;
							elseif (not func_access_is_granted(9, _WorkOrderAssignorID, _ownerid, _ownerclassid, 3, false)) then
								_TOZAssignor = FALSE;
							else
								_TOZAssignor = TRUE;
							end if;
						end if;
						if 	_TOZAssignor then
							return TRUE;
						end if;
					end if;
			end if;
			if (_ownerclassid = 9 and 
				exists(select 1 from user_role where user_id = _ownerid and 
				role_id in (select distinct role_id from role_operation where operation_id = 357)) --SDadmin
				) then
						if (_WorkOrderInitiatorID is not null and _TOZInitiator is null) then
							if (not func_access_is_granted(9, _WorkOrderInitiatorID, _ownerid, _ownerclassid, 2, false)) then
								_TOZInitiator = FALSE;
							elseif (not func_access_is_granted(9, _WorkOrderInitiatorID, _ownerid, _ownerclassid, 3, false)) then
								_TOZInitiator = FALSE;
							else
								_TOZInitiator = TRUE;
							end if;
						end if;
						if 	_TOZInitiator then
								return TRUE;
						end if;
						if (_WorkOrderAssignorID is not null and _TOZAssignor is null) then
								if (not func_access_is_granted(9, _WorkOrderAssignorID, _ownerid, _ownerclassid, 2, false)) then
									_TOZAssignor = FALSE;
								elseif (not func_access_is_granted(9, _WorkOrderAssignorID, _ownerid, _ownerclassid, 3, false)) then
									_TOZAssignor = FALSE;
								else
									_TOZAssignor = TRUE;
								end if;
							end if;
						if 	_TOZAssignor then
							return TRUE;
						end if;
						if (_WorkOrderExecutorID is not null and
							func_access_is_granted(9, _WorkOrderExecutorID, _ownerid, _ownerclassid, 2, false) and 
							func_access_is_granted(9, _WorkOrderExecutorID, _ownerid, _ownerclassid, 3, false)
						) then
							return TRUE;
						end if;
				end if;
			if im.func_user_in_negotiation(_id, _classid, _ownerid) then
				return true;
			end if;
		end if;
		--
		return FALSE;
	elseif _classid = 701 then --call
	    select into _CallQueueID,  _CallServiceAttendanceID,  _CallServiceItemID, _CallOwnerID, _CallClientID, _CallInitiatorID,  _CallExecutorID, _CallWorkflowSchemeID, _CallServicePlaceID,  _CallServicePlaceClassID
			queue_id, cs.service_attendance_id, cs.service_item_id, owner_id, client_id, initiator_id, executor_id, workflow_scheme_id, service_place_id, service_place_class_id
		from call c 
		join call_service cs on c.call_service_id = cs.id
		where c.id=_id fetch first 1 rows only;
		if ( _CallClientID = _ownerid or _CallOwnerID = _ownerid or _CallExecutorID = _ownerid or _CallInitiatorID = _ownerid or _CallQueueID = _ownerid) then
			return TRUE;
		end if;
		if  _CallQueueID is not null and exists(select qu.queue_id from queue_user qu where qu.queue_id = _CallQueueID and qu.user_id = _ownerid limit 1) then
			return TRUE;
		end if;
		if _ownerclassid = 9 and im.func_user_in_negotiation(_id, _classid, _ownerid) then
			return true;
		end if;
		if (_CallOwnerID is null) then
			if _ownerclassid = 9 and (_CallWorkflowSchemeID is null) and ((_CallServiceItemID is null) and (_CallServiceAttendanceID is null)) 
				and not exists(select * from user_role where user_id = _ownerid and role_id in (select distinct role_id from role_operation where operation_id = 656)) then --UnClassified
				return FALSE;
			end if;
			-- service place toz sks
			if ((_CallServicePlaceClassID is not null) and (_CallServicePlaceID is not null)) then
				if (_CallServicePlaceClassID = 3) then
					_TOZSKS = func_access_is_granted(3, _CallServicePlaceID, _ownerid, _ownerclassid, 2, false);
				elseif (_CallServicePlaceClassID = 22) then
					_TOZSKS = func_access_is_granted(22, _CallServicePlaceID, _ownerid, _ownerclassid, 2, false);
				else 
					_TOZSKS = FALSE;
				end if;
			else
				_TOZSKS = func_access_is_granted(9, _CallClientID, _ownerid, _ownerclassid, 2, false);
			end if;
			---
			if (not _TOZSKS ) then
				return FALSE;
			end if;
			_TOZORG = func_access_is_granted(9, _CallClientID, _ownerid, _ownerclassid, 3, false);
			if (not _TOZORG) then
				return FALSE;
			end if;
				--if  _CallQueueID is not null and exists(select qu.queue_id from queue_user qu where qu.queue_id = _CallQueueID and qu.user_id = _owneridfetch first 1 rows only)
			--	return 1
			--if func_access_is_granted(9, _CallClientID, _ownerid, _ownerclassid, 2, 0) = 0 --toz sks
			--	return 0
			--if func_access_is_granted(9, _CallClientID, _ownerid, _ownerclassid, 3, 0) = 0 --toz org
			--	return 0
			--
			--	select  TOP 1 _tmpID = c.service_item_id, _tmpID2 = c.service_attendance_id
			--	from call c 
			--	where c.id = _id 
			--
			if ( _CallServiceItemID is not null) then
				_TOZServiceItem = func_access_is_granted(406, _CallServiceItemID, _ownerid, _ownerclassid, 4, false);
			elseif ( _CallServiceAttendanceID is not null) then
				_TOZServiceAttendance = func_access_is_granted(407, _CallServiceAttendanceID, _ownerid, _ownerclassid, 4, false);
			end if;
			--if (not _CallServiceItemID is null) or (not _CallServiceAttendanceID is null)
			if (_TOZServiceItem is not null and not _TOZServiceItem ) then
				return FALSE;
			end if;
			if (_TOZServiceAttendance is not null and not _TOZServiceAttendance ) then
				return FALSE;
			end if;
			--begin
				--set _TOZServiceItem = func_access_is_granted(406, _CallServiceItemID, _ownerid, _ownerclassid, 4, 0)
				--if (not _CallServiceItemID is null) and func_access_is_granted(406, _CallServiceItemID, _ownerid, _ownerclassid, 4, 0) = 0
				--if (_TOZServiceItem = 0)
				--	return 0
				--elseif (not _CallServiceAttendanceID is null) and func_access_is_granted(407, _CallServiceAttendanceID, _ownerid, _ownerclassid, 4, 0) = 0
				--	return 0
			--end
--			elseif _ownerclassid = 9 and (_CallWorkflowSchemeID is null) 
--					and not exists(select * from user_role where user_id = _ownerid and role_id in (select distinct role_id from role_operation where operation_id = 656)) --UnClassified
--				return 0   --move up
			if _ownerclassid = 9 and 
			--_CallOwnerID is null and
						exists(select * from user_role where user_id = _ownerid and role_id in (select distinct role_id from role_operation where operation_id = 373)) then --owner
				return TRUE;
			end if;
		end if;
		--
		--if ( _CallOwnerID = _ownerid or _CallExecutorID = _ownerid or _CallInitiatorID = _ownerid or _CallQueueID = _ownerid)  --go first
		--	return 1
		--
		if _ownerclassid != 9 or exists(select * from user_role where user_id = _ownerid and role_id in (select distinct role_id from role_operation where operation_id = 357)) then --SDadmin
			-- select  TOP 1 _tmpID = c.client_id from call c where c.id = _id  
			--	if func_access_is_granted(9, _CallClientID, _ownerid, _ownerclassid, 2, 0) = 1 --toz sks
			--		and 
			--		func_access_is_granted(9, _CallClientID, _ownerid, _ownerclassid, 3, 0) = 1 --toz org
			if (_TOZSKS is null) then
						if ((_CallServicePlaceClassID is not null) and (_CallServicePlaceID is not null)) then
								if (_CallServicePlaceClassID = 3) then
									_TOZSKS = func_access_is_granted(3, _CallServicePlaceID, _ownerid, _ownerclassid, 2, false);
								elseif (_CallServicePlaceClassID = 22) then
									_TOZSKS = func_access_is_granted(22, _CallServicePlaceID, _ownerid, _ownerclassid, 2, false);
								else 
									_TOZSKS = FALSE;
								end if;
						else
							_TOZSKS = func_access_is_granted(9, _CallClientID, _ownerid, _ownerclassid, 2, false);
						end if;
			end if;
				--set _TOZSKS = func_access_is_granted(9, _CallClientID, _ownerid, _ownerclassid, 2, false)
			if (_TOZORG is null) then
				_TOZORG = func_access_is_granted(9, _CallClientID, _ownerid, _ownerclassid, 3, false);
			end if;
			if 	(_TOZSKS and _TOZORG) then
				--select  TOP 1 _tmpID = c.service_item_id, _tmpID2 = c.service_attendance_id
				--from call c  
				--where c.id = _id
				--
				if ( _CallServiceItemID is not null) then
					if(_TOZServiceItem is null) then
						_TOZServiceItem = func_access_is_granted(406, _CallServiceItemID, _ownerid, _ownerclassid, 4, false);
					end if;
					if(_TOZServiceItem) then
						return TRUE;
					end if;
				elseif ( _CallServiceAttendanceID is not null) then
						if(_TOZServiceAttendance is null) then
							_TOZServiceAttendance = func_access_is_granted(407, _CallServiceAttendanceID, _ownerid, _ownerclassid, 4, false);
						end if;
						if(_TOZServiceAttendance) then
							return TRUE;
						end if;
				else
				  return TRUE;
				 end if;
				--if ( _CallServiceItemID is not null)
				--begin
				--	if func_access_is_granted(406, _CallServiceItemID, _ownerid, _ownerclassid, 4, 0) = 1
				--		return 1
				--end
				--elseif (_CallServiceAttendanceID is not null)
				--begin
				--	if func_access_is_granted(407, _CallServiceAttendanceID, _ownerid, _ownerclassid, 4, 0) = 1
				--		return 1
				--end
				--else
				--	return 1
			end if;
		end if;
		--
		if _ownerclassid = 9 then   --user, not group
		    -- go second
			--if  _CallQueueID is not null and exists(select TOP 1 qu.queue_id from queue_user qu where qu.queue_id = _CallQueueID and qu.user_id = _ownerid)
			--	return 1
			--
			--if _ownerclassid = 9 and 
			--_CallOwnerID is null and
			--			exists(select TOP 1  1 from user_role where user_id = _ownerid and role_id in (select distinct role_id from role_operation where operation_id = 373)) --owner
			--	return 1
			--
			--597 Р вЂ™Р С‘Р Т‘Р ВµРЎвЂљРЎРЉ Р В·Р В°РЎРЏР Р†Р С”Р С‘ Р ВР Сћ-РЎРѓР С•РЎвЂљРЎР‚РЎС“Р Т‘Р Р…Р С‘Р С”Р С•Р Р†
			if exists(select   1
						from user_role ur
						inner join role_operation ro on ro.role_id = ur.role_id    --inner
						where ur.user_id = _ownerid and ro.operation_id = 597) then
				select into _CLOwnerDeptID, _CLOwnerOrgID uO.division_id, uO.organization_id from view_user uO where uO.id = _CallOwnerID fetch first 1 rows only;
				select into _CLExecutorDeptID, _CLExecutorOrgID uE.division_id, uE.organization_id from view_user uE where uE.id = _CallExecutorID fetch first 1 rows only;
				--
				--select _typeID = department_id
				--from users
				--where im_obj_id = _ownerid
				select into _CLUserDeptID, _CLUserOrgID uO.division_id, uO.organization_id from view_user uO where uO.id = _ownerid fetch first 1 rows only;
				if (_CLUserOrgID is not null and _CLOwnerOrgID is not null and _CLUserOrgID = _CLOwnerOrgID 
				and func_subdivision_is_sibling(_CLUserDeptID, _CLOwnerDeptID)) then
					return TRUE;
				end if;
				if	(_CLUserOrgID is not null and _CLExecutorOrgID is not null and _CLUserOrgID = _CLExecutorOrgID 
				and func_subdivision_is_sibling(_CLUserDeptID, _CLExecutorDeptID)) then
					return TRUE;
				end if;
			end if;
			--
			--596 Р вЂ™Р С‘Р Т‘Р ВµРЎвЂљРЎРЉ Р В·Р В°РЎРЏР Р†Р С”Р С‘ РЎРѓР С•РЎвЂљРЎР‚РЎС“Р Т‘Р Р…Р С‘Р С”Р С•Р Р†
			if exists(select ro.operation_id
						from user_role ur
						inner join role_operation ro on ro.role_id = ur.role_id
						where ur.user_id = _ownerid and ro.operation_id = 596) then    --inner
				select into _ClientDeptID, _ClientOrgID uO.division_id, uO.organization_id from view_user uO where uO.id = _CallClientID fetch first 1 rows only;
				select into _UserDeptID, _UserOrgID uO.division_id, uO.organization_id from view_user uO where uO.id = _ownerid fetch first 1 rows only;
				--set _tmpID = null
				--select  TOP 1  _tmpID = u.department_id
				--from  users u where u.im_obj_id = _CallClientID   --inner
				--
				--set _tmpID2 = null
				--select _tmpID2 = department_id
				--from users
				--where im_obj_id = _ownerid
				----
				--if func_subdivision_is_sibling(_tmpID2, _tmpID) = 1
				--	return 1
				if
				(_UserOrgID is not null and _ClientOrgID is not null and _UserOrgID = _ClientOrgID 
				and func_subdivision_is_sibling(_UserDeptID, _ClientDeptID)) then
				--	if func_subdivision_is_sibling(_UserDeptID, _OwnerDeptID) = 1 or
				--		func_subdivision_is_sibling(_UserDeptID, _ExecutorDeptID) = 1
					return TRUE;
				end if;
			end if;
		end if;
		--
		return FALSE;
	elseif _classid = 702 then --problem
		if _ownerclassid = 9 then
			if exists(select * from user_role where user_id = _ownerid and role_id in (select distinct role_id from role_operation where operation_id = 357)) then --SDadmin
				return TRUE;
			end if;
			if exists(select * from problem where id = _id and owner_id = _ownerid) then
				return TRUE;
			end if;
			if exists(select * from problem where id = _id and owner_id is null) and
				exists(select * from user_role where user_id = _ownerid and role_id in (select distinct role_id from role_operation where operation_id = 373)) then --owner
				return TRUE;
			end if;
			if im.func_user_in_negotiation(_id, _classid, _ownerid) then
				return true;
			end if;
			--
			--598 Р вЂ™Р С‘Р Т‘Р ВµРЎвЂљРЎРЉ Р С—РЎР‚Р С•Р В±Р В»Р ВµР СРЎвЂ№ Р ВР Сћ-РЎРѓР С•РЎвЂљРЎР‚РЎС“Р Т‘Р Р…Р С‘Р С”Р С•Р Р†
			if not exists(select ro.operation_id
						from user_role ur
						inner join role_operation ro on ro.role_id = ur.role_id
						where ur.user_id = _ownerid and ro.operation_id = 598) then
				return FALSE;
			end if;
			--
			select into _tmpID uO.department_id
			from problem p
			left join users uO on uO.im_obj_id = p.owner_id
			where p.id = _id;
			--
			select into _tmpID2 department_id
			from users
			where im_obj_id = _ownerid;
			--
			if func_subdivision_is_sibling(_tmpID2, _tmpID) then
				return TRUE;
			end if;
		end if;
		--
		return FALSE;
	elseif _classid = 703 then --RFC
		if _ownerclassid = 9 then
			if exists(select * from user_role where user_id = _ownerid and role_id in (select distinct role_id from role_operation where operation_id = 357)) then --SDadmin
				return TRUE;
			end if;
			if exists(select * from rfc where id = _id and owner_id = _ownerid) then
				return TRUE;
			end if;
			if exists(select * from rfc where id = _id and owner_id is null) and
				exists(select * from user_role where user_id = _ownerid and role_id in (select distinct role_id from role_operation where operation_id = 373)) then --owner
				return TRUE;
			end if;
			if exists(select * from rfc where id = _id and initiator_id = _ownerid) then
				return TRUE;
			end if;
			if im.func_user_in_negotiation(_id, _classid, _ownerid) then
				return true;
			end if;
			--
			--377 Р вЂ™Р С‘Р Т‘Р ВµРЎвЂљРЎРЉ rfc Р ВР Сћ-РЎРѓР С•РЎвЂљРЎР‚РЎС“Р Т‘Р Р…Р С‘Р С”Р С•Р Р†
			if not exists(select ro.operation_id
						from user_role ur
						inner join role_operation ro on ro.role_id = ur.role_id
						where ur.user_id = _ownerid and ro.operation_id = 377) then
				return FALSE;
			end if;
			--
			select into _tmpID uO.department_id
			from rfc p
			left join users uO on uO.im_obj_id = p.owner_id
			where p.id = _id;
			--
			select into _tmpID2 department_id
			from users
			where im_obj_id = _ownerid;
			--
			if func_subdivision_is_sibling(_tmpID2, _tmpID) then
				return TRUE;
			end if;
		end if;
		--
		return FALSE;
	elseif _classid = 164 then --deviceApplication
		select into _tmpID, tmpint device_id, device_class_id
		from device_application
		where id = _id;
		--
		return func_access_is_granted(tmpint, _tmpID, _ownerid, _ownerclassid, _type, true); --terminalDevice/networkDevice
	elseif _classid = 165 then --dataEntity
		select into _tmpID device_application_id
		from data_entity
		where id = _id;
		--
		return func_access_is_granted(164, _tmpID, _ownerid, _ownerclassid, _type, true); --deviceApplication
	elseif _classid = 130 then --sla
		if exists(select *
				from organization_item_group
				where id = _id and 
					func_access_is_granted(organization_item_class_id, organization_item_id, _ownerid, _ownerclassid, _type, true)) then	--organization/subdivision/user/queue
			return TRUE;
		end if;
		--
		return FALSE;
	elseif _classid = 127 then --serviceCatalogue
		if exists(select * from object_access where owner_id = _ownerid and type = _type and (not _propagate or propagate) and class_id = _classid) then --serviceCatalogue
			return TRUE;
		end if;
		--
		return FALSE;
	elseif _classid = 405 then --serviceCategory
		if exists(select * from object_access where owner_id = _ownerid and type = _type and (not _propagate or propagate) and class_id = _classid and object_id = _id) then --serviceCategory
			return TRUE;
		end if;
		--serviceCatalogue
		return func_access_is_granted(127, '00000000-0000-0000-0000-000000000000', _ownerid, _ownerclassid, _type, true);
	elseif _classid = 408 then --service
		if exists(select * from object_access where owner_id = _ownerid and type = _type and (not _propagate or propagate) and class_id = _classid and object_id = _id) then --service
			return TRUE;
		end if;
		select into _tmpID service_category_id from service where id = _id;
		--serviceCategory
		return func_access_is_granted(405, _tmpID, _ownerid, _ownerclassid, _type, true);
	elseif _classid = 406 then --serviceItem
		if exists(select * from object_access where owner_id = _ownerid and type = _type and (not _propagate or propagate) and class_id = _classid and object_id = _id) then --serviceItem
			return TRUE;
		end if;
		--
		select into _tmpID service_id from service_item where id = _id;
		--service
		return func_access_is_granted(408, _tmpID, _ownerid, _ownerclassid, _type, true);
	elseif _classid = 407 then --serviceAttendance
		if exists(select * from object_access where owner_id = _ownerid and type = _type and (not _propagate or propagate) and class_id = _classid and object_id = _id) then --serviceAttendance
			return TRUE;
		end if;
		--
		select into _tmpID service_id from service_attendance where id = _id;
		--service
		return func_access_is_granted(408, _tmpID, _ownerid, _ownerclassid, _type, true);
	elseif _classid = 30 then --deviceCatalogue
		if exists(select * from object_access where owner_id = _ownerid and type = _type and (not _propagate or propagate) and class_id = _classid) then --deviceCatalogue
			return TRUE;
		end if;
		--
		return FALSE;
	elseif _classid = 374 then --productCatalogCategory
		if exists(select * from object_access where owner_id = _ownerid and type = _type and (not _propagate or propagate) and class_id = _classid and object_id = _id) then --current category
			return TRUE;
		end if;
		--
		select into _tmpID parent_product_catalog_category_id
		from product_catalog_category
		where id = _id;
		--
		--look at parent actegory
		if not (_tmpID is null) then
			return func_access_is_granted(374, _tmpID, _ownerid, _ownerclassid, _type, true);
		end if;
		--
		--deviceCatalogue
		return func_access_is_granted(30, '00000000-0000-0000-0000-000000000000', _ownerid, _ownerclassid, _type, true);
	elseif _classid = 378 then --productCatalogType
		if exists(select * from object_access where owner_id = _ownerid and type = _type and (not _propagate or propagate) and class_id = _classid and object_id = _id) then --current type
			return TRUE;
		end if;
		--
		select into _tmpID product_catalog_category_id
		from product_catalog_type
		where id = _id;
		--productCatalogCategory
		return func_access_is_granted(374, _tmpID, _ownerid, _ownerclassid, _type, true);
	elseif _classid = 95 then --adapterModel
		select into _tmpID product_catalog_type_id
		from adapter_type
		where adapter_type_id = _id;
		--adapterType
		return func_access_is_granted(378, _tmpID, _ownerid, _ownerclassid, _type, true);
	elseif _classid = 96 then --peripheralModel
		select into _tmpID product_catalog_type_id
		from peripheral_type
		where peripheral_type_id = _id;
		--peripheralType
		return func_access_is_granted(378, _tmpID, _ownerid, _ownerclassid, _type, true);
	elseif _classid = 93 then --networkDeviceModel
		select into _tmpID m.product_catalog_type_id
		from active_equipment_types m
		where m.im_obj_id = _id;
		--networkDeviceType
		return func_access_is_granted(378, _tmpID, _ownerid, _ownerclassid, _type, true);
	elseif _classid = 94 then --terminalDeviceModel
		select into _tmpID m.product_catalog_type_id
		from terminal_equipment_types m
		where m.im_obj_id = _id;
		--terminalDeviceType
		return func_access_is_granted(378, _tmpID, _ownerid, _ownerclassid, _type, true);
	end if;
	--
	return TRUE; --other class objects
end;
$$;