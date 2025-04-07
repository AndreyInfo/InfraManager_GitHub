DO $$
BEGIN
    if 'service_id' not in (select column_name from information_schema.columns where table_schema = 'im' and table_name = 'mass_incident')
    THEN
        alter table im.mass_incident add service_id uuid null;
		update im.mass_incident set service_id = (select x.service_id from im.call_service x where x.id = call_service_id);
		alter table im.mass_incident alter column service_id set not null;
        alter table im.mass_incident add constraint fk_mass_incident_service foreign key (service_id) references service (id);
		alter table im.mass_incident drop constraint if exists fk_massive_incident_call_service_id;
		alter table im.mass_incident drop column call_service_id;
    END IF;	
	
    drop table if exists im.massive_incident_call_service;
    drop sequence if exists im.massive_incident_call_service_id;
    create sequence if not exists mass_incident_affected_service_id start 1 increment 1;
    create table if not exists mass_incident_affected_service (
    	id bigint DEFAULT nextval('mass_incident_affected_service_id'::regclass) NOT NULL PRIMARY KEY,
        mass_incident_id integer NOT NULL CONSTRAINT fk_mass_incident_affected_service_mass_incident REFERENCES mass_incident,
        service_id uuid NOT NULL CONSTRAINT fk_mass_incident_affected_service_service REFERENCES service,
        CONSTRAINT uk_mass_incident_service UNIQUE (mass_incident_id, service_id));	
	delete from im.web_filter_using where filter_id in (select id from im.web_filters where name = '_ALL_' and view_name = 'AllMassiveIncidentsList');
	delete from im.web_filters where name = '_ALL_' and view_name = 'AllMassiveIncidentsList';
	delete from im.web_filter_using where filter_id in (select id from im.web_filters where name = '_ALL_' and view_name = 'AllMassIncidentsList');
	delete from im.web_filters where name = '_ALL_' and view_name = 'AllMassIncidentsList';
	insert into im.web_filters (id, name, standart, view_name, others) values (gen_random_uuid(), '_ALL_', true, 'AllMassIncidentsList', true);	
END
$$