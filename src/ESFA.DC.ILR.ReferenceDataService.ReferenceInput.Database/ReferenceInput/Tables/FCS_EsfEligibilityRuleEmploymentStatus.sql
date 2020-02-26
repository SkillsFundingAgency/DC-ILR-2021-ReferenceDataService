CREATE TABLE [ReferenceInput].[FCS_EsfEligibilityRuleEmploymentStatus] (
    [Id]                        INT IDENTITY (1, 1) NOT NULL,
    [Code]                      INT NOT NULL,
    [FCS_EsfEligibilityRule_Id] INT NULL,
    CONSTRAINT [PK_ReferenceInput.FCS_EsfEligibilityRuleEmploymentStatus] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.FCS_EsfEligibilityRuleEmploymentStatus_ReferenceInput.FCS_EsfEligibilityRule_EsfEligibilityRule_Id] FOREIGN KEY ([FCS_EsfEligibilityRule_Id]) REFERENCES [ReferenceInput].[FCS_EsfEligibilityRule] ([Id])
);


