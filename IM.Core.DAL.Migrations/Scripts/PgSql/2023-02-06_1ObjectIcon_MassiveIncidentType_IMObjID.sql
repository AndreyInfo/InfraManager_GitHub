do $$
begin
	create sequence if not exists im.object_icon_id start with 1 increment by 1;

	create table if not exists im.object_icon (
		id int not null default(nextval('object_icon_id')),
		object_id uuid not null,
		object_class_id int not null,
		name character varying(100),
		content bytea,
		constraint pk_object_icon primary key (id),
		constraint uk_object_icon_object unique(object_id, object_class_id));

	if 'im_obj_id' not in (select column_name from information_schema.columns where table_schema = 'im' and table_name = 'massive_incident_type') then
		alter table im.massive_incident_type
			add im_obj_id uuid not null default(gen_random_uuid());
		alter table im.massive_incident_type 
			add constraint uk_massive_incident_type_im_obj_id
			unique(im_obj_id);
	end if;
END
$$