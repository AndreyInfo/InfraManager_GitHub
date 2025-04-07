IF OBJECT_ID ('dbo.Tag', 'U') IS NULL
BEGIN  
  CREATE TABLE [dbo].[Tag](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name][nvarchar](50) NOT NULL,
	CONSTRAINT [PK_Tag_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
	);  

CREATE UNIQUE INDEX [UniqueIndex_Tag_Name] ON [dbo].[Tag] ([Name]);
END
GO

IF OBJECT_ID ('dbo.UserAccount', 'U') IS NULL
BEGIN  
  CREATE TABLE [dbo].[UserAccount](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name][nvarchar](50) NOT NULL,
	[Type][int] NOT NULL,
	[Login][nvarchar](50) NULL,
	[Password][nvarchar](500) NULL,
	[SSH_Passphrase][nvarchar](500) NULL,
	[SSH_PrivateKey][nvarchar](500) NULL,
	[AuthenticationProtocol][int] NOT NULL,
	[AuthenticationKey][nvarchar](500) NULL,
	[PrivacyProtocol][int] NOT NULL,
	[PrivacyKey][nvarchar](500) NULL,
	[Removed] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[RemovedDate] [datetime] NULL,
	[RowVersion] [timestamp] NOT NULL,
	CONSTRAINT [PK_UserAccount_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
	);  

CREATE INDEX [Index_UserAccount_Name] ON [dbo].[UserAccount] ([Name]);
END
GO

IF OBJECT_ID ('dbo.UserAccountTag', 'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[UserAccountTag](
	[UserAccountID] [int] NOT NULL,
	[TagID][int] NOT NULL,	
	CONSTRAINT [PK_UserAccountTag] PRIMARY KEY ([UserAccountID], [TagID]),
	CONSTRAINT [FK_UserAccountTag_UserAccount_ID] FOREIGN KEY([UserAccountID]) REFERENCES [UserAccount]([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserAccountTag_Tag_ID] FOREIGN KEY([TagID]) REFERENCES [Tag]([ID]) ON DELETE CASCADE,
);
END
GO