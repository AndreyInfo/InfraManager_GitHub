IF COL_LENGTH('WorkflowActivityForm','MainID') IS NULL
BEGIN
	ALTER TABLE WorkflowActivityForm ADD MainID uniqueidentifier not null default newid()
END
