alter table ui_setting add column if not exists removed bool not null default False;

alter table uidb_settings add column if not exists removed bool not null default False;