drop index ui_name_life_cycle_state_operation_by_life_cycle_state_id;

insert into im.life_cycle_state_operation (id, name, sequence, command_type, work_order_template_id, life_cycle_state_id, icon, icon_name)
select '00000000-0000-0000-0000-000000000001', name, sequence, command_type, work_order_template_id, life_cycle_state_id, icon, icon_name
from im.life_cycle_state_operation where id = '00000000-0000-0000-0000-000000000000';

update role_life_cycle_state_operation set life_cycle_state_operation_id = '00000000-0000-0000-0000-000000000001'
where life_cycle_state_operation_id = '00000000-0000-0000-0000-000000000000';

delete from life_cycle_state_operation where id = '00000000-0000-0000-0000-000000000000';

CREATE UNIQUE INDEX 
    if not exists ui_name_life_cycle_state_operation_by_life_cycle_state_id
    on life_cycle_state_operation(name, life_cycle_state_id);