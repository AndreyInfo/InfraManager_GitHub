alter table user_session add column if not exists location smallint default 2;
alter table user_session add column if not exists licence_type smallint default 1;
