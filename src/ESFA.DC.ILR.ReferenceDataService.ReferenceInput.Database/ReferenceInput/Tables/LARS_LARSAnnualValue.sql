CREATE TABLE [ReferenceInput].[LARS_LARSAnnualValue] (
    [Id]                            INT             IDENTITY (1, 1) NOT NULL,
    [LearnAimRef]                   NVARCHAR (MAX)  NULL,
    [BasicSkills]                   INT             NULL,
    [BasicSkillsType]               INT             NULL,
    [FullLevel2EntitlementCategory] INT             NULL,
    [FullLevel3EntitlementCategory] INT             NULL,
    [FullLevel2Percent]             DECIMAL (5, 2) NULL,
    [FullLevel3Percent]             DECIMAL (5, 2) NULL,
    [EffectiveFrom]                 DATETIME        NOT NULL,
    [EffectiveTo]                   DATETIME        NULL,
    [LARS_LARSLearningDelivery_Id]  INT             NULL,
    CONSTRAINT [PK_ReferenceInput.LARS_LARSAnnualValue] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.LARS_LARSAnnualValue_ReferenceInput.LARS_LARSLearningDelivery_LARSLearningDelivery_Id] FOREIGN KEY ([LARS_LARSLearningDelivery_Id]) REFERENCES [ReferenceInput].[LARS_LARSLearningDelivery] ([Id])
);


