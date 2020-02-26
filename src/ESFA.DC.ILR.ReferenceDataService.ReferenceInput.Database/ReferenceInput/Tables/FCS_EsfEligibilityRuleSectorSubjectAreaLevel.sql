CREATE TABLE [ReferenceInput].[FCS_EsfEligibilityRuleSectorSubjectAreaLevel] (
    [Id]                        INT             IDENTITY (1, 1) NOT NULL,
    [SectorSubjectAreaCode]     DECIMAL (18, 2) NULL,
    [MinLevelCode]              NVARCHAR (MAX)  NULL,
    [MaxLevelCode]              NVARCHAR (MAX)  NULL,
    [FCS_EsfEligibilityRule_Id] INT             NULL,
    CONSTRAINT [PK_ReferenceInput.FCS_EsfEligibilityRuleSectorSubjectAreaLevel] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.FCS_EsfEligibilityRuleSectorSubjectAreaLevel_ReferenceInput.FCS_EsfEligibilityRule_EsfEligibilityRule_Id] FOREIGN KEY ([FCS_EsfEligibilityRule_Id]) REFERENCES [ReferenceInput].[FCS_EsfEligibilityRule] ([Id])
);


