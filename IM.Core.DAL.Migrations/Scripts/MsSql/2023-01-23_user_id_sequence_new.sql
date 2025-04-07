DECLARE @take INT = (select top 1 Идентификатор + 1 from dbo.Пользователи order by Идентификатор desc);

DECLARE @resetSQL nvarchar(255) = 'ALTER SEQUENCE dbo.pk_users_seq RESTART WITH ' + CONVERT(varchar, @take);

exec sp_executesql @resetSQL;
