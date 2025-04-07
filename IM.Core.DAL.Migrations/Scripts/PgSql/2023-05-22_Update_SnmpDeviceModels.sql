DO $$
begin

INSERT INTO im.active_equipment_types (identificator, name, manufacturer_id, port_count, image, width, height, height_size, cyr_product_number,
									   o_id, slot_count, product_number, external_id, code, note, removed, im_obj_id, complementary_id, depth,
									   is_rackmount, hypervisor_model_id, max_load, nominal_load, color_print, photo_print, duplex_mode, print_technology,
									   max_print_format, print_speed_unit, roll_number, speed, product_catalog_type_id, can_buy)
	VALUES (0, null, null, null, null, 0.0, 0.0, null, null, null, 0, null, 'Default', null,null, true, im.gen_random_uuid(), null, null, false, null, null, null, null, null,
			null,null, null, null, null, null, '00000000-0000-0000-0000-000000000115', false)
ON CONFLICT DO NOTHING;

ALTER TABLE IF EXISTS im.snmp_device_models 
	ADD COLUMN model_id INT NOT NULL DEFAULT 0,	
	ADD CONSTRAINT fk_snmp_device_models_active_equipment_types_id FOREIGN KEY (model_id) REFERENCES im.active_equipment_types (identificator) ON DELETE CASCADE ON UPDATE CASCADE;

--SnmpDeviceModel
insert into im.operation (id, class_id, name, operation_name, description)
	values (1369, 195, 'SnmpDeviceModel_Add', 'Добавить', 'Добавление объекта «Правило распознавания»')
	on conflict (id) do nothing;
insert into im.role_operation (operation_id, role_id) 
	values (1369, '00000000-0000-0000-0000-000000000001');

insert into im.operation (id, class_id, name, operation_name, description)
	values (1370, 195, 'SnmpDeviceModel_Update', 'Редактировать', 'Редактирование объекта «Правило распознавания»')
	on conflict (id) do nothing;
insert into im.role_operation (operation_id, role_id) 
	values (1370, '00000000-0000-0000-0000-000000000001');

insert into im.operation (id, class_id, name, operation_name, description)
	values (1371, 195, 'SnmpDeviceModel_Delete', 'Удалить', 'Удаление объекта «Правило распознавания»')
	on conflict (id) do nothing;
insert into im.role_operation (operation_id, role_id) 
values (1371, '00000000-0000-0000-0000-000000000001');

insert into im.operation (id, class_id, name, operation_name, description)
	values (1372, 195, 'SnmpDeviceModel_Properties', 'Открыть свойства', 'Открыть свойства объекта «Правило распознавания»')
	on conflict (id) do nothing;
insert into im.role_operation (operation_id, role_id) 
	values (1372, '00000000-0000-0000-0000-000000000001');

--SnmpDeviceProfile
insert into im.operation (id, class_id, name, operation_name, description)
	values (1373, 196, 'SnmpDeviceProfile_Add', 'Добавить', 'Добавление профиля объекта «Правило распознавания»')
	on conflict (id) do nothing;
insert into im.role_operation (operation_id, role_id) 
	values (1369, '00000000-0000-0000-0000-000000000001');

insert into im.operation (id, class_id, name, operation_name, description)
	values (1374, 196, 'SnmpDeviceProfile_Update', 'Редактировать', 'Редактирование профиля объекта «Правило распознавания»')
	on conflict (id) do nothing;
insert into im.role_operation (operation_id, role_id) 
	values (1370, '00000000-0000-0000-0000-000000000001');

insert into im.operation (id, class_id, name, operation_name, description)
	values (1375, 196, 'SnmpDeviceProfile_Delete', 'Удалить', 'Удаление профиля объекта «Правило распознавания»')
	on conflict (id) do nothing;
insert into im.role_operation (operation_id, role_id) 
	values (1371, '00000000-0000-0000-0000-000000000001');

insert into im.operation (id, class_id, name, operation_name, description)
	values (1376, 196, 'SnmpDeviceProfile_Properties', 'Открыть свойства', 'Открыть свойства профиля объекта «Правило распознавания»')
	on conflict (id) do nothing;
insert into im.role_operation (operation_id, role_id) 
	values (1372, '00000000-0000-0000-0000-000000000001');

end;
$$;