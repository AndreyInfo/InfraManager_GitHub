DO $$
BEGIN

	drop table if exists im.mass_incident_supervisor;
	
	if 'executor_user_id' in (select column_name from information_schema.columns where table_schema = 'im' and table_name = 'mass_incident') then
		alter table im.mass_incident rename column executor_user_id to executed_by_user_id;
	end if;
	
	if 'group_id' in (select column_name from information_schema.columns where table_schema = 'im' and table_name = 'mass_incident') then
		alter table im.mass_incident rename column group_id to executed_by_group_id;
	end if;
	
	if 'full_description' in (select column_name from information_schema.columns where table_schema = 'im' and table_name = 'mass_incident') then
		alter table im.mass_incident rename column full_description to description;
	end if;
	
	if 'full_description_plain' in (select column_name from information_schema.columns where table_schema = 'im' and table_name = 'mass_incident') then
		alter table im.mass_incident rename column full_description_plain to description_plain;
	end if;
	
	if 'short_description' in (select column_name from information_schema.columns where table_schema = 'im' and table_name = 'mass_incident') then
		alter table im.mass_incident drop column short_description;
	end if;
	
	if 'short_description_plain' in (select column_name from information_schema.columns where table_schema = 'im' and table_name = 'mass_incident') then
		alter table im.mass_incident drop column short_description_plain;
	end if;
	
	if 'utc_completed_at' in (select column_name from information_schema.columns where table_schema = 'im' and table_name = 'mass_incident') then
		alter table im.mass_incident drop column utc_completed_at;
	end if;
END;
$$