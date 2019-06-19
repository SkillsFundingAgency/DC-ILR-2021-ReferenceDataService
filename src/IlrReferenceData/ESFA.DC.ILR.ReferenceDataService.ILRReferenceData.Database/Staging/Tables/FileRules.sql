CREATE TABLE [Staging].[FileRules]
(
	[Rulename] NVARCHAR(50) NOT NULL, 
	[Severity] NCHAR(1),
    [Message] NVARCHAR(2000) NULL,
	[Online] BIT NOT NULL DEFAULT 1,
	[Desktop] BIT NOT NULL DEFAULT 1,
	CONSTRAINT [PK_Staging_FileRules] PRIMARY KEY CLUSTERED ([Rulename]) WITH (FILLFACTOR = 90)
)

