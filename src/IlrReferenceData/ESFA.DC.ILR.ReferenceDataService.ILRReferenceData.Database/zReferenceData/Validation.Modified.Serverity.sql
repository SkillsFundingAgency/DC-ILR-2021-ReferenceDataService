
SET NOCOUNT ON;
BEGIN

	DECLARE @ModifiedServerity TABLE ([Rulename] NVARCHAR(50) NOT NULL PRIMARY KEY, [Severity] NCHAR(1));

	INSERT INTO @ModifiedServerity( [Rulename],[Severity])
	VALUES 
	 ('DelLocPostCode_18','E')
	,('DelLocPostCode_15','E')
	,('LearnDelFAMType_04','E')
	,('R91','E')
	,('UKPRN_12','E')
	,('LearnDelFAMType_06','E')
	,('R112','E')
	,('PlanLearnHours_03','E')
	,('R20','E')
	,('Sex_01','E')
	,('EmpStat_08','E')
	,('StdCode_03','E')
	--,('UKPRN_13','E','You do not have a contract for apprenticeship funding. You must have a contract for non-levy apprenticeship to be able to claim this type of funding. Check ''UK provider reference number'' is correct, a contract is in place and the contract has been signed.')
	--,('R106','E','There are too many ''learning delivery FAM types'' with a code of LSF. The learner can only have one LSF (learning support) record. Check only one record with a ''learning delivery FAM type'' of LSF exists for this learner.')
	--,('LearnStartDate_07','E','''Learning start date'' is wrong. It''s currently outside of the ''Pathway effective to'' date for this component on the LARS framework table. "Check ''learning start date'' is correct.')
	--,('DelLocPostCode_11','E','''Delivery location postcode'' is wrong. It''s not a valid UK postcode. Check ''delivery location postcode'' is in the correct format. If it''s unknown, use ZZ99 9ZZ.')
	--,('DelLocPostCode_16','E','''Delivery location postcode'' is wrong. It''s not a valid UK postcode. Check ''delivery location postcode'' is correct. If it''s unknown, use ZZ99 9ZZ.')
	--,('WorkPlaceStartDate_01','E','''Work placement'' record is missing. As this is a work experience aim, you need to return this information. Check the ''work placement'' record is filled in.')
	--,('LearnAimRef_88','E','''Learning start date'' or ''funding model'' may be wrong. The start date you''ve entered is outside the dates LARS states are eligible for funding under this model. Check ''learning start date'' and ''funding model'' are correct.')
	--,('EmpId_10','E','''Employer identifier'' is missing. As the learner will be in paid employment at the start of the programme, you must supply the employer identifier number. Check you''ve supplied the ''employer identifier'' for the organisation the learner will be employed by at the start of the programme.')
	--,('PlanLearnHours_01','E','''Planned learning hours'' is missing. For this learner, you must provide details of the total planned timetabled hours for learning activities for the teaching year. Check ''planned learning hours'' and ''learning aim'' are complete and correct.')
	--,('EmpId_13','E','''Employer identifier'' is wrong. You must use the actual employer identifier number (not the default 999999999) as the programme started more than 60 days ago. Submit the correct employer identifier for the learner''s employer.')
	--,('PriorAttain_01','E','''Prior attainment code'' is missing. You must enter details of what the learner''s prior attainment was at the time they enrolled. Check ''prior attainment'' is complete and correct.')
	--,('SSN_02','E','''Student support number'' is wrong. We don''t recognise the number you''ve entered. Check the learner''s ''student support number'' is correct.')
	--,('UKPRN_10','E','You do not have a contract for apprenticeship funding. You must have a contract to claim this funding. Check ''UK provider reference number'' is correct, a contract is in place and the contract has been signed.')
	--,('R90','E','Some ''learning actual end dates'' are missing. There are learning aims recorded without an end date, where you''ve given an end date for the corresponding programme aim. Check any learning aims that have a corresponding programme aim that''s ended have a ''learning actual end date'' recorded.')
	--,('EmpStat_14','E','''Employment status'' is wrong. The employment status must be one of these codes: 10 = in paid employment; 11 = not in paid employment, looking for work and available to start work; 12 = not in paid employment, not looking for work and/or not available to start work; 98 = not known / not provided. Check ''employment status'' is correct.')
	--,('LearnDelFAMType_64','E','''Apprenticeship contract type'' is missing. You''ve indicated that this learner is funded through apprenticeship funding, so you need to provide details of the type of apprenticeship contract you have. Check ''funding model'' and ''learning delivery aim'' are included and correct.')
	--,('DelLocPostCode_17','E','''Delivery location postcode'' is wrong. It''s not within the local authority stated in the tender specification. Check ''delivery location postcode'' is correct and within the local authority stated in the tender specification.')
	--,('EngGrade_01','E','''GCSE English qualification grade'' is missing. You must enter the ''GCSE English qualification grade'' for all learners who are 16-19 funded. Check ''GCSE English qualification grade'' is complete and correct.')
	--,('MathGrade_01','E','''GCSE maths qualification grade'' is missing. You must enter the ''GCSE maths qualification grade'' for all learners who are 16-19 funded. Check ''GCSE maths qualification grade'' is complete and correct.')
	--,('AFinType_13','E','''Price record'' is missing. For all apprenticeship-funded programmes, there must be a price record that starts on the same day the programme started. Check ''price record'' is complete and correct.')
	--,('AFinType_12','E','''Price record'' is missing. You must enter the price details for all apprenticeship-funded programmes. Check ''price record'' is complete and correct.')
	--,('LearnDelFAMType_66','E','''Full or co-funding indicator'' is wrong. You must not claim full funding for learners aged 24 or over for aims that are level 2 or below who aren''t claiming benefits, unless they are in one of the exemption groups detailed in the funding rules. Check ''date of birth'', ''learning aim'' and ''full or co-funding indicator'' are correct.')
	--,('OrigLearnStartDate_05','E','''Original learning start date'' may be wrong. It must be within the valid start and end dates for this aim on LARS. Check the ''original learning start date'' is correct.')
	--,('PlanEEPHours_01','E','''Planned employability, enrichment and pastoral hours'' are missing. You must enter ''planned employability, enrichment and pastoral hours'' for each learner that''s 16-19 funded. Check ''planned employability, enrichment and pastoral hours'' and ''funding model'' are complete and correct.')
	--,('ULN_11','E','''Unique learner number'' (ULN) is wrong. The default learner number (9999999999) should only be used until 1 January, while you wait for the learner''s ULN registration. Check ''unique learner number'' and ''learning delivery FAM type'' are complete and correct.')
	--,('MathGrade_02','E','''GCSE maths qualification grade'' is wrong. The code you enter must be a valid code. See Appendix Q of the ILR Technical Specification. Check ''GCSE maths qualification grade'' is complete and correct.')
	--,('LearnAimRef_01','E','''Learning aim reference'' is wrong. It doesn''t match any learning aim reference in our system. Check LARS to find the correct learning aim.')
	--,('NUMHUS_01',NULL,'''Student instance identifier'' is missing. You must enter ''''student instance identifier'' details for all HE learners from 1 August 2011. Check ''student instance identifier'' and ''learning aim start date'' are complete and correct.')
	--,('QUALENT3_01',NULL,'''Qualification on entry'' is missing. You must enter ''qualification on entry'' details for all HE learners from 1 August 2010. Check ''qualifications on entry'' and ''learning aim start date'' are complete and correct.')
	--,('PCFLDCS_02',NULL,'''Percentage taught in the first LDCS subject'' is missing. You must enter the percentage of the academic year that the learner will spend studying the subject entered in LDCS 1 (Learning Directory Classification System). Check ''percentage taught in the first LDCS subject'' is complete and correct.')
	--,('TTACCOM_01',NULL,'''Term time accommodation'' is wrong: You must enter a value from 1 to 9 (depending on where the learner is living during term time). The ILR data specification provides details of what each code should be used for. Check ''term time accommodation'' is correct.')
	--,('TTACCOM_04',NULL,'Term time accommodation'' is missing: You must enter a value from 1 to 9 (depending on where the learner is living during term time). The ILR data specification provides details of what each code should be used for. Check ''term time accommodation'' is complete and correct.')
	--,('NETFEE_01',NULL,'''Net tuition fee'' is missing: The ''net tuition fee'' is the fee for the student on this course this year after any financial support (such as waivers or bursaries) is taken into account. Check ''net tuition fee'' is complete and correct.')
	--,('ELQ_01',NULL,'''Equivalent or lower qualification'' is missing: You must provide details of whether the learner is aiming for an equivalent or lower level qualification than one already achieved. Check ''equivalent or lower qualification'' and ''learning aim'' are complete and correct.')
	--,('LearnDelFAMType_02',NULL,'''Learning delivery funding and monitoring'' entry must be added for ''fully funded indicator'' for this funding model. You''ve indicated that the learner is adult skills funded. This means that you must add ''FFI: full or co-funding indicator'' to a ''learning delivery funding and monitoring'' entry. Check ''learning delivery funding and monitoring'' entries and ''funding model'' are correct.')
	--,('UKPRN_06',NULL,'You do not have a current contract for adult skills funding: You must have a contract for adult skills funding to be able to claim this type of funding. Check ''''UK provider reference number'''' is correct and that you have a contract in place for adult skills funding.')
	--,('R119','E','The apprenticeship financial record cannot be before the Learning start date when the total negotiated price has been returned')
	--,('LearnAimRef_89','E','The Learning aim reference is not valid in the LARS database for this Funding model for this teaching year')

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
			--		(	SELECT 
			--				 Target.[Severity]
			--		EXCEPT 
			--			SELECT 
			--				 Source.[Severity]
			--		)
		 -- THEN
			--UPDATE SET   [Severity] = Source.[Severity]
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
