if not exists(SELECT 1 FROM sys.indexes
  WHERE name='UC_Knowledge_Base_Classifier_Name' AND object_id = OBJECT_ID('[dbo].[KBArticleFolder]'))
begin
	alter table [dbo].[KBArticleFolder] add constraint UC_Knowledge_Base_Classifier_Name UNIQUE(name);
end;


IF (OBJECT_ID('dbo.FK_Knowledge_Base_Classifier_Expert', 'F') IS NULL)
begin 
	 ALTER TABLE [dbo].[KBArticleFolder]
          ADD ExpertID integer DEFAULT 1 not null;

     ALTER TABLE [dbo].[KBArticleFolder]
          ADD UpdatePeriod integer DEFAULT 0 NOT NULL;

     ALTER TABLE [dbo].[KBArticleFolder]
    	  ADD CONSTRAINT FK_Knowledge_Base_Classifier_Expert FOREIGN KEY (ExpertID) REFERENCES dbo.Пользователи (Идентификатор);
end

