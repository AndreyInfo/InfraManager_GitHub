IF EXISTS(SELECT TOP 1 1 --достаточно 1 значения
          FROM sys.columns
          WHERE Name = N'Removed'
            AND Object_ID = Object_ID(N'dbo.UISettings'))
    BEGIN
        alter table UISetting
            add Removed bit not null DEFAULT 0
    END

IF EXISTS(SELECT TOP 1 1 --достаточно 1 значения
          FROM sys.columns
          WHERE Name = N'Removed'
            AND Object_ID = Object_ID(N'dbo.UIDBSettings'))
    BEGIN
        alter table UIDBSetting
            add Removed bit not null DEFAULT 0
    END
