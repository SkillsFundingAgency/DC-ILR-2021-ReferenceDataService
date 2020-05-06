CREATE TABLE [ReferenceInput].[MetaData_EasFileDetails]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileName] VARCHAR(50) NULL,
	[UploadDateTime] [datetime] NULL,
	CONSTRAINT [PK_ReferenceInput.MetaData_EasFileDetails] PRIMARY KEY ([Id]),
)
