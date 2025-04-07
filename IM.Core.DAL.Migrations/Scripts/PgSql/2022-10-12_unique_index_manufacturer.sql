DO $$
    BEGIN
        update adapter_type set vendor_id=0 where vendor_id=2000005;
        update peripheral_type set vendor_id=0 where vendor_id=2000005;
        update active_equipment_types set manufacturer_id = 0 where manufacturer_id=2000005;
        update terminal_equipment_types set manufacturer_id=0 where manufacturer_id=2000005;
        delete from manufacturers where identificator = 2000005;
        update manufacturers set name = N'не определен' where identificator=2000005;
        CREATE UNIQUE INDEX if not exists ui_name on manufacturers(name);
    END
 $$
