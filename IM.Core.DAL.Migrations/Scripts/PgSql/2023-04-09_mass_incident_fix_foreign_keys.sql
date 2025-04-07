DO $$
BEGIN
    IF EXISTS (SELECT 1
               FROM information_schema.referential_constraints t
               WHERE t.constraint_schema = 'im'
                 AND t.constraint_name = 'fk_mass_incident_change_request_change_request'
                 AND t.delete_rule <> 'CASCADE')
    THEN
        ALTER TABLE im.mass_incident_change_request
            DROP CONSTRAINT fk_mass_incident_change_request_change_request;

        ALTER TABLE im.mass_incident_change_request
            ADD CONSTRAINT fk_mass_incident_change_request_change_request
            FOREIGN KEY ( change_request_id )
            REFERENCES im.rfc (id)
            ON DELETE cascade;
    END IF;
    
    IF EXISTS (SELECT 1
               FROM information_schema.referential_constraints t
               WHERE t.constraint_schema = 'im'
                 AND t.constraint_name = 'fk_mass_incident_change_request_mass_incident'
                 AND t.delete_rule <> 'CASCADE')
    THEN
        ALTER TABLE im.mass_incident_change_request
            DROP CONSTRAINT fk_mass_incident_change_request_mass_incident;

        ALTER TABLE im.mass_incident_change_request
            ADD CONSTRAINT fk_mass_incident_change_request_mass_incident
            FOREIGN KEY (mass_incident_id)
            REFERENCES im.mass_incident (id)
            ON DELETE cascade;
    END IF;
	
	IF EXISTS (SELECT 1
               FROM information_schema.referential_constraints t
               WHERE t.constraint_schema = 'im'
                 AND t.constraint_name = 'fk_mass_incident_call_call'
                 AND t.delete_rule <> 'CASCADE')
    THEN
        ALTER TABLE im.mass_incident_call
            DROP CONSTRAINT fk_mass_incident_call_call;

        ALTER TABLE im.mass_incident_call
            ADD CONSTRAINT fk_mass_incident_call_call
            FOREIGN KEY (call_id)
            REFERENCES im.call (id)
            ON DELETE cascade;
    END IF;
    
    IF EXISTS (SELECT 1
               FROM information_schema.referential_constraints t
               WHERE t.constraint_schema = 'im'
                 AND t.constraint_name = 'fk_mass_incident_call_mass_incident'
                 AND t.delete_rule <> 'CASCADE')
    THEN
        ALTER TABLE im.mass_incident_call
            DROP CONSTRAINT fk_mass_incident_call_mass_incident;

        ALTER TABLE im.mass_incident_call
            ADD CONSTRAINT fk_mass_incident_call_mass_incident
            FOREIGN KEY (mass_incident_id)
            REFERENCES im.mass_incident (id)
            ON DELETE cascade;
    END IF;
	
	IF EXISTS (SELECT 1
               FROM information_schema.referential_constraints t
               WHERE t.constraint_schema = 'im'
                 AND t.constraint_name = 'fk_mass_incident_affected_service_mass_incident'
                 AND t.delete_rule <> 'CASCADE')
    THEN
        ALTER TABLE im.mass_incident_affected_service
            DROP CONSTRAINT fk_mass_incident_affected_service_mass_incident;

        ALTER TABLE im.mass_incident_affected_service
            ADD CONSTRAINT fk_mass_incident_affected_service_mass_incident
            FOREIGN KEY (mass_incident_id)
            REFERENCES im.mass_incident (id)
            ON DELETE cascade;
    END IF;
    
    IF EXISTS (SELECT 1
               FROM information_schema.referential_constraints t
               WHERE t.constraint_schema = 'im'
                 AND t.constraint_name = 'fk_mass_incident_affected_service_service'
                 AND t.delete_rule <> 'CASCADE')
    THEN
        ALTER TABLE im.mass_incident_affected_service
            DROP CONSTRAINT fk_mass_incident_affected_service_service;

        ALTER TABLE im.mass_incident_affected_service
            ADD CONSTRAINT fk_mass_incident_affected_service_service
            FOREIGN KEY (service_id)
            REFERENCES im.service (id)
            ON DELETE cascade;
    END IF;
END;
$$

