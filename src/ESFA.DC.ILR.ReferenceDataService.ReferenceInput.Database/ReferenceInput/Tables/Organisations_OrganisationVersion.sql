CREATE TABLE [ReferenceInput].[Organisations_OrganisationVersion]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Version] [nvarchar](max) NULL,
	CONSTRAINT [PK_ReferenceInput.Organisations_OrganisationVersion] PRIMARY KEY ([Id]),
)
