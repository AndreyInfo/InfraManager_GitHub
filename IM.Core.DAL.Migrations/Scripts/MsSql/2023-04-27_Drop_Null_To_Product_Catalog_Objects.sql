IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
   WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'Adapter' AND [COLUMN_NAME] = 'TerminalDeviceId' AND [IS_NULLABLE] = 'NO')

	BEGIN

		ALTER TABLE [dbo].[Adapter] ALTER COLUMN [TerminalDeviceId] INT NULL;

	END
	
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
   WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'Adapter' AND [COLUMN_NAME] = 'TerminalDeviceID' AND [COLUMN_DEFAULT] = '(0)')

	BEGIN

		ALTER TABLE [dbo].[Adapter] DROP CONSTRAINT DF_Adapter_TerminalDeviceID;

	END
	
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
   WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'Adapter' AND [COLUMN_NAME] = 'NetworkDeviceId' AND [IS_NULLABLE] = 'NO')

	BEGIN

		ALTER TABLE [dbo].[Adapter] ALTER COLUMN [NetworkDeviceId] INT NULL;

	END
	
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
   WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'Adapter' AND [COLUMN_NAME] = 'NetworkDeviceId' AND [COLUMN_DEFAULT] = '(0)')

	BEGIN

		ALTER TABLE [dbo].[Adapter] DROP CONSTRAINT DF_Adapter_NetworkDeviceID;

	END
	
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
   WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'Peripheral' AND [COLUMN_NAME] = 'TerminalDeviceId' AND [IS_NULLABLE] = 'NO')

	BEGIN

		ALTER TABLE [dbo].[Peripheral] ALTER COLUMN [TerminalDeviceId] INT NULL;

	END

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
   WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'Peripheral' AND [COLUMN_NAME] = 'TerminalDeviceID' AND [COLUMN_DEFAULT] = '(0)')

	BEGIN

		ALTER TABLE [dbo].[Peripheral] DROP CONSTRAINT DF_Peripheral_TerminalDeviceID;

	END
	
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
   WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'Peripheral' AND [COLUMN_NAME] = 'NetworkDeviceId' AND [IS_NULLABLE] = 'NO')

	BEGIN

		ALTER TABLE [dbo].[Peripheral] ALTER COLUMN [NetworkDeviceId] INT NULL;

	END

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
   WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'Peripheral' AND [COLUMN_NAME] = 'NetworkDeviceId' AND [COLUMN_DEFAULT] = '(0)')

	BEGIN

		ALTER TABLE [dbo].[Peripheral] DROP CONSTRAINT DF_Peripheral_NetworkDeviceID;

	END
		
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
   WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'Активное устройство' AND [COLUMN_NAME] = 'LogicalLocation' AND [IS_NULLABLE] = 'NO')

	BEGIN

		ALTER TABLE [dbo].[Активное устройство] ALTER COLUMN [LogicalLocation] NVARCHAR(250) NULL;

	END
		
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
   WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'Активное устройство' AND [COLUMN_NAME] = 'Description' AND [IS_NULLABLE] = 'NO')

	BEGIN

		ALTER TABLE [dbo].[Активное устройство] ALTER COLUMN [Description] NVARCHAR(250) NULL;

	END
		
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
   WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'Активное устройство' AND [COLUMN_NAME] = 'Identifier' AND [IS_NULLABLE] = 'NO')

	BEGIN

		ALTER TABLE [dbo].[Активное устройство] ALTER COLUMN [Identifier] NVARCHAR(50) NULL;

	END
		
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
   WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'Оконечное оборудование' AND [COLUMN_NAME] = 'LogicalLocation' AND [IS_NULLABLE] = 'NO')

	BEGIN

		ALTER TABLE [dbo].[Оконечное оборудование] ALTER COLUMN [LogicalLocation] NVARCHAR(250) NULL;

	END
		
if not exists(
    select top 1 0
    from sys.all_columns c
        join sys.tables t on t.object_id = c.object_id
        join sys.schemas s on s.schema_id = t.schema_id
        join sys.default_constraints d on c.default_object_id = d.object_id
    where t.name = 'Активное устройство'
        and c.name = 'LogicalLocation'
        and s.name = 'dbo')
    begin
        alter table [dbo].[Активное устройство]
            add constraint [DF_Активное устройство_LogicalLocation] default ('') FOR LogicalLocation;
    end

if not exists(
    select top 1 0
    from sys.all_columns c
        join sys.tables t on t.object_id = c.object_id
        join sys.schemas s on s.schema_id = t.schema_id
        join sys.default_constraints d on c.default_object_id = d.object_id
    where t.name = 'Активное устройство'
        and c.name = 'Description'
        and s.name = 'dbo')
    begin
        alter table [dbo].[Активное устройство]
            add constraint [DF_Активное устройство_Description] default ('') FOR Description;
    end

if not exists(
    select top 1 0
    from sys.all_columns c
        join sys.tables t on t.object_id = c.object_id
        join sys.schemas s on s.schema_id = t.schema_id
        join sys.default_constraints d on c.default_object_id = d.object_id
    where t.name = 'Активное устройство'
        and c.name = 'Identifier'
        and s.name = 'dbo')
    begin
        alter table [dbo].[Активное устройство]
            add constraint [DF_Активное устройство_Identifier] default ('') FOR Identifier;
    end

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
   WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'Оконечное оборудование' AND [COLUMN_NAME] = 'Description' AND [IS_NULLABLE] = 'NO')

	BEGIN

		ALTER TABLE [dbo].[Оконечное оборудование] ALTER COLUMN [Description] NVARCHAR(250) NULL;

	END
		
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
   WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'Оконечное оборудование' AND [COLUMN_NAME] = 'Identifier' AND [IS_NULLABLE] = 'NO')

	BEGIN

		ALTER TABLE [dbo].[Оконечное оборудование] ALTER COLUMN [Identifier] NVARCHAR(50) NULL;

	END