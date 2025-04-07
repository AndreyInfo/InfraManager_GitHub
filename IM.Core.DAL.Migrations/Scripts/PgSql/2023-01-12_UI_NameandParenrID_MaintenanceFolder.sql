CREATE UNIQUE INDEX 
    if not exists ui_name_parent_id_maintenance_folder
    on maintenance_folder(name, parent_id);