﻿CREATE TABLE [dbo].[Rules]
(
	[Rulename] NVARCHAR(50) NOT NULL, 
    [Severity] NVARCHAR(1) NULL, 
    [Message] NVARCHAR(2000) NULL,
	[Online] BIT NOT NULL DEFAULT 1,
	[Desktop] BIT NOT NULL DEFAULT 1,
	CONSTRAINT [PK_dbo_Rules] PRIMARY KEY CLUSTERED ([Rulename]) WITH (FILLFACTOR = 90)
)