IF EXISTS(SELECT 1
          FROM INFORMATION_SCHEMA.TABLES t
          WHERE t.TABLE_SCHEMA = 'dbo'
            AND t.TABLE_NAME = 'MassiveIncidentWorkOrder')
BEGIN
    DROP TABLE dbo.MassiveIncidentWorkOrder;
END
GO