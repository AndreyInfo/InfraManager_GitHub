DROP INDEX IF EXISTS pct_name_parent;

CREATE UNIQUE INDEX 
    if not exists ui_name_product_catalog_type_category_id
    on product_catalog_type(name, product_catalog_category_id)
    where removed = false;