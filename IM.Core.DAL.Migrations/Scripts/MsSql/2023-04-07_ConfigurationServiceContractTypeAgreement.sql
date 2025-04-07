DELETE FROM [ServiceContractTypeAgreement]
		WHERE [AgreementLifeCycleID] IS NULL;

ALTER TABLE [ServiceContractTypeAgreement]
		ALTER COLUMN [AgreementLifeCycleID] uniqueidentifier NOT NULL;