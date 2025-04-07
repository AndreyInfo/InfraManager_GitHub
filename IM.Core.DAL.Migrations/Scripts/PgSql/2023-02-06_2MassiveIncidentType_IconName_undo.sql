do $$
begin

	if 'icon_name' in (select column_name from information_schema.columns where table_schema = 'im' and table_name = 'massive_incident_type') then
			
			insert into im.object_icon(object_id, object_class_id, name)
				select s.im_obj_id, 824, s.icon_name
				from im.massive_incident_type s where length(s.icon_name) > 0
				on conflict do nothing;
				
			alter table im.massive_incident_type drop column icon_name;
	
	end if;

end
$$