CREATE TABLE [ReferenceInput].[Postcodes_Postcode]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PostCode] [nvarchar](max) NULL,
	CONSTRAINT [PK_ReferenceInput.Postcodes_Postcode] PRIMARY KEY ([Id]),
)
