DO $$
    BEGIN
        CREATE UNIQUE INDEX if not exists ui_user_activity_type_user on im.user_activity_type_reference(object_id, user_activity_type_id);
    END
$$