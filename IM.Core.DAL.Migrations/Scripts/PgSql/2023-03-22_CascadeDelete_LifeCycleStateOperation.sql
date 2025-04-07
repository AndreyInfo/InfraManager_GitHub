alter table life_cycle_state_operation_transition
drop constraint fk_life_cycle_state_operation_transition_life_cycle_state_opera,
add constraint fk_life_cycle_state_operation_transition_life_cycle_state_opera
   foreign key (life_cycle_state_operation_id)
   references life_cycle_state_operation(id)
   on delete cascade;


alter table life_cycle_state_operation_transition
drop constraint fk_life_cycle_state_operation_transition_life_cycle_state_finis,
add constraint fk_life_cycle_state_operation_transition_life_cycle_state_finis
   foreign key (finish_life_cycle_state_id)
   references life_cycle_state(id)
   on delete cascade;