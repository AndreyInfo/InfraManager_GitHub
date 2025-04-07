CREATE UNIQUE INDEX 
    if not exists ui_name_life_cycle_state_by_life_cycle_id
    on life_cycle_state(name, life_cycle_id);