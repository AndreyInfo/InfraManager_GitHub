DO $$
BEGIN
    INSERT INTO im.role_operation (operation_id, role_id)
    SELECT t.id, 'e13e5209-83c4-4046-904f-7e8f1f941c33'
    FROM im.operation t
    LEFT JOIN im.role_operation x ON x.operation_id = t.id AND x.role_id = 'e13e5209-83c4-4046-904f-7e8f1f941c33'
    WHERE (t.id = 799) AND (x.operation_id IS NULL);
END
$$