CREATE TABLE [ReferenceInput].[MetaData_MetaData] (
    [Id]                       INT      IDENTITY (1, 1) NOT NULL,
    [DateGenerated]            DATETIME NOT NULL,
    [CollectionDates_Id]       INT      NULL,
    [ReferenceDataVersions_Id] INT      NULL,
    CONSTRAINT [PK_ReferenceInput.MetaData_MetaData] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.MetaData_MetaData_ReferenceInput.MetaData_IlrCollectionDates_CollectionDates_Id] FOREIGN KEY ([CollectionDates_Id]) REFERENCES [ReferenceInput].[MetaData_IlrCollectionDates] ([Id]),
    CONSTRAINT [FK_ReferenceInput.MetaData_MetaData_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceDataVersions_Id] FOREIGN KEY ([ReferenceDataVersions_Id]) REFERENCES [ReferenceInput].[MetaData_ReferenceDataVersion] ([Id])
);


