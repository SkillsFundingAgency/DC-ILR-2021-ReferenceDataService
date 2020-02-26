CREATE TABLE [ReferenceInput].[LARS_LARSFunding] (
    [Id]                           INT             IDENTITY (1, 1) NOT NULL,
    [LearnAimRef]                  NVARCHAR (MAX)  NULL,
    [FundingCategory]              NVARCHAR (MAX)  NULL,
    [RateUnWeighted]               DECIMAL (18, 2) NULL,
    [RateWeighted]                 DECIMAL (18, 2) NULL,
    [WeightingFactor]              NVARCHAR (MAX)  NULL,
    [EffectiveFrom]                DATETIME        NOT NULL,
    [EffectiveTo]                  DATETIME        NULL,
    [LARS_LARSLearningDelivery_Id] INT             NULL,
    CONSTRAINT [PK_ReferenceInput.LARS_LARSFunding] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.LARS_LARSFunding_ReferenceInput.LARS_LARSLearningDelivery_LARSLearningDelivery_Id] FOREIGN KEY ([LARS_LARSLearningDelivery_Id]) REFERENCES [ReferenceInput].[LARS_LARSLearningDelivery] ([Id])
);


