ALTER TABLE IF EXISTS im.product_catalog_type
    ADD COLUMN IF NOT EXISTS form_id uuid;
	
DO
$$
BEGIN
    if not exists (SELECT 1 FROM information_schema.table_constraints WHERE constraint_name='product_catalog_type_form_fk' 
                   AND table_name='product_catalog_type')
    THEN
        ALTER TABLE IF EXISTS product_catalog_type
            ADD CONSTRAINT product_catalog_type_form_fk 
			FOREIGN KEY (form_id)
            REFERENCES form_builder_form (id)
            ON DELETE SET NULL
            ON Update no action;
    end if;
end
$$