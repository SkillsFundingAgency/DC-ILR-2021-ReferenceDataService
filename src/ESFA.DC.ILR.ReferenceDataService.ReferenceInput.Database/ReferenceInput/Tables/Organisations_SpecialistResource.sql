CREATE TABLE [ReferenceInput].[Organisations_SpecialistResource] (
    [Id]                                            INT      IDENTITY (1, 1) NOT NULL,
    [IsSpecialistResource]                          BIT      NOT NULL,
    [EffectiveFrom]                                 DATETIME NOT NULL,
    [EffectiveTo]                                   DATETIME NULL,
    [Organisations_OrganisationCampusIdentifier_Id] INT      NULL,
    CONSTRAINT [PK_ReferenceInput.Organisations_SpecialistResource] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.Organisations_SpecialistResource_ReferenceInput.Organisations_OrganisationCampusIdentifier_Id] FOREIGN KEY ([Organisations_OrganisationCampusIdentifier_Id]) REFERENCES [ReferenceInput].[Organisations_OrganisationCampusIdentifier] ([Id])
);


