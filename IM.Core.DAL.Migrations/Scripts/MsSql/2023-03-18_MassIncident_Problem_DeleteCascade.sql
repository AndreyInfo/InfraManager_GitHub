IF EXISTS (SELECT 1
           FROM information_schema.referential_constraints t
           WHERE t.constraint_schema = 'dbo'
             AND t.constraint_name = 'FK_MassiveIncidentProblem_MassiveIncident'
             AND t.delete_rule <> 'CASCADE')
BEGIN
    ALTER TABLE dbo.MassiveIncidentProblem
        DROP CONSTRAINT FK_MassiveIncidentProblem_MassiveIncident;

    ALTER TABLE dbo.MassiveIncidentProblem
        ADD CONSTRAINT FK_MassiveIncidentProblem_MassiveIncident
            FOREIGN KEY ( MassiveIncidentID )
            REFERENCES dbo.MassIncident ( ID )
            ON DELETE cascade;
END
GO
    
IF EXISTS (SELECT 1
           FROM information_schema.referential_constraints t
           WHERE t.constraint_schema = 'dbo'
             AND t.constraint_name = 'FK_MassiveIncidentProblem_Problem'
             AND t.delete_rule <> 'CASCADE')
BEGIN
    ALTER TABLE dbo.MassiveIncidentProblem
        DROP CONSTRAINT FK_MassiveIncidentProblem_Problem;
    
    ALTER TABLE dbo.MassiveIncidentProblem
        ADD CONSTRAINT FK_MassiveIncidentProblem_Problem
            FOREIGN KEY ( ProblemID )
            REFERENCES dbo.Problem ( ID )
            ON DELETE cascade;
END
GO

