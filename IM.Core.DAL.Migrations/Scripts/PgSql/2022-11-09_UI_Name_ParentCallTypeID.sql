CREATE UNIQUE INDEX if not exists ui_name_parent_call_type_id on call_type(name, parent_call_type_id) where Removed = false;
