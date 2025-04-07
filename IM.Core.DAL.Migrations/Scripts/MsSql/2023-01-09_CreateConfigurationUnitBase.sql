IF 'ConfigurationUnitBase' NOT IN (SELECT [TABLE_NAME] FROM [INFORMATION_SCHEMA].[TABLES] WHERE [TABLE_SCHEMA] = 'dbo')
    CREATE SEQUENCE df_configuration_unit_base_seq AS INT
    START WITH 1
    MINVALUE 1
    INCREMENT BY 1;

    CREATE TABLE [dbo].ConfigurationUnitBase (
        [ID] UNIQUEIDENTIFIER NOT NULL,
        [Number] INT NOT NULL CONSTRAINT DF_CONFIGURATION_UNIT_BASE_NUMBER DEFAULT (NEXT VALUE FOR df_configuration_unit_base_seq),
        [Name] NVARCHAR(250) NOT NULL,
        [Description] NVARCHAR(250) NULL,
        [Note] NVARCHAR(500) NULL,
        [ExternalID] NVARCHAR(250) NULL,
        [Tags] NVARCHAR(250) NULL,
        [CreatedBy] UNIQUEIDENTIFIER NULL,
        [DateReceived] DATETIME NULL,
        [ProductCatalogTypeID] UNIQUEIDENTIFIER NOT NULL,
        [LifeCycleStateID] UNIQUEIDENTIFIER NULL,
        [InfrastructureSegmentID] UNIQUEIDENTIFIER NULL,
        [CriticalityID] UNIQUEIDENTIFIER NULL,
        [DateChanged] DATETIME NULL,
        [ChangedBy] UNIQUEIDENTIFIER NULL,
        [DateLastInquired] DATETIME NULL,
        [DateAnnulated] DATETIME NULL,
        [OrganizationItemID] UNIQUEIDENTIFIER NULL, 
        [OrganizationItemClassID] INT NULL,
        [OwnerID] UNIQUEIDENTIFIER NULL,
        [ClientID] UNIQUEIDENTIFIER NULL,
        [ConfigurationUnitSchemeID] UNIQUEIDENTIFIER NULL,
        [RowVersion] TIMESTAMP NOT NULL,
        CONSTRAINT PK_ConfigurationUnitBase PRIMARY KEY CLUSTERED ([ID]),
        CONSTRAINT FK_ConfigurationUnitBase_Criticality FOREIGN KEY ([CriticalityID]) REFERENCES [dbo].[Criticality]([ID]),
        CONSTRAINT FK_ConfigurationUnitBase_InfrastructureSegment FOREIGN KEY ([InfrastructureSegmentID]) REFERENCES [dbo].[InfrastructureSegment]([ID]),
        CONSTRAINT FK_ConfigurationUnitBase_ProductCatalogType FOREIGN KEY ([ProductCatalogTypeID]) REFERENCES [dbo].[ProductCatalogType]([ID]),
        CONSTRAINT FK_ConfigurationUnitBase_LifeCycleState FOREIGN KEY ([LifeCycleStateID]) REFERENCES [dbo].[LifeCycleState]([ID]));
GO