CREATE TABLE [Staging].[Rules]
(
	[Rulename] NVARCHAR(50) NOT NULL, 
    [Severity] NVARCHAR NULL, 
    [Message] NVARCHAR(2000) NULL
	CONSTRAINT [PK_Staging_Rules] PRIMARY KEY CLUSTERED ([Rulename]) WITH (FILLFACTOR = 90)
)

