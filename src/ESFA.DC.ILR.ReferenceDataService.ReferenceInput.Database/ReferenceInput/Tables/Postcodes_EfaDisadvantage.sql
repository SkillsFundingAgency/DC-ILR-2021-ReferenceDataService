﻿CREATE TABLE [ReferenceInput].[Postcodes_EfaDisadvantage] (
    [Id]                    INT             IDENTITY (1, 1) NOT NULL,
    [Uplift]                DECIMAL (10, 5) NULL,
    [EffectiveFrom]         DATETIME        NOT NULL,
    [EffectiveTo]           DATETIME        NULL,
    [Postcodes_Postcode_Id] INT             NULL,
    CONSTRAINT [PK_ReferenceInput.Postcodes_EfaDisadvantage] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.Postcodes_EfaDisadvantage_ReferenceInput.Postcodes_Postcode_Postcode_Id] FOREIGN KEY ([Postcodes_Postcode_Id]) REFERENCES [ReferenceInput].[Postcodes_Postcode] ([Id])
);


