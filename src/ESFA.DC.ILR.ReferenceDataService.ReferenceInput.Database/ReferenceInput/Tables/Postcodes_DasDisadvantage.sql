CREATE TABLE [ReferenceInput].[Postcodes_DasDisadvantage] (
    [Id]                    INT             IDENTITY (1, 1) NOT NULL,
    [Uplift]                DECIMAL (18, 2) NULL,
    [EffectiveFrom]         DATETIME        NOT NULL,
    [EffectiveTo]           DATETIME        NULL,
    [Postcodes_Postcode_Id] INT             NULL,
    CONSTRAINT [PK_ReferenceInput.Postcodes_DasDisadvantage] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.Postcodes_DasDisadvantage_ReferenceInput.Postcodes_Postcode_Postcode_Id] FOREIGN KEY ([Postcodes_Postcode_Id]) REFERENCES [ReferenceInput].[Postcodes_Postcode] ([Id])
);


