DECLARE @tmp   nvarchar(max)
DECLARE @reset nvarchar(max)
DECLARE @state int =0;
DECLARE @errorname nvarchar(max)
if not exists(
    select 1
    from sys.objects c
    where object_id = OBJECT_ID(N'[dbo].[pk_active_equipment_types_seq]') and type = 'SO')        
    BEGIN
        create sequence pk_active_equipment_types_seq;
        SET @tmp = 'ALTER SEQUENCE pk_active_equipment_types_seq RESTART WITH <<const>>'
        SELECT @reset = (REPLACE(@tmp, '<<const>>', (SELECT MAX([Идентификатор]) + 1 from [dbo].[Активное устройство])))
        alter table [dbo].[Активное устройство]
            add constraint DF_active_equipment_types_default default next value for
                pk_active_equipment_types_seq for [Идентификатор]
        exec sp_executesql @reset
    END

if not exists(
    select top 1 0
    from sys.all_columns c
        join sys.tables t on t.object_id = c.object_id
        join sys.schemas s on s.schema_id = t.schema_id
        join sys.default_constraints d on c.default_object_id = d.object_id
    where t.name = 'Активное устройство'
        and c.name = 'Connected'
        and s.name = 'dbo')
    begin
        alter table [dbo].[Активное устройство]
            add constraint [DF_Активное устройство_Connected] default (0) FOR Connected;
    end

if not exists(
    select top 1 0
    from sys.all_columns c
        join sys.tables t on t.object_id = c.object_id
        join sys.schemas s on s.schema_id = t.schema_id
        join sys.default_constraints d on c.default_object_id = d.object_id
    where t.name = 'Активное устройство'
        and c.name = 'Removed'
        and s.name = 'dbo')
    begin
        alter table [dbo].[Активное устройство]
            add constraint [DF_Активное устройство_Removed] default (0) FOR Removed;
    end

if not exists(
    select top 1 0
    from sys.all_columns c
        join sys.tables t on t.object_id = c.object_id
        join sys.schemas s on s.schema_id = t.schema_id
        join sys.default_constraints d on c.default_object_id = d.object_id
    where t.name = 'Активное устройство'
        and c.name = 'ExternalID'
        and s.name = 'dbo')
    begin
        alter table [dbo].[Активное устройство]
            add constraint [DF_Активное устройство_ExternalID] default ('') FOR ExternalID;
    end

if not exists(
    select top 1 0
    from sys.all_columns c
        join sys.tables t on t.object_id = c.object_id
        join sys.schemas s on s.schema_id = t.schema_id
        join sys.default_constraints d on c.default_object_id = d.object_id
    where t.name = 'Активное устройство'
        and c.name = 'CsVendorID'
        and s.name = 'dbo')
    begin
        alter table [dbo].[Активное устройство]
            add constraint [DF_Активное устройство_CsVendorID] default (0) FOR CsVendorID;
    end

if not exists(
    select top 1 0
    from sys.all_columns c
        join sys.tables t on t.object_id = c.object_id
        join sys.schemas s on s.schema_id = t.schema_id
        join sys.default_constraints d on c.default_object_id = d.object_id
    where t.name = 'Активное устройство'
        and c.name = 'MbVendorID'
        and s.name = 'dbo')
    begin
        alter table [dbo].[Активное устройство]
            add constraint [DF_Активное устройство_MbVendorID] default (0) FOR MbVendorID;
    end

if not exists(
    select top 1 0
    from sys.all_columns c
        join sys.tables t on t.object_id = c.object_id
        join sys.schemas s on s.schema_id = t.schema_id
        join sys.default_constraints d on c.default_object_id = d.object_id
    where t.name = 'Активное устройство'
        and c.name = 'RoomID'
        and s.name = 'dbo')
    begin
        alter table [dbo].[Активное устройство]
            add constraint [DF_Активное устройство_RoomID] default (0) FOR RoomID;
    end

if not exists(
    select top 1 0
    from sys.all_columns c
        join sys.tables t on t.object_id = c.object_id
        join sys.schemas s on s.schema_id = t.schema_id
        join sys.default_constraints d on c.default_object_id = d.object_id
    where t.name = 'Активное устройство'
        and c.name = 'IMObjID'
        and s.name = 'dbo')
    begin
        alter table [dbo].[Активное устройство]
            add constraint [DF_Активное устройство_IMObjID] default NEWID() FOR IMObjID;
    end
