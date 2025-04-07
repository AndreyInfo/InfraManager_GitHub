IF NOT EXISTS(SELECT 1
              FROM information_schema.referential_constraints t
              WHERE t.constraint_schema = 'dbo'
                AND t.constraint_name = 'FK_MassiveIncident_OlaID')
BEGIN
	ALTER TABLE dbo.MassIncident
		DROP CONSTRAINT FK_MassiveIncident_SlaID
	
	ALTER TABLE dbo.MassIncident
		DROP COLUMN SlaId;
		
	ALTER TABLE dbo.MassIncident
		ADD OlaID int null;
	
    ALTER TABLE dbo.MassIncident
        ADD CONSTRAINT FK_MassiveIncident_OlaID
        FOREIGN KEY (OlaID)
        REFERENCES dbo.OperationLevelAgreement (ID); 
END
GO