CREATE UNIQUE INDEX 
    if not exists ui_name_rfs_result on rfs_result(name)
    where removed = false;