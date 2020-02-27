CREATE TABLE [ReferenceInput].[MetaData_Lookup]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](2000) NULL,
	[Code] [nvarchar](2000) NULL,
	[EffectiveFrom] [datetime] NULL,
	[EffectiveTo] [datetime] NULL,
	CONSTRAINT [PK_ReferenceInput.MetaData_Lookup] PRIMARY KEY ([Id]),
)
