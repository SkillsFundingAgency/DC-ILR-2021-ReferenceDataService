CREATE TABLE [ReferenceInput].[FCS_FcsContractAllocation] (
    [Id]                        INT             IDENTITY (1, 1) NOT NULL,
    [ContractAllocationNumber]  NVARCHAR (MAX)  NULL,
    [DeliveryUKPRN]             INT             NOT NULL,
    [LearningRatePremiumFactor] DECIMAL (10, 2) NULL,
    [TenderSpecReference]       NVARCHAR (MAX)  NULL,
    [LotReference]              NVARCHAR (MAX)  NULL,
    [FundingStreamPeriodCode]   NVARCHAR (MAX)  NULL,
    [StartDate]                 DATETIME        NULL,
    [EndDate]                   DATETIME        NULL,
    [StopNewStartsFromDate]     DATETIME        NULL,
    [EsfEligibilityRule_Id]     INT             NULL,
    CONSTRAINT [PK_ReferenceInput.FCS_FcsContractAllocation] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.FCS_FcsContractAllocation_ReferenceInput.FCS_EsfEligibilityRule_EsfEligibilityRule_Id] FOREIGN KEY ([EsfEligibilityRule_Id]) REFERENCES [ReferenceInput].[FCS_EsfEligibilityRule] ([Id])
);


