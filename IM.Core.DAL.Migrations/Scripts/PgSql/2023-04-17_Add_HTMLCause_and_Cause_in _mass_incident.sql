ALTER TABLE IF EXISTS im.mass_incident
    ADD COLUMN IF NOT EXISTS cause_plain character varying(1000);
ALTER TABLE IF EXISTS im.mass_incident
    ADD COLUMN IF NOT EXISTS cause character varying(4000);
