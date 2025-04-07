CREATE UNIQUE INDEX 
    if not exists ui_name_incident_result on incident_result(name)
    where removed = false;