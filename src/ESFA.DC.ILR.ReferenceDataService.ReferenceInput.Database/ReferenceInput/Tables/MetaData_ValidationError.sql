﻿CREATE TABLE [ReferenceInput].[MetaData_ValidationError]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RuleName] [nvarchar](2000) NULL,
	[Severity] [int] NOT NULL,
	[Message] [nvarchar](2000) NULL,
	CONSTRAINT [PK_ReferenceInput.MetaData_ValidationError] PRIMARY KEY ([Id]),
)
