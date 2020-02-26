CREATE TABLE [ReferenceInput].[Employers_LargeEmployerVersion]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Version] [nvarchar](max) NULL,
	CONSTRAINT [PK_ReferenceInput.Employers_LargeEmployerVersion] PRIMARY KEY ([Id]),
)
