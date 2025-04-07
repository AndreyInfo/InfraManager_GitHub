CREATE SEQUENCE if not exists pk_user_id_seq;
ALTER SEQUENCE pk_user_id_seq OWNED BY users.identificator;
ALTER TABLE users
        ALTER COLUMN identificator SET DEFAULT nextval('pk_user_id_seq');
select  setval('pk_user_id_seq', mx.mx)
            FROM (SELECT MAX(identificator) + 1 AS mx FROM users) mx;
alter table users alter column im_obj_id set default gen_random_uuid()
