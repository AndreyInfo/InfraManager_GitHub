if OBJECT_ID('dbo.DF_KBARTICLE_NUMBER', 'D') IS NULL
	alter table [dbo].[KBArticle] add constraint DF_KBARTICLE_NUMBER default NEXT VALUE FOR [dbo].[KBArticleNumber]	for [Number]