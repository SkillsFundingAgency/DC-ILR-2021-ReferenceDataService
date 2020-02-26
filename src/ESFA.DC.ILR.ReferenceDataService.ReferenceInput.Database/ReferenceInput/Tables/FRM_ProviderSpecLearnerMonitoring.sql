CREATE TABLE [ReferenceInput].[FRM_ProviderSpecLearnerMonitoring] (
    [Id]                    INT            IDENTITY (1, 1) NOT NULL,
    [ProvSpecLearnMonOccur] NVARCHAR (MAX) NULL,
    [ProvSpecLearnMon]      NVARCHAR (MAX) NULL,
    [FRM_FrmLearner_Id]     INT            NULL,
    CONSTRAINT [PK_ReferenceInput.FRM_ProviderSpecLearnerMonitoring] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.FRM_ProviderSpecLearnerMonitoring_ReferenceInput.FRM_FrmLearner_FrmLearner_Id] FOREIGN KEY ([FRM_FrmLearner_Id]) REFERENCES [ReferenceInput].[FRM_FrmLearner] ([Id])
);


