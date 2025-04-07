ALTER TABLE im.building
    ALTER COLUMN note TYPE character varying(1000) COLLATE pg_catalog."default";

ALTER TABLE im.floor
    ALTER COLUMN note TYPE character varying(1000) COLLATE pg_catalog."default";

ALTER TABLE im.room
    ALTER COLUMN note TYPE character varying(1000) COLLATE pg_catalog."default";

ALTER TABLE im.workplace
    ALTER COLUMN note TYPE character varying(1000) COLLATE pg_catalog."default";

