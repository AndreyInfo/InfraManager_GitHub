alter table im.slot_template 
	add constraint uk_slot_template_number unique (object_id, number);
