DECLARE @fk NVARCHAR(200);
DECLARE @fkName NVARCHAR(100) = 'FK_ProductCatalogType_Icons';

SELECT @fk = f.name
FROM sys.tables t
JOIN sys.foreign_keys f ON t.object_id = f.parent_object_id
JOIN sys.tables rt ON f.referenced_object_id = rt.object_id
WHERE t.name = 'ProductCatalogType'
  AND rt.name = 'icons'
  AND f.is_system_named = 'true'

IF (@fk IS NOT NULL)
BEGIN
  exec sp_rename @fk, @fkName
END
GO