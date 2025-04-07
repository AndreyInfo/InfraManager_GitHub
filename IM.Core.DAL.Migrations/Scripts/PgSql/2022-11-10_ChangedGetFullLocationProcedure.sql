CREATE OR REPLACE FUNCTION im.func_get_full_object_location(IN _classid integer,IN _id uuid)
    RETURNS character varying
    LANGUAGE 'plpgsql'
    VOLATILE
    PARALLEL UNSAFE
    COST 100
    
AS $$

	declare _retval varchar(2000);
begin
	
	_retval = '';
	--
	if _classid = 101 then --Organization
	    
	elseif _classid = 102 then --Subdivision
		SELECT into _retval ('Организация "' || o.Name || '"')
		FROM im.department s 
		INNER JOIN im.Organization o ON o.identificator = s.organization_id
		where s.identificator = _id;
	elseif _classid = 1 then --Building
		SELECT into _retval  ('Организация "' || o.name || '"')
		FROM im.building b
		INNER JOIN im.Organization o on o.identificator = b.organization_id
		where b.im_obj_id = _id;
	elseif _classid = 2 then--Floor
		SELECT into _retval ('Здание "' || b.name || '", ' || im.func_get_full_object_location(1, b.im_obj_id))
		FROM im.Floor f 
		INNER JOIN im.building b   ON b.Identificator = f.building_id
		WHERE f.im_obj_id = _id;
	elseif _classid = 3 then--Room
		SELECT into _retval ('Этаж "' || f.name || '", ' || im.func_get_full_object_location(2, f.im_obj_id))
		FROM im.Room ro  
		INNER JOIN im.Floor f   ON f.Identificator = ro.floor_id
		WHERE ro.im_obj_id = _id;
	elseif _classid = 22 then--Workplace
		SELECT into _retval ('Комната "' || r.name || '", ' || im.func_get_full_object_location(3, r.im_obj_id))
		FROM im.workplace w  
		INNER JOIN im.Room r   ON r.Identificator = w.room_id
		WHERE w.im_obj_id = _id;
	elseif _classid = 223 then--softwareLicence
		SELECT into _retval ('Комната "' || r.name || '", ' || im.func_get_full_object_location(3, r.im_obj_id))
		FROM im.software_licence s  
		INNER JOIN im.Room r   ON r.Identificator = s.room_int_id
		WHERE s.id = _id;
	elseif _classid = 120 then--material
		SELECT into _retval (case when r.Identificator IS NULL then 'Общий склад' else ('Комната "' || r.name || '", ' || im.func_get_full_object_location(3, r.im_obj_id)) end)
		FROM im.material m  
		LEFT JOIN im.Room r   ON r.Identificator = m.room_id AND ISNULL(m.room_id, 0) <> 0
		WHERE m.material_id = _id;
	elseif _classid = 9 then--User
		SELECT into _retval ('Рабочее место "' || w.name || '", ' || im.func_get_full_object_location(22, w.im_obj_id))
		FROM im.users u  
		INNER JOIN im.workplace w  ON w.Identificator = u.workplace_id
		WHERE u.im_obj_id = _id;
	elseif _classid = 4 then--Rack
		SELECT into _retval ('Комната "' || ro.name || '", ' || im.func_get_full_object_location(3, ro.im_obj_id))
		FROM im.cabinet ra  
		INNER JOIN im.Room ro   ON ro.Identificator = ra.room_id
		WHERE ra.im_obj_id = _id;
	elseif _classid = 8 then-- Outlet
		SELECT into _retval ('Комната "' || r.name || '", ' || im.func_get_full_object_location(3, r.im_obj_id))
		FROM im.socket o  
		INNER JOIN im.Room r   ON r.Identificator = o.room_id
		WHERE o.im_obj_id = _id;
	elseif _classid = 7 then--Panel
		SELECT into _retval ('Шкаф "' || ra.name || '", ' || im.func_get_full_object_location(4, ra.im_obj_id))
		FROM im.panel p  
		INNER JOIN im.cabinet ra   ON ra.Identificator = p.cabinet_id
		WHERE p.im_obj_id = _id;
	elseif _classid = 14 then--PanelJack
		SELECT into _retval ('Панель "' || p.name || '", ' || im.func_get_full_object_location(7, p.im_obj_id))
		FROM im.passive_port pp  
		INNER JOIN im.panel p   ON p.Identificator = pp.socket_panel
		WHERE pp.im_obj_id = _id;
	elseif _classid = 15 then--OutletJack
		SELECT into _retval ('Розетка "' || o.name || '", ' || im.func_get_full_object_location(8, o.im_obj_id))
		FROM im.passive_port pp  
		INNER JOIN im.socket o   ON o.Identificator = pp.socket_panel
		WHERE pp.im_obj_id = _id;
	elseif _classiD = 6 then--TerminalDevice
		SELECT into _retval (case when d.workplace_id <> 0 then ('Рабочее место "' || w.name || '", ' || im.func_get_full_object_location(22, w.im_obj_id))
					when d.room_id <> 0 then ('Комната "' || r.name || '", ' || im.func_get_full_object_location(3, r.im_obj_id))
				else d.logical_location end)
		FROM im.terminal_equipment d  
		INNER JOIN im.Room r   ON r.Identificator = d.room_id
		LEFT JOIN im.workplace w   ON w.Identificator = d.workplace_id AND d.workplace_id <> 0
		WHERE d.im_obj_id = _id;
	elseif _classid = 5 then--NetworkDevice
		SELECT into _retval (case when d.cabinet_id <> 0 then ('Шкаф "' || ra.name || '", ' || im.func_get_full_object_location(4, ra.im_obj_id))
					when d.room_id <> 0 then ('Комната "' || ro.name || '", ' || im.func_get_full_object_location(3, ro.im_obj_id))
				else d.logical_location end)
		FROM im.active_equipment d  
		INNER JOIN im.Room ro   ON ro.Identificator = d.room_id
		LEFT JOIN im.cabinet ra   ON ra.Identificator = d.cabinet_id AND d.cabinet_id <> 0
		WHERE d.im_obj_id = _id;
	elseif _classid = 13 then-- ActivePort
		SELECT into _retval ('Оборудование "' || nd.name || '", ' || im.func_get_full_object_location(5, nd.im_obj_id))
		FROM im.active_port pa  
		INNER JOIN im.active_equipment nd   on nd.Identificator = pa.active_equipment_id
		WHERE pa.im_obj_id = _id;
	elseif _classid = 35 then--SplitterJack
		SELECT into _retval ('Порт №' || CAST(ppp.port_number AS varchar(50)) || ' (' || ppp.marking || '), ' || im.func_get_full_object_location(ppp.class_id,  ppp.im_obj_id))
		FROM im.passive_port pp  
		INNER JOIN im.passive_port ppp   ON ppp.Identificator = pp.socket_panel
		WHERE pp.class_id = 35 AND pp.im_obj_id = _id;
	elseif _classid = 33 then--Adapter
		SELECT into _retval (CASE
			WHEN (s.terminal_device_id <> 0 AND s.network_device_id = 0) THEN (
				SELECT 'Оборудование "' || d.name || '", ' ||  im.func_get_full_object_location(6, d.im_obj_id)
				FROM im.terminal_equipment d  
				WHERE d.Identificator = s.terminal_device_id)
			WHEN (s.terminal_device_id = 0 AND s.network_device_id <> 0) THEN (
				SELECT 'Оборудование "' || d.name || '", ' ||  im.func_get_full_object_location(5, d.im_obj_id)
				FROM im.active_equipment d  
				WHERE d.Identificator = s.network_device_id)
			WHEN (s.terminal_device_id = 0 AND s.network_device_id = 0 AND s.room_id <> 0) THEN (
				SELECT 'Комната "' || r.name || '", ' ||im.func_get_full_object_location(3, r.im_obj_id)
				FROM im.Room r  
				WHERE r.Identificator = s.room_id) ELSE ''
			END)
		FROM im.adapter s  
		WHERE s.adapter_id = _id;
	elseif _classid = 34 then-- Peripheral
		SELECT into _retval (CASE
			WHEN (s.terminal_device_id <> 0 AND s.network_device_id = 0) THEN (
				SELECT 'Оборудование "' || d.name || '", ' ||  im.func_get_full_object_location(6, d.im_obj_id)
				FROM im.terminal_equipment d  
				WHERE d.Identificator = s.terminal_device_id)
			WHEN (s.terminal_device_id = 0 AND s.network_device_id <> 0) THEN (
				SELECT 'Оборудование "' || d.name || '", ' ||  im.func_get_full_object_location(5, d.im_obj_id)
				FROM im.active_equipment d  
				WHERE d.Identificator = s.network_device_id)
			WHEN (s.terminal_device_id = 0 AND s.network_device_id = 0 AND s.room_id <> 0) THEN (
				SELECT im.func_get_full_object_location(3, r.im_obj_id)
				FROM im.Room r  
				WHERE r.Identificator = s.room_id) ELSE ''
			END)
		FROM im.peripheral s  
		WHERE s.peripheral_id = _id;
	elseif _classid = 71 then-- SoftwareInstallation
		SELECT into _retval (CASE
			WHEN si.device_class_id = 6 THEN (
				SELECT 'Оборудование "' || d.name || '", ' ||  im.func_get_full_object_location(6, d.im_obj_id)
				FROM im.terminal_equipment d  
				WHERE d.im_obj_id = si.device_id)
			WHEN si.device_class_id = 5 THEN (
				SELECT 'Оборудование "' || d.name || '", ' ||  im.func_get_full_object_location(5, d.im_obj_id)
				FROM im.active_equipment d  
				WHERE d.im_obj_id = si.device_id) ELSE ''
			END)
		FROM im.software_installation si  
		WHERE si.ID = _id;
	elseif _classid = 163 then--LogicalPort
		SELECT into _retval ('Оборудование "' || device.Name || '", ' || im.func_get_full_object_location(device.ClassID, device.ID))
		FROM im.logical_port lp  
		INNER JOIN
		(
			SELECT im_obj_id as ID, name as Name, 5 as ClassID FROM im.active_equipment  
			UNION
			SELECT im_obj_id as ID, name as Name, 6 as ClassID FROM im.terminal_equipment  
		) device on device.id = lp.device_id
		WHERE lp.id = _id;
	elseif _classid = 164 then--DeviceApplication
		SELECT into _retval ('Оборудование "' || device.name || '", ' || im.func_get_full_object_location(device.ClassID, device.ID))
		FROM im.device_application a  
		INNER JOIN
		(
			SELECT im_obj_id as ID, name as Name, 5 as ClassID FROM im.active_equipment  
			UNION
			SELECT im_obj_id as ID, name as Name, 6 as ClassID FROM im.terminal_equipment  
		) device on device.id = a.device_id
		WHERE a.id = _id;
	end if;
	--
	return _retval;
end;
$$;