CREATE TABLE [ReferenceInput].[LARS_LARSFramework] (
    [Id]                           INT      IDENTITY (1, 1) NOT NULL,
    [FworkCode]                    INT      NOT NULL,
    [ProgType]                     INT      NOT NULL,
    [PwayCode]                     INT      NOT NULL,
    [EffectiveFromNullable]        DATETIME NULL,
    [EffectiveTo]                  DATETIME NULL,
    [LARSFrameworkAim_Id]          INT      NULL,
    [LARS_LARSLearningDelivery_Id] INT      NULL,
    CONSTRAINT [PK_ReferenceInput.LARS_LARSFramework] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.LARS_LARSFramework_ReferenceInput.LARS_LARSFrameworkAim_LARSFrameworkAim_Id] FOREIGN KEY ([LARSFrameworkAim_Id]) REFERENCES [ReferenceInput].[LARS_LARSFrameworkAim] ([Id]),
    CONSTRAINT [FK_ReferenceInput.LARS_LARSFramework_ReferenceInput.LARS_LARSLearningDelivery_LARSLearningDelivery_Id] FOREIGN KEY ([LARS_LARSLearningDelivery_Id]) REFERENCES [ReferenceInput].[LARS_LARSLearningDelivery] ([Id])
);


