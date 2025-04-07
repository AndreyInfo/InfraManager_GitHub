ALTER TABLE IF EXISTS im.service_contract_type_agreement
    DROP CONSTRAINT 
    IF EXISTS fk_service_contract_type_agreement_agreement_life_cycle_id;

ALTER TABLE IF EXISTS im.service_contract_type_agreement
    ADD CONSTRAINT fk_service_contract_type_agreement_agreement_life_cycle_id
    FOREIGN KEY (agreement_life_cycle_id)
    REFERENCES im.life_cycle (id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE CASCADE;

ALTER TABLE IF EXISTS im.service_contract_type_agreement
    DROP CONSTRAINT 
    IF EXISTS fk_service_contract_type_agreement_product_catalog_type_id;
ALTER TABLE IF EXISTS im.service_contract_type_agreement
    ADD CONSTRAINT fk_service_contract_type_agreement_product_catalog_type_id 
        FOREIGN KEY (product_catalog_type_id)
        REFERENCES im.product_catalog_type (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE;

DELETE FROM im.service_contract_type_agreement
where agreement_life_cycle_id IS NULL;
ALTER TABLE IF EXISTS im.service_contract_type_agreement
    ALTER COLUMN agreement_life_cycle_id SET NOT NULL;

DO $$
BEGIN
  if NOT exists (select 1 
                from information_schema.table_constraints 
                where table_name = 'service_contract_type_agreement' and constraint_type = 'PRIMARY KEY') then
    ALTER TABLE IF EXISTS im.service_contract_type_agreement
        ADD CONSTRAINT pk_service_contract_type_agreement 
        PRIMARY KEY (product_catalog_type_id, agreement_life_cycle_id);
end if;
END $$;

