IF 'NetworkNode' NOT IN (SELECT [TABLE_NAME] FROM [INFORMATION_SCHEMA].[TABLES] WHERE [TABLE_SCHEMA] = 'dbo')
    CREATE TABLE [dbo].NetworkNode (
        [ID] UNIQUEIDENTIFIER NOT NULL,
        [IPAddress] NVARCHAR(15) NOT NULL,
	    [IPMask] NVARCHAR(15) NOT NULL,
        [NetworkDeviceID] INT NULL,
        [TerminalDeviceID] INT NULL,
        [DeviceApplicationID] UNIQUEIDENTIFIER NULL,
        CONSTRAINT PK_NetworkNode PRIMARY KEY CLUSTERED ([ID]))      
GO
	ALTER TABLE [dbo].[NetworkNode]
	ADD CONSTRAINT [FK_NetworkNode_ConfigurationUnitBaseID] 
	FOREIGN KEY ([ID]) REFERENCES [dbo].[ConfigurationUnitBase] ([ID])
	ON DELETE CASCADE;
GO
	ALTER TABLE [dbo].[NetworkNode]
	CHECK CONSTRAINT [FK_NetworkNode_ConfigurationUnitBaseID];
GO

	ALTER TABLE [dbo].[NetworkNode] WITH NOCHECK
	ADD CONSTRAINT [FK_NetworkNode_NetworkDevice] 
	FOREIGN KEY ([NetworkDeviceID]) REFERENCES [dbo].[Активное устройство] ([Идентификатор])
GO
	ALTER TABLE [dbo].[NetworkNode]
	CHECK CONSTRAINT [FK_NetworkNode_NetworkDevice];
GO

	ALTER TABLE [dbo].[NetworkNode] WITH NOCHECK
	ADD CONSTRAINT [FK_NetworkNode_TerminalDevice] 
	FOREIGN KEY ([TerminalDeviceID]) REFERENCES [dbo].[Оконечное оборудование] ([Идентификатор])
GO
	ALTER TABLE [dbo].[NetworkNode]
	CHECK CONSTRAINT [FK_NetworkNode_TerminalDevice];
GO

ALTER TABLE [dbo].[NetworkNode] WITH NOCHECK
	ADD CONSTRAINT [FK_NetworkNode_DeviceApplication] 
	FOREIGN KEY ([DeviceApplicationID]) REFERENCES [dbo].[DeviceApplication] ([ID])
GO
	ALTER TABLE [dbo].[NetworkNode]
	CHECK CONSTRAINT [FK_NetworkNode_DeviceApplication];
GO