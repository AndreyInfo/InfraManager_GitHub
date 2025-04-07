if (EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'PctNameParent' 
								and object_id = OBJECT_ID('ProductCatalogType')))
			DROP INDEX PctNameParent ON ProductCatalogType;

		if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Name_ProductCatalogType_CategoryID' 
							and object_id = OBJECT_ID('ProductCatalogType')))
        BEGIN
            CREATE UNIQUE INDEX UI_Name_ProductCatalogType_CategoryID on ProductCatalogType (Name, ProductCatalogCategoryID)
			where Removed = 0;
        end;