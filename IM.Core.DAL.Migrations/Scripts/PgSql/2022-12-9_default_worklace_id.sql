CREATE SEQUENCE if not exists pk_workplace_id_seq;
ALTER SEQUENCE pk_workplace_id_seq OWNED BY workplace.identificator;
ALTER TABLE workplace
        ALTER COLUMN identificator SET DEFAULT nextval('pk_workplace_id_seq');
select  setval('pk_workplace_id_seq', mx.mx)
            FROM (SELECT MAX(identificator) + 1 AS mx FROM workplace) mx;
alter table workplace alter column im_obj_id set default gen_random_uuid()
