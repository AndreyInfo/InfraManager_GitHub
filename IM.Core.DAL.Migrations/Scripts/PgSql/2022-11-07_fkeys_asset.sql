DO $$
BEGIN

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_asset_service_center') THEN
        ALTER TABLE im.asset
            ADD CONSTRAINT fk_asset_service_center
            FOREIGN KEY (service_center_id)
            REFERENCES im.supplier (supplier_id)
            NOT VALID;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_asset_service_contract') THEN
        ALTER TABLE im.asset
            ADD CONSTRAINT fk_asset_service_contract
            FOREIGN KEY (service_contract_id)
            REFERENCES im.service_contract (service_contract_id)
            NOT VALID;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_asset_supplier') THEN
        ALTER TABLE im.asset
            ADD CONSTRAINT fk_asset_supplier
            FOREIGN KEY (supplier_id)
            REFERENCES im.supplier (supplier_id)
            NOT VALID;
    END IF;

END;
$$;