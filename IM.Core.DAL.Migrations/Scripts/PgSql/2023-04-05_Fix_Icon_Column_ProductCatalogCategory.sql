ALTER TABLE IF EXISTS im.product_catalog_category DROP COLUMN IF EXISTS icon_id;

ALTER TABLE IF EXISTS im.product_catalog_category
    ADD COLUMN icon_name character varying(50);
ALTER TABLE IF EXISTS im.product_catalog_category DROP CONSTRAINT IF EXISTS product_catalog_category_icon_id_fkey;