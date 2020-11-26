CREATE TABLE [ReferenceInput].[Organisations_OrganisationFunding] (
    [Id]                            INT            IDENTITY (1, 1) NOT NULL,
    [OrgFundFactor]                 NVARCHAR (MAX) NULL,
    [OrgFundFactType]               NVARCHAR (MAX) NULL,
    [OrgFundFactValue]              NVARCHAR (MAX) NULL,
    [EffectiveFrom]                 DATETIME       NOT NULL,
    [EffectiveTo]                   DATETIME       NULL,
    [Organisations_Organisation_Id] INT            NULL,
    CONSTRAINT [PK_ReferenceInput.Organisations_OrganisationFunding] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.Organisations_OrganisationFunding_ReferenceInput.Organisations_Organisation_Organisation_Id] FOREIGN KEY ([Organisations_Organisation_Id]) REFERENCES [ReferenceInput].[Organisations_Organisation] ([Id])
);


