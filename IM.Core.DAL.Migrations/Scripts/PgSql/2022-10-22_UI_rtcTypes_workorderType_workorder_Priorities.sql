CREATE UNIQUE INDEX if not exists ui_work_order_type_name on work_order_type(name) where Removed = false;
CREATE UNIQUE INDEX if not exists ui_work_order_priority_name on work_order_priority(name) where Removed = false;
CREATE UNIQUE INDEX if not exists ui_rfc_type_name on rfc_type(name) where Removed = false;