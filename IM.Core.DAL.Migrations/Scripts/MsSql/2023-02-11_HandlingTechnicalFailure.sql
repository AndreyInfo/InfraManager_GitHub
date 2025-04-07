IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HandlingTechnicalFailures]') AND type in (N'U'))
BEGIN
	CREATE TABLE HandlingTechnicalFailures
	(
	    ID uniqueidentifier default NEWID(),
	    ServiceID uniqueidentifier NOT NULL,
		CategoryID INT NOT NULL,
		GroupID uniqueidentifier NOT NULL,
		CONSTRAINT PK_HandlingTechnicalFailures PRIMARY KEY (ID),
		CONSTRAINT FK_HandlingTechnicalFailures_Service 
			FOREIGN KEY  (ServiceID) 
			REFERENCES dbo.Service (ID) 
			ON DELETE CASCADE,
		CONSTRAINT FK_HandlingTechnicalFailures_Category 
			FOREIGN KEY (CategoryID) 
		REFERENCES dbo.TechnicalFailuresCategory (ID)
			ON DELETE CASCADE,
		CONSTRAINT FK_HandlingTechnicalFailures_Group 
			FOREIGN KEY (GroupID) 
			REFERENCES dbo.Queue (ID)
			ON DELETE CASCADE,
	);

	CREATE UNIQUE INDEX UI_HandlingTechnicalFailures_Service_Category on HandlingTechnicalFailures (ServiceID, CategoryID);
END;