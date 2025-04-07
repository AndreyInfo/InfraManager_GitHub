CREATE OR REPLACE FUNCTION im.func_item_in_organization_item(
    _organizationItemClassID integer,
    _organizationItemID uuid,
    _itemClassID integer,
    _itemID uuid
)
RETURNS boolean
LANGUAGE plpgsql
AS
$$
DECLARE
    _tmpID uuid;
BEGIN
    IF _organizationItemClassID IS NULL OR _organizationItemID IS NULL OR _itemClassID IS NULL OR _itemID IS NULL THEN
        RETURN FALSE;
    END IF;
    
    IF _itemClassID = 9 THEN -- User
        RETURN im.func_user_in_organization_item(
            _organizationitemclassid := _organizationItemClassID,
            _organizationitemid := _organizationItemID,
            _userid := _itemID);
    END IF;
    
    IF _itemClassID = 102 THEN -- SubDivision
        IF _organizationItemClassID = 102 THEN
            IF _organizationItemID = _itemID THEN
                RETURN TRUE;
            END IF;
            
            _tmpID := _itemID;
            
            WHILE (NOT _tmpID IS NULL) LOOP
                IF _tmpID = _organizationItemID THEN
                    RETURN TRUE;
                END IF;
                SELECT INTO _tmpID d.department_id
                FROM department d
                WHERE d.identificator = _tmpID; 
            END LOOP;
        END IF;
        
        IF _organizationItemClassID = 101 THEN
            RETURN EXISTS(
                SELECT 1 FROM department d
                WHERE d.identificator = _itemID AND d.organization_id = _organizationItemID);
        END IF;

    ELSEIF _itemClassID = 101 THEN -- Organization
        IF _organizationItemID = _itemID THEN
            RETURN TRUE;
        END IF;

    ELSEIF _itemClassID = 722 THEN -- Group (Queue)
        IF _organizationItemClassID = 722 THEN
            IF _organizationItemID = _itemID THEN
                RETURN TRUE;
            END IF;

            IF _organizationItemClassID = 9 THEN
                RETURN EXISTS(
                    SELECT 1 FROM queue q
                    JOIN queue_user qu on q.id = qu.queue_id
                    WHERE q.ID = _itemID AND qu.user_id = _organizationItemID);
            END IF;
        END IF;
    END IF;
    
    RETURN FALSE;
END;
$$;