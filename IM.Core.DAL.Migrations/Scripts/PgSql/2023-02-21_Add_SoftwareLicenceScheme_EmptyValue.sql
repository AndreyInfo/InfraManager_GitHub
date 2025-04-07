DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM im.software_licence_scheme WHERE id = '00000000-0000-0000-0000-000000000001') THEN
        INSERT INTO im.software_licence_scheme (id, name, description, scheme_type, licensing_object_type, is_link_license_to_object, is_license_all_hosts, is_link_license_to_user, is_allow_install_on_vm, is_licence_by_access, increase_for_vm, is_deleted, created_date, updated_date, is_can_have_sub_licence, compatibility_type_id) VALUES
            ('00000000-0000-0000-0000-000000000001', '-', 'Отсутствие лицензии', 1, 4, false, false, false, false, false, 0, false, '2023-02-21 13:00:00', '2023-02-21 13:00:00', false, NULL);
    END IF;
END $$;