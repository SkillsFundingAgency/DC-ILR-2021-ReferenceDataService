CREATE TABLE [ReferenceInput].[MetaData_ValidationRule]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RuleName] [nvarchar](2000) NULL,
	[Desktop] [bit] NOT NULL,
	[Online] [bit] NOT NULL,
	CONSTRAINT [PK_ReferenceInput.MetaData_ValidationRule] PRIMARY KEY ([Id]),
)
