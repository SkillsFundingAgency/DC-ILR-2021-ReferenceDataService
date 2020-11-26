CREATE TABLE [ReferenceInput].[Organisations_PostcodesSpecialistResources] (
    [Id]                    INT             IDENTITY (1, 1) NOT NULL,
	[UKPRN]					BIGINT			NOT NULL,
    [Postcode]				NVARCHAR(10)	NOT NULL,
    [SpecialistResources]	NVARCHAR(1)		NOT NULL,
    [EffectiveFrom]         DATETIME        NOT NULL,
    [EffectiveTo]           DATETIME        NULL,
    [Organisations_Organisation_Id] INT            NULL,
    CONSTRAINT [PK_ReferenceInput.Organisations_OrganisationPostcodeSpecResources] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.Organisations_OrganisationPostcodeSpecResources_ReferenceInput.Organisations_Organisation_Organisation_Id] FOREIGN KEY ([Organisations_Organisation_Id]) REFERENCES [ReferenceInput].[Organisations_Organisation] ([Id])
);

