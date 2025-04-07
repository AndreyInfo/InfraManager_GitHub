DO $$
    BEGIN
        create unique index if not exists ui_ad_path_settings_Path on uiad_path(ad_setting_id, path);
    END
$$