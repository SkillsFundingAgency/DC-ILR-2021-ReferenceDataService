/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- Set ExtendedProperties fro DB.
	:r .\z.ExtendedProperties.sql

GO
SET NOCOUNT ON;

RAISERROR('----------------------------------------------------------------------------------------------------------------------------------------',10,1) WITH NOWAIT;
RAISERROR('		   Populate Staging [ModifiedServerity] with values from Rule table that are Diffrent from the BAU data. This should be the environment specific changes to serverity.',10,1) WITH NOWAIT;

		-- Load Current Serverity Records form ENV into Staging Tables
	INSERT INTO [Staging].[ModifiedServerity]([Rulename],[Severity])
	SELECT [Rulename],[Severity]
	FROM
	(
		SELECT TOP 100 PERCENT 
			'UNION SELECT ''' + MR.[Rulename] + ''' as [Rulename],''' + R.[Severity] + ''' as [Severity]' as [Value], MR.[Rulename], R.[Severity]
		FROM [Staging].[Rules] MR
		INNER JOIN [dbo].[Rules] R
		ON R.[Rulename] = MR.[Rulename]
		WHERE MR.[Severity] <> R.[Severity]
		ORDER BY  MR.[Rulename]
	) as CurrentAmendments
	WHERE [Rulename] NOT IN (SELECT [Rulename] FROM [Staging].[ModifiedServerity])

GO
RAISERROR('----------------------------------------------------------------------------------------------------------------------------------------',10,1) WITH NOWAIT;

GO

RAISERROR('		   Ref Data',10,1) WITH NOWAIT;
	:r .\zReferenceData\Validation.File.Rules.sql
	:r .\zReferenceData\Validation.Modified.Messages.sql
	:r .\zReferenceData\Validation.Modified.Serverity.sql

RAISERROR('		   Update User Account Passwords',10,1) WITH NOWAIT;
GO

RAISERROR('		       RO User',10,1) WITH NOWAIT;
ALTER USER [ILR1819ReferenceDataD_RO_User] WITH PASSWORD = N'$(ROUserPassword)';
GO
RAISERROR('		       DSCI User',10,1) WITH NOWAIT;
ALTER USER [ILR1819ReferenceDataD_RW_User] WITH PASSWORD = N'$(RWUserPassword)';
GO
RAISERROR('		       DSCI User',10,1) WITH NOWAIT;
ALTER USER [User_DSCI] WITH PASSWORD = N'$(DsciUserPassword)';
GO

REVOKE REFERENCES ON SCHEMA::[dbo] FROM [DataProcessor];
REVOKE REFERENCES ON SCHEMA::[dbo] FROM [DataViewer];
GO

---- This is ONLY to be turned on after at least 1 round of deployments to PRODUCTION
---- DROP TABEL IF EXISTS [Staging].[ModifiedMessages];
----
GO
RAISERROR('		Process Records',10,1) WITH NOWAIT;
--EXEC [Staging].[usp_Process]
GO
RAISERROR('Completed',10,1) WITH NOWAIT;
GO




