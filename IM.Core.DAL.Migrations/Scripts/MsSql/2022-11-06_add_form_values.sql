SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
if NOT EXISTS (SELECT 1
                   FROM   INFORMATION_SCHEMA.TABLES
                   WHERE  TABLE_NAME = 'WorkflowActivityForm'
                          AND TABLE_SCHEMA = 'dbo')
BEGIN

	CREATE TABLE [dbo].[WorkflowActivityForm](
		[ID] [uniqueidentifier] NOT NULL,
		[FieldsIsRequired] [bit] NOT NULL,
		[Identifier] [nvarchar](500) NOT NULL,
		[ClassID] [int] NOT NULL,
		[Name] [nvarchar](500) NOT NULL,
		[Width] [int] NOT NULL,
		[Height] [int] NOT NULL,
		[RowVersion] [timestamp] NOT NULL,
		[ObjectID] [uniqueidentifier] NOT NULL,
		[MinorVersion] [int] NOT NULL,
		[MajorVersion] [int] NOT NULL,
		[Description] [nvarchar](max) NULL,
		[Status] [int] NOT NULL,
		[LastIndex] [float] NOT NULL,
		[UtcChanged] [datetime] NOT NULL,
		[ProductTypeID] [uniqueidentifier] NULL,
	 CONSTRAINT [PK_WorkflowActivityForm] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

END
if NOT EXISTS (SELECT 1
                   FROM   INFORMATION_SCHEMA.TABLES
                   WHERE  TABLE_NAME = 'WorkflowActivityFormTab'
                          AND TABLE_SCHEMA = 'dbo')
BEGIN

	CREATE TABLE [dbo].[WorkflowActivityFormTab](
		[ID] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](500) NOT NULL,
		[Type] [nvarchar](50) NULL,
		[Icon] [nvarchar](100) NOT NULL,
		[Order] [int] NOT NULL,
		[WorkflowActivityFormID] [uniqueidentifier] NOT NULL,
		[RowVersion] [timestamp] NOT NULL,
		[Model] [nvarchar](50) NULL,
		[Identifier] [nvarchar](50) NULL,
	 CONSTRAINT [PK_WorkflowActivityFormTab] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY];

	ALTER TABLE [dbo].[WorkflowActivityFormTab]  WITH CHECK ADD  CONSTRAINT [FK_WorkflowActivityFormTab_WorkflowActivityForm] FOREIGN KEY([WorkflowActivityFormID])
	REFERENCES [dbo].[WorkflowActivityForm] ([ID])
	ON DELETE CASCADE;

	ALTER TABLE [dbo].[WorkflowActivityFormTab] CHECK CONSTRAINT [FK_WorkflowActivityFormTab_WorkflowActivityForm];

END
if NOT EXISTS (SELECT 1
                   FROM   INFORMATION_SCHEMA.TABLES
                   WHERE  TABLE_NAME = 'WorkflowActivityFormField'
                          AND TABLE_SCHEMA = 'dbo')
BEGIN

	CREATE TABLE [dbo].[WorkflowActivityFormField](
		[ID] [uniqueidentifier] NOT NULL,
		[Model] [nvarchar](50) NULL,
		[Identifier] [nvarchar](100) NOT NULL,
		[CategoryName] [nvarchar](100) NOT NULL,
		[Order] [int] NOT NULL,
		[Value] [nvarchar](max) NULL,
		[WorkflowActivityFormTabID] [uniqueidentifier] NOT NULL,
		[Type] [nvarchar](50) NOT NULL,
		[SpecialFields] [nvarchar](max) NOT NULL,
		[RowVersion] [timestamp] NOT NULL,
	 CONSTRAINT [PK_WorkflowActivityFormField] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];


	ALTER TABLE [dbo].[WorkflowActivityFormField]  WITH CHECK ADD  CONSTRAINT [FK_WorkflowActivityFormField_WorkflowActivityFormTab] FOREIGN KEY([WorkflowActivityFormTabID])
	REFERENCES [dbo].[WorkflowActivityFormTab] ([ID])
	ON DELETE CASCADE;

	ALTER TABLE [dbo].[WorkflowActivityFormField] CHECK CONSTRAINT [FK_WorkflowActivityFormField_WorkflowActivityFormTab];

END

IF NOT EXISTS (SELECT 'X'
                   FROM   INFORMATION_SCHEMA.TABLES
                   WHERE  TABLE_NAME = 'FormValues'
                          AND TABLE_SCHEMA = 'dbo')
BEGIN
    CREATE TABLE dbo.[FormValues] (
    ID bigint IDENTITY(1,1) CONSTRAINT PK_FormValues PRIMARY KEY CLUSTERED (ID),
    FormBuilderFormID uniqueidentifier NOT NULL CONSTRAINT FK_FormValues_FormBuilderForm
	FOREIGN KEY (FormBuilderFormID) REFERENCES WorkflowActivityForm(ID)
);
END

IF NOT EXISTS (SELECT 'X'
                   FROM   INFORMATION_SCHEMA.TABLES
                   WHERE  TABLE_NAME = 'Values'
                          AND TABLE_SCHEMA = 'dbo')
BEGIN
	CREATE TABLE dbo.[Values] (
		ID bigint IDENTITY(1,1) CONSTRAINT PK_Values PRIMARY KEY CLUSTERED (ID),
		FormValuesID bigint NOT NULL 
		CONSTRAINT FK_Values_FormValues
		FOREIGN KEY (FormValuesID) REFERENCES FormValues(ID),
		WorkflowActivityFormFieldID uniqueidentifier NOT NULL CONSTRAINT FK_Values_WorkflowActivityFormField
		FOREIGN KEY (WorkflowActivityFormFieldID) REFERENCES WorkflowActivityFormField(ID),
		Value nvarchar(4000) NOT NULL,
);
END


IF NOT EXISTS (SELECT 'X'
                   FROM   INFORMATION_SCHEMA.TABLES
                   WHERE  TABLE_NAME = 'WorkflowFieldOptions'
                          AND TABLE_SCHEMA = 'dbo')

BEGIN
	CREATE TABLE [dbo].[WorkflowFieldOptions](
		[ID] [uniqueidentifier] NOT NULL,
		[Constant] [nvarchar](255) NULL,
		[OperationID] [int] NOT NULL,
		[ActionID] [int] NOT NULL,
		[ParentIdentifier] [nvarchar](255) NOT NULL,
		[RowVersion] [timestamp] NOT NULL,
		[WorkflowActivityFormFieldID] [uniqueidentifier] NOT NULL,
	 CONSTRAINT [FormBuilderFieldOptions] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY];

	ALTER TABLE [dbo].[WorkflowFieldOptions]  WITH CHECK ADD  CONSTRAINT [FK_FormBuilderFieldOptions_WorkflowActivityFormField] FOREIGN KEY([WorkflowActivityFormFieldID])
	REFERENCES [dbo].[WorkflowActivityFormField] ([ID])
	ON DELETE CASCADE;

	ALTER TABLE [dbo].[WorkflowFieldOptions] CHECK CONSTRAINT [FK_FormBuilderFieldOptions_WorkflowActivityFormField];
END

IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'FormValuesID'
          AND Object_ID = Object_ID(N'dbo.[Problem]'))
BEGIN
    ALTER TABLE dbo.[Problem]
	ADD FormValuesID bigint NULL,
	CONSTRAINT FK_Problem_FormValues
	FOREIGN KEY(FormValuesID) REFERENCES FormValues(ID);
END

IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'FormValuesID'
          AND Object_ID = Object_ID(N'dbo.[Call]'))
BEGIN
	ALTER TABLE dbo.[Call]
	ADD FormValuesID bigint NULL,
	CONSTRAINT FK_Call_FormValues
	FOREIGN KEY(FormValuesID) REFERENCES FormValues(ID);
END

IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'FormValuesID'
          AND Object_ID = Object_ID(N'dbo.[RFC]'))
BEGIN
    ALTER TABLE dbo.[RFC]
	ADD FormValuesID bigint NULL,
	CONSTRAINT FK_RFC_FormValues
	FOREIGN KEY(FormValuesID) REFERENCES FormValues(ID);
END

IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'FormValuesID'
          AND Object_ID = Object_ID(N'dbo.[WorkOrder]'))
BEGIN
	ALTER TABLE dbo.[WorkOrder]
	ADD FormValuesID bigint NULL;
	ALTER TABLE dbo.[WorkOrder]
	ADD CONSTRAINT FK_WorkOrder_FormValues
	FOREIGN KEY(FormValuesID) REFERENCES FormValues(ID)
END
