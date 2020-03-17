CREATE TABLE [ReferenceInput].[PostcodesDevolution_McaGlaSofLookup] (
    [Id]                                       INT            IDENTITY (1, 1) NOT NULL,
    [SofCode]                                  NVARCHAR (MAX) NULL,
    [McaGlaShortCode]                          NVARCHAR (MAX) NULL,
    [McaGlaFullName]                           NVARCHAR (MAX) NULL,
    [EffectiveFrom]                            DATETIME       NOT NULL,
    [EffectiveTo]                              DATETIME       NULL,
    CONSTRAINT [PK_ReferenceInput.PostcodesDevolution_McaGlaSofLookup] PRIMARY KEY CLUSTERED ([Id] ASC)
);


