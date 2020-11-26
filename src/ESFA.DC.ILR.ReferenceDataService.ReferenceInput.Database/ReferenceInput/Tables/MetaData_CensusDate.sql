CREATE TABLE [ReferenceInput].[MetaData_CensusDate]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Period] [int] NOT NULL,
	[Start] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	CONSTRAINT [PK_ReferenceInput.MetaData_CensusDate] PRIMARY KEY ([Id]),
)
