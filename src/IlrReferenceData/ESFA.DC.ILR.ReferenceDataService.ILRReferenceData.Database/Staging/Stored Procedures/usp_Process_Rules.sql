CREATE PROCEDURE [Staging].[usp_Process_Rules]
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		
		--MERGE INTO [dbo].[Rules] AS Target
		--USING (

----;WITH ModList
----AS
----  (
----	  SELECT DISTINCT [Rulename]
----	  FROM 
----	  (      SELECT [Rulename] FROM  [Staging].[FileRules] 
----	   UNION SELECT [Rulename] FROM  [Staging].[ModifiedServerity] 
----	   UNION SELECT [Rulename] FROM  [Staging].[ModifiedMessages] 
----	  ) ModList
----) 

--SELECT [Rulename]
--	  ,[Severity]
--	  ,[Message]
--FROM 
--(
----	     -- New File Rules
--	SELECT [Rulename]
--		  ,[Severity]
--		  ,[Message]
--	FROM [Staging].[FileRules]
-- UNION 
--	SELECT R.[Rulename]
--		  ,ISNULL(S.[Severity],R.[Severity]) as [Severity]
--		  ,ISNULL(M.[Message],R.[Message]) as [Message]	  
--	FROM [Staging].[Rules] R
--	LEFT JOIN [Staging].[ModifiedMessages] M 
--		ON M.[Rulename] = R.[Rulename]
--	LEFT JOIN [Staging].[ModifiedServerity] S
--		ON M.[Rulename] = R.[Rulename]
--  ) as RecordSetToProcess
----WHERE [Rulename] IN (SELECT [Rulename] FROM ModList)


		--	)
		--	  AS Source ([Rulename],[Severity],[Message])
		--    ON Target.[Rulename] = Source.[Rulename]
		--	WHEN MATCHED 
		--		AND EXISTS 
		--			(	SELECT 
		--					 Target.[Severity]
		--					,Target.[Message]
		--			EXCEPT 
		--				SELECT 
		--					 Source.[Severity]
		--					,Source.[Message]
		--			)
		--  THEN
		--	UPDATE SET   [Severity] = Source.[Severity]
		--				,[Message] = Source.[Message]
		--WHEN NOT MATCHED BY TARGET THEN
		--INSERT (     [Rulename]
		--			,[Severity]
		--			,[Message]
		--			)
		--	VALUES ( Source.[Rulename]
		--			,Source.[Severity]
		--			,Source.[Message]
		--		  )
		--WHEN NOT MATCHED BY SOURCE THEN DELETE
		--;

		
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