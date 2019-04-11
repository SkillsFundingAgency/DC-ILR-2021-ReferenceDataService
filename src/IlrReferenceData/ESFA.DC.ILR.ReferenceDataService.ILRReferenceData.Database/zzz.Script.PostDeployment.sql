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

RAISERROR('		   Ref Data',10,1) WITH NOWAIT;
	:r .\zReferenceData\Validation.File.Rules.sql
	:r .\zReferenceData\Validation.Modified.Messages.sql
	:r .\zReferenceData\Validation.Modified.Serverity.sql

-- Set ExtendedProperties fro DB.
	:r .\z.ExtendedProperties.sql


RAISERROR('		   Update User Account Passwords',10,1) WITH NOWAIT;
GO

ALTER ROLE [db_datawriter] DROP MEMBER [ILRReferenceData_RW_User];
GO
ALTER ROLE [db_datareader] DROP MEMBER [ILRReferenceData_RW_User];
GO
ALTER ROLE [db_datareader] DROP MEMBER [ILRReferenceData_RO_User];
GO

RAISERROR('		       RO User',10,1) WITH NOWAIT;
ALTER USER [ILRReferenceData_RO_User] WITH PASSWORD = N'$(ROUserPassword)';
GO
RAISERROR('		       DSCI User',10,1) WITH NOWAIT;
ALTER USER [ILRReferenceData_RW_User] WITH PASSWORD = N'$(RWUserPassword)';
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


RAISERROR('		Process Records',10,1) WITH NOWAIT;
--EXEC [Staging].[usp_Process]
GO
RAISERROR('Completed',10,1) WITH NOWAIT;
GO




