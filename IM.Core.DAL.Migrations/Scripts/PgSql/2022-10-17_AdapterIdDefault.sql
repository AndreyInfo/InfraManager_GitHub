DO $$
    BEGIN
        alter table adapter alter adapter_id set default gen_random_uuid();
    END
$$