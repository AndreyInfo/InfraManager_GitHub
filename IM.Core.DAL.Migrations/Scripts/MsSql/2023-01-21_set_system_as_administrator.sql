﻿IF NOT EXISTS (select * from UserRole
                where UserId = '00000000-0000-0000-0000-000000000000'
                and RoleId = '00000000-0000-0000-0000-000000000001')
BEGIN
    insert into UserRole (RoleID, UserID)
    values ('00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000000');
END