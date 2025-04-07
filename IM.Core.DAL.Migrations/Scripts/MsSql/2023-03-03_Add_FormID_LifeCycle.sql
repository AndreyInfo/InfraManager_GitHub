IF NOT EXISTS(SELECT 1 FROM sys.columns
	WHERE Name = N'FormID'
    AND Object_ID = Object_ID(N'dbo.LifeCycle'))
BEGIN
    ALTER TABLE LifeCycle
		ADD FormID uniqueidentifier NULL;
END


IF (OBJECT_ID(N'dbo.FK_LifeCycle_FormID', 'F') IS NULL)
BEGIN
ALTER TABLE [dbo].LifeCycle WITH NOCHECK
    ADD CONSTRAINT FK_LifeCycle_FormID
    FOREIGN KEY (FormID)
    REFERENCES WorkflowActivityForm ([ID])

ALTER TABLE [dbo].LifeCycle CHECK CONSTRAINT FK_LifeCycle_FormID
END
GO
