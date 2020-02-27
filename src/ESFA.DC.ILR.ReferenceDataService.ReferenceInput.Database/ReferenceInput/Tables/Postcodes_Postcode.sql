CREATE TABLE [ReferenceInput].[Postcodes_Postcode]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PostCode] [nvarchar](2000) NULL,
	CONSTRAINT [PK_ReferenceInput.Postcodes_Postcode] PRIMARY KEY ([Id]),
)
