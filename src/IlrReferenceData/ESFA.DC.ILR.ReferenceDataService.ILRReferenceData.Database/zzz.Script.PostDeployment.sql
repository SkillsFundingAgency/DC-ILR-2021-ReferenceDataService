﻿/*
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
ALTER USER [ILR1920ReferenceData_RO_User] WITH PASSWORD = N'$(ROUserPassword)';
GO
RAISERROR('		       DSCI User',10,1) WITH NOWAIT;
ALTER USER [ILR1920ReferenceData_RW_User] WITH PASSWORD = N'$(RWUserPassword)';
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

DELETE FROM [dbo].[Lookup]
GO

INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Accom', N'5', N'Learner is living away from home (in accommodation owned or managed by the provider).', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'AimType', N'1', N'Programme Aim', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'AimType', N'3', N'Component learning aim within a programme', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'AimType', N'4', N'Learning aim that is not part of a programme', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'AimType', N'5', N'Core aim – EFA funded learning aims only', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'AppFinType', N'PMR', N'Payment record', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'AppFinType', N'TNP', N'Total negotiated price', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'CompStatus', N'1', N'The learner is continuing or intending to continue the learning activities leading to the learning aim', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'CompStatus', N'2', N'The learner has completed the learning activities leading to the learning aim', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'CompStatus', N'3', N'The learner has withdrawn from the learning activities leading to the learning aim', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'CompStatus', N'6', N'Learner has temporarily withdrawn from the aim due to an agreed break in learning', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ContPrefType', N'PMC', N'Preferred method of contact', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ContPrefType', N'RUI', N'Restricted use indicator', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ContType', N'1', N'Prior to enrolment (the permanent or home postcode of the learner prior to enrolling at the provider)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ContType', N'2', N'Current (learner''s current or last known residence)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'AD', N'Andorra', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'AE', N'United Arab Emirates', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'AF', N'Afghanistan', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'AG', N'Antigua and Barbuda', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'AI', N'Anguilla', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'AL', N'Albania', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'AM', N'Armenia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'AN', N'Netherlands Antilles', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'AO', N'Angola', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'AR', N'Argentina', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'AS', N'American Samoa', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'AT', N'Austria', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'AU', N'Australia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'AW', N'Aruba', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'AX', N'Aland Islands {Ahvenamaa}', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'AZ', N'Azerbaijan', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BA', N'Bosnia and Herzegovina', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BB', N'Barbados', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BD', N'Bangladesh', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BE', N'Belgium', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BF', N'Burkina [Burkina Faso]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BG', N'Bulgaria', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BH', N'Bahrain', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BI', N'Burundi', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BJ', N'Benin', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BL', N'St Barthelemy', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BM', N'Bermuda', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BN', N'Brunei [Brunei Darussalam]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BO', N'Bolivia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BQ', N'Bonaire, Sint Eustatius and Saba', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BR', N'Brazil', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BS', N'Bahamas, The', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BT', N'Bhutan', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BW', N'Botswana', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BY', N'Belarus', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'BZ', N'Belize', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'CA', N'Canada', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'CC', N'Cocos (Keeling) Islands', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'CD', N'Congo (Democratic Republic) [Congo (The Democratic Republic of the)] {formerly Zaire}', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'CF', N'Central African Republic', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'CG', N'Congo', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'CH', N'Switzerland', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'CI', N'Ivory Coast [Cote D''ivoire]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'CK', N'Cook Islands', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'CL', N'Chile', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'CM', N'Cameroon', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'CN', N'China', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'CO', N'Colombia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'CR', N'Costa Rica', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'CU', N'Cuba', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'CV', N'Cape Verde', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'CW', N'Curaçao', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'CX', N'Christmas Island', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'CZ', N'Czech Republic', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'DE', N'Germany', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'DJ', N'Djibouti', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'DK', N'Denmark', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'DM', N'Dominica', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'DO', N'Dominican Republic', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'DZ', N'Algeria', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'EC', N'Ecuador', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'EE', N'Estonia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'EG', N'Egypt', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'EH', N'Western Sahara', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'ER', N'Eritrea', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'ES', N'Spain {includes Ceuta, Melilla}', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'ET', N'Ethiopia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'EU', N'European Union not otherwise specified', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'FI', N'Finland', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'FJ', N'Fiji', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'FK', N'Falkland Islands [Falkland Islands (Malvinas)]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'FM', N'Micronesia [Micronesia, Federated States of]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'FO', N'Faroe Islands', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'FR', N'France {includes Corsica}', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'GA', N'Gabon', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'GD', N'Grenada', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'GE', N'Georgia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'GF', N'French Guiana', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'GG', N'Guernsey', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'GH', N'Ghana', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'GI', N'Gibraltar', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'GL', N'Greenland', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'GM', N'Gambia, The', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'GN', N'Guinea', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'GP', N'Guadeloupe', NULL, NULL)
GO
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'GQ', N'Equatorial Guinea', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'GR', N'Greece', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'GS', N'South Georgia and The South Sandwich Islands', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'GT', N'Guatemala', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'GU', N'Guam', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'GW', N'Guinea-Bissau', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'GY', N'Guyana', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'HK', N'Hong Kong (Special Administrative Region of China) [Hong Kong]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'HN', N'Honduras', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'HR', N'Croatia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'HT', N'Haiti', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'HU', N'Hungary', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'IC', N'Canary Islands', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'ID', N'Indonesia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'IE', N'Ireland', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'IL', N'Israel', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'IM', N'Isle of Man', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'IN', N'India', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'IQ', N'Iraq', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'IR', N'Iran [Iran, Islamic Republic of]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'IS', N'Iceland', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'IT', N'Italy {Includes Sardinia, Sicily}', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'JE', N'Jersey', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'JM', N'Jamaica', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'JO', N'Jordan', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'JP', N'Japan', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'KE', N'Kenya', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'KG', N'Kyrgyzstan', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'KH', N'Cambodia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'KI', N'Kiribati', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'KM', N'Comoros', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'KN', N'St Kitts and Nevis', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'KP', N'Korea (North) [Korea, Democratic People''s Republic of]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'KR', N'Korea (South) [Korea, Republic of]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'KW', N'Kuwait', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'KY', N'Cayman Islands', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'KZ', N'Kazakhstan', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'LA', N'Laos [Lao People''s Democratic Republic]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'LB', N'Lebanon', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'LC', N'St Lucia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'LI', N'Liechtenstein', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'LK', N'Sri Lanka', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'LR', N'Liberia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'LS', N'Lesotho', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'LT', N'Lithuania', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'LU', N'Luxembourg', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'LV', N'Latvia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'LY', N'Libya [Libyan Arab Jamahiriya]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MA', N'Morocco', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MC', N'Monaco', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MD', N'Moldova [Moldova, Republic of]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'ME', N'Montenegro', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MF', N'St Martin (French Part) [St Martin]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MG', N'Madagascar', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MH', N'Marshall Islands', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MK', N'Macedonia [Macedonia, The Former Yugoslav Republic of]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'ML', N'Mali', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MM', N'Burma [Myanmar]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MN', N'Mongolia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MO', N'Macao (Special Administrative Region of China) [Macao]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MP', N'Northern Mariana Islands', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MQ', N'Martinique', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MR', N'Mauritania', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MS', N'Montserrat', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MT', N'Malta', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MU', N'Mauritius', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MV', N'Maldives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MW', N'Malawi', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MX', N'Mexico', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MY', N'Malaysia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'MZ', N'Mozambique', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'NA', N'Namibia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'NC', N'New Caledonia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'NE', N'Niger', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'NF', N'Norfolk Island', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'NG', N'Nigeria', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'NI', N'Nicaragua', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'NL', N'Netherlands', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'NO', N'Norway', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'NP', N'Nepal', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'NR', N'Nauru', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'NU', N'Niue', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'NZ', N'New Zealand', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'OM', N'Oman', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'PA', N'Panama', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'PE', N'Peru', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'PF', N'French Polynesia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'PG', N'Papua New Guinea', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'PH', N'Philippines', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'PK', N'Pakistan', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'PL', N'Poland', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'PM', N'St Pierre and Miquelon', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'PN', N'Pitcairn, Henderson, Ducie and Oeno Islands [Pitcairn]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'PR', N'Puerto Rico', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'PS', N'Occupied Palestinian Territories [Palestinian Territory, Occupied] {formerly West Bank (including East Jerusalem) and Gaza Strip}', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'PT', N'Portugal {includes Madeira, Azores}', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'PW', N'Palau', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'PY', N'Paraguay', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'QA', N'Qatar', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'QO', N'Kosovo', NULL, NULL)
GO
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'RE', N'Reunion', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'RO', N'Romania', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'RS', N'Serbia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'RU', N'Russia [Russian Federation]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'RW', N'Rwanda', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SA', N'Saudi Arabia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SB', N'Solomon Islands', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SC', N'Seychelles', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SD', N'Sudan', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SE', N'Sweden', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SG', N'Singapore', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SH', N'St Helena, Ascension and Tristan da Cunha', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SI', N'Slovenia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SJ', N'Svalbard and Jan Mayen', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SK', N'Slovakia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SL', N'Sierra Leone', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SM', N'San Marino', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SN', N'Senegal', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SO', N'Somalia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SR', N'Surinam [Suriname]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SS', N'South Sudan', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'ST', N'Sao Tome and Principe', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SV', N'El Salvador', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SX', N'Sint Maarten (Dutch part)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SY', N'Syria [Syrian Arab Republic]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'SZ', N'Swaziland', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'TC', N'Turks and Caicos Islands', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'TD', N'Chad', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'TG', N'Togo', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'TH', N'Thailand', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'TJ', N'Tajikistan', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'TK', N'Tokelau', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'TL', N'East Timor [Timor Leste]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'TM', N'Turkmenistan', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'TN', N'Tunisia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'TO', N'Tonga', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'TR', N'Turkey', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'TT', N'Trinidad and Tobago', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'TV', N'Tuvalu', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'TW', N'Taiwan [Taiwan, Province of China]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'TZ', N'Tanzania [Tanzania, United Republic of]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'UA', N'Ukraine', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'UG', N'Uganda', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'US', N'United States', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'UY', N'Uruguay', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'UZ', N'Uzbekistan', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'VA', N'Vatican City [Holy See (Vatican City State)]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'VC', N'St Vincent and The Grenadines', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'VE', N'Venezuela', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'VG', N'British Virgin Islands [Virgin Islands, British]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'VI', N'United States Virgin Islands [Virgin Islands, U. S.]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'VN', N'Vietnam [Viet Nam]', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'VU', N'Vanuatu', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'WF', N'Wallis and Futuna', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'WS', N'Samoa', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'XA', N'Cyprus (European Union)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'XB', N'Cyprus (Non-European Union)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'XC', N'Cyprus not otherwise specified', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'XF', N'England', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'XG', N'Northern Ireland', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'XH', N'Scotland', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'XI', N'Wales', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'XK', N'United Kingdom, not otherwise specified', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'XL', N'Channel Islands not otherwise specified', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'XP', N'Europe not otherwise specified', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'XQ', N'Africa not otherwise specified', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'XR', N'Middle East not otherwise specified', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'XS', N'Asia (Except Middle East) not otherwise specified', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'XT', N'North America not otherwise specified', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'XU', N'Central America not otherwise specified', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'XV', N'South America not otherwise specified', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'XW', N'Caribbean not otherwise specified', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'XX', N'Antarctica and Oceania not otherwise specified', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'YE', N'Yemen', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'YT', N'Mayotte', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'ZA', N'South Africa', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'ZM', N'Zambia', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'ZW', N'Zimbabwe', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Domicile', N'ZZ', N'Not known', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ELQ', N'1', N'Non-exempt ELQ', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ELQ', N'2', N'Exempt ELQ', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ELQ', N'3', N'Not ELQ', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ELQ', N'9', N'Not required', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'EmpOutcome', N'1', N'Employment outcome (with training) gained on eligible funded programme', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'EmpOutcome', N'2', N'Employment outcome (without training) gained on eligible funded programme', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'EmpStat', N'10', N'In paid employment', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'EmpStat', N'11', N'Not in paid employment, looking for work and available to start work', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'EmpStat', N'12', N'Not in paid employment, and not looking for work and/or not available to start work', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'EmpStat', N'98', N'Not known/not provided', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'BSI', N'Benefit status indicator', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'EII', N'Employment intensity indicator', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'LOE', N'Length of employment', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'LOU', N'Length of unemployment', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'PEI', N'Previous education indicator', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'SEI', N'Self employment indicator', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'SEM', N'Small employer', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Ethnicity', N'31', N'English / Welsh / Scottish / Northern Irish / British', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Ethnicity', N'32', N'Irish', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Ethnicity', N'33', N'Gypsy or Irish Traveller', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Ethnicity', N'34', N'Any Other White background', NULL, NULL)
GO
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Ethnicity', N'35', N'White and Black Caribbean', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Ethnicity', N'36', N'White and Black African', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Ethnicity', N'37', N'White and Asian', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Ethnicity', N'38', N'Any Other Mixed / multiple ethnic background', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Ethnicity', N'39', N'Indian', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Ethnicity', N'40', N'Pakistani', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Ethnicity', N'41', N'Bangladeshi', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Ethnicity', N'42', N'Chinese', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Ethnicity', N'43', N'Any other Asian background', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Ethnicity', N'44', N'African', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Ethnicity', N'45', N'Caribbean', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Ethnicity', N'46', N'Any other Black / African / Caribbean background', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Ethnicity', N'47', N'Arab', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Ethnicity', N'98', N'Any other ethnic group', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Ethnicity', N'99', N'Not provided', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FinType', N'1', N'Cash', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FinType', N'2', N'Near cash', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FinType', N'3', N'Accommodation discounts', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FinType', N'4', N'Other', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FundComp', N'1', N'Completed the current year of programme of study', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FundComp', N'2', N'Did not complete the current year of programme of study', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FundComp', N'3', N'Year of programme of study not yet completed, but has not failed to complete', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FundComp', N'9', N'Not in HEIFES population', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FundLev', N'10', N'Undergraduate', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FundLev', N'11', N'Long undergraduate', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FundLev', N'20', N'Postgraduate taught', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FundLev', N'21', N'Long postgraduate taught', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FundLev', N'30', N'Postgraduate research', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FundLev', N'31', N'Long postgraduate research', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FundLev', N'99', N'Not in HEIFES population', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FundModel', N'10', N'Community Learning', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FundModel', N'25', N'16-19 (excluding Apprenticeships)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FundModel', N'35', N'Adult skills', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FundModel', N'36', N'Apprenticeships (from 1 May 2017)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FundModel', N'70', N'ESF', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FundModel', N'81', N'Other Adult', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FundModel', N'82', N'Other 16-19', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'FundModel', N'99', N'Non-funded (No ESFA funding for this learning aim)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'1', N'1', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'2', N'2', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'3', N'3', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'4', N'4', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'5', N'5', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'6', N'6', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'7', N'7', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'8', N'8', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'9', N'9', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'A', N'A', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'A*', N'A*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'A*A', N'A*A', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'A*A*', N'A*A*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'AA', N'AA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'AB', N'AB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'B', N'B', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'BB', N'BB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'BC', N'BC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'C', N'C', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'CC', N'CC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'CD', N'CD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'D', N'D', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'DD', N'DD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'DE', N'DE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'E', N'E', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'EE', N'EE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'EF', N'EF', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'F', N'F', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'FF', N'FF', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'FG', N'FG', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'G', N'G', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'GG', N'GG', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'N', N'N', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'NONE', N'NONE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'GCSEGrade', N'U', N'U', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'ACT', N'Apprenticeship contract type', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'ADL', N'Advanced Learner Loans indicator', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'ALB', N'Advanced Learner Loans Bursary funding', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'ASL', N'Adult Safeguarded Learning', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'Devolved area monitoring', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'EEF', N'Eligibility for enhanced Apprenticeship funding', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'FFI', N'Full or co-funding indicator', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'FLN', N'Family English, Maths and Language', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'HEM', N'HE monitoring', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'HHS', N'Household situation', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'Learning delivery monitoring', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LSF', N'Learning support funding', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'National Skills Academy indicator', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'POD', N'Percentage of online delivery', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'RES', N'Restart indicator', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'SOF', N'Source of funding', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'WPP', N'Work programme participation', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'DLA', N'Disabled students allowance', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'ECF', N'GCSE English condition of funding', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'EDF', N'Eligibility for 16-19 (excluding Apprenticeships) disadvantage funding', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'EHC', N'Education Health Care plan', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'FME', N'Free meals eligiblity', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'HNS', N'High needs students', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'LSR', N'Learner support reason', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'MCF', N'GCSE maths condition of funding', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'NLM', N'National learner monitoring', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'PPE', N'Pupil premium funding eligiblity', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'SEN', N'Special educational needs', NULL, NULL)
GO
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0001', N'LearningAimType 0001', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0002', N'LearningAimType 0002', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'LearningAimType 0003', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1413', N'LearningAimType 1413', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'LearningAimType 1422', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1430', N'LearningAimType 1430', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1433', N'LearningAimType 1433', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1434', N'LearningAimType 1434', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1435', N'LearningAimType 1435', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1453', N'LearningAimType 1453', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'LearningAimType 2999', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'1', N'Emotional/behavioural difficulties', CAST(N'1900-01-01' AS Date), CAST(N'2015-07-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'10', N'Moderate learning difficulty', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'11', N'Severe learning difficulty', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'12', N'Dyslexia', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'13', N'Dyscalculia', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'14', N'Autism spectrum disorder', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'15', N'Asperger''s syndrome', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'16', N'Temporary disability after illness (for example post-viral) or accident', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'17', N'Speech, Language and Communication Needs', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'2', N'Multiple disabilities', CAST(N'1900-01-01' AS Date), CAST(N'2015-07-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'3', N'Multiple learning difficulties', CAST(N'1900-01-01' AS Date), CAST(N'2015-07-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'4', N'Visual impairment', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'5', N'Hearing impairment', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'6', N'Disability affecting mobility', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'7', N'Profound complex disabilities', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'8', N'Social and emotional difficulties', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'9', N'Mental health difficulty', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'93', N'Other physical disability', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'94', N'Other specific learning difficulty (e.g. Dyspraxia)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'95', N'Other medical condition (for example epilepsy, asthma, diabetes)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'96', N'Other learning difficulty', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'97', N'Other disability', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'98', N'Prefer not to say', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDCat', N'99', N'Not provided', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDHealthProb', N'1', N'Learner considers himself or herself to have a learning difficulty and/or disability and/or health problem', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDHealthProb', N'2', N'Learner does not consider himself or herself to have a learning difficulty and/or disability and/or health problem', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LLDDHealthProb', N'9', N'No information provided by the learner', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LocType', N'1', N'Postal Address', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LocType', N'2', N'Post Code', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LocType', N'3', N'Telephone', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LocType', N'4', N'Email Address', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ModeStud', N'1', N'Full-time and sandwich', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ModeStud', N'2', N'Sandwich year-out', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ModeStud', N'3', N'Part-time', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ModeStud', N'99', N'Not in Early Statistics/HESES population', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'1', N'No award or financial backing', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'11', N'Research Council - BBSRC', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'12', N'Research Council - MRC', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'13', N'Research Council - NERC', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'14', N'Research Council - EPSRC', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'15', N'Research Council - ESRC', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'17', N'Arts and Humanities Research Council', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'18', N'Science & Technology Facilities Council (STFC)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'19', N'Research Council - not specified', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'2', N'English or Welsh LEA award', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'22', N'International agency', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'23', N'Cancer Research UK', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'24', N'Wellcome Trust', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'25', N'Other AMRC charity', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'26', N'Other charitable foundation', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'3', N'Student Awards Agency for Scotland (SAAS)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'31', N'Departments of Health/NHS/Social Care', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'32', N'Departments of Social Services', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'33', N'BIS', CAST(N'1900-01-01' AS Date), CAST(N'2018-07-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'34', N'Other HM government departments/public bodies', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'35', N'Scholarship of HM forces', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'36', N'Scottish Enterprise/Highlands and Islands Enterprise/Training Enterprise Council/Local Enterprise Company', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'37', N'LEA training grants scheme', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'38', N'Department of Agriculture & Rural Development for Northern Ireland (DARD)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'39', N'Scottish Local Authority discretionary award', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'4', N'DELNI/Northern Ireland Education and Library Boards', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'41', N'EU Commission (EC)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'42', N'Overseas learner award from HM government/British Council', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'43', N'Overseas government', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'44', N'Overseas Development Administration', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'45', N'Overseas institution', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'46', N'Overseas industry or commerce', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'47', N'Other overseas funding', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'48', N'Other overseas - repayable loan', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'49', N'ORSAS', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'5', N'Provider waiver of support costs', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'52', N'Mix of learner & SLC', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'53', N'Mix of learner & SAAS/SLC', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'54', N'Mix of learner & DELNI/NIELB', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'6', N'Local Government - Channel Islands and Isle of Man', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'61', N'UK industry/commerce', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'7', N'Fee waiver under government unemployed learners scheme', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'71', N'Absent for a year', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'8', N'British Academy', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'81', N'Learner''s employer', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'9', N'Part-time graduate apprentice study programme', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'96', N'FE student New Deal', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'97', N'Other', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'98', N'No fees', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'MSTuFee', N'99', N'Not known', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Outcome', N'1', N'Achieved', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Outcome', N'2', N'Partial achievement', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Outcome', N'3', N'No achievement', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Outcome', N'8', N'Learning activities are complete but the outcome is not yet known', NULL, NULL)
GO
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'*', N'*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'**', N'**', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'***', N'***', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'***D', N'***D', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'**D', N'**D', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'*A', N'*A', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'*B', N'*B', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'*C', N'*C', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'*D', N'*D', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'*DD', N'*DD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'*E', N'*E', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'*F', N'*F', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'*G', N'*G', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'01', N'Percentage mark 01', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'02', N'Percentage mark 02', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'03', N'Percentage mark 03', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'04', N'Percentage mark 04', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'05', N'Percentage mark 05', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'06', N'Percentage mark 06', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'07', N'Percentage mark 07', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'08', N'Percentage mark 08', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'09', N'Percentage mark 09', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'1', N'1', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'10', N'Percentage mark 10', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'11', N'Percentage mark 11', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'12', N'Percentage mark 12', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'13', N'Percentage mark 13', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'14', N'Percentage mark 14', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'15', N'Percentage mark 15', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'16', N'Percentage mark 16', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'17', N'Percentage mark 17', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'18', N'Percentage mark 18', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'19', N'Percentage mark 19', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'2', N'2', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'20', N'Percentage mark 20', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'21', N'Percentage mark 21', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'22', N'Percentage mark 22', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'23', N'Percentage mark 23', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'24', N'Percentage mark 24', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'25', N'Percentage mark 25', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'26', N'Percentage mark 26', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'27', N'Percentage mark 27', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'28', N'Percentage mark 28', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'29', N'Percentage mark 29', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'3', N'3', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'30', N'Percentage mark 30', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'31', N'Percentage mark 31', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'32', N'Percentage mark 32', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'33', N'Percentage mark 33', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'34', N'Percentage mark 34', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'35', N'Percentage mark 35', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'36', N'Percentage mark 36', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'37', N'Percentage mark 37', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'38', N'Percentage mark 38', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'39', N'Percentage mark 39', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'4', N'4', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'40', N'Percentage mark 40', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'41', N'Percentage mark 41', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'42', N'Percentage mark 42', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'43', N'Percentage mark 43', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'44', N'Percentage mark 44', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'45', N'Percentage mark 45', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'46', N'Percentage mark 46', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'47', N'Percentage mark 47', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'48', N'Percentage mark 48', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'49', N'Percentage mark 49', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'5', N'5', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'50', N'Percentage mark 50', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'51', N'Percentage mark 51', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'52', N'Percentage mark 52', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'53', N'Percentage mark 53', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'54', N'Percentage mark 54', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'55', N'Percentage mark 55', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'56', N'Percentage mark 56', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'57', N'Percentage mark 57', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'58', N'Percentage mark 58', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'59', N'Percentage mark 59', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'6', N'6', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'60', N'Percentage mark 60', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'61', N'Percentage mark 61', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'62', N'Percentage mark 62', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'63', N'Percentage mark 63', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'64', N'Percentage mark 64', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'65', N'Percentage mark 65', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'66', N'Percentage mark 66', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'67', N'Percentage mark 67', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'68', N'Percentage mark 68', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'69', N'Percentage mark 69', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'7', N'7', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'70', N'Percentage mark 70', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'71', N'Percentage mark 71', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'72', N'Percentage mark 72', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'73', N'Percentage mark 73', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'74', N'Percentage mark 74', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'75', N'Percentage mark 75', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'76', N'Percentage mark 76', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'77', N'Percentage mark 77', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'78', N'Percentage mark 78', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'79', N'Percentage mark 79', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'8', N'8', NULL, NULL)
GO
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'80', N'Percentage mark 80', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'81', N'Percentage mark 81', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'82', N'Percentage mark 82', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'83', N'Percentage mark 83', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'84', N'Percentage mark 84', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'85', N'Percentage mark 85', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'86', N'Percentage mark 86', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'87', N'Percentage mark 87', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'88', N'Percentage mark 88', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'89', N'Percentage mark 89', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'9', N'9', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'90', N'Percentage mark 90', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'91', N'Percentage mark 91', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'92', N'Percentage mark 92', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'93', N'Percentage mark 93', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'94', N'Percentage mark 94', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'95', N'Percentage mark 95', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'96', N'Percentage mark 96', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'97', N'Percentage mark 97', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'98', N'Percentage mark 98', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'99', N'Percentage mark 99', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'A', N'A', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'A*', N'A*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'A*A', N'A*A', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'A*A*', N'A*A*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'AA', N'AA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'AA*', N'AA*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'AAA', N'AAA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'AAB', N'AAB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'AAC', N'AAC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'AAD', N'AAD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'AAE', N'AAE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'AB', N'AB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ABA', N'ABA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ABB', N'ABB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ABC', N'ABC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ABD', N'ABD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ABE', N'ABE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'AC', N'AC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ACA', N'ACA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ACB', N'ACB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ACC', N'ACC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ACD', N'ACD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ACE', N'ACE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'AD', N'AD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ADA', N'ADA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ADB', N'ADB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ADC', N'ADC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ADD', N'ADD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ADE', N'ADE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'AE', N'AE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'AEA', N'AEA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'AEB', N'AEB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'AEC', N'AEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'AED', N'AED', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'AEE', N'AEE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'AF', N'AF', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'AG', N'AG', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'B', N'B', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'B*', N'B*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BA', N'BA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BAA', N'BAA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BAB', N'BAB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BAC', N'BAC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BAD', N'BAD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BAE', N'BAE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BB', N'BB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BBA', N'BBA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BBB', N'BBB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BBC', N'BBC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BBD', N'BBD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BBE', N'BBE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BC', N'BC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BCA', N'BCA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BCB', N'BCB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BCC', N'BCC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BCD', N'BCD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BCE', N'BCE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BD', N'BD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BDA', N'BDA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BDB', N'BDB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BDC', N'BDC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BDD', N'BDD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BDE', N'BDE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BE', N'BE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BEA', N'BEA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BEB', N'BEB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BEC', N'BEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BED', N'BED', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BEE', N'BEE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BF', N'BF', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'BG', N'BG', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'C', N'C', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'C*', N'C*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CA', N'CA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CAA', N'CAA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CAB', N'CAB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CAC', N'CAC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CAD', N'CAD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CAE', N'CAE', NULL, NULL)
GO
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CB', N'CB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CBA', N'CBA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CBB', N'CBB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CBC', N'CBC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CBD', N'CBD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CBE', N'CBE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CC', N'CC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CCA', N'CCA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CCB', N'CCB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CCC', N'CCC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CCD', N'CCD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CCE', N'CCE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CD', N'CD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CDA', N'CDA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CDB', N'CDB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CDC', N'CDC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CDD', N'CDD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CDE', N'CDE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CE', N'CE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CEA', N'CEA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CEB', N'CEB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CEC', N'CEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CED', N'CED', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CEE', N'CEE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CF', N'CF', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CG', N'CG', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'CR', N'Credit', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D', N'D', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*', N'D*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D**', N'D**', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D***', N'D***', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*D', N'D*D', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*D*', N'D*D*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*D*D', N'D*D*D', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*D*D*', N'D*D*D*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*D*M', N'D*D*M', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*D*P', N'D*D*P', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*DD', N'D*DD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*DD*', N'D*DD*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*DM', N'D*DM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*DP', N'D*DP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*M', N'D*M', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*MD', N'D*MD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*MD*', N'D*MD*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*MM', N'D*MM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*MP', N'D*MP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*P', N'D*P', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*PD', N'D*PD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*PD*', N'D*PD*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*PM', N'D*PM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D*PP', N'D*PP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D1', N'D1', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D2', N'D2', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'D3', N'D3', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DA', N'DA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DAA', N'DAA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DAB', N'DAB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DAC', N'DAC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DAD', N'DAD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DAE', N'DAE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DB', N'DB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DBA', N'DBA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DBB', N'DBB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DBC', N'DBC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DBD', N'DBD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DBE', N'DBE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DC', N'DC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DCA', N'DCA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DCB', N'DCB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DCC', N'DCC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DCD', N'DCD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DCE', N'DCE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DD', N'DD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DD*', N'DD*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DD*D', N'DD*D', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DD*D*', N'DD*D*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DD*M', N'DD*M', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DD*P', N'DD*P', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DDA', N'DDA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DDB', N'DDB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DDC', N'DDC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DDD', N'DDD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DDD*', N'DDD*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DDDD', N'DDDD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DDDM', N'DDDM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DDDP', N'DDDP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DDE', N'DDE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DDM', N'DDM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DDMD', N'DDMD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DDMM', N'DDMM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DDMP', N'DDMP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DDP', N'DDP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DDPD', N'DDPD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DDPM', N'DDPM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DDPP', N'DDPP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DE', N'DE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DEA', N'DEA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DEB', N'DEB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DEC', N'DEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DED', N'DED', NULL, NULL)
GO
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DEE', N'DEE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DF', N'DF', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DG', N'DG', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DM', N'DM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DMD', N'DMD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DMD*', N'DMD*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DMDD', N'DMDD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DMDM', N'DMDM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DMDP', N'DMDP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DMM', N'DMM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DMMD', N'DMMD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DMMM', N'DMMM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DMMP', N'DMMP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DMP', N'DMP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DMPD', N'DMPD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DMPM', N'DMPM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DMPP', N'DMPP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DP', N'DP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DPD', N'DPD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DPD*', N'DPD*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DPDD', N'DPDD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DPDM', N'DPDM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DPDP', N'DPDP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DPM', N'DPM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DPMD', N'DPMD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DPMM', N'DPMM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DPMP', N'DPMP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DPP', N'DPP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DPPD', N'DPPD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DPPM', N'DPPM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DPPP', N'DPPP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DS', N'Distinction', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'DS*', N'Distinction *', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'E', N'E', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'E*', N'E*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EA', N'EA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EAA', N'EAA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EAB', N'EAB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EAC', N'EAC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EAD', N'EAD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EAE', N'EAE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EB', N'EB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EBA', N'EBA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EBB', N'EBB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EBC', N'EBC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EBD', N'EBD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EBE', N'EBE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EC', N'EC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ECA', N'ECA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ECB', N'ECB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ECC', N'ECC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ECD', N'ECD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ECE', N'ECE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ED', N'ED', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EDA', N'EDA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EDB', N'EDB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EDC', N'EDC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EDD', N'EDD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EDE', N'EDE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EE', N'EE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EEA', N'EEA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EEB', N'EEB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EEC', N'EEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EED', N'EED', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EEE', N'EEE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EF', N'EF', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EG', N'EG', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EL1', N'Achievement at entry level 1', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EL2', N'Achievement at entry level 2', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'EL3', N'Achievement at entry level 3', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'F', N'F', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'F*', N'F*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'FA', N'FA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'FB', N'FB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'FC', N'FC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'FD', N'FD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'FE', N'FE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'FF', N'FF', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'FG', N'FG', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'FI', N'First class honours', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'FL', N'Fail', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'FO', N'Fourth class honours', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'G', N'G', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'G*', N'G*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'GA', N'GA', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'GB', N'GB', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'GC', N'GC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'GD', N'GD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'GE', N'GE', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'GF', N'GF', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'GG', N'GG', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'GN', N'General', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'L1', N'L1', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'L2', N'L2', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'L2D', N'L2D', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'L2D*', N'L2D*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'L2D*D', N'L2D*D', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'L2D*D*', N'L2D*D*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'L2DD', N'L2DD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'L2DM', N'L2DM', NULL, NULL)
GO
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'L2M', N'L2M', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'L2MM', N'L2MM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'L2MP', N'L2MP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'L2P', N'L2P', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'L2PP', N'L2PP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'M1', N'M1', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'M2', N'M2', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'M3', N'M3', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MD', N'MD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MD*', N'MD*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MD*D', N'MD*D', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MD*D*', N'MD*D*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MD*M', N'MD*M', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MD*P', N'MD*P', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MDD', N'MDD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MDD*', N'MDD*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MDDD', N'MDDD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MDDM', N'MDDM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MDDP', N'MDDP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MDM', N'MDM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MDMD', N'MDMD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MDMM', N'MDMM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MDMP', N'MDMP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MDP', N'MDP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MDPD', N'MDPD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MDPM', N'MDPM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MDPP', N'MDPP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'ME', N'Merit', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MM', N'MM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MMD', N'MMD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MMD*', N'MMD*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MMDD', N'MMDD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MMDM', N'MMDM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MMDP', N'MMDP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MMM', N'MMM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MMMD', N'MMMD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MMMM', N'MMMM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MMMP', N'MMMP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MMP', N'MMP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MMPD', N'MMPD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MMPM', N'MMPM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MMPP', N'MMPP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MP', N'MP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MPD', N'MPD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MPD*', N'MPD*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MPDD', N'MPDD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MPDM', N'MPDM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MPDP', N'MPDP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MPM', N'MPM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MPMD', N'MPMD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MPMM', N'MPMM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MPMP', N'MPMP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MPP', N'MPP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MPPD', N'MPPD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MPPM', N'MPPM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'MPPP', N'MPPP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'N', N'N', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'OR', N'Ordinary', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'OTH', N'Other grade not included on the list above', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'P1', N'P1', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'P2', N'P2', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'P3', N'P3', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PA', N'Pass', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PD', N'PD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PD*', N'PD*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PD*D', N'PD*D', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PD*D*', N'PD*D*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PD*M', N'PD*M', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PD*P', N'PD*P', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PDD', N'PDD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PDD*', N'PDD*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PDDD', N'PDDD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PDDM', N'PDDM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PDDP', N'PDDP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PDM', N'PDM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PDMD', N'PDMD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PDMM', N'PDMM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PDMP', N'PDMP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PDP', N'PDP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PDPD', N'PDPD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PDPM', N'PDPM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PDPP', N'PDPP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PM', N'PM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PMD', N'PMD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PMD*', N'PMD*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PMDD', N'PMDD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PMDM', N'PMDM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PMDP', N'PMDP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PMM', N'PMM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PMMD', N'PMMD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PMMM', N'PMMM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PMMP', N'PMMP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PMP', N'PMP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PMPD', N'PMPD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PMPM', N'PMPM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PMPP', N'PMPP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PP', N'PP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PPD', N'PPD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PPD*', N'PPD*', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PPDD', N'PPDD', NULL, NULL)
GO
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PPDM', N'PPDM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PPDP', N'PPDP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PPM', N'PPM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PPMD', N'PPMD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PPMM', N'PPMM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PPMP', N'PPMP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PPP', N'PPP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PPPD', N'PPPD', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PPPM', N'PPPM', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'PPPP', N'PPPP', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'SE', N'Undivided second class honours', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'SL', N'Lower second class honours', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'SU', N'Upper second class honours', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'TH', N'Third class honours', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'U', N'U', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'UH', N'Unclassified honours', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'X', N'X', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutGrade', N'Y', N'Y', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'EDU', N'Education', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'EMP', N'In Paid Employment', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'GAP', N'Gap Year', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'NPE', N'Not in Paid Employment', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'OTH', N'Other', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'SDE', N'Social Destination (High needs students only)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'VOL', N'Voluntary work', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'PriorAttain', N'1', N'Level 1', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'PriorAttain', N'10', N'Level 4', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'PriorAttain', N'11', N'Level 5', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'PriorAttain', N'12', N'Level 6', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'PriorAttain', N'13', N'Level 7 and above', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'PriorAttain', N'2', N'Full level 2', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'PriorAttain', N'3', N'Full level 3', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'PriorAttain', N'4', N'Level 4', CAST(N'1900-01-01' AS Date), CAST(N'2013-07-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'PriorAttain', N'5', N'Level 5 and above', CAST(N'1900-01-01' AS Date), CAST(N'2013-07-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'PriorAttain', N'7', N'Other qualifications below level 1', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'PriorAttain', N'9', N'Entry level', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'PriorAttain', N'97', N'Other qualification, level not known', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'PriorAttain', N'98', N'Not known', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'PriorAttain', N'99', N'No qualifications', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ProgType', N'2', N'Advanced Level Apprenticeship', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ProgType', N'20', N'Higher Apprenticeship - level 4', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ProgType', N'21', N'Higher Apprenticeship - level 5', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ProgType', N'22', N'Higher Apprenticeship – level 6', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ProgType', N'23', N'Higher Apprenticeship – level 7+', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ProgType', N'24', N'Traineeship', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ProgType', N'25', N'Apprenticeship standard', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ProgType', N'3', N'Intermediate Level Apprenticeship', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'C20', N'Certificate of Higher Education', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'C30', N'Higher National Certificate (including BTEC and SQA equivalents)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'C44', N'Higher Apprenticeships (Level 4)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'C80', N'Other Qualification at Level C', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'C90', N'Undergraduate credits', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'D80', N'Other qualification at level D', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'DUK', N'UK Doctorate degree', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'DZZ', N'Non-UK Doctorate degree', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'H11', N'First degree leading to QTS', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'H71', N'Professional Graduate Certificate in Education', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'H80', N'Other Qualification at Level H', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'HUK', N'UK First degree', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'HZZ', N'Non-UK first degree', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'J10', N'Foundation degree', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'J20', N'Diploma of Higher Education', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'J30', N'Higher National Diploma (including BTEC and SQA equivalents)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'J48', N'Certificate or diploma of education (i.e. non-graduate initial teacher training qualification)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'J49', N'Foundation course at HE level', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'J80', N'Other Qualification at Level J', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'JUK', N'UK ordinary (non-honours) first degree', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'M2X', N'Integrated undergraduate/postgraduate taught Masters degree on the enhanced / extended pattern', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'M41', N'Diploma at Level M (Postgraduate Diploma)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'M44', N'Certificate at Level M (Postgraduate Certificate)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'M71', N'Postgraduate Certificate of Education or Professional Graduate Diploma in Education', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'M80', N'Other Qualification at Level M', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'M90', N'Postgraduate credits', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'MUK', N'UK Masters degree', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'MZZ', N'Non-UK Masters degree', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'P41', N'Diploma at Level 3', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'P42', N'Certificate at Level 3', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'P46', N'Award at Level 3', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'P47', N'AQA Baccalaureate', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'P50', N'GCE and VCE A/AS Level', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'P51', N'14-19 Advanced Diploma (Level 3)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'P53', N'Scottish Baccalaureate', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'P54', N'Scottish Highers / Advanced Highers', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'P62', N'International Baccalaureate (IB) Diploma', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'P63', N'International Baccalaureate (IB) Certificate', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'P64', N'Cambridge Pre-U Diploma', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'P65', N'Cambridge Pre-U Certificate', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'P68', N'Welsh Baccalaureate Advanced Diploma (Level 3)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'P70', N'Professional Qualification at Level 3', CAST(N'1900-01-01' AS Date), CAST(N'2013-07-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'P80', N'Other Qualification at Level 3', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'P91', N'Mixed Level 3 qualifications of which some or all are subject to Tariff', CAST(N'1900-01-01' AS Date), CAST(N'2014-07-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'P92', N'Mixed Level 3 qualifications of which none are subject to Tariff', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'P93', N'Level 3 qualifications of which all are subject to UCAS Tariff', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'P94', N'Level 3 qualifications of which some are subject to UCAS Tariff', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'Q51', N'14-19 Higher Diploma (Level 2)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'Q52', N'Welsh Baccalaureate Intermediate Diploma (Level 2)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'Q80', N'Other Qualification at Level 2', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'R51', N'14-19 Foundation Diploma (Level 1)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'R52', N'Welsh Baccalaureate Foundation Diploma (Level 1)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
GO
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'R80', N'Other Qualification at Level 1', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'X00', N'HE Access Course, QAA recognised', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'X01', N'HE Access Course, not QAA recognised', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'X02', N'Mature student admitted on basis of previous experience and/or admissions test', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'X04', N'Other qualification level not known', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'X05', N'Student has no formal qualification', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'QualEnt3', N'X06', N'Not known', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SEC', N'1', N'Higher managerial and professional occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SEC', N'2', N'Lower managerial and professional occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SEC', N'3', N'Intermediate occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SEC', N'4', N'Small employers and own-account workers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SEC', N'5', N'Lower supervisory and technical occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SEC', N'6', N'Semi-routine occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SEC', N'7', N'Routine occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SEC', N'8', N'Never worked and long term unemployed', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SEC', N'9', N'Not classified', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Sex', N'F', N'Female', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'Sex', N'M', N'Male', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'0', N'Not stated', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1', N'Information refused', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1111', N'Senior officials in national government', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1112', N'Directors and chief executives of major organisations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1113', N'Senior officials in local government', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1114', N'Senior officials of special interest organisations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1121', N'Production, works and maintenance managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1122', N'Managers in construction', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1123', N'Managers in mining and energy', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1131', N'Financial managers and chartered secretaries', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1132', N'Marketing and sales managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1133', N'Purchasing managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1134', N'Advertising and public relations managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1135', N'Personnel, training and industrial relations managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1136', N'Information and communication technology managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1137', N'Research and development managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1141', N'Quality assurance managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1142', N'Customer care managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1151', N'Financial institution managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1152', N'Office managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1161', N'Transport and distribution managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1162', N'Storage and warehouse managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1163', N'Retail and wholesale managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1171', N'Officers in armed forces', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1172', N'Police officers (inspectors and above)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1173', N'Senior officers in fire, ambulance, prison and related services', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1174', N'Security managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1181', N'Hospital and health service managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1182', N'Pharmacy managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1183', N'Healthcare practice managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1184', N'Social services managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1185', N'Residential and day care managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1211', N'Farm managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1212', N'Natural environment and conservation managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1219', N'Managers in animal husbandry, forestry and fishing NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1221', N'Hotel and accommodation managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1222', N'Conference and exhibition managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1223', N'Restaurant and catering managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1224', N'Publicans and managers of licensed premises', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1225', N'Leisure and sports managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1226', N'Travel agency managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1231', N'Property, housing and land managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1232', N'Garage managers and proprietors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1233', N'Hairdressing and beauty salon managers and proprietors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1234', N'Shopkeepers and wholesale/retail dealers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1235', N'Recycling and refuse disposal managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'1239', N'Managers and proprietors in other services NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2111', N'Chemists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2112', N'Biological scientists and biochemists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2113', N'Physicists, geologists and meteorologists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2121', N'Civil engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2122', N'Mechanical engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2123', N'Electrical engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2124', N'Electronics engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2125', N'Chemical engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2126', N'Design and development engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2127', N'Production and process engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2128', N'Planning and quality control engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2129', N'Engineering professionals NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2131', N'IT strategy and planning professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2132', N'Software professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2211', N'Medical practitioners', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2212', N'Psychologists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2213', N'Pharmacists/Pharmacologists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2214', N'Ophthalmic opticians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2215', N'Dental practitioners', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2216', N'Veterinarians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2311', N'Higher education teaching professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2312', N'Further education teaching professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2313', N'Education officers, school inspectors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2314', N'Secondary education teaching professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2315', N'Primary and nursery education teaching professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2316', N'Special needs education teaching professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2317', N'Registrars and senior administrators of educational establishments', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2319', N'Teaching professionals NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2321', N'Scientific researchers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2322', N'Social science researchers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2329', N'Researchers NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2411', N'Solicitors and lawyers, judges and coroners', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2419', N'Legal professionals NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2421', N'Chartered and certified accountants', NULL, NULL)
GO
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2422', N'Management accountants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2423', N'Management consultants, actuaries, economists and statisticians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2431', N'Architects', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2432', N'Town planners', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2433', N'Quantity surveyors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2434', N'Chartered surveyors (not quantity surveyors)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2441', N'Public service administrative professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2442', N'Social workers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2443', N'Probation officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2444', N'Clergy', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2451', N'Librarians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'2452', N'Archivists and curators', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3111', N'Laboratory technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3112', N'Electrical/electronics technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3113', N'Engineering technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3114', N'Building and civil engineering technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3115', N'Quality assurance technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3119', N'Science and engineering technicians NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3121', N'Architectural technologists and town planning technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3122', N'Draughtspersons', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3123', N'Building inspectors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3131', N'IT operations technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3132', N'IT user support technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3211', N'Nurses', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3212', N'Midwives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3213', N'Paramedics', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3214', N'Medical radiographers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3215', N'Chiropodists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3216', N'Dispensing opticians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3217', N'Pharmaceutical dispensers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3218', N'Medical and dental technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3221', N'Physiotherapists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3222', N'Occupational therapists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3223', N'Speech and language therapists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3229', N'Therapists NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3231', N'Youth and community workers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3232', N'Housing and welfare officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3311', N'NCOs and other ranks', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3312', N'Police officers (sergeant and below)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3313', N'Fire service officers (leading fire officer and below)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3314', N'Prison service officers (below principal officer)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3319', N'Protective service associate professionals NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3411', N'Artists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3412', N'Authors, writers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3413', N'Actors, entertainers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3414', N'Dancers and choreographers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3415', N'Musicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3416', N'Arts officers, producers and directors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3421', N'Graphic designers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3422', N'Product, clothing and related designers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3431', N'Journalists, newspaper and periodical editors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3432', N'Broadcasting associate professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3433', N'Public relations officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3434', N'Photographers and audio-visual equipment operators', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3441', N'Sports players', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3442', N'Sports coaches, instructors and officials', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3443', N'Fitness instructors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3449', N'Sports and fitness occupations NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3511', N'Air traffic controllers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3512', N'Aircraft pilots and flight engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3513', N'Ship and hovercraft officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3514', N'Train drivers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3520', N'Legal associate professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3531', N'Estimators, valuers and assessors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3532', N'Brokers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3533', N'Insurance underwriters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3534', N'Finance and investment analysts/advisors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3535', N'Taxation experts', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3536', N'Importers, exporters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3537', N'Financial and accounting technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3539', N'Business and related associate professionals NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3541', N'Buyers and purchasing officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3542', N'Sales representatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3543', N'Marketing associate professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3544', N'Estate agents, auctioneers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3551', N'Conservation and environmental protection officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3552', N'Countryside and park rangers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3561', N'Public service associate professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3562', N'Personnel and industrial relations officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3563', N'Vocational and industrial trainers and instructors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3564', N'Careers advisers and vocational guidance specialists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3565', N'Inspectors of factories, utilities and trading standards', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3566', N'Statutory examiners', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3567', N'Occupational hygienists and safety officers (health and safety)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'3568', N'Environmental health officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4111', N'Civil service executive officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4112', N'Civil service administrative officers and assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4113', N'Local government clerical officers and assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4114', N'Officers of non-governmental organisations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4121', N'Credit controllers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4122', N'Accounts and wages clerks, book-keepers, other financial clerks', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4123', N'Counter clerks', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4131', N'Filing and other records assistants/clerks', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4132', N'Pensions and insurance clerks', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4133', N'Stock control clerks', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4134', N'Transport and distribution clerks', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4135', N'Library assistants/clerks', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4136', N'Database assistants/clerks', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4137', N'Market research interviewers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4141', N'Telephonists', NULL, NULL)
GO
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4142', N'Communication operators', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4150', N'General office assistants/clerks', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4211', N'Medical secretaries', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4212', N'Legal secretaries', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4213', N'School secretaries', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4214', N'Company secretaries', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4215', N'Personal assistants and other secretaries', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4216', N'Receptionists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'4217', N'Typists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5111', N'Farmers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5112', N'Horticultural trades', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5113', N'Gardeners and groundsmen/groundswomen', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5119', N'Agricultural and fishing trades NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5211', N'Smiths and forge workers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5212', N'Moulders, core makers, die casters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5213', N'Sheet metal workers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5214', N'Metal plate workers, shipwrights, riveters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5215', N'Welding trades', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5216', N'Pipe fitters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5221', N'Metal machining setters and setter-operators', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5222', N'Tool makers, tool fitters and markers-out', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5223', N'Metal working production and maintenance fitters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5224', N'Precision instrument makers and repairers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5231', N'Motor mechanics, auto engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5232', N'Vehicle body builders and repairers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5233', N'Auto electricians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5234', N'Vehicle spray painters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5241', N'Electricians, electrical fitters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5242', N'Telecommunications engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5243', N'Lines repairers and cable jointers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5244', N'TV, video and audio engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5245', N'Computer engineers, installation and maintenance', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5249', N'Electrical/electronics engineers NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5311', N'Steel erectors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5312', N'Bricklayers, masons', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5313', N'Roofers, roof tilers and slaters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5314', N'Plumbers, heating and ventilating engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5315', N'Carpenters and joiners', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5316', N'Glaziers, window fabricators and fitters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5319', N'Construction trades NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5321', N'Plasterers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5322', N'Floorers and wall tilers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5323', N'Painters and decorators', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5411', N'Weavers and knitters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5412', N'Upholsterers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5413', N'Leather and related trades', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5414', N'Tailors and dressmakers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5419', N'Textiles, garments and related trades NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5421', N'Originators, compositors and print preparers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5422', N'Printers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5423', N'Bookbinders and print finishers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5424', N'Screen printers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5431', N'Butchers, meat cutters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5432', N'Bakers, flour confectioners', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5433', N'Fishmongers, poultry dressers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5434', N'Chefs, cooks', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5491', N'Glass and ceramics makers, decorators and finishers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5492', N'Furniture makers, other craft woodworkers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5493', N'Pattern makers (moulds)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5494', N'Musical instrument makers and tuners', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5495', N'Goldsmiths, silversmiths, precious stone workers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5496', N'Floral arrangers, florists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'5499', N'Hand craft occupations NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6111', N'Nursing auxiliaries and assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6112', N'Ambulance staff (excluding paramedics)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6113', N'Dental nurses', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6114', N'Houseparents and residential wardens', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6115', N'Care assistants and home carers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6121', N'Nursery nurses', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6122', N'Childminders and related occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6123', N'Playgroup leaders/assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6124', N'Educational assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6131', N'Veterinary nurses and assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6139', N'Animal care occupations NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6211', N'Sports and leisure assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6212', N'Travel agents', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6213', N'Travel and tour guides', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6214', N'Air travel assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6215', N'Rail travel assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6219', N'Leisure and travel service occupations NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6221', N'Hairdressers, barbers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6222', N'Beauticians and related occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6231', N'Housekeepers and related occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6232', N'Caretakers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6291', N'Undertakers and mortuary assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'6292', N'Pest control officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'7', N'Retired', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'7111', N'Sales and retail assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'7112', N'Retail cashiers and check-out operators', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'7113', N'Telephone salespersons', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'7121', N'Collector salespersons and credit agents', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'7122', N'Debt, rent and other cash collectors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'7123', N'Roundsmen/women and van salespersons', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'7124', N'Market and street traders and assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'7125', N'Merchandisers and window dressers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'7129', N'Sales related occupations NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'7211', N'Call centre agents/operators', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'7212', N'Customer care occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8', N'Unemployed', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8111', N'Food, drink and tobacco process operatives', NULL, NULL)
GO
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8112', N'Glass and ceramics process operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8113', N'Textile process operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8114', N'Chemical and related process operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8115', N'Rubber process operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8116', N'Plastics process operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8117', N'Metal making and treating process operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8118', N'Electroplaters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8119', N'Process operatives NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8121', N'Paper and wood machine operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8122', N'Coal mine operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8123', N'Quarry workers and related operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8124', N'Energy plant operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8125', N'Metal working machine operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8126', N'Water and sewerage plant operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8129', N'Plant and machine operatives NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8131', N'Assemblers (electrical products)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8132', N'Assemblers (vehicles and metal goods)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8133', N'Routine inspectors and testers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8134', N'Weighers, graders, sorters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8135', N'Tyre, exhaust and windscreen fitters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8136', N'Clothing cutters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8137', N'Sewing machinists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8138', N'Routine laboratory testers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8139', N'Assemblers and routine operatives NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8141', N'Scaffolders, stagers, riggers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8142', N'Road construction operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8143', N'Rail construction and maintenance operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8149', N'Construction operatives NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8211', N'Heavy goods vehicle drivers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8212', N'Van drivers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8213', N'Bus and coach drivers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8214', N'Taxi, cab drivers and chauffeurs', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8215', N'Driving instructors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8216', N'Rail transport operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8217', N'Seafarers (merchant navy); barge, lighter and boat operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8218', N'Air transport operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8219', N'Transport operatives NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8221', N'Crane drivers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8222', N'Fork-lift truck drivers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8223', N'Agricultural machinery drivers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'8229', N'Mobile machine drivers and operatives NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9', N'Not known', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9111', N'Farm workers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9112', N'Forestry workers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9119', N'Fishing and agriculture related occupations NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9121', N'Labourers in building and woodworking trades', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9129', N'Labourers in other construction trades NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9131', N'Labourers in foundries', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9132', N'Industrial cleaning process occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9133', N'Printing machine minders and assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9134', N'Packers, bottlers, canners, fillers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9139', N'Labourers in process and plant operations NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9141', N'Stevedores, dockers and slingers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9149', N'Other goods handling and storage occupations NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9211', N'Postal workers, mail sorters, messengers, couriers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9219', N'Elementary office occupations NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9221', N'Hospital porters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9222', N'Hotel porters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9223', N'Kitchen and catering assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9224', N'Waiters, waitresses', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9225', N'Bar staff', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9226', N'Leisure and theme park attendants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9229', N'Elementary personal service occupations NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9231', N'Window cleaners', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9232', N'Road sweepers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9233', N'Cleaners, domestics', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9234', N'Launderers, dry cleaners, pressers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9235', N'Refuse and salvage occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9239', N'Elementary cleaning occupations NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9241', N'Security guards and related occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9242', N'Traffic wardens', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9243', N'School crossing patrol attendants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9244', N'School midday assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9245', N'Car park attendants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9249', N'Elementary security operations NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9251', N'Shelf fillers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2000', N'9259', N'Elementary sales occupations NEC', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'0', N'Not stated', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1', N'Information refused', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1115', N'Chief executives and senior officials', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1116', N'Elected officers and representatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1121', N'Production managers and directors in manufacturing', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1122', N'Production managers and directors in construction', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1123', N'Production managers and directors in mining and energy', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1131', N'Financial managers and directors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1132', N'Marketing and sales directors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1133', N'Purchasing managers and directors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1134', N'Advertising and public relations directors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1135', N'Human resource managers and directors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1136', N'Information technology and telecommunications directors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1139', N'Functional managers and directors n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1150', N'Financial institution managers and directors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1161', N'Managers and directors in transport and distribution', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1162', N'Managers and directors in storage and warehousing', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1171', N'Officers in armed forces', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1172', N'Senior police officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1173', N'Senior officers in fire, ambulance, prison and related services', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1181', N'Health services and public health managers and directors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1184', N'Social services managers and directors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1190', N'Managers and directors in retail and wholesale', NULL, NULL)
GO
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1211', N'Managers and proprietors in agriculture and horticulture', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1213', N'Managers and proprietors in forestry, fishing and related services', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1221', N'Hotel and accommodation managers and proprietors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1223', N'Restaurant and catering establishment managers and proprietors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1224', N'Publicans and managers of licensed premises', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1225', N'Leisure and sports managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1226', N'Travel agency managers and proprietors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1241', N'Health care practice managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1242', N'Residential, day and domiciliary  care managers and proprietors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1251', N'Property, housing and estate managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1252', N'Garage managers and proprietors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1253', N'Hairdressing and beauty salon managers and proprietors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1254', N'Shopkeepers and proprietors – wholesale and retail', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1255', N'Waste disposal and environmental services managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'1259', N'Managers and proprietors in other services n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2111', N'Chemical scientists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2112', N'Biological scientists and biochemists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2113', N'Physical scientists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2114', N'Social and humanities scientists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2119', N'Natural and social science professionals n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2121', N'Civil engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2122', N'Mechanical engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2123', N'Electrical engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2124', N'Electronics engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2126', N'Design and development engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2127', N'Production and process engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2129', N'Engineering professionals n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2133', N'IT specialist managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2134', N'IT project and programme managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2135', N'IT business analysts, architects and systems designers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2136', N'Programmers and software development professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2137', N'Web design and development professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2139', N'Information technology and telecommunications professionals n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2141', N'Conservation professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2142', N'Environment professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2150', N'Research and development managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2211', N'Medical practitioners', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2212', N'Psychologists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2213', N'Pharmacists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2214', N'Ophthalmic opticians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2215', N'Dental practitioners', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2216', N'Veterinarians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2217', N'Medical radiographers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2218', N'Podiatrists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2219', N'Health professionals n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2221', N'Physiotherapists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2222', N'Occupational therapists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2223', N'Speech and language therapists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2229', N'Therapy professionals n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2231', N'Nurses', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2232', N'Midwives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2311', N'Higher education teaching professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2312', N'Further education teaching professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2314', N'Secondary education teaching professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2315', N'Primary and nursery education teaching professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2316', N'Special needs education teaching professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2317', N'Senior professionals of educational establishments', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2318', N'Education advisers and school inspectors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2319', N'Teaching and other educational professionals n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2412', N'Barristers and judges', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2413', N'Solicitors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2419', N'Legal professionals n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2421', N'Chartered and certified accountants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2423', N'Management consultants and business analysts', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2424', N'Business and financial project management professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2425', N'Actuaries, economists and statisticians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2426', N'Business and related research professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2429', N'Business, research and administrative professionals n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2431', N'Architects', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2432', N'Town planning officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2433', N'Quantity surveyors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2434', N'Chartered surveyors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2435', N'Chartered architectural technologists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2436', N'Construction project managers and related professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2442', N'Social workers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2443', N'Probation officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2444', N'Clergy', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2449', N'Welfare professionals n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2451', N'Librarians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2452', N'Archivists and curators', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2461', N'Quality control and planning engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2462', N'Quality assurance and regulatory professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2463', N'Environmental health professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2471', N'Journalists, newspaper and periodical editors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2472', N'Public relations professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'2473', N'Advertising accounts managers and creative directors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3111', N'Laboratory technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3112', N'Electrical and electronics technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3113', N'Engineering technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3114', N'Building and civil engineering technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3115', N'Quality assurance technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3116', N'Planning, process and production technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3119', N'Science, engineering and production technicians n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3121', N'Architectural and town planning technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3122', N'Draughtspersons', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3131', N'IT operations technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3132', N'IT user support technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3213', N'Paramedics', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3216', N'Dispensing opticians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3217', N'Pharmaceutical technicians', NULL, NULL)
GO
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3218', N'Medical and dental technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3219', N'Health associate professionals n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3231', N'Youth and community workers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3233', N'Child and early years officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3234', N'Housing officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3235', N'Counsellors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3239', N'Welfare and housing associate professionals n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3311', N'NCOs and other ranks', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3312', N'Police officers (sergeant and below)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3313', N'Fire service officers (watch manager and below)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3314', N'Prison service officers (below principal officer)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3315', N'Police community support officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3319', N'Protective service associate professionals n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3411', N'Artists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3412', N'Authors, writers and translators', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3413', N'Actors, entertainers and presenters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3414', N'Dancers and choreographers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3415', N'Musicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3416', N'Arts officers, producers and directors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3417', N'Photographers, audio-visual and broadcasting equipment operators', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3421', N'Graphic designers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3422', N'Product, clothing and related designers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3441', N'Sports players', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3442', N'Sports coaches, instructors and officials', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3443', N'Fitness instructors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3511', N'Air traffic controllers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3512', N'Aircraft pilots and flight engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3513', N'Ship and hovercraft officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3520', N'Legal associate professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3531', N'Estimators, valuers and assessors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3532', N'Brokers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3533', N'Insurance underwriters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3534', N'Finance and investment analysts and advisers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3535', N'Taxation experts', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3536', N'Importers and exporters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3537', N'Financial and accounting technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3538', N'Financial accounts managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3539', N'Business and related associate professionals n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3541', N'Buyers and procurement officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3542', N'Business sales executives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3543', N'Marketing associate professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3544', N'Estate agents and auctioneers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3545', N'Sales accounts and business development managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3546', N'Conference and exhibition managers and organisers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3550', N'Conservation and environmental associate professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3561', N'Public services associate professionals', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3562', N'Human resources and industrial relations officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3563', N'Vocational and industrial trainers and instructors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3564', N'Careers advisers and vocational guidance specialists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3565', N'Inspectors of standards and regulations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'3567', N'Health and safety officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4112', N'National government administrative occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4113', N'Local government administrative occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4114', N'Officers of non-governmental organisations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4121', N'Credit controllers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4122', N'Book-keepers, payroll managers and wages clerks', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4123', N'Bank and post office clerks', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4124', N'Finance officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4129', N'Financial administrative occupations n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4131', N'Records clerks and assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4132', N'Pensions and insurance clerks and assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4133', N'Stock control clerks and assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4134', N'Transport and distribution clerks and assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4135', N'Library clerks and assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4138', N'Human resources administrative occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4151', N'Sales administrators', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4159', N'Other administrative occupations n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4161', N'Office managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4162', N'Office supervisors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4211', N'Medical secretaries', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4212', N'Legal secretaries', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4213', N'School secretaries', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4214', N'Company secretaries', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4215', N'Personal assistants and other secretaries', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4216', N'Receptionists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'4217', N'Typists and related keyboard occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5111', N'Farmers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5112', N'Horticultural trades', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5113', N'Gardeners and landscape gardeners', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5114', N'Groundsmen and greenkeepers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5119', N'Agricultural and fishing trades n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5211', N'Smiths and forge workers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5212', N'Moulders, core makers and die casters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5213', N'Sheet metal workers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5214', N'Metal plate workers, and riveters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5215', N'Welding trades', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5216', N'Pipe fitters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5221', N'Metal machining setters and setter-operators', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5222', N'Tool makers, tool fitters and markers-out', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5223', N'Metal working production and maintenance fitters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5224', N'Precision instrument makers and repairers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5225', N'Air-conditioning and refrigeration engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5231', N'Vehicle technicians, mechanics and electricians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5232', N'Vehicle body builders and repairers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5234', N'Vehicle paint technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5235', N'Aircraft maintenance and related trades', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5236', N'Boat and ship builders and repairers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5237', N'Rail and rolling stock builders and repairers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5241', N'Electricians and electrical fitters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5242', N'Telecommunications engineers', NULL, NULL)
GO
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5244', N'TV, video and audio engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5245', N'IT engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5249', N'Electrical and electronic trades n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5250', N'Skilled metal, electrical and electronic trades supervisors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5311', N'Steel erectors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5312', N'Bricklayers and masons', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5313', N'Roofers, roof tilers and slaters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5314', N'Plumbers and heating and ventilating engineers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5315', N'Carpenters and joiners', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5316', N'Glaziers, window fabricators and fitters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5319', N'Construction and building trades n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5321', N'Plasterers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5322', N'Floorers and wall tilers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5323', N'Painters and decorators', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5330', N'Construction and building trades supervisors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5411', N'Weavers and knitters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5412', N'Upholsterers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5413', N'Footwear and leather working trades', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5414', N'Tailors and dressmakers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5419', N'Textiles, garments and related trades n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5421', N'Pre-press technicians', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5422', N'Printers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5423', N'Print finishing and binding workers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5431', N'Butchers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5432', N'Bakers and flour confectioners', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5433', N'Fishmongers and poultry dressers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5434', N'Chefs', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5435', N'Cooks', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5436', N'Catering and bar managers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5441', N'Glass and ceramics makers, decorators and finishers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5442', N'Furniture makers and other craft woodworkers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5443', N'Florists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'5449', N'Other skilled trades n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6121', N'Nursery nurses and assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6122', N'Childminders and related occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6123', N'Playworkers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6125', N'Teaching assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6126', N'Educational support assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6131', N'Veterinary nurses', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6132', N'Pest control officers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6139', N'Animal care services occupations n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6141', N'Nursing auxiliaries and assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6142', N'Ambulance staff (excluding paramedics)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6143', N'Dental nurses', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6144', N'Houseparents and residential wardens', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6145', N'Care workers and home carers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6146', N'Senior care workers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6147', N'Care escorts', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6148', N'Undertakers, mortuary and crematorium assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6211', N'Sports and leisure assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6212', N'Travel agents', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6214', N'Air travel assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6215', N'Rail travel assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6219', N'Leisure and travel service occupations n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6221', N'Hairdressers and barbers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6222', N'Beauticians and related occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6231', N'Housekeepers and related occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6232', N'Caretakers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'6240', N'Cleaning and housekeeping managers and supervisors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'7', N'Retired', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'7111', N'Sales and retail assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'7112', N'Retail cashiers and check-out operators', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'7113', N'Telephone salespersons', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'7114', N'Pharmacy and other dispensing assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'7115', N'Vehicle and parts salespersons and advisers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'7121', N'Collector salespersons and credit agents', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'7122', N'Debt, rent and other cash collectors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'7123', N'Roundspersons and van salespersons', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'7124', N'Market and street traders and assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'7125', N'Merchandisers and window dressers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'7129', N'Sales related occupations n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'7130', N'Sales supervisors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'7211', N'Call and contact centre occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'7213', N'Telephonists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'7214', N'Communication operators', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'7215', N'Market research interviewers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'7219', N'Customer service occupations n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'7220', N'Customer service managers and supervisors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8', N'Unemployed', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8111', N'Food, drink and tobacco process operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8112', N'Glass and ceramics process operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8113', N'Textile process operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8114', N'Chemical and related process operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8115', N'Rubber process operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8116', N'Plastics process operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8117', N'Metal making and treating process operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8118', N'Electroplaters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8119', N'Process operatives n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8121', N'Paper and wood machine operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8122', N'Coal mine operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8123', N'Quarry workers and related operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8124', N'Energy plant operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8125', N'Metal working machine operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8126', N'Water and sewerage plant operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8127', N'Printing machine assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8129', N'Plant and machine operatives n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8131', N'Assemblers (electrical and electronic products)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8132', N'Assemblers (vehicles and metal goods)', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8133', N'Routine inspectors and testers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8134', N'Weighers, graders and sorters', NULL, NULL)
GO
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8135', N'Tyre, exhaust and windscreen fitters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8137', N'Sewing machinists', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8139', N'Assemblers and routine operatives n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8141', N'Scaffolders, stagers and riggers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8142', N'Road construction operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8143', N'Rail construction and maintenance operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8149', N'Construction operatives n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8211', N'Large goods vehicle drivers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8212', N'Van drivers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8213', N'Bus and coach drivers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8214', N'Taxi and cab drivers and chauffeurs', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8215', N'Driving instructors', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8221', N'Crane drivers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8222', N'Fork-lift truck drivers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8223', N'Agricultural machinery drivers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8229', N'Mobile machine drivers and operatives n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8231', N'Train and tram drivers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8232', N'Marine and waterways transport operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8233', N'Air transport operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8234', N'Rail transport operatives', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'8239', N'Other drivers and transport operatives n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9', N'Not known', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9111', N'Farm workers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9112', N'Forestry workers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9119', N'Fishing and other elementary agriculture occupations n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9120', N'Elementary construction occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9132', N'Industrial cleaning process occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9134', N'Packers, bottlers, canners and fillers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9139', N'Elementary process plant occupations n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9211', N'Postal workers, mail sorters, messengers and couriers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9219', N'Elementary administration occupations n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9231', N'Window cleaners', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9232', N'Street cleaners', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9233', N'Cleaners and domestics', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9234', N'Launderers, dry cleaners and pressers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9235', N'Refuse and salvage occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9236', N'Vehicle valeters and cleaners', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9239', N'Elementary cleaning occupations n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9241', N'Security guards and related occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9242', N'Parking and civil enforcement occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9244', N'School midday and crossing patrol occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9249', N'Elementary security occupations n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9251', N'Shelf fillers', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9259', N'Elementary sales occupations n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9260', N'Elementary storage occupations', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9271', N'Hospital porters', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9272', N'Kitchen and catering assistants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9273', N'Waiters and waitresses', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9274', N'Bar staff', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9275', N'Leisure and theme park attendants', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SOC2010', N'9279', N'Other elementary services occupations n.e.c.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SpecFee', N'0', N'Standard/Prescribed fee', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SpecFee', N'1', N'Sandwich placement', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SpecFee', N'2', N'Language year abroad & not full-year outgoing ERASMUS', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SpecFee', N'3', N'Full-year outgoing ERASMUS', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SpecFee', N'4', N'Final year of full-time course lasting less than 15 weeks', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SpecFee', N'5', N'Final year of a full-time course lasting more than 14 weeks but less than 24 weeks', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'SpecFee', N'9', N'Other fee', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'TTAccom', N'1', N'Institution-maintained property', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'TTAccom', N'2', N'Parental/guardian home', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'TTAccom', N'4', N'Other', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'TTAccom', N'5', N'Not known', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'TTAccom', N'6', N'Not in attendance at the institution', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'TTAccom', N'7', N'Own residence', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'TTAccom', N'8', N'Other rented accommodation', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'TTAccom', N'9', N'Private sector halls', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'TypeYr', N'1', N'Year of instance contained within the reporting period 01 August to 31 July', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'TypeYr', N'2', N'Year of instance not contained within the reporting period 01 August to 31 July', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'TypeYr', N'3', N'Learner commencing a year of instance of a course running across reporting periods', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'TypeYr', N'4', N'Learner mid-way through a learning aim running across reporting periods', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'TypeYr', N'5', N'Learner finishing a year of instance of a course running across reporting periods', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'WithdrawReason', N'2', N'Learner transferred to another provider', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'WithdrawReason', N'28', N'OLASS learner withdrawn due to circumstances outside the providers'' control', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'WithdrawReason', N'29', N'Learner has been made redundant', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'WithdrawReason', N'3', N'Learner injury/illness', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'WithdrawReason', N'40', N'The learner has transferred to a new learning aim with the same provider.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'WithdrawReason', N'41', N'The learner has transferred to another provider to undertake learning that meets a specific government strategy.', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'WithdrawReason', N'42', N'Academic failure/left in bad standing/not permitted to progress – HE learning aims only', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'WithdrawReason', N'43', N'Financial reasons', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'WithdrawReason', N'44', N'Other personal reasons', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'WithdrawReason', N'45', N'Written off after lapse of time – HE learning aims only', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'WithdrawReason', N'46', N'Exclusion', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'WithdrawReason', N'47', N'Learner has transferred to another provider due to merger', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'WithdrawReason', N'48', N'Industrial placement learner has withdrawn due to circumstances outside the providers'' control', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'WithdrawReason', N'7', N'Learner has transferred between providers due to intervention by or with the written agreement of the Skills Funding Agency', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'WithdrawReason', N'97', N'Other', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'WithdrawReason', N'98', N'Reason not known', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'WorkPlaceMode', N'1', N'Internal (simulated) work placement', NULL, NULL)
INSERT [dbo].[Lookup] ([Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'WorkPlaceMode', N'2', N'External work placement', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'AppFinType', N'PMR', N'1', N'Training payment', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'AppFinType', N'PMR', N'2', N'Assessment payment', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'AppFinType', N'PMR', N'3', N'Employer payment reimbursed by provider', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'AppFinType', N'TNP', N'1', N'Total training price', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'AppFinType', N'TNP', N'2', N'Total assessment price', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'AppFinType', N'TNP', N'3', N'Residual training price', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'AppFinType', N'TNP', N'4', N'Residual assessment price', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ContPrefType', N'PMC', N'1', N'Learner does not wish to be contacted by post	', CAST(N'1900-01-01' AS Date), CAST(N'2018-05-25' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ContPrefType', N'PMC', N'2', N'Learner does not wish to be contacted by telephone', CAST(N'1900-01-01' AS Date), CAST(N'2018-05-25' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ContPrefType', N'PMC', N'3', N'Learner does not wish to be contacted by e-mail', CAST(N'1900-01-01' AS Date), CAST(N'2018-05-25' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ContPrefType', N'PMC', N'4', N'Learner agrees to be contacted by post', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ContPrefType', N'PMC', N'5', N'Learner agrees to be contacted by telephone', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ContPrefType', N'PMC', N'6', N'Learner agrees to be contacted by e-mail', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ContPrefType', N'RUI', N'1', N'Learner does not wish to be contacted about courses or learning opportunities', CAST(N'1900-01-01' AS Date), CAST(N'2018-05-25' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ContPrefType', N'RUI', N'2', N'Learner does not wish to be contacted for survey and research', CAST(N'1900-01-01' AS Date), CAST(N'2018-05-25' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ContPrefType', N'RUI', N'3', N'Learner is not to be contacted, for example where a learner has died, or suffered severe illness during the programme', CAST(N'1900-01-01' AS Date), CAST(N'2013-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ContPrefType', N'RUI', N'4', N'Learner is not to be contacted, for example where a learner has suffered severe illness during the programme or other circumstance', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ContPrefType', N'RUI', N'5', N'Learner is not to be contacted - learner has died', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ContPrefType', N'RUI', N'6', N'Learner agrees to be contacted about courses or learning opportunities', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ContPrefType', N'RUI', N'7', N'Learner agrees to be contacted for survey and research', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'BSI', N'1', N'Learner is in receipt of Job Seekers Allowance (JSA)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'BSI', N'2', N'Learner is in receipt of Employment and Support Allowance - Work Related Activity Group (ESA WRAG)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'BSI', N'3', N'Learner is in receipt of another state benefit other than JSA, Universal Credit or ESA (WRAG)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'BSI', N'4', N'Learner is in receipt of Universal Credit', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'EII', N'1', N'Learner is employed for 16 hours or more per week', CAST(N'1900-01-01' AS Date), CAST(N'2013-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'EII', N'2', N'Learner is employed for less than 16 hours per week', CAST(N'1900-01-01' AS Date), CAST(N'2018-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'EII', N'3', N'Learner is employed for 16 – 19 hours per week', CAST(N'1900-01-01' AS Date), CAST(N'2018-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'EII', N'4', N'Learner is employed for 20 hours or more per week', CAST(N'1900-01-01' AS Date), CAST(N'2018-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'EII', N'5', N'Learner is employed for 0 to 10 hours per week', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'EII', N'6', N'Learner is employed for 11 to 20 hours per week', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'EII', N'7', N'Learner is employed for 21 to 30 hours per week', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'EII', N'8', N'Learner is employed for 31+ hours per week	', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'LOE', N'1', N'Learner has been employed for up to 3 months', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'LOE', N'2', N'Learner has been employed for 4 months – 6 months', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'LOE', N'3', N'Learner has been employed for 7 months - 12 months', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'LOE', N'4', N'Learner has been employed for more than 12 months', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'LOU', N'1', N'Learner has been unemployed for less than 6 months', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'LOU', N'2', N'Learner has been unemployed for 6-11 months', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'LOU', N'3', N'Learner has been unemployed for 12-23 months', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'LOU', N'4', N'Learner has been unemployed for 24-35 months', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'LOU', N'5', N'Learner has been unemployed for 36 months or more', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'PEI', N'1', N'Learner was in full time education or training prior to enrolment', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'SEI', N'1', N'Learner is self employed', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'ESMType', N'SEM', N'1', N'Small employer', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'ACT', N'1', N'Apprenticeship fund through a contract for services with the employer', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'ACT', N'2', N'Apprenticeship fund through a contract for services with the Skills Funding Agency', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'ACT', N'3', N'Unassigned', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'ACT', N'4', N'Unassigned', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'ACT', N'5', N'Unassigned', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'ACT', N'6', N'Unassigned', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'ADL', N'1', N'Aim is financed by an Advanced Learner Loan', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'ALB', N'1', N'Rate 1', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'ALB', N'2', N'Rate 2', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'ALB', N'3', N'Rate 3', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'ASL', N'1', N'Personal and community development learning', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'ASL', N'2', N'Neighbourhood learning in deprived communities', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'ASL', N'3', N'Family English Maths and Language', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'ASL', N'4', N'Wider family learning', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'001', N'Postcode validation exclusion', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'002', N'Procured Adult Education Budget', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'003', N'Devolved AEB Innovation Fund', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'004', N'Devolved AEB flexible allocation provision', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'005', N'Test and Learn Pilot English and Maths', CAST(N'2019-08-01' AS Date), CAST(N'2021-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'006', N'Test and Learn Pilot ESOL', CAST(N'2019-08-01' AS Date), CAST(N'2021-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'007', N'Test and Learn Pilot Digital', CAST(N'2019-08-01' AS Date), CAST(N'2021-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'008', N'"Sector Specific Monitoring', CAST(N'2019-08-01' AS Date), CAST(N'2020-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'009', N'Devolved AEB British Sign Language Entitlement', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'010', N'Devolved AEB Low Wage Pilot', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'011', N'Level2 contingency', CAST(N'2019-08-01' AS Date), CAST(N'2020-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'012', N'Level 3 contingency', CAST(N'2019-08-01' AS Date), CAST(N'2020-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'013', N'Sector Based Work Academy Programme', CAST(N'2019-08-01' AS Date), CAST(N'2021-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'014', N'Sector Based Work Academy Programme Work Experience', CAST(N'2019-08-01' AS Date), CAST(N'2021-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'015', N'Sector Based Work Academy Programme Job Outcome', CAST(N'2019-08-01' AS Date), CAST(N'2021-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'016', N'Sector Based Work Academy Component Learning Aim', CAST(N'2019-08-01' AS Date), CAST(N'2021-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'017', N'Innovation Engagement Payment', CAST(N'2019-08-01' AS Date), CAST(N'2021-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'018', N'Innovation On-Programme Payment', CAST(N'2019-08-01' AS Date), CAST(N'2021-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'019', N'Innovation Outcome Payment Type 1', CAST(N'2019-08-01' AS Date), CAST(N'2021-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'020', N'Innovation Outcome Payment Type 2', CAST(N'2019-08-01' AS Date), CAST(N'2021-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'DAM', N'021', N'Innovation Outcome Payment Type 3', CAST(N'2019-08-01' AS Date), CAST(N'2021-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'EEF', N'2', N'Entitlement to 16–18 Apprenticeship funding, where the learner is 19 or over', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'EEF', N'3', N'Entitlement to 19-23 Apprenticeship funding, where the learner is 24 or over', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'EEF', N'4', N'Entitlement to extended funding for apprentices', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'FFI', N'1', N'Fully funded learning aim', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'FFI', N'2', N'Co funded learning aim', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'FLN', N'1', N'Family English, Maths or Language learning aim formula funded through the Adult Education Budget', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'HEM', N'1', N'Student is funded by HEFCE using the old funding regime (only for learning aims starting on or after 1 September 2012)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'HEM', N'3', N'Student has received an award under the National Scholarship programme for this learning aim', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'HEM', N'5', N'Student''s qualifications and grades prior to enrolment are included in the student number control exemption list according to HEFCE', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'HHS', N'1', N'No household member is in employment and the household includes one or more dependent children', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'HHS', N'2', N'No household member is in employment and the household does not include any dependent children', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'HHS', N'3', N'Learner lives in a single adult household with dependent children', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'HHS', N'98', N'Learner has withheld this information', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'HHS', N'99', N'None of HHS1, HHS2 or HHS3 applies', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'034', N'OLASS – Offenders in custody', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'086', N'OLASS – Offenders in the community', CAST(N'2007-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'117', N'Apprenticeship supported or funded by ESF', CAST(N'2009-08-01' AS Date), CAST(N'2015-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'118', N'Proxy Learning Aim', CAST(N'2010-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'127', N'Access to Apprenticeships', CAST(N'2011-08-01' AS Date), CAST(N'2014-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'129', N'Group Training Association (GTA)', CAST(N'2011-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'130', N'Apprenticeship Training Agency (ATA)', CAST(N'2011-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'131', N'NEET Apprenticeship starts', CAST(N'2011-09-01' AS Date), CAST(N'2015-08-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'132', N'Apprenticeship Grant for Employers for 16 to 24 year olds (AGE 16 to 24) - Provider Payment Trigger', CAST(N'2012-02-08' AS Date), CAST(N'2014-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'133', N'Apprenticeship Grant for Employers for 16 to 24 year olds (AGE 16 to 24) - Strategic Partner', CAST(N'2012-02-08' AS Date), CAST(N'2014-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'135', N'Greater Manchester Commitment to Youth Employment', CAST(N'2012-04-01' AS Date), CAST(N'2014-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'315', N'Norfolk Apprenticeships Subsidy', CAST(N'2012-09-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'318', N'Mandation to Skills Training', CAST(N'2012-08-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'319', N'Employer Ownership Pilot - Employer defined programme', CAST(N'2012-08-01' AS Date), CAST(N'2017-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'320', N'14-16 EFA Direct Funded Students', CAST(N'2013-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'321', N'14-16 Home Educated Students', CAST(N'2013-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'322', N'Residential Courses', CAST(N'2013-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'324', N'Personalised learning programme for learners with learning difficulties and/or disabilities', CAST(N'2013-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'326', N'Skills Made Easy-Sheffield City Deal', CAST(N'2013-08-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'328', N'RoTL', CAST(N'2013-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'329', N'ESOL Plus (Mandation)', CAST(N'2013-08-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'331', N'Princes Trust Team programme', CAST(N'2014-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'333', N'Apprenticeship Grant for Employers for 16 to 24 year olds (AGE 16 to 24) - Provider Payment Trigger from  1 Jan 2015 to 31 Mar 2016', CAST(N'2015-01-01' AS Date), CAST(N'2016-03-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'334', N'Apprenticeship Grant for Employers for 16 to 24 year olds (AGE 16 to 24) - Strategic Partner from 1 Jan 2015 to 31 Dec 2015', CAST(N'2015-01-01' AS Date), CAST(N'2015-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'335', N'ESF AGE Enhancement 1-49', CAST(N'2015-01-01' AS Date), CAST(N'2015-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'338', N'ESF AGE Enhancements 50-249', CAST(N'2015-01-01' AS Date), CAST(N'2015-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'340', N'Community learning mental health pilot', CAST(N'2015-04-01' AS Date), CAST(N'2017-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'342', N'BBC Make it Digital Traineeship Programme', CAST(N'2015-03-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'344', N'Greater Manchester Skills for Employment Pilot Programme (Growth Deal)', CAST(N'2015-08-01' AS Date), CAST(N'2019-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'345', N'Sheffield City Region Skills Bank (Growth Deal)', CAST(N'2015-08-01' AS Date), CAST(N'2021-03-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'346', N'Armed forces leavers', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'347', N'Steel Industries Redundancy Training', CAST(N'2015-08-01' AS Date), CAST(N'2099-12-31' AS Date))
GO
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'348', N'Apprenticeship seasonal worker pilot', CAST(N'2016-04-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'349', N'Apprenticeship Grant for Employers (AGE) - Provider Payment Trigger from 1 April 2016 to 31 July 2017', CAST(N'2016-04-01' AS Date), CAST(N'2017-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'350', N'Non-apprenticeship workplace learning originally funded under the employer responsive funding model', CAST(N'1900-01-01' AS Date), CAST(N'2013-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'351', N'Former Employer Ownership Pilot Provision', CAST(N'2015-04-01' AS Date), CAST(N'2017-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'352', N'HESA-generated ILR file', CAST(N'2016-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'353', N'Non-apprenticeship Sporting Excellence Award', CAST(N'2017-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'354', N'Non-apprenticeship Theatre and live events', CAST(N'2017-08-01' AS Date), CAST(N'2020-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'355', N'Non-apprenticeship Sea Fishing', CAST(N'2017-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'356', N'Apprenticeship being delivered to own employees', CAST(N'2017-05-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'357', N'Procured Adult Education Budget (AEB)', CAST(N'2017-11-01' AS Date), CAST(N'2020-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'358', N'Extended Allocated Non-Levy Apprenticeships', CAST(N'2018-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'359', N'Career Learning Pilot', CAST(N'2018-08-01' AS Date), CAST(N'2020-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'360', N'Flexible Learning Fund', CAST(N'2018-08-01' AS Date), CAST(N'2019-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'361', N'Waiver to record payment records for apprenticeships', CAST(N'2017-05-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'362', N'Apprentice care leavers', CAST(N'2018-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'363', N'Learners in receipt of low wages', CAST(N'2018-08-01' AS Date), CAST(N'2020-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'364', N'Adult education budget run-down contract for services -continuing learners from 2017/18', CAST(N'2018-08-01' AS Date), CAST(N'2019-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'365', N'Institute of Technology Provision', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'366', N'Non-levy non-procured contract for authorised apprentices', CAST(N'2019-06-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LDM', N'367', N'De-merger monitoring', CAST(N'2019-07-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'LSF', N'1', N'Learning support funding', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'1', N'Fashion Retail', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'10', N'Sport and Active Leisure', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'11', N'Retail', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'12', N'Materials, Production and Supply', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'13', N'National Enterprise Academy', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'14', N'Social Care', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'15', N'Information Technology', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'16', N'Power', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'17', N'Rail Engineering', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'18', N'Environmental Technologies', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'19', N'Logistics', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'2', N'Manufacturing', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'20', N'Health', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'3', N'Financial Services', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'4', N'Construction', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'5', N'Food and Drink Manufacturing', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'6', N'Nuclear', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'7', N'Process Industries', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'8', N'Creative and Cultural', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'NSA', N'9', N'Hospitality', CAST(N'1900-01-01' AS Date), CAST(N'2016-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'POD', N'1', N'0%', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'POD', N'2', N'1 - 9%', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'POD', N'3', N'10 – 24%', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'POD', N'4', N'25 – 49%', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'POD', N'5', N'50 – 74%', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'POD', N'6', N'75 – 99%', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'POD', N'7', N'100%', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'RES', N'1', N'Learning aim restarted', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'SOF', N'1', N'HEFCE', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'SOF', N'105', N'Education and Skills Funding Agency (ESFA) - Adult', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'SOF', N'107', N'Education and Skills Funding Agency (ESFA) - 16-19', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'SOF', N'108', N'Local authority (Community Learning funds)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'SOF', N'110', N'Greater Manchester Combined authority', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'SOF', N'111', N'Liverpool City Region Combined authority', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'SOF', N'112', N'West Midlands Combined authority', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'SOF', N'113', N'West of England Combined authority', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'SOF', N'114', N'Tees Valley Combined authority', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'SOF', N'115', N'Cambridgeshire and Peterborough Combined authority', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'SOF', N'116', N'Greater London Authority', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'SOF', N'998', N'Other', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnDelFAMType', N'WPP', N'1', N'DWP work Programme', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'DLA', N'1', N'Learner is funded by HEFCE and is in receipt of disabled students allowance', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'ECF', N'1', N'Learner is exempt from GCSE English condition of funding due to a learning difficulty', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'ECF', N'2', N'Learner is exempt from GCSE English condition of funding as they hold an equivalent overseas qualification', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'ECF', N'3', N'Learner has met the GCSE English condition of funding as they hold an approved equivalent UK qualification', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'ECF', N'4', N'Learner has met the GCSE English condition of funding by undertaking/completing a valid English GCSE or equivalent qualification at another institution through collaboration with the home institution', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'ECF', N'5', N'Learner holds a pass grade for functional skills level 2 in English', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'EDF', N'1', N'Learner has not achieved a maths GCSE (at grade A*-C/9-4) by the end of year 11', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'EDF', N'2', N'Learner has not achieved an English GCSE (at grade A*-C/9-4) by the end of year 11', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'EHC', N'1', N'Learner has an Education Health Care plan', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'FME', N'1', N'14-15 year old learner is eligible for free meals', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'FME', N'2', N'16-19 year old learner is eligible for and in receipt of free meals', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'HNS', N'1', N'Learner is a high needs student in receipt of element 3 ‘top-up’ funding from the local authority', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'LSR', N'36', N'Care to Learn', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'LSR', N'55', N'16-19 Bursary Fund – learner is a member of a vulnerable group', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'LSR', N'56', N'16-19 Bursary Fund - learner has been awarded a discretionary bursary', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'LSR', N'57', N'Residential support', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'LSR', N'58', N'19+ Hardship (Adult Skills or Advanced Learner Loan funded learners only)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'LSR', N'59', N'20+ Childcare (Adult Skills or Advanced Learner Loan funded learners only)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'LSR', N'60', N'19+ Residential Access Fund (Adult Skills or Advanced Learner Loan funded learners only)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'LSR', N'61', N'ESF funded learner receiving childcare support', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'MCF', N'1', N'Learner is exempt from GCSE maths condition of funding due to a learning difficulty', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'MCF', N'2', N'Learner is exempt from GCSE maths condition of funding as they hold an equivalent overseas qualification', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'MCF', N'3', N'Learner has met the GCSE maths condition of funding as they hold an approved equivalent UK qualification', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'MCF', N'4', N'Learner has met the GCSE maths condition of funding by undertaking/completing a valid maths GCSE or equivalent qualification at another institution through collaboration with the home institution', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'MCF', N'5', N'Learner holds a pass grade for functional skills level 2 in mathematics', CAST(N'2019-08-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'NLM', N'17', N'Learner migrated as part of provider merger', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'NLM', N'18', N'Learner moved as a result of Minimum Contract Level', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'PPE', N'1', N'Learner is eligible for Service Child premium', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'PPE', N'2', N'Learner is eligible for Adopted from Care premium', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearnFAMType', N'SEN', N'1', N'Special educational needs', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0001', N'A', N'OutGrade A for LearningAimType 0001', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0001', N'B', N'OutGrade B for LearningAimType 0001', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0001', N'C', N'OutGrade C for LearningAimType 0001', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0001', N'D', N'OutGrade D for LearningAimType 0001', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0001', N'E', N'OutGrade E for LearningAimType 0001', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0001', N'N', N'OutGrade N for LearningAimType 0001', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0001', N'U', N'OutGrade U for LearningAimType 0001', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0002', N'A', N'OutGrade A for LearningAimType 0002', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0002', N'A*', N'OutGrade A* for LearningAimType 0002', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0002', N'B', N'OutGrade B for LearningAimType 0002', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0002', N'C', N'OutGrade C for LearningAimType 0002', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0002', N'D', N'OutGrade D for LearningAimType 0002', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0002', N'E', N'OutGrade E for LearningAimType 0002', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0002', N'F', N'OutGrade F for LearningAimType 0002', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0002', N'G', N'OutGrade G for LearningAimType 0002', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0002', N'N', N'OutGrade N for LearningAimType 0002', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0002', N'U', N'OutGrade U for LearningAimType 0002', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'1', N'OutGrade 1 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'11', N'OutGrade 11 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'2', N'OutGrade 2 for LearningAimType 0003', NULL, NULL)
GO
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'21', N'OutGrade 21 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'22', N'OutGrade 22 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'3', N'OutGrade 3 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'32', N'OutGrade 32 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'33', N'OutGrade 33 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'4', N'OutGrade 4 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'43', N'OutGrade 43 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'44', N'OutGrade 44 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'5', N'OutGrade 5 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'54', N'OutGrade 54 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'55', N'OutGrade 55 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'6', N'OutGrade 6 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'65', N'OutGrade 65 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'66', N'OutGrade 66 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'7', N'OutGrade 7 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'76', N'OutGrade 76 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'77', N'OutGrade 77 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'8', N'OutGrade 8 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'87', N'OutGrade 87 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'88', N'OutGrade 88 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'9', N'OutGrade 9 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'98', N'OutGrade 98 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'99', N'OutGrade 99 for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'A', N'OutGrade A for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'A*', N'OutGrade A* for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'A*A', N'OutGrade A*A for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'A*A*', N'OutGrade A*A* for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'AA', N'OutGrade AA for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'AB', N'OutGrade AB for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'B', N'OutGrade B for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'BB', N'OutGrade BB for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'BC', N'OutGrade BC for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'C', N'OutGrade C for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'CC', N'OutGrade CC for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'CD', N'OutGrade CD for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'D', N'OutGrade D for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'DD', N'OutGrade DD for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'DE', N'OutGrade DE for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'E', N'OutGrade E for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'EE', N'OutGrade EE for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'EF', N'OutGrade EF for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'F', N'OutGrade F for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'FF', N'OutGrade FF for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'FG', N'OutGrade FG for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'G', N'OutGrade G for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'GG', N'OutGrade GG for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'N', N'OutGrade N for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'0003', N'U', N'OutGrade U for LearningAimType 0003', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1413', N'A', N'OutGrade A for LearningAimType 1413', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1413', N'A*', N'OutGrade A* for LearningAimType 1413', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1413', N'B', N'OutGrade B for LearningAimType 1413', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1413', N'C', N'OutGrade C for LearningAimType 1413', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1413', N'D', N'OutGrade D for LearningAimType 1413', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1413', N'E', N'OutGrade E for LearningAimType 1413', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1413', N'F', N'OutGrade F for LearningAimType 1413', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1413', N'G', N'OutGrade G for LearningAimType 1413', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1413', N'N', N'OutGrade N for LearningAimType 1413', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1413', N'U', N'OutGrade U for LearningAimType 1413', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'A', N'OutGrade A for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'A*', N'OutGrade A* for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'A*A', N'OutGrade A*A for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'A*A*', N'OutGrade A*A* for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'AA', N'OutGrade AA for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'AB', N'OutGrade AB for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'B', N'OutGrade B for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'BB', N'OutGrade BB for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'BC', N'OutGrade BC for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'C', N'OutGrade C for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'CC', N'OutGrade CC for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'CD', N'OutGrade CD for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'D', N'OutGrade D for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'DD', N'OutGrade DD for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'DE', N'OutGrade DE for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'E', N'OutGrade E for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'EE', N'OutGrade EE for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'EF', N'OutGrade EF for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'F', N'OutGrade F for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'FF', N'OutGrade FF for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'FG', N'OutGrade FG for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'G', N'OutGrade G for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'GG', N'OutGrade GG for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'N', N'OutGrade N for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1422', N'U', N'OutGrade U for LearningAimType 1422', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1430', N'A', N'OutGrade A for LearningAimType 1430', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1430', N'A*', N'OutGrade A* for LearningAimType 1430', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1430', N'B', N'OutGrade B for LearningAimType 1430', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1430', N'C', N'OutGrade C for LearningAimType 1430', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1430', N'D', N'OutGrade D for LearningAimType 1430', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1430', N'E', N'OutGrade E for LearningAimType 1430', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1430', N'F', N'OutGrade F for LearningAimType 1430', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1430', N'G', N'OutGrade G for LearningAimType 1430', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1430', N'N', N'OutGrade N for LearningAimType 1430', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1430', N'U', N'OutGrade U for LearningAimType 1430', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1433', N'AA', N'OutGrade AA for LearningAimType 1433', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1433', N'AB', N'OutGrade AB for LearningAimType 1433', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1433', N'BB', N'OutGrade BB for LearningAimType 1433', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1433', N'BC', N'OutGrade BC for LearningAimType 1433', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1433', N'CC', N'OutGrade CC for LearningAimType 1433', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1433', N'CD', N'OutGrade CD for LearningAimType 1433', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1433', N'DD', N'OutGrade DD for LearningAimType 1433', NULL, NULL)
GO
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1433', N'DE', N'OutGrade DE for LearningAimType 1433', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1433', N'EE', N'OutGrade EE for LearningAimType 1433', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1433', N'N', N'OutGrade N for LearningAimType 1433', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1433', N'U', N'OutGrade U for LearningAimType 1433', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1434', N'A', N'OutGrade A for LearningAimType 1434', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1434', N'A*', N'OutGrade A* for LearningAimType 1434', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1434', N'B', N'OutGrade B for LearningAimType 1434', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1434', N'C', N'OutGrade C for LearningAimType 1434', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1434', N'D', N'OutGrade D for LearningAimType 1434', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1434', N'E', N'OutGrade E for LearningAimType 1434', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1434', N'F', N'OutGrade F for LearningAimType 1434', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1434', N'G', N'OutGrade G for LearningAimType 1434', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1434', N'N', N'OutGrade N for LearningAimType 1434', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1434', N'U', N'OutGrade U for LearningAimType 1434', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1435', N'A*A', N'OutGrade A*A for LearningAimType 1435', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1435', N'A*A*', N'OutGrade A*A* for LearningAimType 1435', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1435', N'AA', N'OutGrade AA for LearningAimType 1435', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1435', N'AB', N'OutGrade AB for LearningAimType 1435', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1435', N'BB', N'OutGrade BB for LearningAimType 1435', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1435', N'BC', N'OutGrade BC for LearningAimType 1435', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1435', N'CC', N'OutGrade CC for LearningAimType 1435', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1435', N'CD', N'OutGrade CD for LearningAimType 1435', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1435', N'DD', N'OutGrade DD for LearningAimType 1435', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1435', N'DE', N'OutGrade DE for LearningAimType 1435', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1435', N'EE', N'OutGrade EE for LearningAimType 1435', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1435', N'EF', N'OutGrade EF for LearningAimType 1435', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1435', N'FF', N'OutGrade FF for LearningAimType 1435', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1435', N'FG', N'OutGrade FG for LearningAimType 1435', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1435', N'N', N'OutGrade N for LearningAimType 1435', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1435', N'U', N'OutGrade U for LearningAimType 1435', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1453', N'A*A', N'OutGrade A*A for LearningAimType 1453', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1453', N'A*A*', N'OutGrade A*A* for LearningAimType 1453', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1453', N'AA', N'OutGrade AA for LearningAimType 1453', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1453', N'AB', N'OutGrade AB for LearningAimType 1453', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1453', N'BB', N'OutGrade BB for LearningAimType 1453', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1453', N'BC', N'OutGrade BC for LearningAimType 1453', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1453', N'CC', N'OutGrade CC for LearningAimType 1453', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1453', N'CD', N'OutGrade CD for LearningAimType 1453', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1453', N'DD', N'OutGrade DD for LearningAimType 1453', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1453', N'DE', N'OutGrade DE for LearningAimType 1453', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1453', N'EE', N'OutGrade EE for LearningAimType 1453', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1453', N'EF', N'OutGrade EF for LearningAimType 1453', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1453', N'N', N'OutGrade N for LearningAimType 1453', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'1453', N'U', N'OutGrade U for LearningAimType 1453', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'A', N'OutGrade A for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'A*', N'OutGrade A* for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'A*A', N'OutGrade A*A for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'A*A*', N'OutGrade A*A* for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'AA', N'OutGrade AA for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'AB', N'OutGrade AB for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'B', N'OutGrade B for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'BB', N'OutGrade BB for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'BC', N'OutGrade BC for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'C', N'OutGrade C for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'CC', N'OutGrade CC for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'CD', N'OutGrade CD for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'D', N'OutGrade D for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'DD', N'OutGrade DD for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'DE', N'OutGrade DE for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'E', N'OutGrade E for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'EE', N'OutGrade EE for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'EF', N'OutGrade EF for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'F', N'OutGrade F for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'FF', N'OutGrade FF for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'FG', N'OutGrade FG for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'G', N'OutGrade G for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'GG', N'OutGrade GG for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'N', N'OutGrade N for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'LearningAimType', N'2999', N'U', N'OutGrade U for LearningAimType 2999', NULL, NULL)
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'EDU', N'1', N'Traineeship', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'EDU', N'2', N'Apprenticeship', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'EDU', N'3', N'Supported Internship', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'EDU', N'4', N'Other FE (Full-time)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'EDU', N'5', N'Other FE (Part-time)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'EDU', N'6', N'HE', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'EMP', N'1', N'In paid employment for 16 hours or more per week', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'EMP', N'2', N'In paid employment for less than 16 hours per week', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'EMP', N'3', N'Self employed', CAST(N'1900-01-01' AS Date), CAST(N'2015-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'EMP', N'4', N'Self-employed for 16 hours or more per week', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'EMP', N'5', N'Self-employed for less than 16 hours per week', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'GAP', N'1', N'Gap year before starting HE', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'NPE', N'1', N'Not in paid employment, looking for work and available to start work', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'NPE', N'2', N'Not in paid employment, not looking for work and/or not available to start work (including retired)', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'OTH', N'1', N'Other outcome - not listed', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'OTH', N'2', N'Not reported', CAST(N'1900-01-01' AS Date), CAST(N'2015-07-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'OTH', N'3', N'Unable to contact learner', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'OTH', N'4', N'Not known', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'SDE', N'1', N'Supported independent living', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'SDE', N'2', N'Independent living', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'SDE', N'3', N'Learner returning home', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'SDE', N'4', N'Long term residential placement', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))
INSERT [dbo].[LookupSubCategory] ([ParentName], [Name], [Code], [Description], [EffectiveFrom], [EffectiveTo]) VALUES (N'OutType', N'VOL', N'1', N'Voluntary work', CAST(N'1900-01-01' AS Date), CAST(N'2099-12-31' AS Date))

RAISERROR('Lookups Completed',10,1) WITH NOWAIT;
GO


DELETE FROM [dbo].[Rules]
GO


INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Accom_01', N'E', N'The Accommodation is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AchDate_02', N'W', N'The Achievement date should not be after the current teaching year end date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AchDate_03', N'E', N'The Achievement date must not be before the Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AchDate_04', N'E', N'The Learning actual end date must be returned for this Achievement date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AchDate_05', N'E', N'The Achievement date must be after the Learning actual end date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AchDate_07', N'E', N'The Achievement date must be before the file preparation date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AchDate_08', N'E', N'The Achievement date must be completed for apprenticeship standards and traineeship programmes with a status of achieved');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AchDate_09', N'E', N'The Achievement date must not be completed for aims that are not traineeship or apprenticeship standard programme aims');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AchDate_10', N'E', N'The Achievement date must not be more than 6 months after the Learning actual end date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AchDate_11', N'W', N'The Achievement date must be 7 days after the Learning actual end date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AddHours_01', N'E', N'The Additional delivery hours must not be returned for learning aims that started prior to 1 August 2015');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AddHours_02', N'E', N'The Additional delivery hours must not be returned for EFA, Community Learning or Non-funded provision');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AddHours_03', N'E', N'The Additional delivery hours must only be returned for ESOL qualification or unit aims');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AddHours_04', N'E', N'The Additional delivery hours must not exceed 24 hours in a day');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AddHours_05', N'W', N'The Additional delivery hours should not exceed 35 hours in a week');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AddHours_06', N'W', N'The Additional delivery hours divided by the number of days of study is greater than 9 and less than 24');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Addline1_03', N'E', N'Address line 1 has not been returned.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AFinDate_03', N'E', N'''Apprenticeship financial record date'' is wrong. It can''t be after the file preparation date''. Check ''apprenticeship financial record date'' is correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AFinDate_04', N'E', N'The price must not be after the learning actual end date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AFinDate_05', N'E', N'A total training price must not be returned after a residual training price has been recorded');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AFinDate_06', N'E', N'A total assessment price must not be returned after a residual assessment price has been recorded');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AFinDate_07', N'E', N'A total training price and residual training price record must not have the same financial date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AFinDate_08', N'E', N'A total assessment price and residual assessment price record must not have the same financial date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AFinDate_09', N'E', N'The apprenticeship financial record date must not be more than one year before the start of the apprenticeship programme');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AFinDate_12', N'E', N'The apprenticeship financial record date should not be more than one year after the Learning actual end date of the apprenticeship programme');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AFinType_01', N'E', N'An apprenticeship Financial Record for the Total training price must be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AFinType_02', N'E', N'The financial type and code is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AFinType_04', N'E', N'The Apprenticeship Financial Record must not be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AFinType_07', N'E', N'A financial record for the total assessment price must be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AFinType_08', N'E', N'This Financial code must not be used for this funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AFinType_10', N'W', N'An ''end-point assessment price'' is missing. An end point assessment price is required for all apprenticeship standards. Once you''ve agreed an ''end point assessment price'' with the assessor, you must record it on the ILR. ');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AFinType_11', N'E', N'This Financial code must not be used for apprenticeship frameworks');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AFinType_12', N'E', N'The price must be returned for this programme');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AFinType_13', N'E', N'There must be a price record that applies from the start of the programme');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AFinType_14', N'E', N'A financial record for the training price must be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AgreeId_02', N'E', N'Learning delivery FAM type'' and code is wrong. By entering an ''Agreement identifier'', the ''Learning delivery FAM type'' must be ACT and the ''Learning delivery FAM code'' must be 1. This tells us the apprenticeship is funded by a contract of services with the employer. Check that you have the right type (ACT) and code (1) in ''Learning delivery FAM type'' and ''Learning delivery FAM code''. Also check the start date of the employment status is correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AimSeqNumber_02', N'E', N'The Aim sequence number is greater than the count of the learning aims');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AimType_01', N'E', N'The Aim type is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AimType_05', N'E', N'The Aim type is not valid for this Funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'AimType_07', N'E', N'A core aim must not be recorded with a source of funding of 105');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ALSCost_02', N'E', N'The Learning support cost field must only be returned for High Needs Students');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'CampId_01', N'E', N'The Campus identifier is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'CampId_02', N'E', N'The UKPRN is not the same as recorded in the Header');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'CompStatus_01', N'E', N'The Completion status is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'CompStatus_02', N'E', N'The Learning actual end date must not be returned for this Completion status');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'CompStatus_03', N'E', N'The Learning actual end date must be returned for this Completion status');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'CompStatus_04', N'E', N'The Outcome must be returned for this completion status');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'CompStatus_05', N'W', N'The Completion status is not valid for this Outcome');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'CompStatus_06', N'E', N'The Completion status is not valid for this Outcome');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'CompStatus_07', N'E', N'The completion status has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ConRefNumber_01', N'E', N'The Contract reference number is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ConRefNumber_03', N'E', N'The Contract reference number must not be returned for this Funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ContPrefType_01', N'E', N'The Contact preference type and code is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ContPrefType_02', N'E', N'The combination of Contact preference type and code is not valid, this learner is not to be contacted');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ContPrefType_03', N'E', N'The Contact preference type and code is not valid for this Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ContPrefType_04', N'E', N'These contact preference types cannot all be used at the same time');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ContPrefType_05', N'E', N'These contact preference types cannot all be used at the same time');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ContPrefType_06', N'E', N'There must not be more than two records with a Contact preference type of RUI');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ContPrefType_07', N'E', N'There must not be more than three records with a Contact preference type of PMC');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateEmpStatApp_01', N'E', N'The Date employment status applies must not be after the current teaching year');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateEmpStatApp_02', N'E', N'The Date employment status applies must not be before 1 August 1990');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_01', N'E', N'The Date of birth must be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_02', N'W', N' ''Date of birth'' may be missing. You should enter the learner''s date of birth. Check the ''date of birth'' is complete and correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_04', N'E', N'The learner is 115 or over');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_05', N'W', N'The learner is under 4');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_06', N'W', N'The learner is under 13');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_07', N'E', N'The learner is 25 or over so the Source of funding must not be EFA');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_12', N'E', N'The learner is under 19 and the Community Learning provision is PCDL or NLDC');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_13', N'W', N'The learner is HEFCE funded and should not be under 16 years old');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_14', N'W', N'The learner is under 18');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_20', N'E', N'The learner is under 19 and the Source of funding is not the EFA');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_23', N'E', N'The Date of birth has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_24', N'E', N'The Unique learner number has been returned and the Date of birth has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_25', N'E', N'The learner is under 19');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_26', N'E', N'The learner is under 19 and financed by an Advanced Learner Loan');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_27', N'E', N'The learner''s Date of birth is on or after the start of the current teaching year');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_30', N'E', N'The learner is between 19 and 24 and has an Education health plan, and the Funding model and/or Source of funding is not the EFA');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_32', N'E', N'The learning aim started on or after 1 August 2015 and the learner is 24 or over and the aim is Level 3 or above and the Funding model is Adult skills');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_34', N'E', N'The learner is recorded as a High Needs Student but there is no Education health care plan recorded');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_35', N'E', N'The learner is under 19 and the planned duration of the programme does not reach the required minimum duration of an apprenticeship');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_36', N'E', N'The learner is 19 or over and the planned duration of the programme does not reach the required minimum duration of an apprenticeship');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_37', N'E', N'The learner is 19 or over and the planned duration of the programme does not reach the required minimum duration of an apprenticeship');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_38', N'E', N'The learner is under 19 and the actual duration of the programme does not reach the required minimum duration of an apprenticeship');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_40', N'E', N'The learner is 19 or over and the actual duration of the programme does not reach the required minimum duration of an apprenticeship');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_41', N'W', N'The learner is under 19');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_43', N'E', N'The learner is under 15');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_44', N'E', N'The learner''s age is outside of the age range described in the ESF Tender Specification');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_46', N'E', N'The planned duration of the programme does not reach the required minimum duration for an apprenticeship standard');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_47', N'E', N'The actual duration of the programme does not reach the required minimum duration for an apprenticeship standard');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_48', N'E', N'The learner is not over the school leaving age');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_49', N'E', N'The learning aim started on or after 1 August 2016 and the learner is 24 or over and the aim is Level 3 or above and the Funding model is Adult skills');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_50', N'E', N'The learner must not start a traineeship programme until after 1 August of the academic year in which they turn 16');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_51', N'E', N'The learners age is ineligible for a traineeship programme');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_52', N'E', N'The programme does not reach the required minimum duration of an apprenticeship');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_53', N'E', N'The programme does not reach the required minimum duration of an apprenticeship');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_54', N'E', N'The learner is not over the school leaving age');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DateOfBirth_55', N'E', N'The learning aim started on or after 1 August 2017 and the learner is 24 or over and the aim is Level 3 or above and the Funding model is Adult skills');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DelLocPostCode_03', N'W', N' ''Delivery location postcode'' is wrong. You must enter a valid UK postcode. If the system isn''t recognising your postcode because it''s a new build, you can temporarily use ZZ99 9ZZ. Check ''delivery location postcode'' is correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DelLocPostCode_11', N'E', N'The Delivery location postcode is not in the correct format');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DelLocPostCode_14', N'E', N'The Delivery location postcode is not valid in the local authority described in the ESF Tender Specification');
GO
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DelLocPostCode_15', N'E', N'The Delivery location postcode is not valid in the LEP described in the ESF Tender Specification');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DelLocPostCode_16', N'E', N'The Delivery location postcode is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DelLocPostCode_17', N'E', N'''Delivery location postcode'' is wrong. It''s not within the local authority stated in the tender specification. Check ''delivery location postcode'' is correct and within the local authority stated in your tender specification.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DelLocPostCode_18', N'E', N'The Delivery location postcode is not valid in the LEP described in the ESF Tender Specification');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DLOCK_01', N'W', N'No matching UKPRN record found');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DLOCK_02', N'W', N'No matching ULN found');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DLOCK_03', N'W', N'No matching standard code found');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DLOCK_04', N'W', N'No matching framework code found');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DLOCK_05', N'W', N'No matching programme type code found');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DLOCK_06', N'W', N'No matching pathway code found');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DLOCK_07', N'W', N'No matching negotiated cost of training and assessment found');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DLOCK_08', N'W', N'Multiple matching records found');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DLOCK_09', N'W', N'The learning delivery start month is before the agreed start month');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DLOCK_10', N'W', N'The employer has stopped payments for this apprentice');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DLOCK_11', N'W', N'The employer is not currently a levy payer');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DLOCK_12', N'W', N'The employer has paused payments for this apprentice');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DOMICILE_01', N'E', N'The Domicile must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'DOMICILE_02', N'E', N'The Domicile is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ELQ_01', N'E', N'The ELQ must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ELQ_02', N'E', N'The ELQ is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpId_01', N'E', N'''Employer identifier'' is wrong. We can''t find the number you''ve provided on our system. Check that the ''Employer identifier'' is correct. You can use 999999999 if you''re waiting for an employer ID to be allocated.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpId_02', N'E', N'The Employer identifier does not pass the checksum calculation');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpId_10', N'E', N'The Employer identifier has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpId_13', N'E', N'The Employer identifier is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpOutcome_01', N'E', N'The Employment outcome must not be returned for this Funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpOutcome_02', N'E', N'The Employment outcome is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpOutcome_03', N'E', N'The Employment outcome must not be returned for apprenticeships');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpStat_01', N'E', N'An Employment status record has not been returned that applies prior to this Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpStat_02', N'E', N'An Employment status record has not been returned that applies prior to this Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpStat_04', N'E', N'The Employment status is not valid for learning aims funded by ESF');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpStat_05', N'E', N'The Employment status is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpStat_06', N'E', N'An Employment status record has not been returned that applies prior to the Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpStat_07', N'E', N'An Employment status record has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpStat_08', N'E', N'An Employment status record has not been returned that applies prior to the Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpStat_09', N'E', N'An Employment status record has not been returned that applies prior to the Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpStat_10', N'E', N'An Employment status record has not been returned that applies prior to the Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpStat_11', N'E', N'An Employment status record has not been returned that applies prior to the Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpStat_12', N'W', N'''Employment status'' may be wrong. The apprentice should be in employment at the start of their apprenticeship. Check ''employment status'' is correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpStat_14', N'E', N'The learner''s Employment status does not match the employment status described in the ESF Tender Specification');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpStat_15', N'E', N'The Employment status on the start date of an apprenticeship programme must not be unknown');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpStat_17', N'E', N'The Employment status on the start date of an traineeship programme must not be unknown');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpStat_18', N'E', N'The learner must not be employed for more than 16 hours her week at the start of the programme');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpStat_19', N'E', N'The learner must not be employed for more than 16 hours her week at the start of the programme');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EmpStat_20', N'E', N'The learner must be in paid employment for this Learning Delivery Monitoring code to be used');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EngGrade_01', N'E', N'The GCSE English qualification grade has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EngGrade_02', N'E', N'The GCSE English qualification grade is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EngGrade_03', N'E', N'The Learner FAM Type of Eligibility for EFA disadvantage funding must be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EngGrade_04', N'E', N'GCSE English qualification grade must be ''NONE''');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Entity_1', N'E', N'You''re unable to submit this file because it doesn''t contain any valid learners. Please upload an updated file.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EPAOrgID_01', N'W', N' ''End point assessment organisation'' may be wrong. It''s not a recognised organisation for the standard you''re delivering on the planned end date of the apprenticeship. Check ''end point assessment organisation'' and ''planned end date'' are correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EPAOrgID_02', N'E', N'The End point assessment organisation payment record has been returned but there is no record of the End point assessment organisation.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'EPAOrgID_03', N'E', N'''Apprenticeship Financial Record'' is missing. As you''ve returned the ''end point assessment organisation'' ID, you also need to return the end point assessment financial details. Check apprenticeship financial record includes the end point assessment price.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ESMType_01', N'E', N'The Employment status monitoring type and code is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ESMType_02', N'E', N'The learner''s Employment status is ''in paid employment'' and the Employment intensity indicator has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ESMType_05', N'E', N'The learner''s Employment status is ''in paid employment'' and the Length of unemployment has been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ESMType_07', N'E', N'The learner is self employed and the Employment status is not ''in paid employment''');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ESMType_08', N'E', N'The Length of unemployment has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ESMType_09', N'E', N'The Length of employment has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ESMType_10', N'E', N'The Length of employment has been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ESMType_11', N'E', N'The Employment status monitoring type and code is not valid for the Employment status date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ESMType_12', N'E', N'The Employment intensity indicator or Self employed indicator has been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ESMType_13', N'E', N'The learner''s Length of unemployment does not match the length of unemployment described in the ESF Tender Specification');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ESMType_14', N'E', N'The learner''s benefit status does not match the benefit status described in the ESF Tender Specification');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ESMType_15', N'E', N'There must not be more than one record with an ESM type of SEI, EII, LOU, LOE, BSI, PEI, or SEM');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Ethnicity_01', N'E', N'The Ethnicity is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FamilyName_01', N'E', N'The learner''s Family name must be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FamilyName_02', N'E', N'The learner''s Family name must be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FamilyName_04', N'E', N'The learner''s Family name must be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_Accom_AL', N'E', N'The value returned for Accom is not 1 character long');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AddHours_AL', N'E', N'The length of the value returned for AddHours is not between 1 and 4 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AddHours_AR', N'E', N'The AddHours is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AddLine1_AL', N'E', N'The length of the value returned for AddLine1 is not between 1 and 50 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AddLine1_AP', N'E', N'The AddLine1 does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AddLine2_AL', N'E', N'The length of the value returned for AddLine2 is not between 1 and 50 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AddLine2_AP', N'E', N'The AddLine2 does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AddLine3_AL', N'E', N'The length of the value returned for AddLine3 is not between 1 and 50 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AddLine3_AP', N'E', N'The AddLine3 does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AddLine4_AL', N'E', N'The length of the value returned for AddLine4 is not between 1 and 50 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AddLine4_AP', N'E', N'The AddLine4 does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AFinAmount_AL', N'E', N'The length of the value returned for AFinAmount is not between 1 and 6 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AFinAmount_AR', N'E', N'The AFinAmount is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AFinAmount_MA', N'E', N'The AFinAmount has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AFinCode_AL', N'E', N'The length of the value returned for AFinCode is not between 1 and 2 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AFinCode_MA', N'E', N'The AFinCode has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AFinDate_MA', N'E', N'The AFinDate has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AFinType_AL', N'E', N'The length of the value returned for AFinType is not between 1 and 3 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AFinType_AP', N'E', N'The AFinType does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AFinType_MA', N'E', N'The AFinType has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AgreeId_AL', N'E', N'The value returned for AgreeId is greater than the permitted length');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AgreeId_AP', N'E', N'The AgreeId does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AimSeqNumber_AL', N'E', N'The length of the value returned for AimSeqNumber is not between 1 and 2 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AimSeqNumber_AR', N'E', N'The AimSeqNumber is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AimSeqNumber_MA', N'E', N'The AimSeqNumber has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AimType_AL', N'E', N'The value returned for AimType is not 1 character long');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_AimType_MA', N'E', N'The AimType has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ALSCost_AL', N'E', N'The length of the value returned for ALSCost is not between 1 and 6 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ALSCost_AR', N'E', N'The ALSCost is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_CampId_AL', N'E', N'The value returned for CampId is greater than the permitted length');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_CampId_AP', N'E', N'The CampId does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_CompStatus_AL', N'E', N'The value returned for CompStatus is not 1 character long');
GO
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_CompStatus_MA', N'E', N'The CompStatus has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ConRefNumber_AL', N'E', N'The length of the value returned for ConRefNumber is not between 1 and 20 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ConRefNumber_AP', N'E', N'The ConRefNumber does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ContactPreference_EO', N'E', N'The ContactPreference has occurred more than allowed');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ContPrefCode_AL', N'E', N'The value returned for ContPrefCode is not 1 character long');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ContPrefCode_MA', N'E', N'The ContPrefCode has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ContPrefType_AL', N'E', N'The length of the value returned for ContPrefType is not between 1 and 3 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ContPrefType_AP', N'E', N'The ContPrefType does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ContPrefType_MA', N'E', N'The ContPrefType has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DateEmpStatApp_MA', N'E', N'The DateEmpStatApp has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DelLocPostCode_AL', N'E', N'The length of the value returned for DelLocPostCode is not between 1 and 8 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DelLocPostCode_AP', N'E', N'The DelLocPostCode does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DelLocPostCode_MA', N'E', N'The DelLocPostCode has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DOMICILE_AL', N'E', N'The length of the value returned for DOMICILE is not between 1 and 2 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DOMICILE_AP', N'E', N'The DOMICILE does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DP_DPOutcome_EO', N'E', N'The DPOutcome has not met the minimum occurrence requirement');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DP_LearnRefNumber_AL', N'E', N'The length of the value returned for LearnRefNumber is not between 1 and 12 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DP_LearnRefNumber_AP', N'E', N'The LearnRefNumber does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DP_LearnRefNumber_MA', N'E', N'The LearnRefNumber has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DP_OutCode_AL', N'E', N'The length of the value returned for OutCode is not between 1 and 3 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DP_OutCode_MA', N'E', N'The OutCode has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DP_OutCollDate_MA', N'E', N'The OutCollDate has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DP_OutStartDate_MA', N'E', N'The OutStartDate has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DP_OutType_AL', N'E', N'The length of the value returned for OutType is not between 1 and 3 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DP_OutType_AP', N'E', N'The OutType does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DP_OutType_MA', N'E', N'The OutType has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DP_ULN_AL', N'E', N'The length of the value returned for ULN is not between 1 and 10 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DP_ULN_AR', N'E', N'The ULN is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_DP_ULN_MA', N'E', N'The ULN has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ELQ_AL', N'E', N'The value returned for ELQ is not 1 character long');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_Email_AL', N'E', N'The length of the value returned for Email is not between 1 and 100 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_Email_AP', N'E', N'The Email does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_EmpId_AL', N'E', N'The length of the value returned for EmpId is not between 1 and 9 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_EmploymentStatusMonitoring_EO', N'E', N'The EmploymentStatusMonitoring has occurred more than allowed');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_EmpOutcome_AL', N'E', N'The value returned for EmpOutcome is not 1 character long');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_EmpStat_AL', N'E', N'The length of the value returned for EmpStat is not between 1 and 2 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_EmpStat_MA', N'E', N'The EmpStat has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_EngGrade_AL', N'E', N'The length of the value returned for EngGrade is not between 1 and 4 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_EngGrade_AP', N'E', N'The EngGrade does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_EPAOrgID_AL', N'E', N'The length of the value returned for EPAOrgID is not between 1 and 7 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_EPAOrgID_AP', N'E', N'The EPAOrgID does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ESMCode_AL', N'E', N'The length of the value returned for ESMCode is not between 1 and 2 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ESMCode_MA', N'E', N'The ESMCode has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ESMType_AL', N'E', N'The length of the value returned for ESMType is not between 1 and 3 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ESMType_AP', N'E', N'The ESMType does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ESMType_MA', N'E', N'The ESMType has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_Ethnicity_AL', N'E', N'The length of the value returned for Ethnicity is not between 1 and 2 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_Ethnicity_MA', N'E', N'The Ethnicity has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_FamilyName_AL', N'E', N'The length of the value returned for FamilyName is not between 1 and 100 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_FamilyName_AP', N'E', N'The FamilyName does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_FINAMOUNT_AL', N'E', N'The length of the value returned for FINAMOUNT is not between 1 and 6 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_FINAMOUNT_AR', N'E', N'The FINAMOUNT is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_FINAMOUNT_MA', N'E', N'The FINAMOUNT has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_FINTYPE_AL', N'E', N'The value returned for FINTYPE is not 1 character long');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_FINTYPE_MA', N'E', N'The FINTYPE has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_FUNDCOMP_AL', N'E', N'The value returned for FUNDCOMP is not 1 character long');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_FUNDCOMP_MA', N'E', N'The FUNDCOMP has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_FUNDLEV_AL', N'E', N'The length of the value returned for FUNDLEV is not between 1 and 2 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_FUNDLEV_MA', N'E', N'The FUNDLEV has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_FundModel_AL', N'E', N'The length of the value returned for FundModel is not between 1 and 2 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_FundModel_MA', N'E', N'The FundModel has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_FworkCode_AL', N'E', N'The length of the value returned for FworkCode is not between 1 and 3 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_GivenNames_AL', N'E', N'The length of the value returned for GivenNames is not between 1 and 100 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_GivenNames_AP', N'E', N'The GivenNames does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_GROSSFEE_AL', N'E', N'The length of the value returned for GROSSFEE is not between 1 and 6 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_GROSSFEE_AR', N'E', N'The GROSSFEE is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_HEPostCode_AL', N'E', N'The length of the value returned for HEPostCode is not between 1 and 8 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_HEPostCode_AP', N'E', N'The HEPostCode does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnAimRef_AL', N'E', N'The length of the value returned for LearnAimRef is not between 1 and 8 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnAimRef_AP', N'E', N'The LearnAimRef does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnAimRef_MA', N'E', N'The LearnAimRef has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnDelFAMCode_AL', N'E', N'The length of the value returned for LearnDelFAMCode is not between 1 and 5 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnDelFAMCode_AP', N'E', N'The LearnDelFAMCode does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnDelFAMCode_MA', N'E', N'The LearnDelFAMCode has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnDelFAMType_AL', N'E', N'The length of the value returned for LearnDelFAMType is not between 1 and 3 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnDelFAMType_AP', N'E', N'The LearnDelFAMType does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnDelFAMType_MA', N'E', N'The LearnDelFAMType has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnerFAM_EO', N'E', N'The LearnerFAM has occurred more than allowed');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnerHE_EO', N'E', N'The LearnerHE has occurred more than allowed');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnerHEFinancialSupport_EO', N'E', N'The LearnerHEFinancialSupport has occurred more than allowed');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnFAMCode_AL', N'E', N'The length of the value returned for LearnFAMCode is not between 1 and 3 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnFAMCode_MA', N'E', N'The LearnFAMCode has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnFAMType_AL', N'E', N'The length of the value returned for LearnFAMType is not between 1 and 3 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnFAMType_AP', N'E', N'The LearnFAMType does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnFAMType_MA', N'E', N'The LearnFAMType has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearningDelivery_EO', N'E', N'The LearningDelivery has not met the minimum occurrence requirement');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearningDeliveryHE_EO', N'E', N'The LearningDeliveryHE has occurred more than allowed');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnPlanEndDate_MA', N'E', N'The LearnPlanEndDate has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnRefNumber_AL', N'E', N'The length of the value returned for LearnRefNumber is not between 1 and 12 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnRefNumber_AP', N'E', N'The LearnRefNumber does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnRefNumber_MA', N'E', N'The LearnRefNumber has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LearnStartDate_MA', N'E', N'The LearnStartDate has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LLDDandHealthProblem_EO', N'E', N'The LLDDandHealthProblem has occurred more than allowed');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LLDDCat_AL', N'E', N'The length of the value returned for LLDDCat is not between 1 and 2 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LLDDCat_MA', N'E', N'The LLDDCat has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LLDDHealthProb_AL', N'E', N'The value returned for LLDDHealthProb is not 1 character long');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_LLDDHealthProb_MA', N'E', N'The LLDDHealthProb has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_MathGrade_AL', N'E', N'The length of the value returned for MathGrade is not between 1 and 4 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_MathGrade_AP', N'E', N'The MathGrade does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_MODESTUD_AL', N'E', N'The length of the value returned for MODESTUD is not between 1 and 2 characters');
GO
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_MODESTUD_MA', N'E', N'The MODESTUD has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_MSTUFEE_AL', N'E', N'The length of the value returned for MSTUFEE is not between 1 and 2 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_MSTUFEE_MA', N'E', N'The MSTUFEE has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_NETFEE_AL', N'E', N'The length of the value returned for NETFEE is not between 1 and 6 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_NETFEE_AR', N'E', N'The NETFEE is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_NINumber_AL', N'E', N'The length of the value returned for NINumber is not between 1 and 9 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_NINumber_AP', N'E', N'The NINumber does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_NUMHUS_AL', N'E', N'The length of the value returned for NUMHUS is not between 1 and 20 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_NUMHUS_AP', N'E', N'The NUMHUS does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_OtherFundAdj_AL', N'E', N'The length of the value returned for OtherFundAdj is not between 1 and 3 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_OtherFundAdj_AR', N'E', N'The OtherFundAdj is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_OTJHours_AL', N'E', N'The value returned for OTJHours does not contain a value between 0 and 9999');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_OTJHours_AR', N'E', N'The OTJHours is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_Outcome_AL', N'E', N'The value returned for Outcome is not 1 character long');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_OutGrade_AL', N'E', N'The length of the value returned for OutGrade is not between 1 and 6 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_OutGrade_AP', N'E', N'The OutGrade does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PartnerUKPRN_AL', N'E', N'The length of the value returned for PartnerUKPRN is not between 1 and 8 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PartnerUKPRN_AR', N'E', N'The PartnerUKPRN is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PCFLDCS_AL', N'E', N'The length of the value returned for PCFLDCS is not between 1 and 4 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PCFLDCS_AR', N'E', N'The PCFLDCS is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PCOLAB_AL', N'E', N'The length of the value returned for PCOLAB is not between 1 and 4 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PCOLAB_AR', N'E', N'The PCOLAB is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PCSLDCS_AL', N'E', N'The length of the value returned for PCSLDCS is not between 1 and 4 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PCSLDCS_AR', N'E', N'The PCSLDCS is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PCTLDCS_AL', N'E', N'The length of the value returned for PCTLDCS is not between 1 and 4 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PCTLDCS_AR', N'E', N'The PCTLDCS is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PlanEEPHours_AL', N'E', N'The length of the value returned for PlanEEPHours is not between 1 and 4 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PlanEEPHours_AR', N'E', N'The PlanEEPHours is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PlanLearnHours_AL', N'E', N'The length of the value returned for PlanLearnHours is not between 1 and 4 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PlanLearnHours_AR', N'E', N'The PlanLearnHours is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PMUKPRN_AL', N'E', N'The length of the value returned for PMUKPRN is not between 1 and 8 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PMUKPRN_AR', N'E', N'The PMUKPRN is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_Postcode_AL', N'E', N'The length of the value returned for Postcode is not between 1 and 8 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_Postcode_AP', N'E', N'The Postcode does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_Postcode_MA', N'E', N'The Postcode has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PostcodePrior_AL', N'E', N'The length of the value returned for PostcodePrior is not between 1 and 8 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PostcodePrior_AP', N'E', N'The PostcodePrior does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PostcodePrior_MA', N'E', N'The PostcodePrior has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PrevLearnRefNumber_AL', N'E', N'The length of the value returned for PrevLearnRefNumber is not between 1 and 12 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PrevLearnRefNumber_AP', N'E', N'The PrevLearnRefNumber does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PrevUKPRN_AL', N'E', N'The length of the value returned for PrevUKPRN is not between 1 and 8 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PrevUKPRN_AR', N'E', N'The PrevUKPRN is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PrimaryLLDD_AL', N'E', N'The value returned for PrimaryLLDD is not 1 character long');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PriorAttain_AL', N'E', N'The length of the value returned for PriorAttain is not between 1 and 2 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PriorLearnFundAdj_AL', N'E', N'The length of the value returned for PriorLearnFundAdj is not between 1 and 2 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PriorLearnFundAdj_AR', N'E', N'The PriorLearnFundAdj is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ProgType_AL', N'E', N'The length of the value returned for ProgType is not between 1 and 2 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ProviderSpecDeliveryMonitoring_EO', N'E', N'The ProviderSpecDeliveryMonitoring has occurred more than allowed');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ProviderSpecLearnerMonitoring_EO', N'E', N'The ProviderSpecLearnerMonitoring has occurred more than allowed');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ProvSpecDelMon_AL', N'E', N'The length of the value returned for ProvSpecDelMon is not between 1 and 20 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ProvSpecDelMon_AP', N'E', N'The ProvSpecDelMon does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ProvSpecDelMon_MA', N'E', N'The ProvSpecDelMon has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ProvSpecDelMonOccur_AL', N'E', N'The value returned for ProvSpecDelMonOccur is not 1 character long');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ProvSpecDelMonOccur_AP', N'E', N'The ProvSpecDelMonOccur does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ProvSpecDelMonOccur_MA', N'E', N'The ProvSpecDelMonOccur has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ProvSpecLearnMon_AL', N'E', N'The length of the value returned for ProvSpecLearnMon is not between 1 and 20 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ProvSpecLearnMon_AP', N'E', N'The ProvSpecLearnMon does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ProvSpecLearnMon_MA', N'E', N'The ProvSpecLearnMon has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ProvSpecLearnMonOccur_AL', N'E', N'The value returned for ProvSpecLearnMonOccur is not 1 character long');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ProvSpecLearnMonOccur_AP', N'E', N'The ProvSpecLearnMonOccur does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ProvSpecLearnMonOccur_MA', N'E', N'The ProvSpecLearnMonOccur has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_PwayCode_AL', N'E', N'The length of the value returned for PwayCode is not between 1 and 3 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_QUALENT3_AL', N'E', N'The length of the value returned for QUALENT3 is not between 1 and 3 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_QUALENT3_AP', N'E', N'The QUALENT3 does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_SEC_AL', N'E', N'The value returned for SEC is not 1 character long');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_Sex_AL', N'E', N'The value returned for Sex is not 1 character long');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_Sex_AP', N'E', N'The Sex does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_Sex_MA', N'E', N'The Sex has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_SOC2000_AL', N'E', N'The length of the value returned for SOC2000 is not between 1 and 4 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_SPECFEE_AL', N'E', N'The value returned for SPECFEE is not 1 character long');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_SPECFEE_MA', N'E', N'The SPECFEE has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_SSN_AL', N'E', N'The length of the value returned for SSN is not between 1 and 13 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_SSN_AP', N'E', N'The SSN does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_StdCode_AL', N'E', N'The length of the value returned for StdCode is not between 1 and 5 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_STULOAD_AL', N'E', N'The length of the value returned for STULOAD is not between 1 and 4 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_STULOAD_AR', N'E', N'The STULOAD is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_SWSupAimId_AL', N'E', N'The length of the value returned for SWSupAimId is not between 1 and 36 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_SWSupAimId_AP', N'E', N'The SWSupAimId does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_TelNo_AL', N'E', N'The length of the value returned for TelNo is not between 1 and 18 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_TelNo_AP', N'E', N'The TelNo does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_TTACCOM_AL', N'E', N'The value returned for TTACCOM is not 1 character long');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_TYPEYR_AL', N'E', N'The value returned for TYPEYR is not 1 character long');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_TYPEYR_MA', N'E', N'The TYPEYR has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_UCASAPPID_AL', N'E', N'The length of the value returned for UCASAPPID is not between 1 and 9 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_UCASAPPID_AP', N'E', N'The UCASAPPID does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_UCASPERID_AL', N'E', N'The length of the value returned for UCASPERID is not between 1 and 10 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_UCASPERID_AP', N'E', N'The UCASPERID does not match the validation pattern');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_UCASPERID_AR', N'E', N'The UCASPERID is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ULN_AL', N'E', N'The length of the value returned for ULN is not between 1 and 10 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ULN_AR', N'E', N'The ULN is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_ULN_MA', N'E', N'The ULN has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_WithdrawReason_AL', N'E', N'The length of the value returned for WithdrawReason is not between 1 and 2 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_WorkPlaceEmpId_AL', N'E', N'The length of the value returned for WorkPlaceEmpId is not between 1 and 9 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_WorkPlaceHours_AL', N'E', N'The length of the value returned for WorkPlaceHours is not between 1 and 4 characters');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_WorkPlaceHours_AR', N'E', N'The WorkPlaceHours is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_WorkPlaceHours_MA', N'E', N'The WorkPlaceHours has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_WorkPlaceMode_AL', N'E', N'The value returned for WorkPlaceMode is not 1 character long');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_WorkPlaceMode_MA', N'E', N'The WorkPlaceMode has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_WorkPlaceStartDate_MA', N'E', N'The WorkPlaceStartDate has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_YEARSTU_AL', N'E', N'The length of the value returned for YEARSTU is not between 1 and 2 characters');
GO
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_YEARSTU_AR', N'E', N'The YEARSTU is out of range');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FD_YEARSTU_MA', N'E', N'The YEARSTU has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Filename_1', N'E', N'There''s a problem. The filename should use the format ILR-LLLLLLLL-YYYY-yyyymmdd-hhmmss-NN.xml	');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Filename_2', N'E', N'There''s a problem.  You have already uploaded a file with the same filename. Upload a file with a different filename.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Filename_4', N'E', N'There''s a problem. The UK Provider Reference Number (UKPRN) is not valid - check the UKPRN is correct.	');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Filename_5', N'E', N'There’s a problem.  The year in the filename should match the current year.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Filename_6', N'E', N'The serial number in the filename (the last two characters) must be a two digit number.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Filename_7', N'E', N'There''s a problem. The serial number in the filename must not be 00.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Filename_8', N'E', N'There''s a problem.  The date/time in your filename must not be in the future.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Filename_9', N'E', N'There''s a problem. The date and time in the filename must not be earlier than a file already uploaded.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FINTYPE_01', N'E', N'The Financial support type is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FINTYPE_02', N'E', N'There must not be more than one record with a Financial support type of Cash, Near cash, Accommodation discounts, or Other');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FUNDCOMP_01', N'E', N'The Completion of year of instance is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FUNDLEV_01', N'E', N'The Level applicable to Funding Council HEIFES is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FUNDLEV_02', N'W', N'The Level applicable to Funding Council HEIFES is not valid for this type of learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FUNDLEV_03', N'W', N'The Level applicable to Funding Council HEIFES is not valid for this type of learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FundModel_01', N'E', N'The Funding model is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FundModel_03', N'E', N'The Funding model is not valid for a learning aim financed by an Advanced Learner Loan');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FundModel_04', N'E', N'The Funding model is not valid for a learning aim which is part of an apprenticeship programme');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FundModel_05', N'E', N'The Funding model is not valid for starts before 1 May 2017');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FundModel_06', N'E', N'The funding model is not valid for this aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FundModel_07', N'E', N'The funding model is not valid for an apprenticeship programme');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FundModel_08', N'E', N'The funding model is not valid for this apprenticeship programme');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FundModel_09', N'E', N'The funding model is not valid for this apprenticeship programme');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FworkCode_01', N'E', N'The Framework code must be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FworkCode_02', N'E', N'The Framework code must not be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'FworkCode_05', N'E', N'The Framework code, Pathway code and Programme type combination is not valid in the LARS database for this Learning aim reference');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'GivenNames_01', N'E', N'The learner''s Given names must be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'GivenNames_02', N'E', N'The learner''s Given names must be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'GivenNames_04', N'E', N'The learner''s Given names must be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'GROSSFEE_01', N'E', N'The Gross tuition fee must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'GROSSFEE_02', N'E', N'The Gross tuition fee must be equal to or greater than the Net tuition fee');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'GROSSFEE_03', N'E', N'The Gross tuition fee must not be greater than £30000');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Header_2', N'E', N'You''re unable to submit this file because the file preparation date in your header record is in the future. Please upload an updated file.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Header_3', N'F', N'You''re unable to submit this file because the UKPRN in the file''s header record doesn''t match the UKPRN in the filename. Please upload an updated file.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'HEPostCode_01', N'W', N'''HE centre location postcode'' is wrong. We don''t recognise the postcode you''ve given as a UK postcode. Check ''HE centre location postcode'' is correct. If it''s unknown, please don''t return that field.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'HEPostCode_02', N'E', N'The Postcode must conform to the valid postcode format');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Inconsistent UKPRN', N'E', N'You''re unable to submit this file because the UKPRN in the file''s header record doesn''t match the UKPRN in the filename. Please upload an updated file.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnActEndDate_01', N'E', N'The Learning actual end date must be after the Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnActEndDate_04', N'E', N'The Learning actual end date must be before the file preparation date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_01', N'E', N'The Learning aim reference is not a valid lookup on LARS');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_29', N'E', N'The Learning aim reference is not valid for this Funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_30', N'E', N'The Learning aim reference is not valid for a programme aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_55', N'E', N'The learning aim must be recorded as the Core aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_56', N'E', N'The learning aim is not valid for Adult skills funding');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_57', N'E', N'The learning aim is not valid for Adult skills funding');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_59', N'E', N'The learning aim is not valid for Adult skills funding');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_71', N'E', N'''Sector subject area'' for this learning aim is wrong. The ''sector subject area'' must match the level in your Tender Specification. Check the learning aim is correct and that the sector subject area level matches what is in your Tender Specification.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_72', N'E', N'The learning aim''s level does not match the level described in the ESF Tender Specification');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_73', N'E', N'''Sector subject area'' and/or level for this learning aim are wrong. They don''t match the area and level set out in your tender specification. Check the learning aim is correct and that the ''sector subject area'' level matches what is in your Tender Specification.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_78', N'E', N'The learning aim is not valid for Adult skills funding');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_79', N'E', N'The learning aim is not valid for Adult skills funding');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_80', N'E', N'The learning aim is not valid for Adult skills funding');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_81', N'E', N'The learning aim is not valid for Adult skills funding');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_84', N'E', N'The learning aim is not valid for Adult skills funding');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_85', N'E', N'The learning aim is not valid for Adult skills funding');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_86', N'E', N'The Learning aim reference is not valid for this funding model where the learning aim is not part of a traineeship');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_87', N'E', N'ZVOC or ZUXA aims must not be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_88', N'E', N'The Learning aim reference is not valid in the LARS database for this Funding model and this Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnAimRef_89', N'E', N'The Learning aim reference is not valid in the LARS database for this Funding model for this teaching year');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMDateFrom_01', N'E', N'The Date applies from or the Date applies to must be returned for this FAM type');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMDateFrom_02', N'E', N'The Date applies from must be after the Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMDateFrom_03', N'E', N'The Date applies from must not be returned for this FAM type');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMDateFrom_04', N'E', N'The Date applies from must be returned for this FAM type');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMDateTo_01', N'E', N'The Date applies to must not be before the Date applies from for this FAM type');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMDateTo_02', N'W', N' ''Date applies to'' may be wrong. Unless the ''learning actual end date'' is later than the ''planned end date'', the ''Date applies to'' shouldn''t be after the planned end date for the learning. Check ''learning planned end date'' and ''Date applies to'' are correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMDateTo_03', N'E', N'The Date applies to must not be after the Learning actual end date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMDateTo_04', N'E', N'The Date applies to must not be returned for this FAM type');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_01', N'E', N'The Source of funding must be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_02', N'E', N'The Full or co-funding indicator must be returned for this Funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_03', N'E', N'The Community Learning provision type must be returned for this Funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_04', N'E', N'The FAM type and code is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_06', N'E', N'The FAM type and code is not valid for this Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_07', N'E', N'The Source of funding is not valid for this Funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_08', N'E', N'The Source of funding is not valid for this Funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_09', N'E', N'The Source of funding is not valid for this Funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_14', N'E', N'The Eligibility for enhanced apprenticeship funding is only valid for apprenticeship aims');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_15', N'E', N'This Learning delivery monitoring code must not be returned where the Learning actual end date is completed');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_16', N'E', N'This Learning delivery monitoring code is not valid for this file preparation date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_18', N'E', N'There must not be more than one record with a FAM type of SOF, FFI, EEF, RES, ADL, ASL, SPP, NSA, WPP, POD or FLN');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_20', N'E', N'There must not be more than three occurrences with a FAM type of HEM');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_22', N'E', N'The Full or co-funding indicator must not be returned for this Funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_24', N'E', N'The FAM type is not valid for this Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_27', N'E', N'The ASL provision type must not be returned for this Funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_30', N'E', N'The Learning Delivery HE record must be returned for this FAM type');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_31', N'E', N'There must not be more than four occurrences for the FAM type LDM');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_35', N'E', N'The Advanced Learner Loans indicator must not be returned for this Funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_36', N'E', N'The Advanced Learner Loans indicator must be completed where the Advanced Learner Loans Bursary funding indicator has been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_38', N'E', N'The Advanced Learner Loans Bursary fund indicator is not required for this Funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_39', N'W', N'FAM type'' is missing. Based on the funding model, you need to return a FAM type of LDM. Check the funding model is correct. If so, enter LDM in FAM type andmake sure the correct code is in ''FAM code'' from the LDM codes list (available online - google ''LDM codes ESFA'').');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_40', N'E', N'The Advanced Learner Loans indicator must not be returned for apprenticeships');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_44', N'E', N'The Household situation must be returned for this Funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_45', N'E', N'There must not be more than two occurrences of Household Situation');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_46', N'E', N'The Funding model must be Adult skills');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_48', N'E', N'The combination of Household situation codes is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_50', N'E', N'Learning Support Funding must not be returned for this Funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_53', N'E', N'''Learning delivery FAM type'' is wrong. The Advanced Learner Bursary rate 1 or 3 can''t be used when the aim is grant funded. Check ''FAM type'' is correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_54', N'E', N'The combination of EEF and FFI codes is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_56', N'E', N'The Full funding indicator is not valid for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_57', N'E', N'The Full funding indicator is not valid for this learning aim');
GO
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_58', N'E', N'The Full funding indicator is not valid for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_59', N'E', N'The Full funding indicator is not valid for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_60', N'E', N'The Full funding indicator is not valid for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_61', N'E', N'The Full funding indicator is not valid for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_62', N'W', N' ''Learning delivery FAM type'' and ''learning delivery FAM code'' is wrong. The type and code you''ve returned indicates that this learner is co-funded. But this learner''s prior attainment must be 2 or above at the start of the aim to be co-funded. Check ''learning delivery FAM type'', ''learning delivery FAM code'' and ''prior attainment'' are correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_63', N'E', N'The Apprenticeship contract type must not be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_64', N'E', N'The Apprenticeship contract type must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_65', N'E', N'The Full funding indicator is not valid for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_66', N'E', N'The Full funding indicator is not valid for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_67', N'E', N'Learning support funding must only be recorded on a programme aim or English/maths aim for apprenticeship funded programmes');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_68', N'E', N'LDM356 can only be recorded on a Funding model 36 programme aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFAMType_69', N'E', N'LDM357 can only be recorded on Funding models 35 or 10 and starting on or after 1st November 2017');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFamType_70', N'E', N'LDM358 can only be recorded against contracts between 1st January and 31 March 2018');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFamType_71', N'E', N'There should be an ACT 2 record for this LDM code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFamType_72', N'E', N'If LDM 359 has been returned and the aims have been recorded against funding model 99, then ADL 1 must be used');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFamType_73', N'E', N'LDM 359 can only be recorded against funding models 81 or 99');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnDelFamType_74', N'W', N'The Source of funding is not valid for this Funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnerHE_02', N'E', N'A Learning Delivery HE record must be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnFAMType_01', N'E', N'The FAM type and code is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnFAMType_03', N'E', N'The FAM type and code is not valid for this Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnFAMType_09', N'E', N'There must not be more than one record with a FAM type of HNS, EHC, DLA, SEN, MCF, ECF or FME');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnFAMType_10', N'E', N'There must not be more than four records with a FAM type of LSR');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnFAMType_11', N'E', N'There must not be more than two records with a FAM type of EDF, NLM or PPE');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnFAMType_14', N'E', N'The learner must not have both a FAM type of SEN and EHC');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnFAMType_15', N'E', N'The learner is recorded as exempt from maths GCSE condition of funding due to a learning difficulty but there is no Education health care plan recorded');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnFAMType_16', N'E', N'The learner is recorded as exempt from English GCSE condition of funding due to a learning difficulty but there is no Education health care plan recorded');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearningDeliveryHE_02', N'E', N'A Learning Delivery HE record must be returned for this Source of funding');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearningDeliveryHE_03', N'E', N'A Learning Delivery HE record must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearningDeliveryHE_06', N'E', N'A Learning Delivery HE record must not be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearningDeliveryHE_07', N'E', N'A Learning Delivery HE record must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearningDeliveryHE_08', N'E', N'A Learning Delivery HE record must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnPlanEndDate_02', N'E', N'The Learning planned end date must not be before the Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnPlanEndDate_03', N'E', N'The Learning planned end date must not be 10 years or more after the Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnStartDate_02', N'E', N'The Learning start date is more than 10 years before the start of the current teaching year');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnStartDate_03', N'E', N'The Learning start date must not be after the end of the current teaching year');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnStartDate_05', N'E', N'The Learning start date must not be on or before the learner''s Date of birth');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnStartDate_06', N'E', N'The Learning start date of the apprenticeship programme must not be after the Effective to date of this apprenticeship pathway');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnStartDate_07', N'E', N'The Learning start date of the apprenticeship programme must not be after the Effective to date of this component aim for this apprenticeship pathway');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnStartDate_12', N'E', N'The Learning start date must not be more than one year in the future');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnStartDate_13', N'E', N'The Learning start date of the apprenticeship standard programme must not be after the Effective to date of this apprenticeship standard');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnStartDate_14', N'E', N'The Learning start date of the apprenticeship standard programme must not be after the Effective to date of this component aim for this standard');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnStartDate_15', N'E', N'The Learning start date must be after the start date of the completed ZESF0001 aim for that contract');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnStartDate_16', N'E', N'The Learning start date must not be before the start date of the contract');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LearnStartDate_17', N'E', N'The Learning start date of the apprenticeship standard programme must not be before the Effective From date of this apprenticeship standard');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LLDDCat_01', N'E', N'The LLDD and health problem category is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LLDDCat_02', N'E', N'The LLDD and health problem category is not valid for this start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LLDDHealthProb_01', N'E', N'The LLDD and health problem is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LLDDHealthProb_04', N'E', N'The LLDD and Health Problem record has been returned and the learner does not consider himself or herself to have a learning difficulty and/or disability or health problem');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LLDDHealthProb_06', N'E', N' ''LLDD and health problem'' is missing. You''ve indicated that the learner feels that they have a learning difficulty and/or disability or health problem, but have not entered the any ''LLDD and health problem'' details. Check ''LLDD and health problem'' is complete and correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LLDDHealthProb_07', N'E', N'The LLDD and Health Problem record has not been returned and the learner does consider himself or herself to have a learning difficulty and/or disability or health problem');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LSDPostcode_01', N'E', N'The Learning start date postcode is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'LSDPostcode_02', N'E', N'The learning aim start date postcode is not valid on the start date of the learning aim and the source of funding');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'MathGrade_01', N'E', N'The GCSE maths qualification grade has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'MathGrade_02', N'E', N'The GCSE maths qualification grade is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'MathGrade_03', N'E', N'The Learner FAM Type of Eligibility for EFA disadvantage funding must be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'MathGrade_04', N'E', N'GCSE maths qualification grade must be ''NONE''');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'MODESTUD_01', N'E', N'The Mode of study is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'MODESTUD_03', N'E', N'The Mode of study is not valid for this Special fee indicator');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'MSTUFEE_01', N'E', N'The Major source of tuition fees is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'MSTUFEE_02', N'E', N'The Major source of tuition fees is not valid for this Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'MSTUFEE_03', N'W', N'The Major source of tuition fees is not consistent with the Domicile');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'MSTUFEE_04', N'W', N'The Major source of tuition fees is not consistent with the Domicile');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'MSTUFEE_05', N'W', N'The Major source of tuition fees is not consistent with the Domicile');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Namespace Mismatch', N'E', N'You''re unable to submit this file because the namespace doesn''t match the namespace outlined in the schema. You can download the ILR schema definitions from the ILR data guidance page [link to https://www.gov.uk/government/collections/individualised-learner-record-ilr] for the relevant academic year. Please upload an updated file.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'NETFEE_01', N'E', N'The Net tuition fee must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'NETFEE_02', N'W', N'''Net tuition fee'' may be wrong. The net fee is the fee after any financial support such as waivers or bursaries are taken into account. It should not be more than £9,000. Check the ''net tuition fee'' is correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'NINumber_01', N'E', N'The National Insurance number is not in the correct format');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'NINumber_02', N'E', N'The National Insurance number must be returned for this learner');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'NUMHUS_01', N'E', N'The Student instance identifier must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OrigLearnStartDate_01', N'E', N'The Original learning start date must not be more than 10 years before the Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OrigLearnStartDate_02', N'E', N'The Original learning start date must not be on or after the Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OrigLearnStartDate_03', N'E', N'The Original learning start date must not be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OrigLearnStartDate_04', N'E', N'The Restart indicator has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OrigLearnStartDate_05', N'E', N'The Learning aim reference is not valid in the LARS database for this Funding model on this Original learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OrigLearnStartDate_06', N'E', N'The Learning aim reference is Adult skills funded and is not valid in the LARS database on this Original learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OrigLearnStartDate_07', N'E', N'The Learning aim reference is not valid in the LARS database for this Funding model on this Original learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OrigLearnStartDate_08', N'E', N'The Learning aim reference is financed by an Advanced Learner Loan and is not valid in the LARS database on this Original learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OrigLearnStartDate_09', N'E', N'The Original learning start date for this funding model must not be before 1 May 2017');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OtherFundAdj_01', N'E', N'The Other funding adjustment must not be returned for this Funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OutCollDate_01', N'E', N'The Outcome collection date must not be after the file preparation date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OutCollDate_02', N'E', N'The Outcome collection date must not be more than 10 years before the start of the current teaching year');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Outcome_01', N'E', N'The Outcome is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Outcome_04', N'E', N'The Achievement date must not be returned for this Outcome');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Outcome_05', N'E', N'The Learning actual end date must be returned for this Outcome');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Outcome_07', N'E', N'There must be a Destination and progression record returned for this Outcome');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Outcome_08', N'E', N'There must be a Destination and progression record returned for this Outcome');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Outcome_09', N'E', N'The Completion status is not valid for this Outcome');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OutEndDate_01', N'E', N'The Outcome end date must not be before the Outcome start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OutGrade_01', N'E', N'The Outcome grade is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OutGrade_03', N'W', N'The outcome grade is not valid for this level of qualification. For an entry level course that counts towards skills for life the achievement should be recorded as either EL1, EL2 or EL3, depending on the level. Check the learning aim and learning delivery outcome are correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OutGrade_04', N'E', N'The Outcome grade must not be returned for this Outcome');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OutGrade_05', N'E', N'The Outcome grade is not valid for this Outcome');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OutGrade_06', N'E', N'The Outcome grade is not valid for this Outcome');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OutGrade_07', N'E', N'The Outcome grade is not valid for this type of qualification');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OutStartDate_01', N'E', N'The Outcome start date must not be more than 10 years before the start of the current teaching year');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OutStartDate_02', N'E', N'The Outcome start date must not be more than 1 year after the end of the current teaching year');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OutType_01', N'E', N'The Outcome type and code is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OutType_02', N'E', N'There must not be more than one record with the same start date and an Outcome type of EMP, NPE, VOL, GAP, SDE, or OTH');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OutType_03', N'E', N'There must not be more than two records with the same start date and an Outcome type of EDU');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OutType_04', N'E', N'There must not be DPOutcome records of both In Paid Employment and Not in paid Employment with the same Outcome start date');
GO
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OutType_05', N'E', N'The Outcome start date must not be after the ''valid to'' date in the OutType Code table');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OutULN_01', N'E', N'The Unique learner number does not pass the checksum calculation');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'OutULN_02', N'E', N'The Unique learner number is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PartnerUKPRN_01', N'E', N'The Subcontracted or partnership UKPRN is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PartnerUKPRN_02', N'E', N'The Subcontracted or partnership UKPRN must not be on programme aims');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PartnerUKPRN_03', N'E', N'The Subcontracted or partnership UKPRN must not be the same as the Learning provider UKPRN');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PCFLDCS_01', N'E', N'The total of the three Percentage taught in LDCS subject fields must equal 100%');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PCFLDCS_02', N'E', N'The Percentage taught in first LDCS subject must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PCFLDCS_03', N'E', N'The Percentage taught in first LDCS subject must not be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PCOLAB_01', N'E', N'The Percentage not taught by this institution must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PCSLDCS_01', N'E', N'The Percentage taught in second LDCS subject must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PCSLDCS_02', N'E', N'The Percentage taught in second LDCS subject must not be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PCTLDCS_01', N'E', N'The Percentage taught in third LDCS subject must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PCTLDCS_02', N'E', N'The Percentage taught in third LDCS subject must not be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PHours_01', N'E', N'Planned hours must be returned for this funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PHours_02', N'E', N'Planned hours must not be less than 278 hours');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PlanEEPHours_01', N'E', N'The Planned employability, enrichment and pastoral hours have not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PlanLearnHours_01', N'E', N'The Planned learning hours have not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PlanLearnHours_02', N'W', N' ''Planned learning hours'' may be wrong. In most cases, we''d expect a figure in ''planned learning hours''. Please check that zero for ''planned learning hours'' is correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PlanLearnHours_03', N'E', N'The sum of the Planned learning hours and the Planned employability, enrichment and pastoral hours must not be zero');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PlanLearnHours_04', N'W', N' ''Planned learning hours'' or ''Planned employability, enrichment and pastoral hours'' may be wrong. The total of ''planned learning hours'' and ''planned employability, enrichment and pastoral hours'' shouldn''t be more than 1,000. Check ''planned learning hours'' and ''planned employability, enrichment and pastoral hours'' are correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PlanLearnHours_05', N'E', N'The sum of the Planned learning hours and the Planned employability, enrichment and pastoral hours must not be greater than 4,000 hours');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PMUKPRN_01', N'E', N'The UKPRN is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Postcode_14', N'W', N' ''Postcode prior to enrolment'' is wrong. You must enter a valid UK postcode. If the system isn''t recognising your postcode because it''s a new build, you can temporarily use ZZ99 9ZZ. Check ''postcode prior to enrolment'' is complete and correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Postcode_15', N'E', N'The Postcode is not in the correct format');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PostcodePrior_01', N'W', N' ''Postcode prior to enrolment'' is wrong. You must enter a valid UK postcode. If the system isn''t recognising your postcode because it''s a new build, you can temporarily use ZZ99 9ZZ. Check ''postcode prior to enrolment'' is correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PostcodePrior_02', N'E', N'The Postcode prior to enrolment is not in the correct format');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PrevUKPRN_01', N'E', N'The UKPRN is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PrimaryLLDD_01', N'E', N'The Primary LLDD and health problem is not recorded on one of the LLDD and health problem records');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PrimaryLLDD_02', N'E', N'The Primary LLDD and health problem is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PrimaryLLDD_03', N'E', N'There must only be one Primary LLDD and Health problem record');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PrimaryLLDD_04', N'E', N'The Primary LLDD and health problem is not recorded');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PriorAttain_01', N'E', N'The Prior attainment has not been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PriorAttain_03', N'E', N'The Prior attainment is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PriorAttain_04', N'E', N'The Prior attainment is not valid if the programme is a funded intermediate or advanced level apprenticeship');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PriorAttain_06', N'E', N'''Prior attainment'' is wrong. It''s either above or below the attainment needed. Please check your ESF Tender Specification for the attainment limits and check ''prior attainment'' is correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PriorAttain_07', N'E', N'The Prior attainment is not valid for a traineeship programme');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PriorLearnFundAdj_01', N'E', N'The Funding adjustment for prior learning must not be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ProgType_01', N'E', N'The Programme type must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ProgType_02', N'E', N'The Programme type must not be returned for this Aim type');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ProgType_03', N'E', N'The Programme type is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ProgType_06', N'E', N'The Programme type is not a valid code for this Funding model');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ProgType_07', N'E', N'The planned duration for the programme exceeds the maximum allowed duration for a traineeship');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ProgType_08', N'E', N'The actual duration of the programme exceeds the maximum allowed duration for a traineeship');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ProgType_12', N'E', N'The learning aim is not valid for this framework');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ProgType_13', N'E', N'The current duration of the programme exceeds the maximum allowed duration for a traineeship');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ProgType_14', N'E', N'This programme type cannot be used for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ProvSpecDelMonOccur_01', N'E', N'The Provider specified delivery monitoring occurrence is not ''A'' , ''B'', ''C'' or ''D''');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ProvSpecLearnMonOccur_01', N'E', N'The Provider specified learner monitoring occurrence is not ''A'' or ''B''');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PwayCode_02', N'E', N'The Apprenticeship pathway must not be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'PwayCode_03', N'E', N'The Apprenticeship pathway must be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'QUALENT3_01', N'E', N'The Qualification on entry must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'QUALENT3_02', N'E', N'The Qualification on entry is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'QUALENT3_03', N'E', N'The Qualification on entry is not a valid code for this Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R06', N'E', N'There must not be more than one Learner record with the same Learner reference number');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R07', N'E', N'There must not be more than one Learning Delivery record with the same Learner reference number and Aim sequence number combination');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R100', N'E', N'There must be an assessment price recorded');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R101', N'E', N'The learning aim must not have more than one Apprenticeship contract type record at the same time');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R102', N'E', N'The aim must have an Apprenticeship contract type in the first day');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R104', N'E', N'There must be an apprenticeship contract type for the full duration of the aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R105', N'E', N'The learner must not have different Apprenticeship contract types recorded at the same time');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R106', N'E', N'There must not be more than one LSF record at the same time');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R107', N'W', N' ''Learner destination and progression record'' is missing. You must return the ''learner destination and progression'' record within two months of the ''learning actual end date''. Make sure the ''learner destination and progression'' information is included within two months of the ''learning actual end date''.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R108', N'E', N' ''Learner destination and progression'' record is missing. As the funding learning aims are now complete, ''learner detsination and progression'' must be entered within 2 months of the actual end date of the last aim to be completed. Check ''learner destination and progression'' is recorded.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R110', N'E', N'The employment status is not valid for this apprenticeship contract type');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R112', N'E', N'There must be a Date applies to that matches the Learning actual end date for this aim.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R113', N'E', N'The Learning actual end date is not known, therefore the Date applies to record must not be returned.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R114', N'E', N'The learner is currently apprenticeship funded and cannot start an Adult skills funded English/maths aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R117', N'E', N'All component aims with LDM357 must have a corresponding programme aim with the same learning delivery monitoring type and code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R118', N'E', N'All programme aims with LDM357 must have a corresponding component aim with the same learning delivery monitoring type and code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R119', N'E', N'The apprenticeship financial record cannot be before the Learning start date when the total negotiated price has been returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R121', N'E', N'There must be a Date applies to that matches the achievement date for this aim.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R122', N'W', N'The achievement date is not known, therefore the Date applies to record must not be returned.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R20', N'E', N'The learner must not have more than one competency aim at the same time');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R30', N'E', N'All aims that are part of a programme must have a corresponding programme aim with the same Programme type, Framework code and Apprenticeship pathway (if returned)');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R31', N'E', N'A learning aim is missing. You''ve got an open programme aim in your file, but there''s no learning aim with the same ''programme type'', ''framework code'', ''apprenticeship pathway'' and ''standard code''. Make sure all programme aims have at least one associated learning aim with the same ''programme type'', ''framework code'', ''apprenticeship pathway'' and ''standard code''.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R43', N'E', N'The learner must not have more than one Learner Employment status record with the same Date employment status applies');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R45', N'E', N'The learner must not have more than one LLDD and Health Problem record with the same LLDD and health problem category');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R47', N'E', N'The learner must not have more than one Learner Contact Preference record with the same Contact preference type and Contact preference code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R49', N'E', N'The learner must not have more than one Learner Provider Specified Monitoring record with the same occurrence code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R50', N'E', N'The learning aim must not have more than one Learning Delivery Provider Specified Monitoring record with the same occurrence code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R51', N'E', N'The learner must not have more than one Learner Funding and Monitoring record with the same FAM type and code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R52', N'E', N'The learning aim must not have more than one Learning Delivery FAM record with the same FAM type and code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R56', N'E', N'An SFA or EFA funded component aim must not have a non-funded programme aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R58', N'E', N'The learner must not have more than one core aim at the same time');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R59', N'W', N'There is more than one record for this ULN. There mustn''t be multiple records with the same ULN. Check each record has a different ULN.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R61', N'E', N'The learning aim must not have more than one Learning Support Funding record at the same time');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R63', N'E', N'A core aim is missing. It''s required for this type of funding model. Check you''ve recorded the learner''s core aim by recording code ''5'' in the AimType field against one of the learning aims.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R64', N'E', N'The learner must not have more than one competency aim with the same Programme type, Framework code and Apprenticeship pathway where an earlier competency aim has been achieved');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R66', N'E', N'All funded component aims must have a corresponding programme aim with the same Programme type, Framework code, Pathway code or Standard code (where applicable)');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R67', N'E', N'The learner must not have more than one Learning Delivery Work Placement record with the same Work placement start date and Work placement employer identifier');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R68', N'E', N'The learner must not have more than one Apprenticeship Financial Record with the same Apprenticeship financial type, code and date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R69', N'E', N'The learner must not have more than one DPOutcome record with the same Outcome type, code and start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R71', N'E', N'There must not be more than one Learner Destination and Progression record with the same Learner reference number');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R72', N'E', N'The payment amounts must not exceed the employer contribution proportion of the total negotiated price');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R73', N'E', N'The payment amounts must not exceed the employer contribution proportion of the total negotiated price');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R85', N'E', N'The ULN for this learner must match between the Learner entity and the Learner Destination and Progression entity');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R89', N'E', N'The Learning actual end date for the latest programme aim must not be before the Learning actual end date of the latest component aim (not including English and maths)');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R90', N'E', N'The Learning actual end date of the component aims must be returned for this programme');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R91', N'E', N'There must be a completed ZESF0001 learning aim for this learner');
GO
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R92', N'E', N'There must not be more than one ZESF0001 learning aim for this contract');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R96', N'E', N'The learner must not have more than one Learning Delivery Work Placement record with the same Work placement start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R97', N'E', N' ''Learner employment status'' records are wrong. The learner can''t have a record that features the same ''employment status'', ''employer identifier'', ''agreement identifier'' and ''employment status monitoring'' as the record before it. Check ''learner employment status'' records are correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'R99', N'E', N'The learner must not have more than one programme aim at the same time');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Schema', N'F', N'The XML is not well formed');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'SEC_01', N'E', N'The Socio-economic indicator is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'SEC_02', N'E', N'The Socio-economic indicator must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'Sex_01', N'E', N'The Sex is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'SOC2000_02', N'E', N'The Occupation code must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'SOC2000_03', N'E', N'The Occupation code is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'SOC2000_04', N'E', N'The Occupation code is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'SPECFEE_01', N'E', N'This Special fee indicator is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'SPECFEE_02', N'E', N'The Special fee indicator is not valid for this Mode of study');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'SSN_02', N'E', N'The Student support number does not pass the checksum calculation');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'StdCode_01', N'E', N'The Apprenticeship standard code must be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'StdCode_02', N'E', N'The Apprenticeship standard code is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'StdCode_03', N'E', N'The Apprenticeship standard code must not be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'STULOAD_04', N'E', N'The Student instance FTE must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'SWSupAimId_01', N'W', N'''Software supplier aim identifier'' is wrong. It needs to be in the UUID standard format. As this is a software problem, please speak to your software vendors to resolve the problem with the identifier.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'TTACCOM_01', N'E', N'The Term time accommodation is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'TTACCOM_02', N'E', N'The Term time accommodation is not a valid code for this Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'TTACCOM_04', N'E', N'The Term time accommodation must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'TYPEYR_01', N'E', N'The Type of instance year is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'TYPEYR_02', N'E', N'The Type of instance year is not valid for this Completion of year of instance');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'UCASAPPID_01', N'E', N'The UCAS application code must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'UKPRN_03', N'F', N'The UKPRN is not the same as recorded in the Header');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'UKPRN_05', N'E', N'There is no ESF funding relationship for this UKPRN');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'UKPRN_06', N'W', N'There is no Adult skills funding relationship for this UKPRN');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'UKPRN_08', N'E', N'There is no Advanced Learner Loans Bursary funding relationship for this UKPRN');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'UKPRN_09', N'E', N'You do not have a contract for apprenticeships.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'UKPRN_10', N'E', N'There is no apprenticeship funding relationship for this UKPRN');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'UKPRN_11', N'E', N'There is no apprenticeship funding relationship for this UKPRN');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'UKPRN_12', N'E', N'There is no Adult skills funding relationship for this UKPRN');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'UKPRN_13', N'E', N'There is no apprenticeship funding relationship for this UKPRN');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'UKPRN_14', N'E', N'There is no apprenticeship funding relationship for this UKPRN');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'UKPRN_15', N'E', N'There is no apprenticeship funding relationship for this UKPRN');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'UKPRN_16', N'E', N'This learning aim cannot be started after the Stop New Starts date for this funding relationship');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'UKPRN_17', N'E', N'This learning aim cannot be started after the Stop New Starts date for this funding relationship');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'UKPRN_18', N'E', N'This learning aim cannot be started after the Stop New Starts date for this funding relationship');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'UKPRN_19', N'E', N'This learning aim cannot be started after the Stop New Starts date for this funding relationship');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'UKPRN_20', N'E', N'This learning aim cannot be started after the Stop New Starts date for this funding relationship');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ULN_02', N'W', N' ''Unique learner number'' (ULN) may be wrong. The unique learner number shouldn''t be 9999999999 for learners who are non-funded or community learning funded. Check ''unique learner number'' and ''funding type'' are correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ULN_03', N'W', N'This Unique learner number should not be used');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ULN_04', N'E', N'The Unique learner number does not pass the checksum calculation');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ULN_05', N'E', N' ''Unique learner number'' (ULN) is wrong. The ''unique learner number'' (ULN) must exist in the Learner Register or be the default learner number (9999999999) if you''re waiting for the learner''s ULN registration. Check ''unique learner number'' is correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ULN_06', N'W', N' ''Unique learner number'' (ULN) may be wrong. In most cases the default learner number (9999999999) should not be used after 1 January, as the learner''s actual ULN should be registered by now. Please return their real ULN. Check ''unique learner number'', ''learning aim duration (planned or actual)'' and ''learning start date'' are correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ULN_07', N'E', N'This Unique learner number must not be used');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ULN_09', N'W', N'''Unique learner number'' (ULN) may be wrong. In most cases the default learner number (9999999999) should only be used until 1 January, while you wait for the learner''s ULN registration to happen. If there are exceptional circumstances why one can''t be issued, you should contact the Learning Records Service (LRS) on LRS.Support@education.gov.uk. Check ''unique learner number'', learning aim duration (planned or actual) and ''learning start date'' are correct.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ULN_10', N'W', N'This Unique learner number should not be used');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ULN_11', N'E', N'This Unique learner number must not be used');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ULN_12', N'E', N'This Unique learner number must not be used');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'WithdrawReason_02', N'E', N'The Withdrawal reason is not a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'WithdrawReason_03', N'E', N'The Withdrawal reason must be returned for this Completion status');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'WithdrawReason_04', N'E', N'The Withdrawal reason must not be returned for this Completion status');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'WithdrawReason_05', N'E', N'The Withdrawal reason is only valid for OLASS - Offenders in custody');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'WithdrawReason_06', N'E', N'The Withdrawal reason is only valid for Industrial placements');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'WorkPlaceEmpId_01', N'E', N'The Work placement employer identifier is not valid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'WorkPlaceEmpId_02', N'E', N'The Work placement employer identifier does not pass the checksum calculation');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'WorkPlaceEmpId_03', N'W', N'Work placement employer identifier'' needs updating soon. You can use 999999999 for 60 days from the start of the work placement, at which point you must provide the real identifier for this learner to be funded. Update the ''work placement employer identifier'' as soon as you can.');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'WorkPlaceEmpId_04', N'E', N'The Work placement employer identifier must not be used');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'WorkPlaceEmpId_05', N'E', N'The Work placement employer identifier must be returned');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'WorkPlaceEndDate_01', N'E', N'The Work placement end date must be after the Work placement start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'WorkPlaceEndDate_02', N'E', N'The Work placement end date must be before the Learning actual end date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'WorkPlaceMode_01', N'E', N'The Work placement mode must be a valid code');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'WorkPlaceStartDate_01', N'E', N'The Work Placement record must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'WorkPlaceStartDate_02', N'E', N'The Work placement start date must be after the Learning start date');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'WorkPlaceStartDate_03', N'E', N'A Work placement record must not be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'WorkPlaceStartDate_04', N'E', N'16-19 Funding model must be returned for this learning aim');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ZIP_CORRUPT', N'F', N'Zip folder is corrupt or invalid');
INSERT [dbo].[Rules] ([Rulename], [Severity], [Message]) VALUES (N'ZIP_EMPTY', N'F', N'Zip folder must contain only one XML file');
GO

RAISERROR('Rules Completed',10,1) WITH NOWAIT;
GO

UPDATE [dbo].[Rules]
SET [Desktop] = 0
WHERE [Rulename] IN
(
	'Filename_2'
	,'Filename_9'
	,'UKPRN_05'
	,'UKPRN_06'
	,'UKPRN_08'
	,'UKPRN_09'
	,'UKPRN_10'
	,'UKPRN_11'
	,'UKPRN_12'
	,'UKPRN_13'
	,'UKPRN_14'
	,'UKPRN_15'
	,'UKPRN_16'
	,'UKPRN_17'
	,'UKPRN_18'
	,'UKPRN_19'
	,'UKPRN_20'
	,'CampId_01'
	,'CampId_02'
	,'ULN_05'
	,'DateOfBirth_44'
	,'PriorAttain_06'
	,'PostcodePrior_01'
	,'Postcode_14'
	,'EmpStat_14'
	,'EmpId_01'
	,'ESMType_13'
	,'ESMType_14'
	,'LearnAimRef_71'
	,'LearnAimRef_72'
	,'LearnAimRef_73'
	,'LearnStartDate_15'
	,'LearnStartDate_16'
	,'DelLocPostCode_03'
	,'DelLocPostCode_14'
	,'DelLocPostCode_15'
	,'DelLocPostCode_16'
	,'DelLocPostCode_17'
	,'DelLocPostCode_18'
	,'LSDPostcode_01'
	,'LSDPostcode_02'
	,'ConRefNumber_01'
	,'EPAOrgID_01'
	,'LearnDelFAMType_53'
	,'LearnDelFamType_70'
	,'LearnDelFamType_71'
	,'WorkPlaceEmpId_01'
	,'HEPostCode_01'
	,'OutULN_02'
)


RAISERROR('Rules Desktop Flags Completed',10,1)
GO

--These rules off for R01 launch
UPDATE [dbo].[Rules]
SET [Online] = 0, [Desktop] = 0
WHERE [Rulename] IN
(
	 'R112'
	,'R113'
	,'R121'
	,'R122'
	,'PHours_01'
)

RAISERROR('Rules Online/Desktop Flags For R01 Completed',10,1)
GO