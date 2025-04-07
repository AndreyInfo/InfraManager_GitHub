DO $$
begin
	
create table if not exists im.massive_incident_information_channel (
	id smallint not null,
	name character varying(50) not null,
	constraint pk_massive_incident_information_channel primary key (id)
);

insert into im.massive_incident_information_channel (id, name) values (1, 'Monitoring') on conflict (id) do nothing;
insert into im.massive_incident_information_channel (id, name) values (2, 'User') on conflict (id) do nothing;
insert into im.massive_incident_information_channel (id, name) values (3, 'Administrator') on conflict (id) do nothing;

create sequence if not exists massive_incident_cause_id start 1 increment 1;

create table if not exists im.massive_incident_cause (
	id int not null default(nextval('massive_incident_cause_id')),
	name character varying(500) not null,
	removed boolean not null default(false),	
	constraint pk_massive_incident_cause primary key (id)
);

create sequence if not exists massive_incident_type_id start 1 increment 1;

create table if not exists im.massive_incident_type (
	id int not null default(nextval('massive_incident_type_id')),
	name character varying(255) not null,
	workflow_scheme_identifier character varying(255) null,
	form_id uuid null,
	removed boolean not null default(false),	
	constraint pk_massive_incident_type primary key (id)
);

create sequence if not exists massive_incident_id start 1 increment 1;

create table if not exists im.massive_incident(
	    id int not null default(nextval('massive_incident_id')),
        im_obj_id uuid not null default (gen_random_uuid()),
		name character varying(255) not null,
		short_description_plain character varying(1000),
		full_description_plain character varying(1000),
		solution_plain character varying(1000),
		short_description character varying(4000),
		full_description character varying(4000),
		solution text,
		information_channel_id smallint not null,
        priority_id uuid not null,
        criticality_id uuid null,
        type_id int not null,
        cause_id int null,
        utc_created_at timestamp(3) not null,
        utc_last_modified_at timestamp(3) not null,
        utc_opened_at timestamp(3) null,
        utc_completed_at timestamp(3) null,
        utc_close_until timestamp(3) null,
		utc_registered_at timestamp(3) null,
        created_by_user_id int not null,
        owned_by_user_id int not null,
        group_id uuid not null,
        sla_id uuid null,
		call_service_id uuid not null,
		entity_state_id character varying(50) null,
		entity_state_name character varying(250) null,
		workflow_scheme_id uuid null,
		workflow_scheme_identifier character varying(50) null,
		workflow_scheme_version character varying(50) null,
        constraint pk_massive_incident primary key (id),
		constraint uk_massive_incident_im_obj_id unique (im_obj_id),
		constraint fk_massive_incident_information_channel foreign key (information_channel_id) references im.massive_incident_information_channel(id),
		constraint fk_massive_incident_priority foreign key (priority_id) references im.priority(id),
		constraint fk_massive_incident_criticality foreign key (criticality_id) references im.criticality(id),
		constraint fk_massive_incident_type foreign key (type_id) references im.massive_incident_type(id),
		constraint fk_massive_incident_cause foreign key (cause_id) references im.massive_incident_cause(id),
		constraint fk_massive_incident_created_by foreign key (created_by_user_id) references im.users(identificator),
		constraint fk_massive_incident_owned_by foreign key (owned_by_user_id) references im.users(identificator),
		constraint fk_massive_incident_group foreign key (group_id) references im.queue(id),
		constraint fk_massive_incident_sla_id foreign key (sla_id) references im.sla(id),
		constraint fk_Massive_incident_call_service_id foreign key (call_service_id) references im.call_service(id)
);

insert into im.operation (id, class_id, name, operation_name, description)
	values (980, 823, 'MassiveIncident.Properties', 'Свойства', 'Операция дает возможность просмотра данных массового инцидента')
	on conflict (id) do nothing;

insert into im.operation (id, class_id, name, operation_name, description)
	values (981, 823, 'MassiveIncident.AllMassiveIncidentsList', 'Список "Все Массовые Инциденты". Просмотр', 'Операция дает возможность просмотра списка "Все Массовые инциденты"')
	on conflict (id) do nothing;

insert into im.operation (id, class_id, name, operation_name, description)
	values (982, 823, 'MassiveIncident.Add', 'Создать', 'Операция дает возможность создания массовых инцидентов')
	on conflict (id) do nothing;

insert into im.operation (id, class_id, name, operation_name, description)
	values (983, 823, 'MassiveIncident.Update', 'Редактировать', 'Операция дает возможность редактирования массовых инцидентов')
	on conflict (id) do nothing;

insert into im.operation (id, class_id, name, operation_name, description)
	values (984, 823, 'MassiveIncident.Delete', 'Удалить', 'Операция дает возможность удаления массовых инцидентов')
	on conflict (id) do nothing;

insert into im.operation (id, class_id, name, operation_name, description)
	values (985, 824, 'MassiveIncidentType.Properties', 'Свойства', 'Операция дает возможность просмотра данных типов массового инцидента')
	on conflict (id) do nothing;

insert into im.operation (id, class_id, name, operation_name, description)
	values (986, 824, 'MassiveIncidentType.Add', 'Создать', 'Операция дает возможность создания типов массовых инцидентов')
	on conflict (id) do nothing;

insert into im.operation (id, class_id, name, operation_name, description)
	values (987, 824, 'MassiveIncidentType.Update', 'Редактировать', 'Операция дает возможность редактирования типов массовых инцидентов')
	on conflict (id) do nothing;

insert into im.operation (id, class_id, name, operation_name, description)
	values (988, 824, 'MassiveIncidentType.Delete', 'Удалить', 'Операция дает возможность удаления типов массовых инцидентов')
	on conflict (id) do nothing;

insert into im.operation (id, class_id, name, operation_name, description)
	values (989, 825, 'MassiveIncidentCause.Properties', 'Свойства', 'Операция дает возможность просмотра данных причины массового инцидента')
	on conflict (id) do nothing;

insert into im.operation (id, class_id, name, operation_name, description)
	values (990, 825, 'MassiveIncidentCause.Add', 'Создать', 'Операция дает возможность создания причин массовых инцидентов')
	on conflict (id) do nothing;

insert into im.operation (id, class_id, name, operation_name, description)
	values (991, 825, 'MassiveIncidentCause.Update', 'Редактировать', 'Операция дает возможность редактирования причин массовых инцидентов')
	on conflict (id) do nothing;

insert into im.operation (id, class_id, name, operation_name, description)
	values (992, 825, 'MassiveIncidentCause.Delete', 'Удалить', 'Операция дает возможность удаления причин массовых инцидентов')
	on conflict (id) do nothing;

insert into im.role_operation (operation_id, role_id)
	select t.id, '00000000-0000-0000-0000-000000000001'
	from im.operation t
	left join im.role_operation x on x.operation_id = t.id and x.role_id = '00000000-0000-0000-0000-000000000001'
	where (t.id between 980 and 992) and (x.operation_id is null);

create sequence if not exists massive_incident_supervisor_id start 1 increment 1;

create table if not exists im.massive_incident_supervisor (
	id bigint not null default(nextval('massive_incident_supervisor_id')),
    massive_incident_id int not null,
	user_id int not null,
	constraint pk_massive_incident_supervisor primary key (id),
	constraint uk_massive_incident_supervisor unique(massive_incident_id, user_id),
	constraint fk_massive_incident_supervisor_massive_incident foreign key (massive_incident_id) references im.massive_incident(id),
	constraint fk_massive_incident_supervisor_user foreign key (user_id) references im.users(identificator)
);

create sequence if not exists massive_incident_call_id start 1 increment 1;

create table if not exists im.massive_incident_call (
	id bigint not null default(nextval('massive_incident_call_id')),
	massive_incident_id int not null,
	call_id uuid not null,
	constraint pk_massive_incident_call primary key (id),
	constraint uk_massive_incident_call unique(massive_incident_id, call_id),
	constraint fk_massive_incident_call_massive_incident foreign key (massive_incident_id) references im.massive_incident(id),
	constraint fk_massive_incident_call_call foreign key (call_id) references im.call(id)
);

create sequence if not exists massive_incident_problem_id start 1 increment 1;

create table if not exists im.massive_incident_problem (
	id bigint not null default(nextval('massive_incident_problem_id')),
	massive_incident_id int not null,
	problem_id uuid not null,
	constraint pk_massive_incident_problem primary key (id),
	constraint uk_massive_incident_problem unique(massive_incident_id, problem_id),
	constraint fk_massive_incident_problem_massive_incident foreign key (massive_incident_id) references im.massive_incident(id),
	constraint fk_massive_incident_problem_problem foreign key (problem_id) references im.problem(id)
);

create sequence if not exists massive_incident_change_request_id start 1 increment 1;

create table if not exists im.massive_incident_change_request (
	id bigint not null default(nextval('massive_incident_change_request_id')),
	massive_incident_id int not null,
	change_request_id uuid not null,
	constraint pk_massive_incident_change_request primary key (id),
	constraint uk_massive_incident_change_request unique(massive_incident_id, change_request_id),
	constraint fk_massive_incident_change_request_massive_incident foreign key (massive_incident_id) references im.massive_incident(id),
	constraint fk_massive_incident_change_request_change_request foreign key (change_request_id) references im.rfc(id)
);

create sequence if not exists massive_incident_call_service_id start 1 increment 1;

create table if not exists im.massive_incident_call_service (
	id bigint not null default(nextval('massive_incident_call_service_id')),
	massive_incident_id int not null,
	call_service_id uuid not null,
	constraint pk_massive_incident_call_service primary key (id),
	constraint uk_massive_incident_call_service unique(massive_incident_id, call_service_id),
	constraint fk_massive_incident_call_service_massive_incident foreign key (massive_incident_id) references im.massive_incident(id),
	constraint fk_massive_incident_call_service_call_service foreign key (call_service_id) references im.call_service(id)
);

delete from im.web_filter_using where filter_id in (select id from im.web_filters where name = '_ALL_' and view_name = 'AllMassiveIncidentsList');
delete from im.web_filters where name = '_ALL_' and view_name = 'AllMassiveIncidentsList';
insert into im.web_filters (id, name, standart, view_name, others) values (gen_random_uuid(), '_ALL_', true, 'AllMassiveIncidentsList', true);
	
end
$$