CREATE TABLE [ReferenceInput].[EPAOrganisations_EPAOrganisation]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EPAID] [nvarchar](2000) NULL,
	[Standard] [nvarchar](2000) NULL,
	[EffectiveFrom] [datetime] NOT NULL,
	[EffectiveTo] [datetime] NULL,
	CONSTRAINT [PK_ReferenceInput.EPAOrganisations_EPAOrganisation] PRIMARY KEY ([Id]),
)
