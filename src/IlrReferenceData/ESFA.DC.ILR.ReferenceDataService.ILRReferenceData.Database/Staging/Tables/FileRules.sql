CREATE TABLE [Staging].[FileRules]
(
	[Rulename] NVARCHAR(50) NOT NULL, 
	[Severity] NCHAR(1),
    [Message] NVARCHAR(2000) NULL,
	CONSTRAINT [PK_Staging_FileRules] PRIMARY KEY CLUSTERED ([Rulename]) WITH (FILLFACTOR = 90)
)

