DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_rfc_urgency') THEN
        ALTER TABLE im.rfc
            ADD CONSTRAINT fk_rfc_urgency
            FOREIGN KEY (urgency_id)
            REFERENCES im.urgency (id)
            NOT VALID;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_rfc_influence') THEN
        ALTER TABLE im.rfc
            ADD CONSTRAINT fk_rfc_influence
            FOREIGN KEY (influence_id)
            REFERENCES im.influence (id)
            NOT VALID;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_rfc_owner') THEN
        ALTER TABLE im.rfc
            ADD CONSTRAINT fk_rfc_owner
            FOREIGN KEY (owner_id)
            REFERENCES im.users (im_obj_id)
            NOT VALID;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_rfc_initiator') THEN
        ALTER TABLE im.rfc
            ADD CONSTRAINT fk_rfc_initiator
            FOREIGN KEY (initiator_id)
            REFERENCES im.users (im_obj_id)
            NOT VALID;
    END IF;
END;
$$;