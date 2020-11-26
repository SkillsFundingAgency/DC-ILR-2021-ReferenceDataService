CREATE TABLE [ReferenceInput].[Postcodes_ONSData] (
    [Id]                    INT            IDENTITY (1, 1) NOT NULL,
    [Termination]           DATETIME       NULL,
    [LocalAuthority]        NVARCHAR (MAX) NULL,
    [Lep1]                  NVARCHAR (MAX) NULL,
    [Lep2]                  NVARCHAR (MAX) NULL,
    [Nuts]                  NVARCHAR (MAX) NULL,
    [EffectiveFrom]         DATETIME       NOT NULL,
    [EffectiveTo]           DATETIME       NULL,
    [Postcodes_Postcode_Id] INT            NULL,
    CONSTRAINT [PK_ReferenceInput.Postcodes_ONSData] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.Postcodes_ONSData_ReferenceInput.Postcodes_Postcode_Postcode_Id] FOREIGN KEY ([Postcodes_Postcode_Id]) REFERENCES [ReferenceInput].[Postcodes_Postcode] ([Id])
);


