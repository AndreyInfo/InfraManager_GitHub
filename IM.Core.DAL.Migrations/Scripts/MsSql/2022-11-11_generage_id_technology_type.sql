  IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PK_TechnologyType_ID_Seq]') AND type = 'SO')
BEGIN 
	DECLARE @max int;
	SELECT @max = MAX([Идентификатор]) + 1
				  FROM [Виды технологий]

	EXEC('CREATE SEQUENCE PK_TechnologyType_ID_Seq 
		  START WITH ' + @max +
		 'INCREMENT BY 1;')

	ALTER TABLE [Виды технологий]
		ADD CONSTRAINT DF_TechnologyType_Generate_ID
		DEFAULT NEXT VALUE FOR PK_TechnologyType_ID_Seq FOR [Идентификатор]
END
