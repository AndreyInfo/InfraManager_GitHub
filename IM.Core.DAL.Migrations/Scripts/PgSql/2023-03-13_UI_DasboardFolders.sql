CREATE UNIQUE INDEX 
    if not exists ui_dashboard_folder_name_by_folder_id
    on dashboard_folder(name, parent_dashboard_folder_id);
    
CREATE UNIQUE INDEX 
    if not exists ui_dashboard_folder_name_parent_is_null
    on dashboard_folder(name)
    where  parent_dashboard_folder_id is null;