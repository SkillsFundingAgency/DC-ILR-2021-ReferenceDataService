CREATE TABLE [ReferenceInput].[MetaData_ValidationError]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RuleName] [nvarchar](max) NULL,
	[Severity] [int] NOT NULL,
	[Message] [nvarchar](max) NULL,
	CONSTRAINT [PK_ReferenceInput.MetaData_ValidationError] PRIMARY KEY ([Id]),
)
