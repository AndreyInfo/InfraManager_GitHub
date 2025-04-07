IF NOT EXISTS(SELECT 1
              FROM information_schema.table_constraints t
              WHERE t.table_schema = 'dbo'
                AND t.table_name = 'Подразделение'
                AND t.constraint_type = 'UNIQUE'
                AND t.constraint_name = 'UQ_DepartmentName')
BEGIN
    CREATE UNIQUE INDEX UQ_DepartmentName on  dbo."Подразделение"("Идентификатор", "ИД организации", "ИД подразделения");
END
GO