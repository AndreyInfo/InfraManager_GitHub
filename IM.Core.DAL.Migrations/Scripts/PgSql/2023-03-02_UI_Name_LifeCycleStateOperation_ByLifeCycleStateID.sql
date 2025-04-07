CREATE UNIQUE INDEX 
    if not exists ui_name_life_cycle_state_operation_by_life_cycle_state_id
    on life_cycle_state_operation(name, life_cycle_state_id);