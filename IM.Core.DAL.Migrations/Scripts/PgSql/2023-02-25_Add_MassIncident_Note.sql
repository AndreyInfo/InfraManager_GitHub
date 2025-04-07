DO $$
begin
	if 'massive_incident' in (select table_name from information_schema.tables where table_schema='im')
	then
        alter table im.massive_incident rename to mass_incident;		
	end if;	
	
	create table if not exists im.mass_incident_note (
		id uuid not null,
		mass_incident_id uuid not null,
		created_by_user_id uuid not null,
		created_at timestamp(3) not null,
		text_plain character varying(4000) not null,
		text_formatted character varying(4000) not null,
		type smallint not null,
		constraint pk_mass_incident_note primary key (id),
		constraint fk_mass_incident_note_mass_incident foreign key(mass_incident_id) references mass_incident (im_obj_id),
		constraint fk_mass_incident_author foreign key (created_by_user_id) references users (im_obj_id)
	);
	
	if 'tech_failure_category_id' not in (select column_name from information_schema.columns where table_schema = 'im' and table_name = 'mass_incident')
	then
		alter table im.mass_incident add tech_failure_category_id int null;
		alter table im.mass_incident 
			add constraint fk_mass_incident_tech_failure_category 
			foreign key (tech_failure_category_id) 
			references technical_failures_category (id);
	end if;
	
end $$