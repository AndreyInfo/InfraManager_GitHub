DO $$
BEGIN
create sequence if not exists operational_level_agreement_id start 1 increment 1;

create table if not exists im.operational_level_agreement (
	id int not null default(nextval('operational_level_agreement_id')),
	im_obj_id uuid not null default (gen_random_uuid()),
	name character varying(255) not null,
	number character varying(255) not null,
	utc_start_data timestamp without time zone  null,
    utc_finish_data timestamp without time zone null,
	time_zone_id character varying(250) null,
	calendar_work_schedule_id uuid null,
	note character varying(4000),
	form_id uuid null,
	
	constraint fk_operational_level_agreement_form foreign key (form_id) references im.form_builder_form(id),
	constraint pk_operational_level_agreement primary key (id)
);

CREATE UNIQUE INDEX 
    if not exists ui_operational_level_agreement_name 
    on operational_level_agreement(name);

create sequence if not exists operational_level_agreement_service_id start 1 increment 1;

create table if not exists im.operational_level_agreement_service (
	id bigint not null default(nextval('operational_level_agreement_service_id')),
	operational_level_agreement_id int not null,
	service_id uuid not null,
	constraint pk_operational_level_agreement_service primary key (id),
	constraint uk_operational_level_agreement_service unique(operational_level_agreement_id, service_id),
	constraint fk_operational_level_agreement_service foreign key (service_id) references im.service(id) on delete cascade,
	constraint fk_operational_level_agreement_service_operational_level_agreem foreign key (operational_level_agreement_id) references im.operational_level_agreement(id) on delete cascade
);

insert into im.class (class_id, name) values (909, 'OLA')
	on conflict (class_id) do nothing;

insert into im.operation (id, class_id, name, operation_name, description)
	values (1340, 909, 'OperationalLevelAgreement.Add', 'Создать', 'Операция дает возможность создавать новый объект Соглашения, но не дает возможности просмотра и изменения объекта Соглашения.')
	on conflict (id) do nothing;

insert into im.operation (id, class_id, name, operation_name, description)
	values (1341, 909, 'OperationalLevelAgreement.Update', 'Сохранить', 'Операция позволяет изменять поля объекта Соглашения через форму свойств.')
	on conflict (id) do nothing;

insert into im.operation (id, class_id, name, operation_name, description)
	values (1342, 909, 'OperationalLevelAgreement.Delete', 'Удалить', 'Операция дает возможность удалять объект Соглашения в случае отсутствия дочерних объектов.')
	on conflict (id) do nothing;
	
insert into im.operation (id, class_id, name, operation_name, description)
	values (1343, 909, 'OperationalLevelAgreement.Properties', 'Открыть свойства', 'Операция позволяет просматривать поля объекта Соглашения через форму свойств, но не позволяет изменять их.')
	on conflict (id) do nothing;

update im.operation set description = 'Операция позволяет изменять поля объекта Соглашения через форму свойств.' where id = 362;

update im.operation set description = 'Операция позволяет просматривать поля объекта Правила через форму свойств.' where id = 364;

update im.class set name = 'Правила соглашений' where class_id = 129;

insert into im.role_operation (operation_id, role_id)
	select t.id, '00000000-0000-0000-0000-000000000001'
	from im.operation t
	left join im.role_operation x on x.operation_id = t.id and x.role_id = '00000000-0000-0000-0000-000000000001'
	where (t.id between 1340 and 1343) and (x.operation_id is null);	


ALTER TABLE IF EXISTS im.rule
    ADD COLUMN IF NOT EXISTS operational_level_agreement_id integer;

if not exists (SELECT 1 FROM pg_constraint WHERE conname = 'fk_rule_operational_level_agreement')
	then
    	ALTER TABLE im.rule
        	ADD CONSTRAINT fk_rule_operational_level_agreement FOREIGN KEY (operational_level_agreement_id) references im.operational_level_agreement(id) on delete cascade;
end if;

END;
$$