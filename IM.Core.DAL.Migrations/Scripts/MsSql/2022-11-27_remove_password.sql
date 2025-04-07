--если элемент существует, то для проверки достаточно 1 значения
IF EXISTS (SELECT TOP 1 1 FROM sys.columns WHERE Name = N'password'
                                       AND Object_ID = Object_ID(N'dbo.UIADPath'))
    BEGIN
        alter table UIADPath drop column password
    end