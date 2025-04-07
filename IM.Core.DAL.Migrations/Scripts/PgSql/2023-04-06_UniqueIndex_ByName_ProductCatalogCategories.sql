DROP INDEX IF EXISTS pct_name_parent;

CREATE UNIQUE INDEX 
    if not exists ui_name_product_catalog_category_parent_null
    on product_catalog_category(name)
    where removed = false 
        AND parent_product_catalog_category_id is null;
        
        
CREATE UNIQUE INDEX 
    if not exists ui_name_product_catalog_category_parent_id
    on product_catalog_category(name, parent_product_catalog_category_id)
    where removed = false
        and parent_product_catalog_category_id is not null;      