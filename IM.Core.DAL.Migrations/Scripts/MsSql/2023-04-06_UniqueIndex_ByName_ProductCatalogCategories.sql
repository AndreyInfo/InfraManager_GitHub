
if (EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'PctNameParent' 
								and object_id = OBJECT_ID('ProductCatalogCategory')))
			DROP INDEX PctNameParent ON ProductCatalogCategory;


if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Name_ProductCatalogCategory_Parent_IS_NULL' 
							and object_id = OBJECT_ID('ProductCatalogCategory')))
        BEGIN
            CREATE UNIQUE INDEX UI_Name_ProductCatalogCategory_Parent_IS_NULL on ProductCatalogCategory (Name)
			where Removed = 0
				AND ParentProductCatalogCategoryID IS NULL;
        end;

if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Name_ProductCatalogCategory_ParentID' 
							and object_id = OBJECT_ID('ProductCatalogCategory')))
        BEGIN
            CREATE UNIQUE INDEX UI_Name_ProductCatalogCategory_ParentID on ProductCatalogCategory (Name, ParentProductCatalogCategoryID)
			where Removed = 0
				AND ParentProductCatalogCategoryID IS NOT NULL;
        end;