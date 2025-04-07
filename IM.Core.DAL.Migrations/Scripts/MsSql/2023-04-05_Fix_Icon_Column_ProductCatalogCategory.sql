DECLARE @fk NVARCHAR(200);
SELECT @fk = f.name
FROM sys.tables t
JOIN sys.default_constraints f ON t.object_id = f.parent_object_id
WHERE t.name = 'ProductCatalogCategory'
	and f.parent_column_id = 7

if(@fk is not null)
BEGIN
  Exec ('ALTER TABLE ProductCatalogCategory DROP CONSTRAINT ' + @fk);
  ALTER TABLE ProductCatalogCategory DROP CONSTRAINT FK_ProductCatalogCategory_Icons;
  ALTER TABLE ProductCatalogCategory DROP COLUMN IconID;
  ALTER TABLE ProductCatalogCategory ADD IconName nvarchar(50) null;
END;