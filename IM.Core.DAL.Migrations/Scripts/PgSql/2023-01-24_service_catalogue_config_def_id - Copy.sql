ALTER TABLE IF EXISTS im.service_catalogue_import_csv_configuration
    ALTER COLUMN id SET DEFAULT gen_random_uuid();