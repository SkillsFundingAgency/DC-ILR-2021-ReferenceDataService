CREATE TABLE [ReferenceInput].[PostcodesDevolution_McaGlaSofLookup] (
    [Id]                                       INT            IDENTITY (1, 1) NOT NULL,
    [SofCode]                                  NVARCHAR (MAX) NULL,
    [McaGlaShortCode]                          NVARCHAR (MAX) NULL,
    [McaGlaFullName]                           NVARCHAR (MAX) NULL,
    [EffectiveFrom]                            DATETIME       NOT NULL,
    [EffectiveTo]                              DATETIME       NULL,
    [PostcodesDevolution_DevolvedPostcodes_Id] INT            NULL,
    CONSTRAINT [PK_ReferenceInput.PostcodesDevolution_McaGlaSofLookup] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.PostcodesDevolution_McaGlaSofLookup_ReferenceInput.PostcodesDevolution_DevolvedPostcodes_DevolvedPostcodes_Id] FOREIGN KEY ([PostcodesDevolution_DevolvedPostcodes_Id]) REFERENCES [ReferenceInput].[PostcodesDevolution_DevolvedPostcodes] ([Id])
);


