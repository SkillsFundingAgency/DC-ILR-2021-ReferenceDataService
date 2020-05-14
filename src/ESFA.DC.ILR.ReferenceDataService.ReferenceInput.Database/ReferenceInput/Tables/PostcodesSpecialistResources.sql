CREATE TABLE [ReferenceInput].[PostcodesSpecialistResources] (
    [Id]                    INT             IDENTITY (1, 1) NOT NULL,
    [SpecialistResources]					NVARCHAR(1) NOT NULL,
    [EffectiveFrom]         DATETIME        NOT NULL,
    [EffectiveTo]           DATETIME        NULL,
    [Postcodes_Postcode_Id] INT             NULL,
    CONSTRAINT [PK_ReferenceInput.PostcodesSpecialistResources] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.PostcodesSpecialistResources_ReferenceInput.Postcodes_Postcode_Postcode_Id] FOREIGN KEY ([Postcodes_Postcode_Id]) REFERENCES [ReferenceInput].[Postcodes_Postcode] ([Id])
);


