CREATE PROCEDURE [Staging].[usp_Process_Rules]
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		
		MERGE INTO [dbo].[Rules] AS Target
		USING (
				SELECT [Rulename]
					  ,[Severity]
					  ,[Message]
				FROM 
				(
				  -- New File Rules
				  SELECT TOP (1000) [Rulename]
					  ,[Severity]
					  ,[Message]
				  FROM [Staging].[FileRules]
				UNION 
				 -- Rules only in ModifiedRules
				  SELECT MR.[Rulename]
					  ,MR.[Severity] as [Severity]
					  ,MR.[Message] as [Message]	  
				  FROM [Staging].[ModifiedRules] MR 
				  LEFT JOIN [Staging].[Rules] R
					  ON MR.[Rulename] = R.[Rulename]
				  WHERE R.[Rulename] IS NULL
				-- Modified Rules
				UNION 
				  SELECT TOP (1000) R.[Rulename]
					  ,ISNULL(MR.[Severity],R.[Severity]) as [Severity]
					  ,ISNULL(MR.[Message],R.[Message]) as [Message]	  
				  FROM [Staging].[Rules] R
				  LEFT JOIN [Staging].[ModifiedRules] MR
				  ON MR.[Rulename] = R.[Rulename]
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
		;

		
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