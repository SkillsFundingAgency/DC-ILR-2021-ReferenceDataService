CREATE TABLE [ReferenceInput].[Organisations_OrganisationCoFRemoval] (
    [Id]                            INT             IDENTITY (1, 1) NOT NULL,
    [CoFRemoval]                    DECIMAL (9, 2) NOT NULL,
    [EffectiveFrom]                 DATETIME        NOT NULL,
    [EffectiveTo]                   DATETIME        NULL,
    [Organisations_Organisation_Id] INT             NULL,
    CONSTRAINT [PK_ReferenceInput.Organisations_OrganisationCoFRemoval] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.Organisations_OrganisationCoFRemoval_ReferenceInput.Organisations_Organisation_Organisation_Id] FOREIGN KEY ([Organisations_Organisation_Id]) REFERENCES [ReferenceInput].[Organisations_Organisation] ([Id])
);


