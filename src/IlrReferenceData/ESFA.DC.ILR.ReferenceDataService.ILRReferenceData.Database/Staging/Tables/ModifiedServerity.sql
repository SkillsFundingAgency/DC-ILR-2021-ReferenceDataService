CREATE TABLE [Staging].[ModifiedServerity]
(
	[Rulename] NVARCHAR(50) NOT NULL , 
	[Severity] NCHAR(1) Null,
	CONSTRAINT [PK_Staging_ModifiedServerity] PRIMARY KEY CLUSTERED ([Rulename]) WITH (FILLFACTOR = 90)
)
