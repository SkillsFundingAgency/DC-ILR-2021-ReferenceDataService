CREATE PROCEDURE [Staging].[usp_Process_Rules]
(	
	@Debug INT = 0
)
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @SummaryOfChanges TABLE ([Rulename]  NVARCHAR(50) NOT NULL, [Action] VARCHAR(100) NOT NULL);

	BEGIN TRY
		
		MERGE INTO [dbo].[Rules] AS Target
		USING (
				SELECT [Rulename]
					  ,[Severity]
					  ,[Message]
				FROM 
				(
				--	     -- New File Rules
					SELECT [Rulename]
						  ,[Severity]
						  ,[Message]
					FROM [Staging].[FileRules]
				 UNION 
					SELECT R.[Rulename]
						  ,ISNULL(S.[Severity],R.[Severity]) as [Severity]
						  ,ISNULL(M.[Message],R.[Message]) as [Message]	  
					FROM [Staging].[Rules] R
					LEFT JOIN [Staging].[ModifiedMessages] M 
						ON M.[Rulename] = R.[Rulename]
					LEFT JOIN [Staging].[ModifiedServerity] S
						ON S.[Rulename] = R.[Rulename]
				  ) as RecordSetToProcess
			)
			  AS Source ([Rulename],[Severity],[Message])
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
		OUTPUT ISNULL(Deleted.[Rulename],Inserted.[Rulename]),$action INTO @SummaryOfChanges([Rulename],[Action])
		;

		IF (@Debug>0)
		BEGIN			
			DECLARE @AddCount INT, @UpdateCount INT, @DeleteCount INT;
			SET @AddCount = ISNULL((SELECT Count(*) FROM @SummaryOfChanges WHERE [Action] = 'Insert' GROUP BY Action),0);
			SET @UpdateCount = ISNULL((SELECT Count(*) FROM @SummaryOfChanges WHERE [Action] = 'Update' GROUP BY Action),0);
			SET @DeleteCount = ISNULL((SELECT Count(*) FROM @SummaryOfChanges WHERE [Action] = 'Delete' GROUP BY Action),0);

			RAISERROR('	%s - Added %i - Update %i - Delete %i',10,1,'    Rules Processed', @AddCount, @UpdateCount, @DeleteCount) WITH NOWAIT;
		END
		
		RETURN 0;

	END TRY
-- 
-------------------------------------------------------------------------------------- 
-- Handle any problems
-------------------------------------------------------------------------------------- 
-- 
	BEGIN CATCH

		DECLARE   @ErrorMessage		NVARCHAR(4000)
				, @ErrorSeverity	INT 
				, @ErrorState		INT
				, @ErrorNumber		INT
						
		SELECT	  @ErrorNumber		= ERROR_NUMBER()
				, @ErrorMessage		= 'Error in :' + ISNULL(OBJECT_NAME(@@PROCID),'') + ' - Error was :' + ERROR_MESSAGE()
				, @ErrorSeverity	= ERROR_SEVERITY()
				, @ErrorState		= ERROR_STATE();
	
		RAISERROR (
					  @ErrorMessage		-- Message text.
					, @ErrorSeverity	-- Severity.
					, @ErrorState		-- State.
				  );
			  
		RETURN @ErrorNumber;

	END CATCH
-- 
-------------------------------------------------------------------------------------- 
-- All done
-------------------------------------------------------------------------------------- 
-- 
END
GO
GRANT EXECUTE ON [Staging].[usp_Process_Rules] TO [ILR1819ReferenceData_RW_User]
GO
