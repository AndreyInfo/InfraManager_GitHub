CREATE UNIQUE INDEX 
    if not exists ui_name_folder_id_maintenance
    on maintenance(name, maintenance_folder_id);