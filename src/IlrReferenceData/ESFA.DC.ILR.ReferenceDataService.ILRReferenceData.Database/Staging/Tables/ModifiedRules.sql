CREATE TABLE [Staging].[ModifiedRules]
(
	[Rulename] NVARCHAR(50) NOT NULL , 
	[Severity] NCHAR(1) Null,
    [Message] NVARCHAR(2000) NULL,
	CONSTRAINT [PK_Staging_ModifiedRules] PRIMARY KEY CLUSTERED ([Rulename]) WITH (FILLFACTOR = 90)
)
