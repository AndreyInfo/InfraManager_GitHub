DO $$
    BEGIN
        create unique index if not exists ui_ad_name_configuration on uiad_configuration(name);
    END
$$