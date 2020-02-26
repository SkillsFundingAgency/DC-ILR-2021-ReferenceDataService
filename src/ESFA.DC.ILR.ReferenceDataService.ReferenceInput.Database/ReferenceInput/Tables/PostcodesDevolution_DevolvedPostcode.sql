CREATE TABLE [ReferenceInput].[PostcodesDevolution_DevolvedPostcode] (
    [Id]                                       INT            IDENTITY (1, 1) NOT NULL,
    [Postcode]                                 NVARCHAR (MAX) NULL,
    [Area]                                     NVARCHAR (MAX) NULL,
    [SourceOfFunding]                          NVARCHAR (MAX) NULL,
    [EffectiveFrom]                            DATETIME       NOT NULL,
    [EffectiveTo]                              DATETIME       NULL,
    [PostcodesDevolution_DevolvedPostcodes_Id] INT            NULL,
    CONSTRAINT [PK_ReferenceInput.PostcodesDevolution_DevolvedPostcode] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.PostcodesDevolution_DevolvedPostcode_ReferenceInput.PostcodesDevolution_DevolvedPostcodes_DevolvedPostcodes_Id] FOREIGN KEY ([PostcodesDevolution_DevolvedPostcodes_Id]) REFERENCES [ReferenceInput].[PostcodesDevolution_DevolvedPostcodes] ([Id])
);


