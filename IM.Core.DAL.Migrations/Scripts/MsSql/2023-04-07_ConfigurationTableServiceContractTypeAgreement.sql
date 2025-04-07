
ALTER TABLE [ServiceContractTypeAgreement] 
	DROP CONSTRAINT FK_ServiceContractTypeAgreement_AgreementLifeCycleID;

ALTER TABLE [ServiceContractTypeAgreement]
	ADD CONSTRAINT FK_ServiceContractTypeAgreement_AgreementLifeCycleID
		FOREIGN KEY ([AgreementLifeCycleID])
		REFERENCES LifeCycle (id)
		ON UPDATE NO ACTION
		ON DELETE CASCADE;


ALTER TABLE [ServiceContractTypeAgreement] 
	DROP CONSTRAINT FK_ServiceContractTypeAgreement_ProductCatalogTypeID;

ALTER TABLE [ServiceContractTypeAgreement]
	ADD CONSTRAINT FK_ServiceContractTypeAgreement_ProductCatalogTypeID
		FOREIGN KEY (ProductCatalogTypeID)
		REFERENCES ProductCatalogType (ID)
		ON UPDATE NO ACTION
		ON DELETE CASCADE;

DELETE FROM [ServiceContractTypeAgreement]
WHERE [AgreementLifeCycleID] IS NULL;

ALTER TABLE [ServiceContractTypeAgreement]
ALTER COLUMN [AgreementLifeCycleID] uniqueidentifier NOT NULL;

if NOT EXISTS (SELECT 1
					FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS C
					WHERE C.TABLE_NAME = 'ServiceContractTypeAgreement'
						AND C.CONSTRAINT_SCHEMA = 'dbo' 
						AND C.CONSTRAINT_NAME = 'PK_ServiceContractTypeAgreement')
BEGIN 
	ALTER TABLE [ServiceContractTypeAgreement]
		ADD CONSTRAINT PK_ServiceContractTypeAgreement PRIMARY KEY CLUSTERED ([AgreementLifeCycleID], ProductCatalogTypeID);
END 