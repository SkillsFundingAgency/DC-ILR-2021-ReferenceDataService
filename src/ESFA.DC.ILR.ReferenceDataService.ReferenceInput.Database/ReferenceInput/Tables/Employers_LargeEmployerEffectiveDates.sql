CREATE TABLE [ReferenceInput].[Employers_LargeEmployerEffectiveDates] (
    [Id]                    INT      IDENTITY (1, 1) NOT NULL,
    [EffectiveFrom]         DATETIME NOT NULL,
    [EffectiveTo]           DATETIME NULL,
    [Employers_Employer_Id] INT      NULL,
    CONSTRAINT [PK_ReferenceInput.Employers_LargeEmployerEffectiveDates] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReferenceInput.Employers_LargeEmployerEffectiveDates_ReferenceInput.Employers_Employer_Employer_Id] FOREIGN KEY ([Employers_Employer_Id]) REFERENCES [ReferenceInput].[Employers_Employer] ([Id])
);


