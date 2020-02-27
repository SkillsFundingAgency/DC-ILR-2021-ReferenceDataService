CREATE TABLE [ReferenceInput].[Postcodes_PostCodeVersion]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PostcodeCurrentVersion] [nvarchar](2000) NULL,
	CONSTRAINT [PK_ReferenceInput.Postcodes_PostCodeVersion] PRIMARY KEY ([Id]),
)
