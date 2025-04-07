DO $$
    BEGIN
        CREATE UNIQUE INDEX if not exists ui_building_name_into_organization on im.building(name, organization_id);
        
        CREATE UNIQUE INDEX if not exists ui_floor_name_into_building on im.floor(name, building_id);
        
        CREATE UNIQUE INDEX if not exists ui_room_name_into_floor on im.room(name, floor_id);
    END
$$