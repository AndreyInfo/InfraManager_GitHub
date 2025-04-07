if NOT EXISTS (SELECT 1
					FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS C
					WHERE C.TABLE_NAME = 'ServiceContractFeature'
						AND C.CONSTRAINT_SCHEMA = 'dbo' 
						AND C.CONSTRAINT_NAME = 'PK_ServiceContractFeature')
BEGIN 
	ALTER TABLE ServiceContractFeature
		ADD CONSTRAINT PK_ServiceContractFeature PRIMARY KEY CLUSTERED (ProductCatalogTypeID, Feature);
END 

ALTER TABLE ServiceContractFeature 
	DROP CONSTRAINT FK_ServiceContractFeature_ProductCatalogTypeID

ALTER TABLE ServiceContractFeature 
  ADD CONSTRAINT FK_ServiceContractFeature_ProductCatalogTypeID 
  FOREIGN KEY (ProductCatalogTypeID) 
  REFERENCES ProductCatalogType(id) 
  ON DELETE CASCADE;