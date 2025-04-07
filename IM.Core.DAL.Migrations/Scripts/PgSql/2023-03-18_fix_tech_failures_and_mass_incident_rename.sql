do $$
begin
	if 'handling_technical_failures' in (select table_name from information_schema.tables where table_schema = 'im') then
		alter table im.handling_technical_failures rename to service_technical_failure_category;
	end if;

	if 'technical_failures_category' in (select table_name from information_schema.tables where table_schema = 'im') then
		alter table im.technical_failures_category rename to technical_failure_category;
	end if;

	create sequence if not exists service_technical_failure_category_id start 1 increment 1;
	
	if exists(select 1 from information_schema.columns where table_schema = 'im' and table_name = 'service_technical_failure_category' and column_name = 'id' and data_type = 'uuid')
		then
		
		delete from im.service_technical_failure_category where service_id is null and category_id is null and group_id is null; --чистим мусор если вдруг появился
		
		alter table im.service_technical_failure_category drop constraint if exists handling_technical_failures_pkey;
		alter table im.service_technical_failure_category drop constraint if exists fk_handling_technical_failures_category;
		alter table im.service_technical_failure_category drop constraint if exists fk_handling_technical_failures_group;
		alter table im.service_technical_failure_category drop constraint if exists fk_handling_technical_failures_service;
		alter table im.service_technical_failure_category drop constraint if exists ui_handling_technical_failures_service_category;
		alter table im.service_technical_failure_category rename column id to im_obj_id;
		alter table im.service_technical_failure_category rename column category_id to technical_failure_category_id;
		alter table im.service_technical_failure_category add id bigint not null default(nextval('service_technical_failure_category_id'));
		alter table im.service_technical_failure_category add primary key (id);		
		alter table im.service_technical_failure_category alter column service_id set not null;
		alter table im.service_technical_failure_category alter column technical_failure_category_id set not null;
		alter table im.service_technical_failure_category alter column group_id set not null;
		alter table im.service_technical_failure_category add constraint fk_service_technical_failure_category_service foreign key (service_id) references im.service (id);
		alter table im.service_technical_failure_category add constraint fk_service_technical_failure_category_category foreign key (technical_failure_category_id) references im.technical_failure_category (id);
		alter table im.service_technical_failure_category add constraint fk_service_technical_failure_category_group foreign key (group_id) references im.queue (id);
		alter table im.service_technical_failure_category add constraint uk_service_technical_failure_category unique (service_id, technical_failure_category_id);
		alter table im.service_technical_failure_category add constraint uk_service_technical_failure_category_im_obj_id unique (im_obj_id);
		
	end if;
	
	-- fix remaining namings of mass incident tables / keys etc.
	
	if 'pk_massive_incident' in (select constraint_name from information_schema.table_constraints where table_schema = 'im' and table_name = 'mass_incident' and constraint_type  = 'PRIMARY KEY') then
		alter table mass_incident rename constraint pk_massive_incident to mass_incident_pkey;
    	alter table mass_incident rename constraint uk_massive_incident_im_obj_id to uk_mass_incident_im_obj_id;
    	alter table mass_incident rename constraint fk_massive_incident_cause to fk_mass_incident_cause;
    	alter table mass_incident rename constraint fk_massive_incident_created_by to fk_mass_incident_created_by;
    	alter table mass_incident rename constraint fk_massive_incident_criticality to fk_mass_incident_criticality;
    	alter table mass_incident rename constraint fk_massive_incident_executor_user_id to fk_mass_incident_executor_user_id;
    	alter table mass_incident rename constraint fk_massive_incident_group to fk_mass_incident_group;
    	alter table mass_incident rename constraint fk_massive_incident_information_channel to fk_mass_incident_information_channel;
    	alter table mass_incident rename constraint fk_massive_incident_owned_by to fk_mass_incident_owned_by;
    	alter table mass_incident rename constraint fk_massive_incident_priority to fk_mass_incident_priority;
    	alter table mass_incident rename constraint fk_massive_incident_sla_id to fk_mass_incident_sla_id;
    	alter table mass_incident rename constraint fk_massive_incident_type to fk_mass_incident_type;
	end if;
	
	if 'massive_incident_call' in (select table_name from information_schema.tables where table_schema = 'im') then
		alter table im.massive_incident_call rename to mass_incident_call;
		alter table im.mass_incident_call rename column massive_incident_id to mass_incident_id;
		alter table im.mass_incident_call rename constraint pk_massive_incident_call to mass_incident_call_pkey;
		alter table im.mass_incident_call rename constraint fk_massive_incident_call_call to fk_mass_incident_call_call;
		alter table im.mass_incident_call rename constraint fk_massive_incident_call_massive_incident to fk_mass_incident_call_mass_incident;
		alter table im.mass_incident_call rename constraint uk_massive_incident_call to uk_mass_incident_call;
	end if;
	
	if 'massive_incident_problem' in (select table_name from information_schema.tables where table_schema = 'im') then
		alter table im.massive_incident_problem rename to mass_incident_problem;
		alter table im.mass_incident_problem rename column massive_incident_id to mass_incident_id;
		alter table im.mass_incident_problem rename constraint pk_massive_incident_problem to mass_incident_problem_pkey;
		alter table im.mass_incident_problem rename constraint fk_massive_incident_problem_problem to fk_mass_incident_problem_problem;
		alter table im.mass_incident_problem rename constraint fk_massive_incident_problem_massive_incident to fk_mass_incident_problem_mass_incident;
		alter table im.mass_incident_problem rename constraint uk_massive_incident_problem to uk_mass_incident_problem;
	end if;
	
	if 'massive_incident_change_request' in (select table_name from information_schema.tables where table_schema = 'im') then
		alter table im.massive_incident_change_request rename to mass_incident_change_request;
		alter table im.mass_incident_change_request rename column massive_incident_id to mass_incident_id;
		alter table im.mass_incident_change_request rename constraint pk_massive_incident_change_request to mass_incident_change_request_pkey;
		alter table im.mass_incident_change_request rename constraint fk_massive_incident_change_request_change_request to fk_mass_incident_change_request_change_request;
		alter table im.mass_incident_change_request rename constraint fk_massive_incident_change_request_massive_incident to fk_mass_incident_change_request_mass_incident;
		alter table im.mass_incident_change_request rename constraint uk_massive_incident_change_request to uk_mass_incident_change_request;
	end if;
	
	if 'massive_incident_work_order' in (select table_name from information_schema.tables where table_schema = 'im') then
		alter table im.massive_incident_work_order rename to mass_incident_work_order;
		alter table im.mass_incident_work_order rename column massive_incident_id to mass_incident_id;
		alter table im.mass_incident_work_order rename constraint pk_massive_incident_work_order to mass_incident_work_order_pkey;
		alter table im.mass_incident_work_order rename constraint fk_massive_incident_work_order_work_order to fk_mass_incident_work_order_work_order;
		alter table im.mass_incident_work_order rename constraint fk_massive_incident_work_order_massive_incident to fk_mass_incident_work_order_mass_incident;
		alter table im.mass_incident_work_order rename constraint uk_massive_incident_work_order to uk_mass_incident_work_order;
	end if;
	
	if 'massive_incident_cause' in (select table_name from information_schema.tables where table_schema = 'im') then
		alter table im.massive_incident_cause rename to mass_incident_cause;
		alter table im.mass_incident_cause rename constraint pk_massive_incident_cause to mass_incident_cause_pkey;
	end if;
	
	if 'massive_incident_information_channel' in (select table_name from information_schema.tables where table_schema = 'im') then
		alter table im.massive_incident_information_channel rename to mass_incident_information_channel;
		alter table im.mass_incident_information_channel rename constraint pk_massive_incident_information_channel to mass_incident_information_channel_pkey;
	end if;
	
	if 'massive_incident_supervisor' in (select table_name from information_schema.tables where table_schema = 'im') then
		alter table im.massive_incident_supervisor rename to mass_incident_supervisor;
		alter table im.mass_incident_supervisor rename column massive_incident_id to mass_incident_id;
		alter table im.mass_incident_supervisor rename constraint pk_massive_incident_supervisor to mass_incident_supervisor_pkey;
		alter table im.mass_incident_supervisor rename constraint fk_massive_incident_supervisor_user to fk_mass_incident_supervisor_user;
		alter table im.mass_incident_supervisor rename constraint fk_massive_incident_supervisor_massive_incident to fk_mass_incident_supervisor_mass_incident;
		alter table im.mass_incident_supervisor rename constraint uk_massive_incident_supervisor to uk_mass_incident_supervisor;
	end if;
	
	if 'massive_incident_type' in (select table_name from information_schema.tables where table_schema = 'im') then
		alter table im.massive_incident_type rename to mass_incident_type;
		alter table im.mass_incident_type rename constraint pk_massive_incident_type to mass_incident_type_pkey;
		alter table im.mass_incident_type rename constraint uk_massive_incident_type_im_obj_id to uk_mass_incident_type_im_obj_id;
	end if;
	
	alter sequence if exists massive_incident_cause_id rename to mass_incident_cause_id;
	alter sequence if exists massive_incident_type_id rename to mass_incident_type_id;
	alter sequence if exists massive_incident_id rename to mass_incident_id;
	alter sequence if exists massive_incident_supervisor_id rename to mass_incident_supervisor_id;
	alter sequence if exists massive_incident_call_id rename to mass_incident_call_id;
	alter sequence if exists massive_incident_problem_id rename to mass_incident_problem_id;
	alter sequence if exists massive_incident_change_request_id rename to mass_incident_change_request_id;
	alter sequence if exists massive_incident_call_service_id rename to mass_incident_call_service_id;
end;
$$