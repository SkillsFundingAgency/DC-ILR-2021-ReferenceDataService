CREATE TABLE [ReferenceInput].[PostcodesDevolution_Postcode] (
    [Id]                                       INT            IDENTITY (1, 1) NOT NULL,
    [Postcode]                                 NVARCHAR (MAX) NULL,
    [Area]                                     NVARCHAR (MAX) NULL,
    [SourceOfFunding]                          NVARCHAR (MAX) NULL,
    [EffectiveFrom]                            DATETIME       NOT NULL,
    [EffectiveTo]                              DATETIME       NULL,
    CONSTRAINT [PK_ReferenceInput.PostcodesDevolution_Postcode] PRIMARY KEY CLUSTERED ([Id] ASC)
);


