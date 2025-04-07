if exists(SELECT * FROM sys.objects where name like '%DF_Peripheral_IntID%')
    ALTER TABLE peripheral DROP DF_Peripheral_IntID;

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PK_Peripheral_IntID_Seq]') AND type = 'SO')
BEGIN 
    DECLARE @max int;
    SELECT @max = MAX([IntID]) + 1
                  FROM [peripheral]
                  
	if(@max is null)
		set @max = 1;

    EXEC('CREATE SEQUENCE PK_Peripheral_IntID_Seq 
          START WITH ' + @max +
         'INCREMENT BY 1;')

    ALTER TABLE [peripheral]
        ADD CONSTRAINT DF_Peripheral_Generate_IntID
        DEFAULT NEXT VALUE FOR PK_Peripheral_IntID_Seq FOR [IntID]
END