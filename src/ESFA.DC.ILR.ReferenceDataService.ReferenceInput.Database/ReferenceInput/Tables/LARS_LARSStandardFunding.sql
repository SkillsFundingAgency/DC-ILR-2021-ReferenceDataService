CREATE TABLE [ReferenceInput].[LARS_LARSStandardFunding] (
    [Id]                         INT             IDENTITY (1, 1) NOT NULL,
    [FundingCategory]            NVARCHAR (MAX)  NULL,
    [BandNumber]                 INT             NULL,
    [CoreGovContributionCap]     DECIMAL (18, 2) NULL,
    [SixteenToEighteenIncentive] DECIMAL (18, 2) NULL,
    [SmallBusinessIncentive]     DECIMAL (18, 2) NULL,
    [AchievementIncentive]       DECIMAL (18, 2) NULL,
    [FundableWithoutEmployer]    NVARCHAR (MAX)  NULL,
    [EffectiveFrom]              DATETIME        NOT NULL,
    [EffectiveTo]                DATETIME        NULL,
    [LARS_LARSStandard_Id]       INT             NULL,
    CONSTRAINT [PK_ReferenceInput.LARS_LARSStandardFunding] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.LARS_LARSStandardFunding_ReferenceInput.LARS_LARSStandard_LARSStandard_Id] FOREIGN KEY ([LARS_LARSStandard_Id]) REFERENCES [ReferenceInput].[LARS_LARSStandard] ([Id])
);


