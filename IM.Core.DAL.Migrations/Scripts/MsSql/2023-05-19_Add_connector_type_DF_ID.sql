DECLARE @tmp   nvarchar(max)
DECLARE @reset nvarchar(max)
DECLARE @state int =0;
DECLARE @errorname nvarchar(max)

if not exists(
    select 1
    from sys.objects c
    where object_id = OBJECT_ID(N'[dbo].[pk_connector_type_seq]') and type = 'SO')        
    BEGIN
        create sequence pk_connector_type_seq;
        SET @tmp = 'ALTER SEQUENCE pk_connector_type_seq RESTART WITH <<const>>'
        SELECT @reset = (REPLACE(@tmp, '<<const>>', (SELECT MAX([Идентификатор]) + 1 from [dbo].[Виды разъемов])))
        alter table [dbo].[Виды разъемов]
            add constraint DF_connector_type_default default next value for
                pk_connector_type_seq for [Идентификатор]
        exec sp_executesql @reset
    END