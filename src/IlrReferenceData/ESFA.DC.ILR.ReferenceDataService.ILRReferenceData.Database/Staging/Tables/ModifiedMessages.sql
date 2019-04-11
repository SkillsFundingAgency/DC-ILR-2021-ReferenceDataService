CREATE TABLE [Staging].[ModifiedMessages]
(
	[Rulename] NVARCHAR(50) NOT NULL , 
    [Message] NVARCHAR(2000) NULL,
	CONSTRAINT [PK_Staging_ModifiedMessages] PRIMARY KEY CLUSTERED ([Rulename]) WITH (FILLFACTOR = 90)
)
