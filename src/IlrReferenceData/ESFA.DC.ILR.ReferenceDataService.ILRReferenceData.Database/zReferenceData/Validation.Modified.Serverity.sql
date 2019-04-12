
/*
		SELECT TOP 100 PERCENT 
			'UNION SELECT ''' + MR.[Rulename] + ''' as [Rulename],''' + R.[Severity] + ''' as [Severity]' as [Value], MR.[Rulename], R.[Severity]
		FROM [Staging].[Rules] MR
		INNER JOIN [dbo].[Rules] R
		ON R.[Rulename] = MR.[Rulename]
		WHERE MR.[Severity] <> R.[Severity]
		ORDER BY  MR.[Rulename]
*/

SET NOCOUNT ON;
BEGIN
	
	-- PK on this table should stop Duplicate Rules with Diffrent Serverity Getting into table.
	DECLARE @ModifiedServerity TABLE ([Rulename] NVARCHAR(50) NOT NULL PRIMARY KEY, [Severity] NCHAR(1));

	-------------- EXAMPLE of Values to add new Serverity Amendment----
	--INSERT INTO @ModifiedServerity( [Rulename],[Severity])
	--SELECT [Rulename],[Severity]
	--FROM
	--(		  SELECT 'RULENAME 1 HERE' as [Rulename],'X' as [Severity]
	--	UNION SELECT 'RULENAME 2 HERE' as [Rulename],'A' as [Severity]
	--	UNION SELECT 'EmpStat_14' as [Rulename],'W' as [Severity]
	--	UNION SELECT 'LearnDelFAMType_01' as [Rulename],'W' as [Severity]
	--	UNION SELECT 'LLDDHealthProb_06' as [Rulename],'W' as [Severity]
	--	UNION SELECT 'PlanLearnHours_01' as [Rulename],'W' as [Severity]
	--	UNION SELECT 'R106' as [Rulename],'W' as [Severity]
	--	UNION SELECT 'R108' as [Rulename],'W' as [Severity]
	--	UNION SELECT 'R112' as [Rulename],'W' as [Severity]
	--	UNION SELECT 'R29' as [Rulename],'W' as [Severity]
	--	UNION SELECT 'R31' as [Rulename],'W' as [Severity]
	--	UNION SELECT 'R63' as [Rulename],'W' as [Severity]
	--	UNION SELECT 'R97' as [Rulename],'W' as [Severity]
	--	UNION SELECT 'StdCode_03' as [Rulename],'W' as [Severity]
	--	UNION SELECT 'WorkPlaceStartDate_01' as [Rulename],'W' as [Severity]
	--) as Amendments

	DECLARE @SummaryOfChanges_ModifiedServerity TABLE ([Rulename]  NVARCHAR(50) NOT NULL, [Action] VARCHAR(100) NOT NULL);

	MERGE INTO [Staging].[ModifiedServerity] AS Target
		USING (
				   SELECT [Rulename]
					 	 ,[Severity]
					FROM @ModifiedServerity
			  )
			  AS Source 
		    ON Target.[Rulename] = Source.[Rulename]
		--WHEN MATCHED 
			--	AND EXISTS 
			--		(	  SELECT Target.[Severity]
			--		   EXCEPT 
			--			  SELECT Source.[Severity]
			--		)
		 -- THEN UPDATE SET   [Severity] = Source.[Severity]
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (     [Rulename]
					,[Severity]
					)
			VALUES ( Source.[Rulename]
					,Source.[Severity]
				  )
		--WHEN NOT MATCHED BY SOURCE THEN DELETE
		OUTPUT ISNULL(Deleted.[Rulename],Inserted.[Rulename]),$action INTO @SummaryOfChanges_ModifiedServerity([Rulename],[Action])
	    ;

		DECLARE @AddCount_Serv INT, @UpdateCount_Serv INT, @DeleteCount_Serv INT;
		SET @AddCount_Serv  = ISNULL((SELECT Count(*) FROM @SummaryOfChanges_ModifiedServerity WHERE [Action] = 'Insert' GROUP BY Action),0);
		SET @UpdateCount_Serv = ISNULL((SELECT Count(*) FROM @SummaryOfChanges_ModifiedServerity WHERE [Action] = 'Update' GROUP BY Action),0);
		SET @DeleteCount_Serv = ISNULL((SELECT Count(*) FROM @SummaryOfChanges_ModifiedServerity WHERE [Action] = 'Delete' GROUP BY Action),0);

		RAISERROR('		      %s - Added %i - Update %i - Delete %i',10,1,'    Modified Serverity', @AddCount_Serv, @UpdateCount_Serv, @DeleteCount_Serv) WITH NOWAIT;
END		
