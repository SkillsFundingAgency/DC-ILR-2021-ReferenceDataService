CREATE TABLE [ReferenceInput].[MetaData_ReferenceDataVersion] (
    [Id]                          INT IDENTITY (1, 1) NOT NULL,
    [CampusIdentifierVersion_Id]  INT NULL,
    [CoFVersion_Id]               INT NULL,
    [DevolvedPostcodesVersion_Id] INT NULL,
    [EasFileDetails_Id]        INT NULL,
    [EmployersVersion_Id]                INT NULL,
    [HmppPostcodesVersion_Id]     INT NULL,
    [LarsVersion_Id]              INT NULL,
    [OrganisationsVersion_Id]     INT NULL,
    [PostcodeFactorsVersion_Id]   INT NULL,
    [PostcodesVersion_Id]         INT NULL,
    CONSTRAINT [PK_ReferenceInput.MetaData_ReferenceDataVersion] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_CampusIdentifierVersion_CampusIdentifierVersion_Id] FOREIGN KEY ([CampusIdentifierVersion_Id]) REFERENCES [ReferenceInput].[MetaData_CampusIdentifierVersion] ([Id]),
    CONSTRAINT [FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_CoFVersion_CoFVersion_Id] FOREIGN KEY ([CoFVersion_Id]) REFERENCES [ReferenceInput].[MetaData_CoFVersion] ([Id]),
    CONSTRAINT [FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_DevolvedPostcodesVersion_DevolvedPostcodesVersion_Id] FOREIGN KEY ([DevolvedPostcodesVersion_Id]) REFERENCES [ReferenceInput].[MetaData_DevolvedPostcodesVersion] ([Id]),
    CONSTRAINT [FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_EasFileDetails_EasFileDetails_Id] FOREIGN KEY ([EasFileDetails_Id]) REFERENCES [ReferenceInput].[MetaData_EasFileDetails] ([Id]),
    CONSTRAINT [FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_EmployersVersion_Employers_Id] FOREIGN KEY ([EmployersVersion_Id]) REFERENCES [ReferenceInput].[MetaData_EmployersVersion] ([Id]),
    CONSTRAINT [FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_HmppPostcodesVersion_HmppPostcodesVersion_Id] FOREIGN KEY ([HmppPostcodesVersion_Id]) REFERENCES [ReferenceInput].[MetaData_HmppPostcodesVersion] ([Id]),
    CONSTRAINT [FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_LarsVersion_LarsVersion_Id] FOREIGN KEY ([LarsVersion_Id]) REFERENCES [ReferenceInput].[MetaData_LarsVersion] ([Id]),
    CONSTRAINT [FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_OrganisationsVersion_OrganisationsVersion_Id] FOREIGN KEY ([OrganisationsVersion_Id]) REFERENCES [ReferenceInput].[MetaData_OrganisationsVersion] ([Id]),
    CONSTRAINT [FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_PostcodeFactorsVersion_PostcodeFactorsVersion_Id] FOREIGN KEY ([PostcodeFactorsVersion_Id]) REFERENCES [ReferenceInput].[MetaData_PostcodeFactorsVersion] ([Id]),
    CONSTRAINT [FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_PostcodesVersion_PostcodesVersion_Id] FOREIGN KEY ([PostcodesVersion_Id]) REFERENCES [ReferenceInput].[MetaData_PostcodesVersion] ([Id])
);


