CREATE OR REPLACE VIEW im.view_software_installation
 AS
 SELECT i.id,
    i.software_model_id,
    m.name AS software_model_name,
    m.version AS software_model_version,
    m.software_type_id,
    m.software_model_using_type_id,
    v.name AS manufacturer_name,
    ut.name AS software_model_using_type_name,
    t.name AS software_type_name,
    m.commercial_model_id,
    cm.name AS commercial_model_name,
    cm.version AS commercial_model_version,
    cm.software_type_id AS commercial_type_id,
    cm.software_model_using_type_id AS commercial_model_using_type_id,
    cv.name AS commercial_manufacturer_name,
    cut.name AS commercial_model_using_type_name,
    ct.name AS commercial_type_name,
    i.unique_number,
    i.install_date,
    i.install_path,
    i.device_id,
    i.device_class_id,
    COALESCE(device.name, ''::character varying) AS device_name,
    COALESCE(device.ownername, ''::character varying) AS device_owner_name,
    COALESCE(device.utilizer_name, ''::character varying) AS device_utilizer_name,
    COALESCE(device.organizationname, ''::character varying) AS device_organization_name,
    i.software_licence_id,
    l.name AS software_licence_name,
    sn.id AS software_licence_serial_number_id,
    l.software_licence_scheme,
    ls.name AS software_licence_scheme_name,
    sn.serial_number,
    i.software_execution_count,
    i.utc_date_last_detected,
    i.state,
    i.xmin
   FROM software_installation i
     JOIN software_model m ON m.id = i.software_model_id
     JOIN software_model_using_type ut ON ut.id = m.software_model_using_type_id
     JOIN software_type t ON t.id = m.software_type_id
     LEFT JOIN manufacturers v ON v.im_obj_id = m.manufacturer_id
     LEFT JOIN software_model cm ON cm.id = m.commercial_model_id
     LEFT JOIN software_model_using_type cut ON cut.id = cm.software_model_using_type_id
     LEFT JOIN software_type ct ON ct.id = cm.software_type_id
     LEFT JOIN manufacturers cv ON cv.im_obj_id = cm.manufacturer_id
     LEFT JOIN software_licence l ON l.id = i.software_licence_id
     LEFT JOIN software_licence_scheme ls ON ls.id = l.software_licence_scheme
     LEFT JOIN software_licence_serial_number sn ON sn.id = i.software_licence_serial_number_id
     LEFT JOIN ( SELECT d.id,
            d.class_id,
            COALESCE(d.name, ''::character varying) AS name,
            COALESCE(o.name, ''::character varying) AS ownername,
            COALESCE(u.name, ''::character varying) AS utilizer_name,
            COALESCE(org.name, ''::character varying) AS organizationname
           FROM ( SELECT td.identificator AS intid,
                    td.im_obj_id AS id,
                    td.name,
                    td.room_id,
                    6 AS class_id
                   FROM terminal_equipment td
                  WHERE td.removed = false
                UNION
                 SELECT nd.identificator AS intid,
                    nd.im_obj_id AS id,
                    nd.name,
                    nd.room_id,
                    5 AS class_id
                   FROM active_equipment nd
                  WHERE nd.removed = false
                UNION
                 SELECT ud.device_id AS intid,
                    ud.im_obj_id AS id,
                    ud.computer_name AS name,
                    0 AS room_id,
                    666 AS class_id
                   FROM undisposed_device ud) d
             LEFT JOIN asset a ON a.device_id = d.intid AND d.class_id <> 666
             LEFT JOIN view_owner o ON o.id = a.owner_id AND a.owner_class_id = o.class_id
             LEFT JOIN view_utilizer u ON u.id = a.utilizer_id AND a.utilizer_class_id = u.class_id
             LEFT JOIN room room ON room.identificator = d.room_id AND d.class_id <> 666
             LEFT JOIN floor fl ON fl.identificator = room.floor_id
             LEFT JOIN building b ON b.identificator = fl.building_id
             LEFT JOIN organization org ON org.identificator = b.organization_id) device ON device.id = i.device_id AND device.class_id = i.device_class_id;

