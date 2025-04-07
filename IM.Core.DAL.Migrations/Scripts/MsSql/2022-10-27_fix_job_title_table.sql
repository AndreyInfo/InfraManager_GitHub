if not exists(select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where [TABLE_NAME] = 'Должности' and [CONSTRAINT_NAME] = 'PK_JobTitle')
begin
    
    if 'FK_Пользователи_Должности' in (select [CONSTRAINT_NAME] from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where [TABLE_NAME] = 'Пользователи' and [CONSTRAINT_TYPE] = 'FOREIGN KEY')
	begin
        alter table [dbo].[Пользователи] drop constraint [FK_Пользователи_Должности]
    end

    if 'TempJobTitle' not in (select [TABLE_NAME] from INFORMATION_SCHEMA.TABLES where [TABLE_SCHEMA] = 'dbo')
    begin
        create table [dbo].[TempJobTitle] (
        [Идентификатор] [int] NOT NULL identity(1,1),
        [Название] [nvarchar](255) NOT NULL,
        [ComplementaryID] [int] NULL,
        [IMObjID] [uniqueidentifier] NOT NULL DEFAULT(NEWID()),
        constraint PK_JobTitle primary key clustered ([Идентификатор]),
        constraint UK_JobTitle_IMObjID unique ([IMObjID]),
        constraint UK_JobTitle_Name unique ([Название]))
    end

    SET IDENTITY_INSERT [dbo].[TempJobTitle] ON

    if 'Должности' in (select [TABLE_NAME] from INFORMATION_SCHEMA.TABLES where [TABLE_SCHEMA] = 'dbo')
    begin
        delete from [dbo].[TempJobTitle]

        insert into [dbo].[TempJobTitle] ([Идентификатор], [Название], [ComplementaryID], [IMObjID])
        select [Идентификатор], ISNULL([Название], ''), [ComplementaryID], ISNULL([IMObjID], NEWID())
        from (select [Идентификатор], [Название], [ComplementaryID], [IMObjID], ROW_NUMBER() over (partition by [Название] order by [Идентификатор]) as rnum
        from 
		[dbo].[Должности]) t
		where rnum=1

		update [dbo].[Пользователи] set [ИД должности] = tmp.[Идентификатор]
		from [dbo].[Пользователи] u
			join [dbo].[Должности] d on u.[ИД должности] = d.Идентификатор
			join [dbo].[TempJobTitle] tmp on tmp.Название = d.Название and tmp.Идентификатор!=d.Идентификатор

        drop table [dbo].[Должности]
    end

    if 'TempJobTitle' in (select [TABLE_NAME] from INFORMATION_SCHEMA.TABLES where [TABLE_SCHEMA] = 'dbo')
    begin
        EXEC sp_rename 'dbo.TempJobTitle', 'Должности';
    end

    if 'FK_Пользователи_Должности' not in (select [CONSTRAINT_NAME] from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where [TABLE_NAME] = 'Пользователи' and [CONSTRAINT_TYPE] = 'FOREIGN KEY')
    begin
        alter table [dbo].[Пользователи]
        add constraint [FK_Пользователи_Должности]
        foreign key ([ИД должности])
        references [dbo].[Должности] ([Идентификатор])
    end
end

go