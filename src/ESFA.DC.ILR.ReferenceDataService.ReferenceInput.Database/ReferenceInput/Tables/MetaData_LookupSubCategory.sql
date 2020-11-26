CREATE TABLE [ReferenceInput].[MetaData_LookupSubCategory] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [Code]               NVARCHAR (MAX) NULL,
    [EffectiveFrom]      DATETIME       NULL,
    [EffectiveTo]        DATETIME       NULL,
    [MetaData_Lookup_Id] INT            NULL,
    CONSTRAINT [PK_ReferenceInput.MetaData_LookupSubCategory] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.MetaData_LookupSubCategory_ReferenceInput.MetaData_Lookup_Lookup_Id] FOREIGN KEY ([MetaData_Lookup_Id]) REFERENCES [ReferenceInput].[MetaData_Lookup] ([Id])
);


