CREATE TABLE [ReferenceInput].[LARS_LARSStandardCommonComponent] (
    [Id]                   INT      IDENTITY (1, 1) NOT NULL,
    [CommonComponent]      INT      NOT NULL,
    [EffectiveFrom]        DATETIME NOT NULL,
    [EffectiveTo]          DATETIME NULL,
    [LARS_LARSStandard_Id] INT      NULL,
    CONSTRAINT [PK_ReferenceInput.LARS_LARSStandardCommonComponent] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.LARS_LARSStandardCommonComponent_ReferenceInput.LARS_LARSStandard_LARSStandard_Id] FOREIGN KEY ([LARS_LARSStandard_Id]) REFERENCES [ReferenceInput].[LARS_LARSStandard] ([Id])
);


