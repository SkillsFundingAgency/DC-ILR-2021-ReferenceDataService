
SET NOCOUNT ON;

BEGIN

	DECLARE @FileRule TABLE ([Rulename] NVARCHAR(50) NOT NULL PRIMARY KEY, [Severity] NVARCHAR(1),[Message] NVARCHAR(2000) NULL);

	INSERT INTO @FileRule( [Rulename],[Severity] ,[Message])
	VALUES ('ZIP_EMPTY','F','Zip folder must contain only one XML file')
		  ,('ZIP_CORRUPT','F','Zip folder is corrupt or invalid')
		  ,('Schema','F','The XML is not well formed')

	DECLARE @SummaryOfChanges_FileRules TABLE ([Rulename]  NVARCHAR(50) NOT NULL, [Action] VARCHAR(100) NOT NULL);

	MERGE INTO [Staging].[FileRules] AS Target
		USING (
				   SELECT [Rulename]
						,[Severity]
						,[Message]
					FROM @FileRule
			  )
			  AS Source 
		    ON Target.[Rulename] = Source.[Rulename]
			WHEN MATCHED 
				AND EXISTS 
					(	SELECT 
							 Target.[Severity]
							,Target.[Message]
					EXCEPT 
						SELECT 
							 Source.[Severity]
							,Source.[Message]
					)
		  THEN
			UPDATE SET   [Severity] = Source.[Severity]
						,[Message] = Source.[Message]
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (     [Rulename]
					,[Severity]
					,[Message]
					)
			VALUES ( Source.[Rulename]
					,Source.[Severity]
					,Source.[Message]
				  )
		WHEN NOT MATCHED BY SOURCE THEN DELETE
		OUTPUT Inserted.[Rulename],$action INTO @SummaryOfChanges_FileRules([Rulename],[Action])
	    ;

		DECLARE @AddCount_FR INT, @UpdateCount_FR INT, @DeleteCount_FR INT;
		SET @AddCount_FR  = ISNULL((SELECT Count(*) FROM @SummaryOfChanges_FileRules WHERE [Action] = 'Insert' GROUP BY Action),0);
		SET @UpdateCount_FR = ISNULL((SELECT Count(*) FROM @SummaryOfChanges_FileRules WHERE [Action] = 'Update' GROUP BY Action),0);
		SET @DeleteCount_FR = ISNULL((SELECT Count(*) FROM @SummaryOfChanges_FileRules WHERE [Action] = 'Delete' GROUP BY Action),0);

		RAISERROR('		      %s     - Added %i - Update %i - Delete %i',10,1,'        File Rules', @AddCount_FR, @UpdateCount_FR, @DeleteCount_FR) WITH NOWAIT;
		
END
