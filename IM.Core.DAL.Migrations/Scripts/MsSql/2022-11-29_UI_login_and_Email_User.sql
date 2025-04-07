if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Login_Users' and object_id = OBJECT_ID('[dbo].[Пользователи]')))
        BEGIN
            CREATE UNIQUE INDEX UI_Login_Users on [dbo].[Пользователи] ([Login name]) 
				where [Login name] <> ''
					  and [Login name] IS NOT NULL;
        end;

if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Email_Users' and object_id = OBJECT_ID('[dbo].[Пользователи]')))
        BEGIN
            CREATE UNIQUE INDEX UI_Email_Users on [dbo].[Пользователи] (Email) 
				where email <> ''
					  and email IS NOT NULL;
        end;