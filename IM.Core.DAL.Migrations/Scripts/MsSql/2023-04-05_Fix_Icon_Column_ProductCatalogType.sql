DECLARE @fk NVARCHAR(200);
SELECT @fk = f.name
FROM sys.tables t
JOIN sys.default_constraints f ON t.object_id = f.parent_object_id
WHERE t.name = 'ProductCatalogType'
	and f.parent_column_id = 17

if(@fk is not null)
BEGIN
  Exec ('ALTER TABLE ProductCatalogType DROP CONSTRAINT ' + @fk);
  ALTER TABLE ProductCatalogType DROP CONSTRAINT FK_ProductCatalogType_Icons;
  ALTER TABLE ProductCatalogType DROP COLUMN IconID;
  ALTER TABLE ProductCatalogType ADD IconName nvarchar(50) null;
END;