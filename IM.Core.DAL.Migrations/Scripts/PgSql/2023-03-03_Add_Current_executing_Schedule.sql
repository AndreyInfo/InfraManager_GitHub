DO $$
BEGIN
    alter table im.schedule_task ADD COLUMN IF NOT EXISTS current_executing_schedule_id uuid null;

    if not exists (SELECT 1 FROM pg_constraint WHERE conname = 'fk_schedule_task_current_executing_schedule_id')
    then
    ALTER TABLE im.schedule_task
        ADD CONSTRAINT fk_schedule_task_current_executing_schedule_id FOREIGN KEY (current_executing_schedule_id) references im.schedule(id);
    end if;
END;
$$