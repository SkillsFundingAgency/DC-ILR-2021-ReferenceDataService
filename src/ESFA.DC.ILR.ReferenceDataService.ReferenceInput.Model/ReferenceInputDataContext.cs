using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class ReferenceInputDataContext : DbContext
    {
        public ReferenceInputDataContext()
        {
        }

        public ReferenceInputDataContext(DbContextOptions<ReferenceInputDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppsEarningHistory_ApprenticeshipEarningsHistory> AppsEarningHistory_ApprenticeshipEarningsHistories { get; set; }
        public virtual DbSet<EPAOrganisations_EPAOrganisation> EPAOrganisations_EPAOrganisations { get; set; }
        public virtual DbSet<Employers_Employer> Employers_Employers { get; set; }
        public virtual DbSet<Employers_LargeEmployerEffectiveDate> Employers_LargeEmployerEffectiveDates { get; set; }
        public virtual DbSet<Employers_LargeEmployerVersion> Employers_LargeEmployerVersions { get; set; }
        public virtual DbSet<FCS_EsfEligibilityRule> FCS_EsfEligibilityRules { get; set; }
        public virtual DbSet<FCS_EsfEligibilityRuleEmploymentStatus> FCS_EsfEligibilityRuleEmploymentStatuses { get; set; }
        public virtual DbSet<FCS_EsfEligibilityRuleLocalAuthority> FCS_EsfEligibilityRuleLocalAuthorities { get; set; }
        public virtual DbSet<FCS_EsfEligibilityRuleLocalEnterprisePartnership> FCS_EsfEligibilityRuleLocalEnterprisePartnerships { get; set; }
        public virtual DbSet<FCS_EsfEligibilityRuleSectorSubjectAreaLevel> FCS_EsfEligibilityRuleSectorSubjectAreaLevels { get; set; }
        public virtual DbSet<FCS_FcsContractAllocation> FCS_FcsContractAllocations { get; set; }
        public virtual DbSet<FCS_FcsContractDeliverable> FCS_FcsContractDeliverables { get; set; }
        public virtual DbSet<FRM_FrmLearner> FRM_FrmLearners { get; set; }
        public virtual DbSet<FRM_FrmReferenceData> FRM_FrmReferenceDatas { get; set; }
        public virtual DbSet<FRM_LearningDeliveryFAM> FRM_LearningDeliveryFAMs { get; set; }
        public virtual DbSet<FRM_ProviderSpecDeliveryMonitoring> FRM_ProviderSpecDeliveryMonitorings { get; set; }
        public virtual DbSet<FRM_ProviderSpecLearnerMonitoring> FRM_ProviderSpecLearnerMonitorings { get; set; }
        public virtual DbSet<LARS_LARSAnnualValue> LARS_LARSAnnualValues { get; set; }
        public virtual DbSet<LARS_LARSFrameworkAim> LARS_LARSFrameworkAims { get; set; }
        public virtual DbSet<LARS_LARSFrameworkApprenticeshipFunding> LARS_LARSFrameworkApprenticeshipFundings { get; set; }
        public virtual DbSet<LARS_LARSFrameworkCommonComponent> LARS_LARSFrameworkCommonComponents { get; set; }
        public virtual DbSet<LARS_LARSFrameworkDesktop> LARS_LARSFrameworkDesktops { get; set; }
        public virtual DbSet<LARS_LARSFunding> LARS_LARSFundings { get; set; }
        public virtual DbSet<LARS_LARSLearningDelivery> LARS_LARSLearningDeliveries { get; set; }
        public virtual DbSet<LARS_LARSLearningDeliveryCategory> LARS_LARSLearningDeliveryCategories { get; set; }
        public virtual DbSet<LARS_LARSStandard> LARS_LARSStandards { get; set; }
        public virtual DbSet<LARS_LARSStandardApprenticeshipFunding> LARS_LARSStandardApprenticeshipFundings { get; set; }
        public virtual DbSet<LARS_LARSStandardCommonComponent> LARS_LARSStandardCommonComponents { get; set; }
        public virtual DbSet<LARS_LARSStandardFunding> LARS_LARSStandardFundings { get; set; }
        public virtual DbSet<LARS_LARSStandardValidity> LARS_LARSStandardValidities { get; set; }
        public virtual DbSet<LARS_LARSValidity> LARS_LARSValidities { get; set; }
        public virtual DbSet<LARS_LARSVersion> LARS_LARSVersions { get; set; }
        public virtual DbSet<McaContracts_McaDevolvedContract> McaContracts_McaDevolvedContracts { get; set; }
        public virtual DbSet<MetaData_CampusIdentifierVersion> MetaData_CampusIdentifierVersions { get; set; }
        public virtual DbSet<MetaData_CensusDate> MetaData_CensusDates { get; set; }
        public virtual DbSet<MetaData_CoFVersion> MetaData_CoFVersions { get; set; }
        public virtual DbSet<MetaData_DevolvedPostcodesVersion> MetaData_DevolvedPostcodesVersions { get; set; }
        public virtual DbSet<MetaData_EasUploadDateTime> MetaData_EasUploadDateTimes { get; set; }
        public virtual DbSet<MetaData_EmployersVersion> MetaData_EmployersVersions { get; set; }
        public virtual DbSet<MetaData_HmppPostcodesVersion> MetaData_HmppPostcodesVersions { get; set; }
        public virtual DbSet<MetaData_IlrCollectionDate> MetaData_IlrCollectionDates { get; set; }
        public virtual DbSet<MetaData_LarsVersion> MetaData_LarsVersions { get; set; }
        public virtual DbSet<MetaData_Lookup> MetaData_Lookups { get; set; }
        public virtual DbSet<MetaData_LookupSubCategory> MetaData_LookupSubCategories { get; set; }
        public virtual DbSet<MetaData_MetaData> MetaData_MetaDatas { get; set; }
        public virtual DbSet<MetaData_OrganisationsVersion> MetaData_OrganisationsVersions { get; set; }
        public virtual DbSet<MetaData_PostcodeFactorsVersion> MetaData_PostcodeFactorsVersions { get; set; }
        public virtual DbSet<MetaData_PostcodesVersion> MetaData_PostcodesVersions { get; set; }
        public virtual DbSet<MetaData_ReferenceDataVersion> MetaData_ReferenceDataVersions { get; set; }
        public virtual DbSet<MetaData_ValidationError> MetaData_ValidationErrors { get; set; }
        public virtual DbSet<MetaData_ValidationRule> MetaData_ValidationRules { get; set; }
        public virtual DbSet<Organisations_Organisation> Organisations_Organisations { get; set; }
        public virtual DbSet<Organisations_OrganisationCampusIdentifier> Organisations_OrganisationCampusIdentifiers { get; set; }
        public virtual DbSet<Organisations_OrganisationCoFRemoval> Organisations_OrganisationCoFRemovals { get; set; }
        public virtual DbSet<Organisations_OrganisationFunding> Organisations_OrganisationFundings { get; set; }
        public virtual DbSet<Organisations_OrganisationVersion> Organisations_OrganisationVersions { get; set; }
        public virtual DbSet<Organisations_SpecialistResource> Organisations_SpecialistResources { get; set; }
        public virtual DbSet<PostcodesDevolution_DevolvedPostcode> PostcodesDevolution_DevolvedPostcodes { get; set; }
        public virtual DbSet<PostcodesDevolution_McaGlaSofLookup> PostcodesDevolution_McaGlaSofLookups { get; set; }
        public virtual DbSet<PostcodesDevolution_Postcode> PostcodesDevolution_Postcodes { get; set; }
        public virtual DbSet<Postcodes_DasDisadvantage> Postcodes_DasDisadvantages { get; set; }
        public virtual DbSet<Postcodes_EfaDisadvantage> Postcodes_EfaDisadvantages { get; set; }
        public virtual DbSet<Postcodes_McaglaSOF> Postcodes_McaglaSOFs { get; set; }
        public virtual DbSet<Postcodes_ONSData> Postcodes_ONSDatas { get; set; }
        public virtual DbSet<Postcodes_PostCodeVersion> Postcodes_PostCodeVersions { get; set; }
        public virtual DbSet<Postcodes_Postcode> Postcodes_Postcodes { get; set; }
        public virtual DbSet<Postcodes_SfaAreaCost> Postcodes_SfaAreaCosts { get; set; }
        public virtual DbSet<Postcodes_SfaDisadvantage> Postcodes_SfaDisadvantages { get; set; }
        public virtual DbSet<ULN> ULNs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\;Database=ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Database;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<AppsEarningHistory_ApprenticeshipEarningsHistory>(entity =>
            {
                entity.ToTable("AppsEarningHistory_ApprenticeshipEarningsHistory", "ReferenceInput");

                entity.Property(e => e.AppIdentifier).HasMaxLength(2000);

                entity.Property(e => e.CollectionReturnCode).HasMaxLength(2000);

                entity.Property(e => e.CollectionYear).HasMaxLength(2000);

                entity.Property(e => e.HistoricEffectiveTNPStartDateInput).HasColumnType("datetime");

                entity.Property(e => e.HistoricLearnDelProgEarliestACT2DateInput).HasColumnType("datetime");

                entity.Property(e => e.HistoricPMRAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HistoricTNP1Input).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HistoricTNP2Input).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HistoricTNP3Input).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HistoricTNP4Input).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HistoricTotal1618UpliftPaymentsInTheYearInput).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HistoricVirtualTNP3EndOfTheYearInput).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HistoricVirtualTNP4EndOfTheYearInput).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LearnRefNumber).HasMaxLength(2000);

                entity.Property(e => e.ProgrammeStartDateIgnorePathway).HasColumnType("datetime");

                entity.Property(e => e.ProgrammeStartDateMatchPathway).HasColumnType("datetime");

                entity.Property(e => e.TotalProgAimPaymentsInTheYear).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UptoEndDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EPAOrganisations_EPAOrganisation>(entity =>
            {
                entity.ToTable("EPAOrganisations_EPAOrganisation", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.Standard).HasMaxLength(2000);
            });

            modelBuilder.Entity<Employers_Employer>(entity =>
            {
                entity.ToTable("Employers_Employer", "ReferenceInput");
            });

            modelBuilder.Entity<Employers_LargeEmployerEffectiveDate>(entity =>
            {
                entity.ToTable("Employers_LargeEmployerEffectiveDates", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.Employers_Employer_)
                    .WithMany(p => p.Employers_LargeEmployerEffectiveDates)
                    .HasForeignKey(d => d.Employers_Employer_Id)
                    .HasConstraintName("FK_ReferenceInput.Employers_LargeEmployerEffectiveDates_ReferenceInput.Employers_Employer_Employer_Id");
            });

            modelBuilder.Entity<Employers_LargeEmployerVersion>(entity =>
            {
                entity.ToTable("Employers_LargeEmployerVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<FCS_EsfEligibilityRule>(entity =>
            {
                entity.ToTable("FCS_EsfEligibilityRule", "ReferenceInput");

                entity.Property(e => e.LotReference).HasMaxLength(2000);

                entity.Property(e => e.MaxPriorAttainment).HasMaxLength(2000);

                entity.Property(e => e.MinPriorAttainment).HasMaxLength(2000);

                entity.Property(e => e.TenderSpecReference).HasMaxLength(2000);
            });

            modelBuilder.Entity<FCS_EsfEligibilityRuleEmploymentStatus>(entity =>
            {
                entity.ToTable("FCS_EsfEligibilityRuleEmploymentStatus", "ReferenceInput");

                entity.HasOne(d => d.FCS_EsfEligibilityRule_)
                    .WithMany(p => p.FCS_EsfEligibilityRuleEmploymentStatuses)
                    .HasForeignKey(d => d.FCS_EsfEligibilityRule_Id)
                    .HasConstraintName("FK_ReferenceInput.FCS_EsfEligibilityRuleEmploymentStatus_ReferenceInput.FCS_EsfEligibilityRule_EsfEligibilityRule_Id");
            });

            modelBuilder.Entity<FCS_EsfEligibilityRuleLocalAuthority>(entity =>
            {
                entity.ToTable("FCS_EsfEligibilityRuleLocalAuthority", "ReferenceInput");

                entity.HasOne(d => d.FCS_EsfEligibilityRule_)
                    .WithMany(p => p.FCS_EsfEligibilityRuleLocalAuthorities)
                    .HasForeignKey(d => d.FCS_EsfEligibilityRule_Id)
                    .HasConstraintName("FK_ReferenceInput.FCS_EsfEligibilityRuleLocalAuthority_ReferenceInput.FCS_EsfEligibilityRule_EsfEligibilityRule_Id");
            });

            modelBuilder.Entity<FCS_EsfEligibilityRuleLocalEnterprisePartnership>(entity =>
            {
                entity.ToTable("FCS_EsfEligibilityRuleLocalEnterprisePartnership", "ReferenceInput");

                entity.HasOne(d => d.FCS_EsfEligibilityRule_)
                    .WithMany(p => p.FCS_EsfEligibilityRuleLocalEnterprisePartnerships)
                    .HasForeignKey(d => d.FCS_EsfEligibilityRule_Id)
                    .HasConstraintName("FK_ReferenceInput.FCS_EsfEligibilityRuleLocalEnterprisePartnership_ReferenceInput.FCS_EsfEligibilityRule_EsfEligibilityRule_Id");
            });

            modelBuilder.Entity<FCS_EsfEligibilityRuleSectorSubjectAreaLevel>(entity =>
            {
                entity.ToTable("FCS_EsfEligibilityRuleSectorSubjectAreaLevel", "ReferenceInput");

                entity.Property(e => e.SectorSubjectAreaCode).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.FCS_EsfEligibilityRule_)
                    .WithMany(p => p.FCS_EsfEligibilityRuleSectorSubjectAreaLevels)
                    .HasForeignKey(d => d.FCS_EsfEligibilityRule_Id)
                    .HasConstraintName("FK_ReferenceInput.FCS_EsfEligibilityRuleSectorSubjectAreaLevel_ReferenceInput.FCS_EsfEligibilityRule_EsfEligibilityRule_Id");
            });

            modelBuilder.Entity<FCS_FcsContractAllocation>(entity =>
            {
                entity.ToTable("FCS_FcsContractAllocation", "ReferenceInput");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.LearningRatePremiumFactor).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.StopNewStartsFromDate).HasColumnType("datetime");

                entity.HasOne(d => d.EsfEligibilityRule_)
                    .WithMany(p => p.FCS_FcsContractAllocations)
                    .HasForeignKey(d => d.EsfEligibilityRule_Id)
                    .HasConstraintName("FK_ReferenceInput.FCS_FcsContractAllocation_ReferenceInput.FCS_EsfEligibilityRule_EsfEligibilityRule_Id");
            });

            modelBuilder.Entity<FCS_FcsContractDeliverable>(entity =>
            {
                entity.ToTable("FCS_FcsContractDeliverable", "ReferenceInput");

                entity.Property(e => e.PlannedValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UnitCost).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.FCS_FcsContractAllocation_)
                    .WithMany(p => p.FCS_FcsContractDeliverables)
                    .HasForeignKey(d => d.FCS_FcsContractAllocation_Id)
                    .HasConstraintName("FK_ReferenceInput.FCS_FcsContractDeliverable_ReferenceInput.FCS_FcsContractAllocation_FcsContractAllocation_Id");
            });

            modelBuilder.Entity<FRM_FrmLearner>(entity =>
            {
                entity.ToTable("FRM_FrmLearner", "ReferenceInput");

                entity.Property(e => e.LearnActEndDate).HasColumnType("datetime");

                entity.Property(e => e.LearnAimRef).HasMaxLength(2000);

                entity.Property(e => e.LearnPlanEndDate).HasColumnType("datetime");

                entity.Property(e => e.LearnRefNumber).HasMaxLength(2000);

                entity.Property(e => e.LearnStartDate).HasColumnType("datetime");

                entity.Property(e => e.LearningAimTitle).HasMaxLength(2000);

                entity.Property(e => e.OrgName).HasMaxLength(2000);

                entity.Property(e => e.PartnerOrgName).HasMaxLength(2000);

                entity.Property(e => e.PrevLearnRefNumber).HasMaxLength(2000);

                entity.Property(e => e.SWSupAimId).HasMaxLength(2000);
            });

            modelBuilder.Entity<FRM_FrmReferenceData>(entity =>
            {
                entity.ToTable("FRM_FrmReferenceData", "ReferenceInput");
            });

            modelBuilder.Entity<FRM_LearningDeliveryFAM>(entity =>
            {
                entity.ToTable("FRM_LearningDeliveryFAM", "ReferenceInput");

                entity.Property(e => e.LearnDelFAMDateFrom).HasColumnType("datetime");

                entity.Property(e => e.LearnDelFAMDateTo).HasColumnType("datetime");

                entity.HasOne(d => d.FRM_FrmLearner_)
                    .WithMany(p => p.FRM_LearningDeliveryFAMs)
                    .HasForeignKey(d => d.FRM_FrmLearner_Id)
                    .HasConstraintName("FK_ReferenceInput.FRM_LearningDeliveryFAM_ReferenceInput.FRM_FrmLearner_FrmLearner_Id");
            });

            modelBuilder.Entity<FRM_ProviderSpecDeliveryMonitoring>(entity =>
            {
                entity.ToTable("FRM_ProviderSpecDeliveryMonitoring", "ReferenceInput");

                entity.HasOne(d => d.FRM_FrmLearner_)
                    .WithMany(p => p.FRM_ProviderSpecDeliveryMonitorings)
                    .HasForeignKey(d => d.FRM_FrmLearner_Id)
                    .HasConstraintName("FK_ReferenceInput.FRM_ProviderSpecDeliveryMonitoring_ReferenceInput.FRM_FrmLearner_FrmLearner_Id");
            });

            modelBuilder.Entity<FRM_ProviderSpecLearnerMonitoring>(entity =>
            {
                entity.ToTable("FRM_ProviderSpecLearnerMonitoring", "ReferenceInput");

                entity.HasOne(d => d.FRM_FrmLearner_)
                    .WithMany(p => p.FRM_ProviderSpecLearnerMonitorings)
                    .HasForeignKey(d => d.FRM_FrmLearner_Id)
                    .HasConstraintName("FK_ReferenceInput.FRM_ProviderSpecLearnerMonitoring_ReferenceInput.FRM_FrmLearner_FrmLearner_Id");
            });

            modelBuilder.Entity<LARS_LARSAnnualValue>(entity =>
            {
                entity.ToTable("LARS_LARSAnnualValue", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.FullLevel2Percent).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FullLevel3Percent).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.LARS_LARSLearningDelivery_)
                    .WithMany(p => p.LARS_LARSAnnualValues)
                    .HasForeignKey(d => d.LARS_LARSLearningDelivery_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSAnnualValue_ReferenceInput.LARS_LARSLearningDelivery_LARSLearningDelivery_Id");
            });

            modelBuilder.Entity<LARS_LARSFrameworkAim>(entity =>
            {
                entity.ToTable("LARS_LARSFrameworkAim", "ReferenceInput");

                entity.Property(e => e.Discriminator)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.LearnAimRef).HasMaxLength(2000);
            });

            modelBuilder.Entity<LARS_LARSFrameworkApprenticeshipFunding>(entity =>
            {
                entity.ToTable("LARS_LARSFrameworkApprenticeshipFunding", "ReferenceInput");

                entity.Property(e => e.CareLeaverAdditionalPayment).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CoreGovContributionCap).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Duration).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.MaxEmployerLevyCap).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ReservedValue2).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ReservedValue3).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SixteenToEighteenEmployerAdditionalPayment).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SixteenToEighteenFrameworkUplift).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SixteenToEighteenIncentive).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SixteenToEighteenProviderAdditionalPayment).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.LARS_LARSFrameworkDesktop_)
                    .WithMany(p => p.LARS_LARSFrameworkApprenticeshipFundings)
                    .HasForeignKey(d => d.LARS_LARSFrameworkDesktop_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSFrameworkApprenticeshipFunding_ReferenceInput.LARS_LARSFrameworkDesktop_LARSFrameworkDesktop_Id");
            });

            modelBuilder.Entity<LARS_LARSFrameworkCommonComponent>(entity =>
            {
                entity.ToTable("LARS_LARSFrameworkCommonComponent", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.LARS_LARSFrameworkDesktop_)
                    .WithMany(p => p.LARS_LARSFrameworkCommonComponents)
                    .HasForeignKey(d => d.LARS_LARSFrameworkDesktop_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSFrameworkCommonComponent_ReferenceInput.LARS_LARSFrameworkDesktop_LARSFrameworkDesktop_Id");
            });

            modelBuilder.Entity<LARS_LARSFrameworkDesktop>(entity =>
            {
                entity.ToTable("LARS_LARSFrameworkDesktop", "ReferenceInput");

                entity.Property(e => e.EffectiveFromNullable).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");
            });

            modelBuilder.Entity<LARS_LARSFunding>(entity =>
            {
                entity.ToTable("LARS_LARSFunding", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.RateUnWeighted).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RateWeighted).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.LARS_LARSLearningDelivery_)
                    .WithMany(p => p.LARS_LARSFundings)
                    .HasForeignKey(d => d.LARS_LARSLearningDelivery_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSFunding_ReferenceInput.LARS_LARSLearningDelivery_LARSLearningDelivery_Id");
            });

            modelBuilder.Entity<LARS_LARSLearningDelivery>(entity =>
            {
                entity.ToTable("LARS_LARSLearningDelivery", "ReferenceInput");

                entity.Property(e => e.AwardOrgCode).HasMaxLength(2000);

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.EnglandFEHEStatus).HasMaxLength(2000);

                entity.Property(e => e.LearnAimRef).HasMaxLength(2000);

                entity.Property(e => e.LearnAimRefTitle).HasMaxLength(2000);

                entity.Property(e => e.LearnAimRefType).HasMaxLength(2000);

                entity.Property(e => e.LearnDirectClassSystemCode1).HasMaxLength(2000);

                entity.Property(e => e.LearnDirectClassSystemCode2).HasMaxLength(2000);

                entity.Property(e => e.LearnDirectClassSystemCode3).HasMaxLength(2000);

                entity.Property(e => e.LearningDeliveryGenre).HasMaxLength(2000);

                entity.Property(e => e.NotionalNVQLevel).HasMaxLength(2000);

                entity.Property(e => e.NotionalNVQLevelv2).HasMaxLength(2000);

                entity.Property(e => e.SectorSubjectAreaTier1).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SectorSubjectAreaTier2).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<LARS_LARSLearningDeliveryCategory>(entity =>
            {
                entity.ToTable("LARS_LARSLearningDeliveryCategory", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.LARS_LARSLearningDelivery_)
                    .WithMany(p => p.LARS_LARSLearningDeliveryCategories)
                    .HasForeignKey(d => d.LARS_LARSLearningDelivery_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSLearningDeliveryCategory_ReferenceInput.LARS_LARSLearningDelivery_LARSLearningDelivery_Id");
            });

            modelBuilder.Entity<LARS_LARSStandard>(entity =>
            {
                entity.ToTable("LARS_LARSStandard", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.NotionalEndLevel).HasMaxLength(2000);

                entity.Property(e => e.StandardSectorCode).HasMaxLength(2000);
            });

            modelBuilder.Entity<LARS_LARSStandardApprenticeshipFunding>(entity =>
            {
                entity.ToTable("LARS_LARSStandardApprenticeshipFunding", "ReferenceInput");

                entity.Property(e => e.CareLeaverAdditionalPayment).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CoreGovContributionCap).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Duration).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.MaxEmployerLevyCap).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ReservedValue2).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ReservedValue3).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SixteenToEighteenEmployerAdditionalPayment).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SixteenToEighteenFrameworkUplift).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SixteenToEighteenIncentive).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SixteenToEighteenProviderAdditionalPayment).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.LARS_LARSStandard_)
                    .WithMany(p => p.LARS_LARSStandardApprenticeshipFundings)
                    .HasForeignKey(d => d.LARS_LARSStandard_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSStandardApprenticeshipFunding_ReferenceInput.LARS_LARSStandard_LARSStandard_Id");
            });

            modelBuilder.Entity<LARS_LARSStandardCommonComponent>(entity =>
            {
                entity.ToTable("LARS_LARSStandardCommonComponent", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.LARS_LARSStandard_)
                    .WithMany(p => p.LARS_LARSStandardCommonComponents)
                    .HasForeignKey(d => d.LARS_LARSStandard_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSStandardCommonComponent_ReferenceInput.LARS_LARSStandard_LARSStandard_Id");
            });

            modelBuilder.Entity<LARS_LARSStandardFunding>(entity =>
            {
                entity.ToTable("LARS_LARSStandardFunding", "ReferenceInput");

                entity.Property(e => e.AchievementIncentive).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CoreGovContributionCap).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.SixteenToEighteenIncentive).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SmallBusinessIncentive).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.LARS_LARSStandard_)
                    .WithMany(p => p.LARS_LARSStandardFundings)
                    .HasForeignKey(d => d.LARS_LARSStandard_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSStandardFunding_ReferenceInput.LARS_LARSStandard_LARSStandard_Id");
            });

            modelBuilder.Entity<LARS_LARSStandardValidity>(entity =>
            {
                entity.ToTable("LARS_LARSStandardValidity", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.LastNewStartDate).HasColumnType("datetime");

                entity.HasOne(d => d.LARS_LARSStandard_)
                    .WithMany(p => p.LARS_LARSStandardValidities)
                    .HasForeignKey(d => d.LARS_LARSStandard_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSStandardValidity_ReferenceInput.LARS_LARSStandard_LARSStandard_Id");
            });

            modelBuilder.Entity<LARS_LARSValidity>(entity =>
            {
                entity.ToTable("LARS_LARSValidity", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.LastNewStartDate).HasColumnType("datetime");

                entity.HasOne(d => d.LARS_LARSLearningDelivery_)
                    .WithMany(p => p.LARS_LARSValidities)
                    .HasForeignKey(d => d.LARS_LARSLearningDelivery_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSValidity_ReferenceInput.LARS_LARSLearningDelivery_LARSLearningDelivery_Id");
            });

            modelBuilder.Entity<LARS_LARSVersion>(entity =>
            {
                entity.ToTable("LARS_LARSVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<McaContracts_McaDevolvedContract>(entity =>
            {
                entity.ToTable("McaContracts_McaDevolvedContract", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.McaGlaShortCode).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaData_CampusIdentifierVersion>(entity =>
            {
                entity.ToTable("MetaData_CampusIdentifierVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaData_CensusDate>(entity =>
            {
                entity.ToTable("MetaData_CensusDate", "ReferenceInput");

                entity.Property(e => e.End).HasColumnType("datetime");

                entity.Property(e => e.Start).HasColumnType("datetime");
            });

            modelBuilder.Entity<MetaData_CoFVersion>(entity =>
            {
                entity.ToTable("MetaData_CoFVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaData_DevolvedPostcodesVersion>(entity =>
            {
                entity.ToTable("MetaData_DevolvedPostcodesVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaData_EasUploadDateTime>(entity =>
            {
                entity.ToTable("MetaData_EasUploadDateTime", "ReferenceInput");

                entity.Property(e => e.UploadDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<MetaData_EmployersVersion>(entity =>
            {
                entity.ToTable("MetaData_EmployersVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaData_HmppPostcodesVersion>(entity =>
            {
                entity.ToTable("MetaData_HmppPostcodesVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaData_IlrCollectionDate>(entity =>
            {
                entity.ToTable("MetaData_IlrCollectionDates", "ReferenceInput");
            });

            modelBuilder.Entity<MetaData_LarsVersion>(entity =>
            {
                entity.ToTable("MetaData_LarsVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaData_Lookup>(entity =>
            {
                entity.ToTable("MetaData_Lookup", "ReferenceInput");

                entity.Property(e => e.Code).HasMaxLength(2000);

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaData_LookupSubCategory>(entity =>
            {
                entity.ToTable("MetaData_LookupSubCategory", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.MetaData_Lookup_)
                    .WithMany(p => p.MetaData_LookupSubCategories)
                    .HasForeignKey(d => d.MetaData_Lookup_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_LookupSubCategory_ReferenceInput.MetaData_Lookup_Lookup_Id");
            });

            modelBuilder.Entity<MetaData_MetaData>(entity =>
            {
                entity.ToTable("MetaData_MetaData", "ReferenceInput");

                entity.Property(e => e.DateGenerated).HasColumnType("datetime");

                entity.HasOne(d => d.CollectionDates_)
                    .WithMany(p => p.MetaData_MetaDatas)
                    .HasForeignKey(d => d.CollectionDates_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_MetaData_ReferenceInput.MetaData_IlrCollectionDates_CollectionDates_Id");

                entity.HasOne(d => d.ReferenceDataVersions_)
                    .WithMany(p => p.MetaData_MetaDatas)
                    .HasForeignKey(d => d.ReferenceDataVersions_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_MetaData_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceDataVersions_Id");
            });

            modelBuilder.Entity<MetaData_OrganisationsVersion>(entity =>
            {
                entity.ToTable("MetaData_OrganisationsVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaData_PostcodeFactorsVersion>(entity =>
            {
                entity.ToTable("MetaData_PostcodeFactorsVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaData_PostcodesVersion>(entity =>
            {
                entity.ToTable("MetaData_PostcodesVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaData_ReferenceDataVersion>(entity =>
            {
                entity.ToTable("MetaData_ReferenceDataVersion", "ReferenceInput");

                entity.HasOne(d => d.CampusIdentifierVersion_)
                    .WithMany(p => p.MetaData_ReferenceDataVersions)
                    .HasForeignKey(d => d.CampusIdentifierVersion_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_CampusIdentifierVersion_CampusIdentifierVersion_Id");

                entity.HasOne(d => d.CoFVersion_)
                    .WithMany(p => p.MetaData_ReferenceDataVersions)
                    .HasForeignKey(d => d.CoFVersion_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_CoFVersion_CoFVersion_Id");

                entity.HasOne(d => d.DevolvedPostcodesVersion_)
                    .WithMany(p => p.MetaData_ReferenceDataVersions)
                    .HasForeignKey(d => d.DevolvedPostcodesVersion_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_DevolvedPostcodesVersion_DevolvedPostcodesVersion_Id");

                entity.HasOne(d => d.EasUploadDateTime_)
                    .WithMany(p => p.MetaData_ReferenceDataVersions)
                    .HasForeignKey(d => d.EasUploadDateTime_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_EasUploadDateTime_EasUploadDateTime_Id");

                entity.HasOne(d => d.Employers_)
                    .WithMany(p => p.MetaData_ReferenceDataVersions)
                    .HasForeignKey(d => d.Employers_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_EmployersVersion_Employers_Id");

                entity.HasOne(d => d.HmppPostcodesVersion_)
                    .WithMany(p => p.MetaData_ReferenceDataVersions)
                    .HasForeignKey(d => d.HmppPostcodesVersion_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_HmppPostcodesVersion_HmppPostcodesVersion_Id");

                entity.HasOne(d => d.LarsVersion_)
                    .WithMany(p => p.MetaData_ReferenceDataVersions)
                    .HasForeignKey(d => d.LarsVersion_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_LarsVersion_LarsVersion_Id");

                entity.HasOne(d => d.OrganisationsVersion_)
                    .WithMany(p => p.MetaData_ReferenceDataVersions)
                    .HasForeignKey(d => d.OrganisationsVersion_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_OrganisationsVersion_OrganisationsVersion_Id");

                entity.HasOne(d => d.PostcodeFactorsVersion_)
                    .WithMany(p => p.MetaData_ReferenceDataVersions)
                    .HasForeignKey(d => d.PostcodeFactorsVersion_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_PostcodeFactorsVersion_PostcodeFactorsVersion_Id");

                entity.HasOne(d => d.PostcodesVersion_)
                    .WithMany(p => p.MetaData_ReferenceDataVersions)
                    .HasForeignKey(d => d.PostcodesVersion_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_PostcodesVersion_PostcodesVersion_Id");
            });

            modelBuilder.Entity<MetaData_ValidationError>(entity =>
            {
                entity.ToTable("MetaData_ValidationError", "ReferenceInput");

                entity.Property(e => e.Message).HasMaxLength(2000);

                entity.Property(e => e.RuleName).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaData_ValidationRule>(entity =>
            {
                entity.ToTable("MetaData_ValidationRule", "ReferenceInput");

                entity.Property(e => e.RuleName).HasMaxLength(2000);
            });

            modelBuilder.Entity<Organisations_Organisation>(entity =>
            {
                entity.ToTable("Organisations_Organisation", "ReferenceInput");

                entity.Property(e => e.LegalOrgType).HasMaxLength(2000);

                entity.Property(e => e.Name).HasMaxLength(2000);
            });

            modelBuilder.Entity<Organisations_OrganisationCampusIdentifier>(entity =>
            {
                entity.ToTable("Organisations_OrganisationCampusIdentifier", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.Organisations_Organisation_)
                    .WithMany(p => p.Organisations_OrganisationCampusIdentifiers)
                    .HasForeignKey(d => d.Organisations_Organisation_Id)
                    .HasConstraintName("FK_ReferenceInput.Organisations_OrganisationCampusIdentifier_ReferenceInput.Organisations_Organisation_Organisation_Id");
            });

            modelBuilder.Entity<Organisations_OrganisationCoFRemoval>(entity =>
            {
                entity.ToTable("Organisations_OrganisationCoFRemoval", "ReferenceInput");

                entity.Property(e => e.CoFRemoval).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.Organisations_Organisation_)
                    .WithMany(p => p.Organisations_OrganisationCoFRemovals)
                    .HasForeignKey(d => d.Organisations_Organisation_Id)
                    .HasConstraintName("FK_ReferenceInput.Organisations_OrganisationCoFRemoval_ReferenceInput.Organisations_Organisation_Organisation_Id");
            });

            modelBuilder.Entity<Organisations_OrganisationFunding>(entity =>
            {
                entity.ToTable("Organisations_OrganisationFunding", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.Organisations_Organisation_)
                    .WithMany(p => p.Organisations_OrganisationFundings)
                    .HasForeignKey(d => d.Organisations_Organisation_Id)
                    .HasConstraintName("FK_ReferenceInput.Organisations_OrganisationFunding_ReferenceInput.Organisations_Organisation_Organisation_Id");
            });

            modelBuilder.Entity<Organisations_OrganisationVersion>(entity =>
            {
                entity.ToTable("Organisations_OrganisationVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<Organisations_SpecialistResource>(entity =>
            {
                entity.ToTable("Organisations_SpecialistResource", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.Organisations_OrganisationCampusIdentifier_)
                    .WithMany(p => p.Organisations_SpecialistResources)
                    .HasForeignKey(d => d.Organisations_OrganisationCampusIdentifier_Id)
                    .HasConstraintName("FK_ReferenceInput.Organisations_SpecialistResource_ReferenceInput.Organisations_OrganisationCampusIdentifier_Id");
            });

            modelBuilder.Entity<PostcodesDevolution_DevolvedPostcode>(entity =>
            {
                entity.ToTable("PostcodesDevolution_DevolvedPostcodes", "ReferenceInput");
            });

            modelBuilder.Entity<PostcodesDevolution_McaGlaSofLookup>(entity =>
            {
                entity.ToTable("PostcodesDevolution_McaGlaSofLookup", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.PostcodesDevolution_DevolvedPostcodes_)
                    .WithMany(p => p.PostcodesDevolution_McaGlaSofLookups)
                    .HasForeignKey(d => d.PostcodesDevolution_DevolvedPostcodes_Id)
                    .HasConstraintName("FK_ReferenceInput.PostcodesDevolution_McaGlaSofLookup_ReferenceInput.PostcodesDevolution_DevolvedPostcodes_DevolvedPostcodes_Id");
            });

            modelBuilder.Entity<PostcodesDevolution_Postcode>(entity =>
            {
                entity.ToTable("PostcodesDevolution_Postcode", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.PostcodesDevolution_DevolvedPostcodes_)
                    .WithMany(p => p.PostcodesDevolution_Postcodes)
                    .HasForeignKey(d => d.PostcodesDevolution_DevolvedPostcodes_Id)
                    .HasConstraintName("FK_ReferenceInput.PostcodesDevolution_Postcode_ReferenceInput.PostcodesDevolution_DevolvedPostcodes_DevolvedPostcodes_Id");
            });

            modelBuilder.Entity<Postcodes_DasDisadvantage>(entity =>
            {
                entity.ToTable("Postcodes_DasDisadvantage", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.Uplift).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Postcodes_Postcode_)
                    .WithMany(p => p.Postcodes_DasDisadvantages)
                    .HasForeignKey(d => d.Postcodes_Postcode_Id)
                    .HasConstraintName("FK_ReferenceInput.Postcodes_DasDisadvantage_ReferenceInput.Postcodes_Postcode_Postcode_Id");
            });

            modelBuilder.Entity<Postcodes_EfaDisadvantage>(entity =>
            {
                entity.ToTable("Postcodes_EfaDisadvantage", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.Uplift).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Postcodes_Postcode_)
                    .WithMany(p => p.Postcodes_EfaDisadvantages)
                    .HasForeignKey(d => d.Postcodes_Postcode_Id)
                    .HasConstraintName("FK_ReferenceInput.Postcodes_EfaDisadvantage_ReferenceInput.Postcodes_Postcode_Postcode_Id");
            });

            modelBuilder.Entity<Postcodes_McaglaSOF>(entity =>
            {
                entity.ToTable("Postcodes_McaglaSOF", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.Postcodes_Postcode_)
                    .WithMany(p => p.Postcodes_McaglaSOFs)
                    .HasForeignKey(d => d.Postcodes_Postcode_Id)
                    .HasConstraintName("FK_ReferenceInput.Postcodes_McaglaSOF_ReferenceInput.Postcodes_Postcode_Postcode_Id");
            });

            modelBuilder.Entity<Postcodes_ONSData>(entity =>
            {
                entity.ToTable("Postcodes_ONSData", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.Termination).HasColumnType("datetime");

                entity.HasOne(d => d.Postcodes_Postcode_)
                    .WithMany(p => p.Postcodes_ONSDatas)
                    .HasForeignKey(d => d.Postcodes_Postcode_Id)
                    .HasConstraintName("FK_ReferenceInput.Postcodes_ONSData_ReferenceInput.Postcodes_Postcode_Postcode_Id");
            });

            modelBuilder.Entity<Postcodes_PostCodeVersion>(entity =>
            {
                entity.ToTable("Postcodes_PostCodeVersion", "ReferenceInput");

                entity.Property(e => e.PostcodeCurrentVersion).HasMaxLength(2000);
            });

            modelBuilder.Entity<Postcodes_Postcode>(entity =>
            {
                entity.ToTable("Postcodes_Postcode", "ReferenceInput");

                entity.Property(e => e.PostCode).HasMaxLength(2000);
            });

            modelBuilder.Entity<Postcodes_SfaAreaCost>(entity =>
            {
                entity.ToTable("Postcodes_SfaAreaCost", "ReferenceInput");

                entity.Property(e => e.AreaCostFactor).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.Postcodes_Postcode_)
                    .WithMany(p => p.Postcodes_SfaAreaCosts)
                    .HasForeignKey(d => d.Postcodes_Postcode_Id)
                    .HasConstraintName("FK_ReferenceInput.Postcodes_SfaAreaCost_ReferenceInput.Postcodes_Postcode_Postcode_Id");
            });

            modelBuilder.Entity<Postcodes_SfaDisadvantage>(entity =>
            {
                entity.ToTable("Postcodes_SfaDisadvantage", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.Uplift).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Postcodes_Postcode_)
                    .WithMany(p => p.Postcodes_SfaDisadvantages)
                    .HasForeignKey(d => d.Postcodes_Postcode_Id)
                    .HasConstraintName("FK_ReferenceInput.Postcodes_SfaDisadvantage_ReferenceInput.Postcodes_Postcode_Postcode_Id");
            });

            modelBuilder.Entity<ULN>(entity =>
            {
                entity.ToTable("ULNs", "ReferenceInput");
            });
        }
    }
}
