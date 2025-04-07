if object_id('dbo.MassIncident', 'U') is not null
begin
	IF COL_LENGTH('dbo.MassIncident', 'CausePlain') IS NULL
		ALTER TABLE dbo.MassIncident ADD CausePlain nvarchar(1000);
	IF COL_LENGTH('dbo.MassIncident', 'Cause') IS NULL
		ALTER TABLE dbo.MassIncident ADD Cause nvarchar(4000);
end
