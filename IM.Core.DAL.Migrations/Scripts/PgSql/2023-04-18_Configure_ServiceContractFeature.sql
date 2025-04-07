DO
$$
BEGIN
  IF NOT exists (select constraint_name 
                    from information_schema.table_constraints 
                    where table_name = 'service_contract_feature' and constraint_type = 'PRIMARY KEY') 
  then
    ALTER TABLE IF EXISTS im.service_contract_feature
        ADD PRIMARY KEY (product_catalog_type_id, feature);
  end if;
END $$;

alter table service_contract_feature
   drop constraint fk_service_contract_feature_product_catalog_type_id,
   add constraint fk_service_contract_feature_product_catalog_type_id
    foreign key (product_catalog_type_id)
    references product_catalog_type(id)
    on delete cascade;