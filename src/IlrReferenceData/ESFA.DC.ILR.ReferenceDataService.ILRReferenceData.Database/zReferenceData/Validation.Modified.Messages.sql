
SET NOCOUNT ON;
BEGIN

	DECLARE @ModifiedMessages TABLE ([Rulename] NVARCHAR(50) NOT NULL PRIMARY KEY,[Message] NVARCHAR(2000) NULL);

	INSERT INTO @ModifiedMessages( [Rulename],[Message])
	VALUES 
	
	 ('DelLocPostCode_18','''Delivery location postcode'' is wrong. The postcode you''ve given isn''t a ''local enterprise partnership'' stated in the ESF tender specification. Check the ''delivery location postcode'' is correct.')
	,('DelLocPostCode_15','''Delivery location postcode'' is wrong. The postcode you''ve given isn''t a ''local enterprise partnership'' stated in the ESF tender specification. Check ''delivery location postcode'' is correct.')
	,('LearnDelFAMType_04','''Learning delivery FAM code'' is wrong. The code you''ve given us isn''t valid for that FAM type.Search the ILR specification for ''learner funding and monitoring type'' to see a full list of codes that can be used.')
	,('R91','The learner is missing a ''ZESF0001 learning aim. For this type of funding, there must be a ZESF0001 learning aim that the learner has completed. Check the ZESF0001 learning aim is complete and has been recorded with a completion status of 2 (the learner has completed the learning activities leading to the learning aim).')
	,('UKPRN_12','You do not have a contract for adult skills funding. You must have a contract for adult skills funding to be able to claim this type of funding. Check ''UK provider reference number'' is correct, a contract is in place and the contract has been signed.')
	,('LearnDelFAMType_06','''Learning start date'' is wrong. It can''t be after the ''valid to'' date found for that FAM type and code. Check the ''valid to'' date in the ILR specification - search for ''learning delivery funding and monitoring code''.')
	,('R112','''Applies to'' date doesn''t match the ''learning actual end date''. The latest ACT record in FAM type should have the same date in ''applies to'' as the ''learning actual end date''. Check ''date applies to'' and ''learning actual end date'' are correct.')
	,('PlanLearnHours_03','The sum of the ''planned learning hours'' and the ''planned employability, enrichment and pastoral hours'' must not be zero. The learner must have planned hours in place for the teaching year. Check ''planned learning hours'' and ''planned employability, enrichment and pastoral hours'' are correct.')
	,('R20','There are too many competency aims. A learner can only have one competency aim at a time. Make sure the ''learning actual start dates'' and ''end dates'' of the competency aims don''t overlap.')
	,('Sex_01','''Sex'' is missing. You must enter the legal sex of each learner. Check ''sex'' is correct.')
	,('EmpStat_08','''Employment status'' is missing. You must provide details of the learner''s employment status prior to the start of their learning aim. Check ''employment status'' is complete and correct.')
	,('StdCode_03','''Apprenticeship standard code'' is wrong. You''ve indicated this is a framework apprenticeship in ''programme type'' so you mustn''t return a code in ''apprenticeship standard code''. Check ''programme type is correct. If it is, make sure ''Apprenticeship standard code'' is empty.')
	,('UKPRN_13','You do not have a contract for apprenticeship funding. You must have a contract for non-levy apprenticeship to be able to claim this type of funding. Check ''UK provider reference number'' is correct, a contract is in place and the contract has been signed.')
	,('R106','There are too many ''learning delivery FAM types'' with a code of LSF. The learner can only have one LSF (learning support) record. Check only one record with a ''learning delivery FAM type'' of LSF exists for this learner.')
	,('LearnStartDate_07','''Learning start date'' is wrong. It''s currently outside of the ''Pathway effective to'' date for this component on the LARS framework table. "Check ''learning start date'' is correct.')
	,('DelLocPostCode_11','''Delivery location postcode'' is wrong. It''s not a valid UK postcode. Check ''delivery location postcode'' is in the correct format. If it''s unknown, use ZZ99 9ZZ.')
	,('DelLocPostCode_16','''Delivery location postcode'' is wrong. It''s not a valid UK postcode. Check ''delivery location postcode'' is correct. If it''s unknown, use ZZ99 9ZZ.')
	,('WorkPlaceStartDate_01','''Work placement'' record is missing. As this is a work experience aim, you need to return this information. Check the ''work placement'' record is filled in.')
	,('LearnAimRef_88','''Learning start date'' or ''funding model'' may be wrong. The start date you''ve entered is outside the dates LARS states are eligible for funding under this model. Check ''learning start date'' and ''funding model'' are correct.')
	,('EmpId_10','''Employer identifier'' is missing. As the learner will be in paid employment at the start of the programme, you must supply the employer identifier number. Check you''ve supplied the ''employer identifier'' for the organisation the learner will be employed by at the start of the programme.')
	,('PlanLearnHours_01','''Planned learning hours'' is missing. For this learner, you must provide details of the total planned timetabled hours for learning activities for the teaching year. Check ''planned learning hours'' and ''learning aim'' are complete and correct.')
	,('EmpId_13','''Employer identifier'' is wrong. You must use the actual employer identifier number (not the default 999999999) as the programme started more than 60 days ago. Submit the correct employer identifier for the learner''s employer.')
	,('PriorAttain_01','''Prior attainment code'' is missing. You must enter details of what the learner''s prior attainment was at the time they enrolled. Check ''prior attainment'' is complete and correct.')
	,('SSN_02','''Student support number'' is wrong. We don''t recognise the number you''ve entered. Check the learner''s ''student support number'' is correct.')
	,('UKPRN_10','You do not have a contract for apprenticeship funding. You must have a contract to claim this funding. Check ''UK provider reference number'' is correct, a contract is in place and the contract has been signed.')
	,('R90','Some ''learning actual end dates'' are missing. There are learning aims recorded without an end date, where you''ve given an end date for the corresponding programme aim. Check any learning aims that have a corresponding programme aim that''s ended have a ''learning actual end date'' recorded.')
	,('EmpStat_14','''Employment status'' is wrong. The employment status must be one of these codes: 10 = in paid employment; 11 = not in paid employment, looking for work and available to start work; 12 = not in paid employment, not looking for work and/or not available to start work; 98 = not known / not provided. Check ''employment status'' is correct.')
	,('LearnDelFAMType_64','''Apprenticeship contract type'' is missing. You''ve indicated that this learner is funded through apprenticeship funding, so you need to provide details of the type of apprenticeship contract you have. Check ''funding model'' and ''learning delivery aim'' are included and correct.')
	,('DelLocPostCode_17','''Delivery location postcode'' is wrong. It''s not within the local authority stated in the tender specification. Check ''delivery location postcode'' is correct and within the local authority stated in the tender specification.')
	,('EngGrade_01','''GCSE English qualification grade'' is missing. You must enter the ''GCSE English qualification grade'' for all learners who are 16-19 funded. Check ''GCSE English qualification grade'' is complete and correct.')
	,('MathGrade_01','''GCSE maths qualification grade'' is missing. You must enter the ''GCSE maths qualification grade'' for all learners who are 16-19 funded. Check ''GCSE maths qualification grade'' is complete and correct.')
	,('AFinType_13','''Price record'' is missing. For all apprenticeship-funded programmes, there must be a price record that starts on the same day the programme started. Check ''price record'' is complete and correct.')
	,('AFinType_12','''Price record'' is missing. You must enter the price details for all apprenticeship-funded programmes. Check ''price record'' is complete and correct.')
	,('LearnDelFAMType_66','''Full or co-funding indicator'' is wrong. You must not claim full funding for learners aged 24 or over for aims that are level 2 or below who aren''t claiming benefits, unless they are in one of the exemption groups detailed in the funding rules. Check ''date of birth'', ''learning aim'' and ''full or co-funding indicator'' are correct.')
	,('OrigLearnStartDate_05','''Original learning start date'' may be wrong. It must be within the valid start and end dates for this aim on LARS. Check the ''original learning start date'' is correct.')
	,('PlanEEPHours_01','''Planned employability, enrichment and pastoral hours'' are missing. You must enter ''planned employability, enrichment and pastoral hours'' for each learner that''s 16-19 funded. Check ''planned employability, enrichment and pastoral hours'' and ''funding model'' are complete and correct.')
	,('ULN_11','''Unique learner number'' (ULN) is wrong. The default learner number (9999999999) should only be used until 1 January, while you wait for the learner''s ULN registration. Check ''unique learner number'' and ''learning delivery FAM type'' are complete and correct.')
	,('MathGrade_02','''GCSE maths qualification grade'' is wrong. The code you enter must be a valid code. See Appendix Q of the ILR Technical Specification. Check ''GCSE maths qualification grade'' is complete and correct.')
	,('LearnAimRef_01','''Learning aim reference'' is wrong. It doesn''t match any learning aim reference in our system. Check LARS to find the correct learning aim.')
	,('R119','The apprenticeship financial record cannot be before the Learning start date when the total negotiated price has been returned')
	,('LearnAimRef_89','The Learning aim reference is not valid in the LARS database for this Funding model for this teaching year')



	DECLARE @SummaryOfChanges_ModifiedMessages TABLE ([Rulename]  NVARCHAR(50) NOT NULL, [Action] VARCHAR(100) NOT NULL);

	MERGE INTO [Staging].[ModifiedMessages] AS Target
		USING (
				   SELECT [Rulename]
						 ,[Message]
					FROM @ModifiedMessages
			  )
			  AS Source 
		    ON Target.[Rulename] = Source.[Rulename]
			WHEN MATCHED 
				AND EXISTS 
					(	SELECT 
							Target.[Message]
					EXCEPT 
						SELECT 
							Source.[Message]
					)
		  THEN
			UPDATE SET   [Message] = Source.[Message]
		WHEN NOT MATCHED BY TARGET THEN
		INSERT (     [Rulename]
					,[Message]
					)
			VALUES ( Source.[Rulename]
					,Source.[Message]
				  )
		WHEN NOT MATCHED BY SOURCE THEN DELETE
		OUTPUT ISNULL(Deleted.[Rulename],Inserted.[Rulename]),$action INTO @SummaryOfChanges_ModifiedMessages([Rulename],[Action])
	    ;

		DECLARE @AddCount_MSG INT, @UpdateCount_MSG INT, @DeleteCount_MSG INT;
		SET @AddCount_MSG  = ISNULL((SELECT Count(*) FROM @SummaryOfChanges_ModifiedMessages WHERE [Action] = 'Insert' GROUP BY Action),0);
		SET @UpdateCount_MSG = ISNULL((SELECT Count(*) FROM @SummaryOfChanges_ModifiedMessages WHERE [Action] = 'Update' GROUP BY Action),0);
		SET @DeleteCount_MSG = ISNULL((SELECT Count(*) FROM @SummaryOfChanges_ModifiedMessages WHERE [Action] = 'Delete' GROUP BY Action),0);

		RAISERROR('		      %s     - Added %i - Update %i - Delete %i',10,1,'    Modified Rules', @AddCount_MSG, @UpdateCount_MSG, @DeleteCount_MSG) WITH NOWAIT;
END		
