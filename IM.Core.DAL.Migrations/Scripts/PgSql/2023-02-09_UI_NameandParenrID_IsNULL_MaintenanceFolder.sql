CREATE UNIQUE INDEX 
    if not exists ui_name_parent_id_is_null_maintenance_folder
    on maintenance_folder(name)
    where parent_id is null;