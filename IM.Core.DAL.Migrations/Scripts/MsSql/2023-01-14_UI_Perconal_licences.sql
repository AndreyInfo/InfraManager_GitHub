if EXISTS (SELECT 1
					FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS C
					WHERE C.TABLE_NAME = 'UserPersonalLicence'
						AND C.CONSTRAINT_SCHEMA = 'dbo' 
						AND C.CONSTRAINT_NAME = 'PK_UserPersonalSession')
BEGIN 
	alter table UserPersonalLicence drop constraint PK_UserPersonalSession;
END 
--
IF COL_LENGTH('UserPersonalLicence','ID') IS NULL
BEGIN
	ALTER TABLE UserPersonalLicence
		ADD ID int identity(1,1);

	alter table UserPersonalLicence add constraint PK_UserPersonalLicence primary key clustered ([ID]);
END
--
if (NOT EXISTS(SELECT 1 FROM sys.indexes where name = 'UI_UserPersonalLicences_UserID' and object_id = OBJECT_ID('[UserPersonalLicence]')))
BEGIN
  CREATE UNIQUE INDEX UI_UserPersonalLicences_UserID on dbo.UserPersonalLicence ([UserID]);
end;
