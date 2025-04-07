DO $$
    BEGIN
        alter table if exists im.positions rename to job_title;

        if exists (select * from information_schema.constraint_column_usage
                  where table_name = 'job_title' and constraint_name = 'pk_positions')
        then
            alter table if exists im.job_title rename constraint pk_positions to pk_job_title;
        end if;
    END
$$