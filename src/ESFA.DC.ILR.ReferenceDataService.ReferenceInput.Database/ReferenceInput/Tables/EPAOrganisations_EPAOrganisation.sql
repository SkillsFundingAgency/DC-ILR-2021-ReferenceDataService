CREATE TABLE [ReferenceInput].[EPAOrganisations_EPAOrganisation]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Standard] [nvarchar](max) NULL,
	[EffectiveFrom] [datetime] NOT NULL,
	[EffectiveTo] [datetime] NULL,
	CONSTRAINT [PK_ReferenceInput.EPAOrganisations_EPAOrganisation] PRIMARY KEY ([Id]),
)
