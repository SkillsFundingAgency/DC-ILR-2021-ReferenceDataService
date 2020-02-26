CREATE TABLE [ReferenceInput].[Organisations_OrganisationCampusIdentifier] (
    [Id]                            INT            IDENTITY (1, 1) NOT NULL,
    [UKPRN]                         BIGINT         NOT NULL,
    [CampusIdentifier]              NVARCHAR (MAX) NULL,
    [EffectiveFrom]                 DATETIME       NOT NULL,
    [EffectiveTo]                   DATETIME       NULL,
    [Organisations_Organisation_Id] INT            NULL,
    CONSTRAINT [PK_ReferenceInput.Organisations_OrganisationCampusIdentifier] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.Organisations_OrganisationCampusIdentifier_ReferenceInput.Organisations_Organisation_Organisation_Id] FOREIGN KEY ([Organisations_Organisation_Id]) REFERENCES [ReferenceInput].[Organisations_Organisation] ([Id])
);


