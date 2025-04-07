CREATE SEQUENCE if not exists pk_active_equipment_id_seq;
ALTER SEQUENCE pk_active_equipment_id_seq OWNED BY active_equipment.identificator;

ALTER TABLE active_equipment
        ALTER COLUMN identificator SET DEFAULT nextval('pk_active_equipment_id_seq');
select  setval('pk_active_equipment_id_seq', mx.mx)
            FROM (SELECT MAX(identificator) + 1 AS mx FROM active_equipment) mx;

alter table active_equipment alter column connected set default 0;

alter table active_equipment alter column removed set default false;

alter table active_equipment alter column external_id set default '';

alter table active_equipment alter column cs_vendor_id set default 0;

alter table active_equipment alter column mb_vendor_id set default 0;

alter table active_equipment alter column room_id set default 0;

alter table active_equipment alter column im_obj_id set default gen_random_uuid();

alter table active_equipment alter column logical_location set default '';

alter table active_equipment alter column description set default '';

alter table active_equipment alter column identifier set default '';
