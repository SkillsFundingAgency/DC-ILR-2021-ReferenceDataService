CREATE TABLE [ReferenceInput].[LARS_LARSFrameworkApprenticeshipFunding] (
    [Id]                                         INT             IDENTITY (1, 1) NOT NULL,
    [FundingCategory]                            NVARCHAR (MAX)  NULL,
    [BandNumber]                                 INT             NULL,
    [CoreGovContributionCap]                     DECIMAL (18, 2) NULL,
    [SixteenToEighteenIncentive]                 DECIMAL (18, 2) NULL,
    [SixteenToEighteenProviderAdditionalPayment] DECIMAL (18, 2) NULL,
    [SixteenToEighteenEmployerAdditionalPayment] DECIMAL (18, 2) NULL,
    [SixteenToEighteenFrameworkUplift]           DECIMAL (18, 2) NULL,
    [CareLeaverAdditionalPayment]                DECIMAL (18, 2) NULL,
    [Duration]                                   DECIMAL (18, 2) NULL,
    [ReservedValue2]                             DECIMAL (18, 2) NULL,
    [ReservedValue3]                             DECIMAL (18, 2) NULL,
    [MaxEmployerLevyCap]                         DECIMAL (18, 2) NULL,
    [FundableWithoutEmployer]                    NVARCHAR (MAX)  NULL,
    [EffectiveFrom]                              DATETIME        NOT NULL,
    [EffectiveTo]                                DATETIME        NULL,
    [LARS_LARSFrameworkDesktop_Id]               INT             NULL,
    [LARS_LARSFramework_Id]                      INT             NULL,
    CONSTRAINT [PK_ReferenceInput.LARS_LARSFrameworkApprenticeshipFunding] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.LARS_LARSFrameworkApprenticeshipFunding_ReferenceInput.LARS_LARSFrameworkDesktop_LARSFrameworkDesktop_Id] FOREIGN KEY ([LARS_LARSFrameworkDesktop_Id]) REFERENCES [ReferenceInput].[LARS_LARSFrameworkDesktop] ([Id])
);


