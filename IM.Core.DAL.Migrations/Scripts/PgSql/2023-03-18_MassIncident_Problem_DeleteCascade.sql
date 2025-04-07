DO $$
BEGIN
    IF EXISTS (SELECT 1
               FROM information_schema.referential_constraints t
               WHERE t.constraint_schema = 'im'
                 AND t.constraint_name = 'fk_massive_incident_problem_massive_incident'
                 AND t.delete_rule <> 'CASCADE')
    THEN
        ALTER TABLE im.massive_incident_problem
            DROP CONSTRAINT fk_massive_incident_problem_massive_incident;

        ALTER TABLE im.massive_incident_problem
            ADD CONSTRAINT fk_massive_incident_problem_massive_incident
            FOREIGN KEY ( massive_incident_id )
            REFERENCES im.mass_incident ( id )
            ON DELETE cascade;
    END IF;
    
    IF EXISTS (SELECT 1
               FROM information_schema.referential_constraints t
               WHERE t.constraint_schema = 'im'
                 AND t.constraint_name = 'fk_massive_incident_problem_problem'
                 AND t.delete_rule <> 'CASCADE')
    THEN
        ALTER TABLE im.massive_incident_problem
            DROP CONSTRAINT fk_massive_incident_problem_problem;

        ALTER TABLE im.massive_incident_problem
            ADD CONSTRAINT fk_massive_incident_problem_problem
            FOREIGN KEY ( problem_id )
            REFERENCES im.problem ( id )
            ON DELETE cascade;
    END IF;
END;
$$

