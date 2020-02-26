CREATE TABLE [ReferenceInput].[FRM_ProviderSpecDeliveryMonitoring] (
    [Id]                  INT            IDENTITY (1, 1) NOT NULL,
    [ProvSpecDelMonOccur] NVARCHAR (MAX) NULL,
    [ProvSpecDelMon]      NVARCHAR (MAX) NULL,
    [FRM_FrmLearner_Id]   INT            NULL,
    CONSTRAINT [PK_ReferenceInput.FRM_ProviderSpecDeliveryMonitoring] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.FRM_ProviderSpecDeliveryMonitoring_ReferenceInput.FRM_FrmLearner_FrmLearner_Id] FOREIGN KEY ([FRM_FrmLearner_Id]) REFERENCES [ReferenceInput].[FRM_FrmLearner] ([Id])
);


