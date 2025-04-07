
IF (NOT EXISTS (SELECT *
                FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_SCHEMA = 'dbo'
                  AND  TABLE_NAME = 'UIDBConfigurations'))
    BEGIN

        create table UIDBConfigurations
        (
            ID uniqueidentifier primary key default NEWID(),
            Name nvarchar(255) not null unique,
            Note nvarchar(500) not null,
            OrganizationTableName nvarchar(50),
            SubdivizionTableName nvarchar(50),
            UserTableName nvarchar(50)
        );
    END

IF (NOT EXISTS (SELECT *
                FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_SCHEMA = 'dbo'
                  AND  TABLE_NAME = 'UIDBSettings'))
    BEGIN

        create table UIDBSettings
        (
            ID uniqueidentifier primary key default NEWID(),
            DBConfigurationID uniqueidentifier references UIDBConfigurations(ID),
            DatabaseName nvarchar(50)
        );
    END

IF (NOT EXISTS (SELECT *
                FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_SCHEMA = 'dbo'
                  AND  TABLE_NAME = 'UIDBFields'))
    BEGIN
        create table UIDBFields
        (
            ID uniqueidentifier primary key default  NEWID(),
            ConfigurationID uniqueidentifier not null references UIDBConfigurations(ID) on delete cascade on update cascade,
            FieldID bigint not null,
            Value varchar(1024)
        );

    END

IF (NOT EXISTS (SELECT *
                FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_SCHEMA = 'dbo'
                  AND  TABLE_NAME = 'UIDBConnectionString'))
    BEGIN
        create table UIDBConnectionString
        (
            ID uniqueidentifier primary key default  NEWID(),
            UIDBSettingsID uniqueidentifier not null references UIDBSettings(ID),
            ConnectionString varchar(1024) not null,
            ImportSourceType int not null,
        );
        CREATE UNIQUE INDEX UX_CONNECTIONSTRING_SETTINGS ON UIDBConnectionString(UIDBSettingsID,ConnectionString,ImportSourceType)
    END