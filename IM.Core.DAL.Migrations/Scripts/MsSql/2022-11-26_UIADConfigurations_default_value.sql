if (NOT EXISTS(select * from sys.default_constraints where name = 'DF_UIADCOnfigurationID'))
begin 
    ALTER TABLE dbo.UIADConfiguration ADD CONSTRAINT DF_UIADCOnfigurationID DEFAULT newid() FOR ID;
end;

if (NOT EXISTS(select * from sys.default_constraints where name = 'DF_UIADClass'))
    begin
        ALTER TABLE dbo.UIADClass ADD CONSTRAINT DF_UIADClass DEFAULT newid() FOR ID;
    end;

if (NOT EXISTS(select * from sys.default_constraints where name = 'DF_UIADPathID'))
    begin
        ALTER TABLE dbo.UIADPath ADD CONSTRAINT DF_UIADPathID DEFAULT newid() FOR ID;
    end;
if (NOT EXISTS(select * from sys.default_constraints where name = 'DF_UIADSettingID'))
    begin
        ALTER TABLE dbo.UIADSetting ADD CONSTRAINT DF_UIADSettingID DEFAULT newid() FOR ID;
    end;