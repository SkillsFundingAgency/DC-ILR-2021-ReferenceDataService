CREATE TABLE [ReferenceInput].[MetaData_ReturnPeriod]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Period] [nvarchar](2000) NOT NULL,
	[Start] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	CONSTRAINT [PK_ReferenceInput.MetaData_ReturnPeriod] PRIMARY KEY ([Id]),
)
