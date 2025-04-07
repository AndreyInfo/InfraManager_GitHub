IF NOT EXISTS(SELECT 1 FROM sys.columns
	WHERE Name = N'FormID'
    AND Object_ID = Object_ID(N'dbo.ProductCatalogType'))
BEGIN
    ALTER TABLE ProductCatalogType
		ADD FormID uniqueidentifier NULL;
END


IF (OBJECT_ID(N'dbo.FK_ProductCatalogType_FormID', 'F') IS NULL)
BEGIN
ALTER TABLE [dbo].ProductCatalogType WITH NOCHECK
    ADD CONSTRAINT FK_ProductCatalogType_FormID
    FOREIGN KEY (FormID)
    REFERENCES WorkflowActivityForm ([ID])
	ON DELETE SET NULL
	ON UPDATE NO ACTION;

ALTER TABLE [dbo].LifeCycle CHECK CONSTRAINT FK_LifeCycle_FormID
END
GO