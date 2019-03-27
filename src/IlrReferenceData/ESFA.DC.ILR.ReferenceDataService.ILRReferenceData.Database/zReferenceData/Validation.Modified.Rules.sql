
SET NOCOUNT ON;
BEGIN

	DECLARE @ModifiedRules TABLE ([Rulename] NVARCHAR(50) NOT NULL PRIMARY KEY, [Severity] NVARCHAR(1),[Message] NVARCHAR(2000) NULL);

	INSERT INTO @ModifiedRules( [Rulename],[Severity] ,[Message])
	VALUES 
	 ('DelLocPostCode_18','E','''Delivery location postcode'' is wrong. The postcode you''ve given isn''t a ''local enterprise partnership'' stated in the ESF tender specification. Check the ''delivery location postcode'' is correct.')
	,('DelLocPostCode_15','E','''Delivery location postcode'' is wrong. The postcode you''ve given isn''t a ''local enterprise partnership'' stated in the ESF tender specification. Check ''delivery location postcode'' is correct.')
	,('LearnDelFAMType_04','E','''Learning delivery FAM code'' is wrong. The code you''ve given us isn''t valid for that FAM type.Search the ILR specification for ''learner funding and monitoring type'' to see a full list of codes that can be used.')
	,('R91','E','The learner is missing a ''ZESF0001 learning aim. For this type of funding, there must be a ZESF0001 learning aim that the learner has completed. Check the ZESF0001 learning aim is complete and has been recorded with a completion status of 2 (the learner has completed the learning activities leading to the learning aim).')
	,('UKPRN_12','E','You do not have a contract for adult skills funding. You must have a contract for adult skills funding to be able to claim this type of funding. Check ''UK provider reference number'' is correct, a contract is in place and the contract has been signed.')
	,('LearnDelFAMType_06','E','''Learning start date'' is wrong. It can''t be after the ''valid to'' date found for that FAM type and code. Check the ''valid to'' date in the ILR specification - search for ''learning delivery funding and monitoring code''.')
	,('R112','E','''Applies to'' date doesn''t match the ''learning actual end date''. The latest ACT record in FAM type should have the same date in ''applies to'' as the ''learning actual end date''. Check ''date applies to'' and ''learning actual end date'' are correct.')
	,('PlanLearnHours_03','E','The sum of the ''planned learning hours'' and the ''planned employability, enrichment and pastoral hours'' must not be zero. The learner must have planned hours in place for the teaching year. Check ''planned learning hours'' and ''planned employability, enrichment and pastoral hours'' are correct.')
	,('R20','E','There are too many competency aims. A learner can only have one competency aim at a time. Make sure the ''learning actual start dates'' and ''end dates'' of the competency aims don''t overlap.')
	,('Sex_01','E','''Sex'' is missing. You must enter the legal sex of each learner. Check ''sex'' is correct.')
	,('EmpStat_08','E','''Employment status'' is missing. You must provide details of the learner''s employment status prior to the start of their learning aim. Check ''employment status'' is complete and correct.')
	,('StdCode_03','E','''Apprenticeship standard code'' is wrong. You''ve indicated this is a framework apprenticeship in ''programme type'' so you mustn''t return a code in ''apprenticeship standard code''. Check ''programme type is correct. If it is, make sure ''Apprenticeship standard code'' is empty.')
	,('UKPRN_13','E','You do not have a contract for apprenticeship funding. You must have a contract for non-levy apprenticeship to be able to claim this type of funding. Check ''UK provider reference number'' is correct, a contract is in place and the contract has been signed.')
	,('R106','E','There are too many ''learning delivery FAM types'' with a code of LSF. The learner can only have one LSF (learning support) record. Check only one record with a ''learning delivery FAM type'' of LSF exists for this learner.')
	,('LearnStartDate_07','E','''Learning start date'' is wrong. It''s currently outside of the ''Pathway effective to'' date for this component on the LARS framework table. "Check ''learning start date'' is correct.')
	,('DelLocPostCode_11','E','''Delivery location postcode'' is wrong. It''s not a valid UK postcode. Check ''delivery location postcode'' is in the correct format. If it''s unknown, use ZZ99 9ZZ.')
	,('DelLocPostCode_16','E','''Delivery location postcode'' is wrong. It''s not a valid UK postcode. Check ''delivery location postcode'' is correct. If it''s unknown, use ZZ99 9ZZ.')
	,('WorkPlaceStartDate_01','E','''Work placement'' record is missing. As this is a work experience aim, you need to return this information. Check the ''work placement'' record is filled in.')
	,('LearnAimRef_88','E','''Learning start date'' or ''funding model'' may be wrong. The start date you''ve entered is outside the dates LARS states are eligible for funding under this model. Check ''learning start date'' and ''funding model'' are correct.')
	,('EmpId_10','E','''Employer identifier'' is missing. As the learner will be in paid employment at the start of the programme, you must supply the employer identifier number. Check you''ve supplied the ''employer identifier'' for the organisation the learner will be employed by at the start of the programme.')
	,('PlanLearnHours_01','E','''Planned learning hours'' is missing. For this learner, you must provide details of the total planned timetabled hours for learning activities for the teaching year. Check ''planned learning hours'' and ''learning aim'' are complete and correct.')
	,('EmpId_13','E','''Employer identifier'' is wrong. You must use the actual employer identifier number (not the default 999999999) as the programme started more than 60 days ago. Submit the correct employer identifier for the learner''s employer.')
	,('PriorAttain_01','E','''Prior attainment code'' is missing. You must enter details of what the learner''s prior attainment was at the time they enrolled. Check ''prior attainment'' is complete and correct.')
	,('SSN_02','E','''Student support number'' is wrong. We don''t recognise the number you''ve entered. Check the learner''s ''student support number'' is correct.')
	,('UKPRN_10','E','You do not have a contract for apprenticeship funding. You must have a contract to claim this funding. Check ''UK provider reference number'' is correct, a contract is in place and the contract has been signed.')
	,('R90','E','Some ''learning actual end dates'' are missing. There are learning aims recorded without an end date, where you''ve given an end date for the corresponding programme aim. Check any learning aims that have a corresponding programme aim that''s ended have a ''learning actual end date'' recorded.')
	,('EmpStat_14','E','''Employment status'' is wrong. The employment status must be one of these codes: 10 = in paid employment; 11 = not in paid employment, looking for work and available to start work; 12 = not in paid employment, not looking for work and/or not available to start work; 98 = not known / not provided. Check ''employment status'' is correct.')
	,('LearnDelFAMType_64','E','''Apprenticeship contract type'' is missing. You''ve indicated that this learner is funded through apprenticeship funding, so you need to provide details of the type of apprenticeship contract you have. Check ''funding model'' and ''learning delivery aim'' are included and correct.')
	,('DelLocPostCode_17','E','''Delivery location postcode'' is wrong. It''s not within the local authority stated in the tender specification. Check ''delivery location postcode'' is correct and within the local authority stated in the tender specification.')
	,('EngGrade_01','E','''GCSE English qualification grade'' is missing. You must enter the ''GCSE English qualification grade'' for all learners who are 16-19 funded. Check ''GCSE English qualification grade'' is complete and correct.')
	,('MathGrade_01','E','''GCSE maths qualification grade'' is missing. You must enter the ''GCSE maths qualification grade'' for all learners who are 16-19 funded. Check ''GCSE maths qualification grade'' is complete and correct.')
	,('AFinType_13','E','''Price record'' is missing. For all apprenticeship-funded programmes, there must be a price record that starts on the same day the programme started. Check ''price record'' is complete and correct.')
	,('AFinType_12','E','''Price record'' is missing. You must enter the price details for all apprenticeship-funded programmes. Check ''price record'' is complete and correct.')
	,('LearnDelFAMType_66','E','''Full or co-funding indicator'' is wrong. You must not claim full funding for learners aged 24 or over for aims that are level 2 or below who aren''t claiming benefits, unless they are in one of the exemption groups detailed in the funding rules. Check ''date of birth'', ''learning aim'' and ''full or co-funding indicator'' are correct.')
	,('OrigLearnStartDate_05','E','''Original learning start date'' may be wrong. It must be within the valid start and end dates for this aim on LARS. Check the ''original learning start date'' is correct.')
	,('PlanEEPHours_01','E','''Planned employability, enrichment and pastoral hours'' are missing. You must enter ''planned employability, enrichment and pastoral hours'' for each learner that''s 16-19 funded. Check ''planned employability, enrichment and pastoral hours'' and ''funding model'' are complete and correct.')
	,('ULN_11','E','''Unique learner number'' (ULN) is wrong. The default learner number (9999999999) should only be used until 1 January, while you wait for the learner''s ULN registration. Check ''unique learner number'' and ''learning delivery FAM type'' are complete and correct.')
	,('MathGrade_02','E','''GCSE maths qualification grade'' is wrong. The code you enter must be a valid code. See Appendix Q of the ILR Technical Specification. Check ''GCSE maths qualification grade'' is complete and correct.')
	,('LearnAimRef_01','E','''Learning aim reference'' is wrong. It doesn''t match any learning aim reference in our system. Check LARS to find the correct learning aim.')
	,('NUMHUS_01',NULL,'''Student instance identifier'' is missing. You must enter ''''student instance identifier'' details for all HE learners from 1 August 2011. Check ''student instance identifier'' and ''learning aim start date'' are complete and correct.')
	,('QUALENT3_01',NULL,'''Qualification on entry'' is missing. You must enter ''qualification on entry'' details for all HE learners from 1 August 2010. Check ''qualifications on entry'' and ''learning aim start date'' are complete and correct.')
	,('PCFLDCS_02',NULL,'''Percentage taught in the first LDCS subject'' is missing. You must enter the percentage of the academic year that the learner will spend studying the subject entered in LDCS 1 (Learning Directory Classification System). Check ''percentage taught in the first LDCS subject'' is complete and correct.')
	,('TTACCOM_01',NULL,'''Term time accommodation'' is wrong: You must enter a value from 1 to 9 (depending on where the learner is living during term time). The ILR data specification provides details of what each code should be used for. Check ''term time accommodation'' is correct.')
	,('TTACCOM_04',NULL,'Term time accommodation'' is missing: You must enter a value from 1 to 9 (depending on where the learner is living during term time). The ILR data specification provides details of what each code should be used for. Check ''term time accommodation'' is complete and correct.')
	,('NETFEE_01',NULL,'''Net tuition fee'' is missing: The ''net tuition fee'' is the fee for the student on this course this year after any financial support (such as waivers or bursaries) is taken into account. Check ''net tuition fee'' is complete and correct.')
	,('ELQ_01',NULL,'''Equivalent or lower qualification'' is missing: You must provide details of whether the learner is aiming for an equivalent or lower level qualification than one already achieved. Check ''equivalent or lower qualification'' and ''learning aim'' are complete and correct.')
	,('LearnDelFAMType_02',NULL,'''Learning delivery funding and monitoring'' entry must be added for ''fully funded indicator'' for this funding model. You''ve indicated that the learner is adult skills funded. This means that you must add ''FFI: full or co-funding indicator'' to a ''learning delivery funding and monitoring'' entry. Check ''learning delivery funding and monitoring'' entries and ''funding model'' are correct.')
	,('UKPRN_06',NULL,'You do not have a current contract for adult skills funding: You must have a contract for adult skills funding to be able to claim this type of funding. Check ''''UK provider reference number'''' is correct and that you have a contract in place for adult skills funding.')
	,('LearnDelFAMType_44',NULL,'Household situation'' is missing: You must provide details of the learner''s household situation for learners that are ''ESF'' (European Social Fund) or ''adult skills'' or ''other SFA'' funded. Check ''learning delivery funding and monitoring'' entry for ''household situation'' and ''funding model'' are complete and correct.')
	,('LearnDelFAMType_03',NULL,'''Community learning provision type'' is missing: You must provide details of the type of community learning activity being undertaken by the learner. Check that you have included a ''learning delivery funding and monitoring'' entry for type ''ASL'' for each learning delivery funded with ''community learning''.')
	,('ULN_07',NULL,'''Unique learner number'' (ULN) is wrong for the combination of course length with funding model and file preparation date and learning start date: The temporary learner number (9999999999) should only be used before 1 January while you wait for the learner''''s ULN registration. Check the ''unique learner number'' and ''file preparation date'' and ''funding model'' and ''learning start/end dates'' are correct.')
	,('LearnActEndDate_04',NULL,'''Learning actual end date'' is wrong: The learning must have actually ended. This means that the ''learning actual end date'' must be before the date the ILR file was created. Check ''learning actual end date'' and ''file preparation date'' are correct.')
	,('MathGrade_03',NULL,'Learner FAM type'' is wrong: If the GCSE maths qualification is ''D'' (or 3) or below you must include a ''''learner FAM type'' of ''eligibility for EFA disadvantage funding''. Check ''learner math grade'' and ''learner FAM type'' entries are correct.')
	,('ULN_12',NULL,'''Unique learner number'' (ULN) is wrong: The default learner number (9999999999) should only be used until 1 January while you wait for the learner''s ULN registration. Check ''unique learner number'' and ''learning delivery FAM type'' are correct.')
	,('ProgType_01',NULL,'Programme type'' is missing: You must include ''programme type'' details for all learning aims that are part of a wider programme (aim type 1 or aim type 3). Check ''programme type'' and ''learning aim type'' are complete and correct.')
	,('ProgType_13',NULL,'Learning actual end date'' is missing or the traineeship is too long: If the ''learning actual end date'' is missing there can''t be more than 8 months between the ''learning start date'' and date this file was created. Check ''learning aim type'' and ''programme type'' and ''learning actual end date'' and ''learning actual start date'' are complete and correct.')
	,('CompStatus_04',NULL,'''Learning delivery outcome status'' is missing: You''ve indicated that this learner has completed their learning aim but haven''t provided details of the outcome. Check ''learning delivery completion status'' and ''learning delivery outcome'' are complete and correct.')
	,('OrigLearnStartDate_04',NULL,'''Restart indicator'' is missing: You''ve indicated that this learner has restarted their learning by completing the ''original learning start date'' but you haven''t entered a ''learning delivery funding and monitoring type restart indicator''. Check ''original learning start date'' and ''learning delivery funding and monitoring type'' are complete and correct.')
	,('LearnAimRef_30',NULL,'Learning aim reference'' is wrong: You''ve indicated this learner is on an apprenticeship programme or traineeship (learning aim type 1). This means their ''learning aim reference'' must be ''ZPROG001''. Check ''learning aim reference'' and ''learning aim'' are correct.')
	,('FundModel_06',NULL,'''Learning aim'' is wrong: You''ve indicated that this learner is apprenticeship funded so they must be on an apprenticeship aim. Check ''funding model'' and ''learning aim'' are correct.')
	,('FworkCode_05',NULL,'''Framework code'' and ''pathway code'' and ''programme type'' combination is wrong: You''ve indicated that the learning aim is part of an apprenticeship framework programme but the framework code and pathway code and programme type isn''t a valid combination for this aim in the LARS database. Check ''learning aim'' and ''framework code'' and ''pathway code'' and ''programme type'' are correct.')
	,('LearnDelFAMDateTo_03',NULL,'''Date applies to'' is wrong: The date that the ''funding and monitoring'' applies to must be on or before the actual end date of the learning. Check ''date applies to'' and ''learning actual end date'' are correct.')
	--,('LearnDelFAMDateTo_03',NULL,'Date applies to'' is wrong: The date that the ''funding and monitoring'' applies to must be on or before the actual end date of the learning. Check ''date applies to'' and ''learning actual end date'' are correct.')
	,('CompStatus_03',NULL,'''Learning actual end date'' is missing: You''ve indicated that the learner has completed their learning aim but haven''t provided a ''learning actual end date''. Check ''learning actual end date'' and ''completion status'' are complete and correct.')
	,('R119','E','The apprenticeship financial record cannot be before the Learning start date when the total negotiated price has been returned')
	,('EmpStat_19',NULL,'The learner must not be employed for more than 16 hours per week at the start of the programme')
	,('LearnAimRef_89','E','The Learning aim reference is not valid in the LARS database for this Funding model for this teaching year')

	DECLARE @SummaryOfChanges_ModifiedRules TABLE ([Rulename]  NVARCHAR(50) NOT NULL, [Action] VARCHAR(100) NOT NULL);

	MERGE INTO [Staging].[ModifiedRules] AS Target
		USING (
				   SELECT [Rulename]
						,[Severity]
						,[Message]
					FROM @ModifiedRules
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
		OUTPUT ISNULL(Deleted.[Rulename],Inserted.[Rulename]),$action INTO @SummaryOfChanges_ModifiedRules([Rulename],[Action])
	    ;

		DECLARE @AddCount_MR INT, @UpdateCount_MR INT, @DeleteCount_MR INT;
		SET @AddCount_MR  = ISNULL((SELECT Count(*) FROM @SummaryOfChanges_ModifiedRules WHERE [Action] = 'Insert' GROUP BY Action),0);
		SET @UpdateCount_MR = ISNULL((SELECT Count(*) FROM @SummaryOfChanges_ModifiedRules WHERE [Action] = 'Update' GROUP BY Action),0);
		SET @DeleteCount_MR = ISNULL((SELECT Count(*) FROM @SummaryOfChanges_ModifiedRules WHERE [Action] = 'Delete' GROUP BY Action),0);

		RAISERROR('		      %s - Added %i - Update %i - Delete %i',10,1,'    Modified Rules', @AddCount_MR, @UpdateCount_MR, @DeleteCount_MR) WITH NOWAIT;
END		
