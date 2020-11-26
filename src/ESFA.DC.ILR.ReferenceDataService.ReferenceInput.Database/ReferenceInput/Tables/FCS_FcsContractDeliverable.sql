CREATE TABLE [ReferenceInput].[FCS_FcsContractDeliverable] (
    [Id]                           INT             IDENTITY (1, 1) NOT NULL,
    [DeliverableCode]              INT             NULL,
    [DeliverableDescription]       NVARCHAR (MAX)  NULL,
    [ExternalDeliverableCode]      NVARCHAR (MAX)  NULL,
    [UnitCost]                     DECIMAL (13, 2) NULL,
    [PlannedVolume]                INT             NULL,
    [PlannedValue]                 DECIMAL (13, 2) NULL,
    [FCS_FcsContractAllocation_Id] INT             NULL,
    CONSTRAINT [PK_ReferenceInput.FCS_FcsContractDeliverable] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.FCS_FcsContractDeliverable_ReferenceInput.FCS_FcsContractAllocation_FcsContractAllocation_Id] FOREIGN KEY ([FCS_FcsContractAllocation_Id]) REFERENCES [ReferenceInput].[FCS_FcsContractAllocation] ([Id])
);


