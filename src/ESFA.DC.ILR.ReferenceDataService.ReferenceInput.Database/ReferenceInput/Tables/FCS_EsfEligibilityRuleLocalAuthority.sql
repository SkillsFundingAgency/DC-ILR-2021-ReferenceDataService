CREATE TABLE [ReferenceInput].[FCS_EsfEligibilityRuleLocalAuthority] (
    [Id]                        INT            IDENTITY (1, 1) NOT NULL,
    [Code]                      NVARCHAR (MAX) NULL,
    [FCS_EsfEligibilityRule_Id] INT            NULL,
    CONSTRAINT [PK_ReferenceInput.FCS_EsfEligibilityRuleLocalAuthority] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.FCS_EsfEligibilityRuleLocalAuthority_ReferenceInput.FCS_EsfEligibilityRule_EsfEligibilityRule_Id] FOREIGN KEY ([FCS_EsfEligibilityRule_Id]) REFERENCES [ReferenceInput].[FCS_EsfEligibilityRule] ([Id])
);


