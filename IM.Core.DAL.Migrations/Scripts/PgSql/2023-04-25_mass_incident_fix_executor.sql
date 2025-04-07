update im.mass_incident set executed_by_user_id = 1 where executed_by_user_id is null;
alter table im.mass_incident alter column executed_by_user_id set not null;