DO $$
begin
	
	if '_MI_MY_NOT_STARTED_' not in (select name from im.web_filters where view_name = 'AllMassIncidentsList') then
		insert into im.web_filters (id, name, standart, view_name, others) values (gen_random_uuid(), '_MI_MY_NOT_STARTED_', true, 'AllMassIncidentsList', true);
	end if;
	
	if '_MI_MY_OPENED_OVERUDE_' not in (select name from im.web_filters where view_name = 'AllMassIncidentsList') then
		insert into im.web_filters (id, name, standart, view_name, others) values (gen_random_uuid(), '_MI_MY_OPENED_OVERUDE_', true, 'AllMassIncidentsList', true);
	end if;
	
	if '_MI_UNASSIGNED_' not in (select name from im.web_filters where view_name = 'AllMassIncidentsList') then
		insert into im.web_filters (id, name, standart, view_name, others) values (gen_random_uuid(), '_MI_UNASSIGNED_', true, 'AllMassIncidentsList', true);
	end if;
	
	if '_MI_OTHERS_OVERDUE_' not in (select name from im.web_filters where view_name = 'AllMassIncidentsList') then
		insert into im.web_filters (id, name, standart, view_name, others) values (gen_random_uuid(), '_MI_OTHERS_OVERDUE_', true, 'AllMassIncidentsList', true);
	end if;	
	
	if '_MI_MY_' not in (select name from im.web_filters where view_name = 'AllMassIncidentsList') then
		insert into im.web_filters (id, name, standart, view_name, others) values (gen_random_uuid(), '_MI_MY_', true, 'AllMassIncidentsList', true);
	end if;	
	
	if '_MI_MY_IN_WORK_' not in (select name from im.web_filters where view_name = 'AllMassIncidentsList') then
		insert into im.web_filters (id, name, standart, view_name, others) values (gen_random_uuid(), '_MI_MY_IN_WORK_', true, 'AllMassIncidentsList', true);
	end if;
	
	if '_MI_MY_COMPLETED_CONFIRM_' not in (select name from im.web_filters where view_name = 'AllMassIncidentsList') then
		insert into im.web_filters (id, name, standart, view_name, others) values (gen_random_uuid(), '_MI_MY_COMPLETED_CONFIRM_', true, 'AllMassIncidentsList', true);
	end if;	
	
	if '_MI_MY_CLOSED_' not in (select name from im.web_filters where view_name = 'AllMassIncidentsList') then
		insert into im.web_filters (id, name, standart, view_name, others) values (gen_random_uuid(), '_MI_MY_CLOSED_', true, 'AllMassIncidentsList', true);
	end if;	
		
	if '_MI_NOT_MY_' not in (select name from im.web_filters where view_name = 'AllMassIncidentsList') then
		insert into im.web_filters (id, name, standart, view_name, others) values (gen_random_uuid(), '_MI_NOT_MY_', true, 'AllMassIncidentsList', true);
	end if;	
	
	if '_MI_NOT_MY_INWORK_' not in (select name from im.web_filters where view_name = 'AllMassIncidentsList') then
		insert into im.web_filters (id, name, standart, view_name, others) values (gen_random_uuid(), '_MI_NOT_MY_INWORK_', true, 'AllMassIncidentsList', true);
	end if;	
	
	if '_MI_NOT_MY_COMPLETED_CONFIRM_' not in (select name from im.web_filters where view_name = 'AllMassIncidentsList') then
		insert into im.web_filters (id, name, standart, view_name, others) values (gen_random_uuid(), '_MI_NOT_MY_COMPLETED_CONFIRM_', true, 'AllMassIncidentsList', true);
	end if;
	
	if '_MI_NOT_MY_CLOSED_' not in (select name from im.web_filters where view_name = 'AllMassIncidentsList') then
		insert into im.web_filters (id, name, standart, view_name, others) values (gen_random_uuid(), '_MI_NOT_MY_CLOSED_', true, 'AllMassIncidentsList', true);
	end if;	
	
	if '_MI_OWNED_OPENED_' not in (select name from im.web_filters where view_name = 'AllMassIncidentsList') then
		insert into im.web_filters (id, name, standart, view_name, others) values (gen_random_uuid(), '_MI_OWNED_OPENED_', true, 'AllMassIncidentsList', true);
	end if;	
		
	if '_MI_EXECUTED_OPENED_' not in (select name from im.web_filters where view_name = 'AllMassIncidentsList') then
		insert into im.web_filters (id, name, standart, view_name, others) values (gen_random_uuid(), '_MI_EXECUTED_OPENED_', true, 'AllMassIncidentsList', true);
	end if;
	
	create index if not exists ix_mass_incident_created_by on im.mass_incident (created_by_user_id);
	create index if not exists ix_mass_incident_owned_by on im.mass_incident (owned_by_user_id);
	create index if not exists ix_mass_incident_type on im.mass_incident (type_id);
	create index if not exists ix_mass_incident_name on im.mass_incident (name);
	create index if not exists ix_mass_incident_description on im.mass_incident (description_plain);
	create index if not exists ix_mass_incident_solution on im.mass_incident (solution_plain);
	create index if not exists ix_mass_incident_service on im.mass_incident (service_id);
	create index if not exists ix_mass_incident_state on im.mass_incident (entity_state_id);
	create index if not exists ix_mass_incident_created_at on im.mass_incident (utc_created_at);
	create index if not exists ix_mass_incident_modified_at on im.mass_incident (utc_last_modified_at);
	create index if not exists ix_mass_incident_opened_at on im.mass_incident (utc_opened_at);
	create index if not exists ix_mass_incident_registered_at on im.mass_incident (utc_registered_at);
	create index if not exists ix_mass_incident_close_until on im.mass_incident (utc_close_until);
	create index if not exists ix_mass_incident_executed_by_group on im.mass_incident (executed_by_group_id);
	create index if not exists ix_mass_incident_ola on im.mass_incident (ola_id);
end
$$