﻿CREATE TABLE [dbo].[Rules]
(
	[Rulename] NVARCHAR(50) NOT NULL, 
    [Severity] NVARCHAR(1) NULL, 
    [Message] NVARCHAR(2000) NULL
	CONSTRAINT [PK_dbo_Rules] PRIMARY KEY CLUSTERED ([Rulename]) WITH (FILLFACTOR = 90)
)