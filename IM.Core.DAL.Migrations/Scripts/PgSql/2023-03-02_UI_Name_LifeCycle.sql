CREATE UNIQUE INDEX 
    if not exists ui_name_lifeCycle
    on life_cycle(name)
    where removed  = false;