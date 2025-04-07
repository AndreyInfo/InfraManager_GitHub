ALTER TABLE IF EXISTS im.product_catalog_type DROP COLUMN IF EXISTS icon_id;

ALTER TABLE IF EXISTS im.product_catalog_type
    ADD COLUMN icon_name character varying(50);
ALTER TABLE IF EXISTS im.product_catalog_type DROP CONSTRAINT IF EXISTS product_catalog_type_icon_id_fkey;