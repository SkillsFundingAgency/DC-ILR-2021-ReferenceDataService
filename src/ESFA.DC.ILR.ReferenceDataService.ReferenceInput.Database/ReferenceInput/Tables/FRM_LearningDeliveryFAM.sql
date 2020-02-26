CREATE TABLE [ReferenceInput].[FRM_LearningDeliveryFAM] (
    [Id]                  INT            IDENTITY (1, 1) NOT NULL,
    [LearnDelFAMType]     NVARCHAR (MAX) NULL,
    [LearnDelFAMCode]     NVARCHAR (MAX) NULL,
    [LearnDelFAMDateFrom] DATETIME       NULL,
    [LearnDelFAMDateTo]   DATETIME       NULL,
    [FRM_FrmLearner_Id]   INT            NULL,
    CONSTRAINT [PK_ReferenceInput.FRM_LearningDeliveryFAM] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.FRM_LearningDeliveryFAM_ReferenceInput.FRM_FrmLearner_FrmLearner_Id] FOREIGN KEY ([FRM_FrmLearner_Id]) REFERENCES [ReferenceInput].[FRM_FrmLearner] ([Id])
);


