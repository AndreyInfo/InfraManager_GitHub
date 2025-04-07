DO
$$
BEGIN

-- Change Primary Key in technical_failures_category from integer to uuid
IF (NOT EXISTS (SELECT FROM information_schema.columns
WHERE table_name = 'technical_failures_category' AND column_name = 'im_obj_id')) THEN

ALTER TABLE im.technical_failures_category ADD COLUMN im_obj_id uuid default(gen_random_uuid()) not null;
ALTER TABLE im.technical_failures_category ADD CONSTRAINT uk_technical_failures_category_im_obj_id UNIQUE (im_obj_id);

END IF;

-- Change Primary Key in massive_incident_cause from integer to uuid
IF (NOT EXISTS (SELECT FROM information_schema.columns
WHERE table_name = 'massive_incident_cause' AND column_name = 'im_obj_id')) THEN

ALTER TABLE im.massive_incident_cause ADD COLUMN im_obj_id uuid default(gen_random_uuid()) not null;
ALTER TABLE im.massive_incident_cause ADD CONSTRAINT uk_massive_incident_cause_im_obj_id UNIQUE (im_obj_id);

END IF;

END
$$;