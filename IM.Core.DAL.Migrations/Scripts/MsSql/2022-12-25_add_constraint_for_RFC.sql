if object_id('[dbo].[DF_RFC_NUMBER]', 'C') is not null
alter table [dbo].[RFC] add constraint [DF_RFC_NUMBER] default (next value for [dbo].[RFCNumber]) for [Number]