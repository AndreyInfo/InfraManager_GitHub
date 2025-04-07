DO $$
    BEGIN
        ALTER TABLE im.uiad_class ALTER COLUMN id SET DEFAULT gen_random_uuid();
        ALTER TABLE im.uiad_configuration ALTER COLUMN id SET DEFAULT gen_random_uuid();
        ALTER TABLE im.uiad_path ALTER COLUMN id SET DEFAULT gen_random_uuid();
        ALTER TABLE im.uiad_setting ALTER COLUMN id SET DEFAULT gen_random_uuid();
    END;
$$

