CREATE TABLE [ReferenceInput].[Organisations_OrganisationVersion]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Version] [nvarchar](2000) NULL,
	CONSTRAINT [PK_ReferenceInput.Organisations_OrganisationVersion] PRIMARY KEY ([Id]),
)
