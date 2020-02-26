CREATE TABLE [ReferenceInput].[LARS_LARSLearningDeliveryCategory] (
    [Id]                           INT            IDENTITY (1, 1) NOT NULL,
    [LearnAimRef]                  NVARCHAR (MAX) NULL,
    [CategoryRef]                  INT            NOT NULL,
    [EffectiveFrom]                DATETIME       NOT NULL,
    [EffectiveTo]                  DATETIME       NULL,
    [LARS_LARSLearningDelivery_Id] INT            NULL,
    CONSTRAINT [PK_ReferenceInput.LARS_LARSLearningDeliveryCategory] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.LARS_LARSLearningDeliveryCategory_ReferenceInput.LARS_LARSLearningDelivery_LARSLearningDelivery_Id] FOREIGN KEY ([LARS_LARSLearningDelivery_Id]) REFERENCES [ReferenceInput].[LARS_LARSLearningDelivery] ([Id])
);


