if exists(SELECT * FROM sys.objects where name like '%DF_Adapter_IntID%')
	ALTER TABLE adapter DROP DF_Adapter_IntID;

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PK_Adapter_IntID_Seq]') AND type = 'SO')
BEGIN 
    DECLARE @max int;
    SELECT @max = MAX([IntID]) + 1 FROM [adapter]

	if(@max is null)
		set @max = 1;

    EXEC('CREATE SEQUENCE PK_Adapter_IntID_Seq 
          START WITH ' + @max +
         'INCREMENT BY 1;')

    ALTER TABLE [adapter]
        ADD CONSTRAINT DF_Adapter_Generate_IntID
        DEFAULT NEXT VALUE FOR PK_Adapter_IntID_Seq FOR [IntID]
END