do $$
begin

alter table im.document_reference add column if not exists object_class_id int NULL;

update im.document_reference set object_class_id = 701 where object_id in (select id from im.call);
update im.document_reference set object_class_id = 702 where object_id in (select id from im.problem);
update im.document_reference set object_class_id = 703 where object_id in (select id from im.rfc);
update im.document_reference set object_class_id = 119 where object_id in (select id from im.work_order);
update im.document_reference set object_class_id = 823 where object_id in (select im_obj_id from im.mass_incident);

end
$$
