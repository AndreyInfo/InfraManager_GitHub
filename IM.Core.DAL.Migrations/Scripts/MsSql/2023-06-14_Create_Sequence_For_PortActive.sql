IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActivePort_ID_Seq]') AND type = 'SO')
BEGIN 
    DECLARE @max int;
    SELECT @max = MAX([Идентификатор]) + 1
                  FROM [Порт активный]
				  
	if(@max is null)
	set @max = 1;

    EXEC('CREATE SEQUENCE ActivePort_ID_Seq 
          START WITH ' + @max +
         'INCREMENT BY 1;')

    ALTER TABLE [Порт активный]
        ADD CONSTRAINT DF_ActivePort_Generate_ID
        DEFAULT NEXT VALUE FOR ActivePort_ID_Seq FOR [Идентификатор]
END