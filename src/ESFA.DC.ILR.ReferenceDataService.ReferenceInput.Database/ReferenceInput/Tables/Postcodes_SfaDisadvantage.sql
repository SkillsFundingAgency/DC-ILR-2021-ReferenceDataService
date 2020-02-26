CREATE TABLE [ReferenceInput].[Postcodes_SfaDisadvantage] (
    [Id]                    INT             IDENTITY (1, 1) NOT NULL,
    [Uplift]                DECIMAL (18, 2) NULL,
    [EffectiveFrom]         DATETIME        NOT NULL,
    [EffectiveTo]           DATETIME        NULL,
    [Postcodes_Postcode_Id] INT             NULL,
    CONSTRAINT [PK_ReferenceInput.Postcodes_SfaDisadvantage] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.Postcodes_SfaDisadvantage_ReferenceInput.Postcodes_Postcode_Postcode_Id] FOREIGN KEY ([Postcodes_Postcode_Id]) REFERENCES [ReferenceInput].[Postcodes_Postcode] ([Id])
);


