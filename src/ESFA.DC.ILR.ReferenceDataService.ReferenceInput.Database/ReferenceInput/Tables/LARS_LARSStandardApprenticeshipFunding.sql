﻿CREATE TABLE [ReferenceInput].[LARS_LARSStandardApprenticeshipFunding] (
    [Id]                                         INT             IDENTITY (1, 1) NOT NULL,
    [ProgType]                                   INT             NOT NULL,
    [PwayCode]                                   INT             NULL,
    [FundingCategory]                            NVARCHAR (MAX)  NULL,
    [BandNumber]                                 INT             NULL,
    [CoreGovContributionCap]                     DECIMAL (10, 5) NULL,
    [SixteenToEighteenIncentive]                 DECIMAL (10, 5) NULL,
    [SixteenToEighteenProviderAdditionalPayment] DECIMAL (10, 5) NULL,
    [SixteenToEighteenEmployerAdditionalPayment] DECIMAL (10, 5) NULL,
    [SixteenToEighteenFrameworkUplift]           DECIMAL (10, 5) NULL,
    [CareLeaverAdditionalPayment]                DECIMAL (10, 5) NULL,
    [Duration]                                   DECIMAL (10, 5) NULL,
    [ReservedValue2]                             DECIMAL (10, 5) NULL,
    [ReservedValue3]                             DECIMAL (10, 5) NULL,
    [MaxEmployerLevyCap]                         DECIMAL (10, 5) NULL,
    [FundableWithoutEmployer]                    NVARCHAR (MAX)  NULL,
    [EffectiveFrom]                              DATETIME        NOT NULL,
    [EffectiveTo]                                DATETIME        NULL,
    [LARS_LARSStandard_Id]                       INT             NULL,
    CONSTRAINT [PK_ReferenceInput.LARS_LARSStandardApprenticeshipFunding] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.LARS_LARSStandardApprenticeshipFunding_ReferenceInput.LARS_LARSStandard_LARSStandard_Id] FOREIGN KEY ([LARS_LARSStandard_Id]) REFERENCES [ReferenceInput].[LARS_LARSStandard] ([Id])
);


