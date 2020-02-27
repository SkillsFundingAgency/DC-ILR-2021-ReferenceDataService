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

        public virtual DbSet<AppsEarningHistoryApprenticeshipEarningsHistory> AppsEarningHistoryApprenticeshipEarningsHistories { get; set; }
        public virtual DbSet<EmployersEmployer> EmployersEmployers { get; set; }
        public virtual DbSet<EmployersLargeEmployerEffectiveDate> EmployersLargeEmployerEffectiveDates { get; set; }
        public virtual DbSet<EmployersLargeEmployerVersion> EmployersLargeEmployerVersions { get; set; }
        public virtual DbSet<EpaorganisationsEpaorganisation> EpaorganisationsEpaorganisations { get; set; }
        public virtual DbSet<FcsEsfEligibilityRule> FcsEsfEligibilityRules { get; set; }
        public virtual DbSet<FcsEsfEligibilityRuleEmploymentStatus> FcsEsfEligibilityRuleEmploymentStatuses { get; set; }
        public virtual DbSet<FcsEsfEligibilityRuleLocalAuthority> FcsEsfEligibilityRuleLocalAuthorities { get; set; }
        public virtual DbSet<FcsEsfEligibilityRuleLocalEnterprisePartnership> FcsEsfEligibilityRuleLocalEnterprisePartnerships { get; set; }
        public virtual DbSet<FcsEsfEligibilityRuleSectorSubjectAreaLevel> FcsEsfEligibilityRuleSectorSubjectAreaLevels { get; set; }
        public virtual DbSet<FcsFcsContractAllocation> FcsFcsContractAllocations { get; set; }
        public virtual DbSet<FcsFcsContractDeliverable> FcsFcsContractDeliverables { get; set; }
        public virtual DbSet<FrmFrmLearner> FrmFrmLearners { get; set; }
        public virtual DbSet<FrmFrmReferenceData> FrmFrmReferenceDatas { get; set; }
        public virtual DbSet<FrmLearningDeliveryFam> FrmLearningDeliveryFams { get; set; }
        public virtual DbSet<FrmProviderSpecDeliveryMonitoring> FrmProviderSpecDeliveryMonitorings { get; set; }
        public virtual DbSet<FrmProviderSpecLearnerMonitoring> FrmProviderSpecLearnerMonitorings { get; set; }
        public virtual DbSet<LarsLarsannualValue> LarsLarsannualValues { get; set; }
        public virtual DbSet<LarsLarsframework> LarsLarsframeworks { get; set; }
        public virtual DbSet<LarsLarsframeworkAim> LarsLarsframeworkAims { get; set; }
        public virtual DbSet<LarsLarsframeworkApprenticeshipFunding> LarsLarsframeworkApprenticeshipFundings { get; set; }
        public virtual DbSet<LarsLarsframeworkCommonComponent> LarsLarsframeworkCommonComponents { get; set; }
        public virtual DbSet<LarsLarsframeworkDesktop> LarsLarsframeworkDesktops { get; set; }
        public virtual DbSet<LarsLarsfunding> LarsLarsfundings { get; set; }
        public virtual DbSet<LarsLarslearningDelivery> LarsLarslearningDeliveries { get; set; }
        public virtual DbSet<LarsLarslearningDeliveryCategory> LarsLarslearningDeliveryCategories { get; set; }
        public virtual DbSet<LarsLarsstandard> LarsLarsstandards { get; set; }
        public virtual DbSet<LarsLarsstandardApprenticeshipFunding> LarsLarsstandardApprenticeshipFundings { get; set; }
        public virtual DbSet<LarsLarsstandardCommonComponent> LarsLarsstandardCommonComponents { get; set; }
        public virtual DbSet<LarsLarsstandardFunding> LarsLarsstandardFundings { get; set; }
        public virtual DbSet<LarsLarsstandardValidity> LarsLarsstandardValidities { get; set; }
        public virtual DbSet<LarsLarsvalidity> LarsLarsvalidities { get; set; }
        public virtual DbSet<LarsLarsversion> LarsLarsversions { get; set; }
        public virtual DbSet<McaContractsMcaDevolvedContract> McaContractsMcaDevolvedContracts { get; set; }
        public virtual DbSet<MetaDataCampusIdentifierVersion> MetaDataCampusIdentifierVersions { get; set; }
        public virtual DbSet<MetaDataCensusDate> MetaDataCensusDates { get; set; }
        public virtual DbSet<MetaDataCoFversion> MetaDataCoFversions { get; set; }
        public virtual DbSet<MetaDataDevolvedPostcodesVersion> MetaDataDevolvedPostcodesVersions { get; set; }
        public virtual DbSet<MetaDataEasUploadDateTime> MetaDataEasUploadDateTimes { get; set; }
        public virtual DbSet<MetaDataEmployersVersion> MetaDataEmployersVersions { get; set; }
        public virtual DbSet<MetaDataHmppPostcodesVersion> MetaDataHmppPostcodesVersions { get; set; }
        public virtual DbSet<MetaDataIlrCollectionDate> MetaDataIlrCollectionDates { get; set; }
        public virtual DbSet<MetaDataLarsVersion> MetaDataLarsVersions { get; set; }
        public virtual DbSet<MetaDataLookup> MetaDataLookups { get; set; }
        public virtual DbSet<MetaDataLookupSubCategory> MetaDataLookupSubCategories { get; set; }
        public virtual DbSet<MetaDataMetaData> MetaDataMetaDatas { get; set; }
        public virtual DbSet<MetaDataOrganisationsVersion> MetaDataOrganisationsVersions { get; set; }
        public virtual DbSet<MetaDataPostcodeFactorsVersion> MetaDataPostcodeFactorsVersions { get; set; }
        public virtual DbSet<MetaDataPostcodesVersion> MetaDataPostcodesVersions { get; set; }
        public virtual DbSet<MetaDataReferenceDataVersion> MetaDataReferenceDataVersions { get; set; }
        public virtual DbSet<MetaDataValidationError> MetaDataValidationErrors { get; set; }
        public virtual DbSet<MetaDataValidationRule> MetaDataValidationRules { get; set; }
        public virtual DbSet<OrganisationsOrganisation> OrganisationsOrganisations { get; set; }
        public virtual DbSet<OrganisationsOrganisationCampusIdentifier> OrganisationsOrganisationCampusIdentifiers { get; set; }
        public virtual DbSet<OrganisationsOrganisationCoFremoval> OrganisationsOrganisationCoFremovals { get; set; }
        public virtual DbSet<OrganisationsOrganisationFunding> OrganisationsOrganisationFundings { get; set; }
        public virtual DbSet<OrganisationsOrganisationVersion> OrganisationsOrganisationVersions { get; set; }
        public virtual DbSet<OrganisationsSpecialistResource> OrganisationsSpecialistResources { get; set; }
        public virtual DbSet<PostcodesDasDisadvantage> PostcodesDasDisadvantages { get; set; }
        public virtual DbSet<PostcodesDevolutionDevolvedPostcode> PostcodesDevolutionDevolvedPostcodes { get; set; }
        public virtual DbSet<PostcodesDevolutionMcaGlaSofLookup> PostcodesDevolutionMcaGlaSofLookups { get; set; }
        public virtual DbSet<PostcodesDevolutionPostcode> PostcodesDevolutionPostcodes { get; set; }
        public virtual DbSet<PostcodesEfaDisadvantage> PostcodesEfaDisadvantages { get; set; }
        public virtual DbSet<PostcodesMcaglaSof> PostcodesMcaglaSofs { get; set; }
        public virtual DbSet<PostcodesOnsdata> PostcodesOnsdatas { get; set; }
        public virtual DbSet<PostcodesPostCodeVersion> PostcodesPostCodeVersions { get; set; }
        public virtual DbSet<PostcodesPostcode> PostcodesPostcodes { get; set; }
        public virtual DbSet<PostcodesSfaAreaCost> PostcodesSfaAreaCosts { get; set; }
        public virtual DbSet<PostcodesSfaDisadvantage> PostcodesSfaDisadvantages { get; set; }
        public virtual DbSet<Uln> Ulns { get; set; }

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

            modelBuilder.Entity<AppsEarningHistoryApprenticeshipEarningsHistory>(entity =>
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

            modelBuilder.Entity<EmployersEmployer>(entity =>
            {
                entity.ToTable("Employers_Employer", "ReferenceInput");
            });

            modelBuilder.Entity<EmployersLargeEmployerEffectiveDate>(entity =>
            {
                entity.ToTable("Employers_LargeEmployerEffectiveDates", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.Employers_Employer_)
                    .WithMany(p => p.EmployersLargeEmployerEffectiveDates)
                    .HasForeignKey(d => d.Employers_Employer_Id)
                    .HasConstraintName("FK_ReferenceInput.Employers_LargeEmployerEffectiveDates_ReferenceInput.Employers_Employer_Employer_Id");
            });

            modelBuilder.Entity<EmployersLargeEmployerVersion>(entity =>
            {
                entity.ToTable("Employers_LargeEmployerVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<EpaorganisationsEpaorganisation>(entity =>
            {
                entity.ToTable("EPAOrganisations_EPAOrganisation", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.Standard).HasMaxLength(2000);
            });

            modelBuilder.Entity<FcsEsfEligibilityRule>(entity =>
            {
                entity.ToTable("FCS_EsfEligibilityRule", "ReferenceInput");

                entity.Property(e => e.LotReference).HasMaxLength(2000);

                entity.Property(e => e.MaxPriorAttainment).HasMaxLength(2000);

                entity.Property(e => e.MinPriorAttainment).HasMaxLength(2000);

                entity.Property(e => e.TenderSpecReference).HasMaxLength(2000);
            });

            modelBuilder.Entity<FcsEsfEligibilityRuleEmploymentStatus>(entity =>
            {
                entity.ToTable("FCS_EsfEligibilityRuleEmploymentStatus", "ReferenceInput");

                entity.HasOne(d => d.FCS_EsfEligibilityRule_)
                    .WithMany(p => p.FcsEsfEligibilityRuleEmploymentStatuses)
                    .HasForeignKey(d => d.FCS_EsfEligibilityRule_Id)
                    .HasConstraintName("FK_ReferenceInput.FCS_EsfEligibilityRuleEmploymentStatus_ReferenceInput.FCS_EsfEligibilityRule_EsfEligibilityRule_Id");
            });

            modelBuilder.Entity<FcsEsfEligibilityRuleLocalAuthority>(entity =>
            {
                entity.ToTable("FCS_EsfEligibilityRuleLocalAuthority", "ReferenceInput");

                entity.HasOne(d => d.FCS_EsfEligibilityRule_)
                    .WithMany(p => p.FcsEsfEligibilityRuleLocalAuthorities)
                    .HasForeignKey(d => d.FCS_EsfEligibilityRule_Id)
                    .HasConstraintName("FK_ReferenceInput.FCS_EsfEligibilityRuleLocalAuthority_ReferenceInput.FCS_EsfEligibilityRule_EsfEligibilityRule_Id");
            });

            modelBuilder.Entity<FcsEsfEligibilityRuleLocalEnterprisePartnership>(entity =>
            {
                entity.ToTable("FCS_EsfEligibilityRuleLocalEnterprisePartnership", "ReferenceInput");

                entity.HasOne(d => d.FCS_EsfEligibilityRule_)
                    .WithMany(p => p.FcsEsfEligibilityRuleLocalEnterprisePartnerships)
                    .HasForeignKey(d => d.FCS_EsfEligibilityRule_Id)
                    .HasConstraintName("FK_ReferenceInput.FCS_EsfEligibilityRuleLocalEnterprisePartnership_ReferenceInput.FCS_EsfEligibilityRule_EsfEligibilityRule_Id");
            });

            modelBuilder.Entity<FcsEsfEligibilityRuleSectorSubjectAreaLevel>(entity =>
            {
                entity.ToTable("FCS_EsfEligibilityRuleSectorSubjectAreaLevel", "ReferenceInput");

                entity.Property(e => e.SectorSubjectAreaCode).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.FCS_EsfEligibilityRule_)
                    .WithMany(p => p.FcsEsfEligibilityRuleSectorSubjectAreaLevels)
                    .HasForeignKey(d => d.FCS_EsfEligibilityRule_Id)
                    .HasConstraintName("FK_ReferenceInput.FCS_EsfEligibilityRuleSectorSubjectAreaLevel_ReferenceInput.FCS_EsfEligibilityRule_EsfEligibilityRule_Id");
            });

            modelBuilder.Entity<FcsFcsContractAllocation>(entity =>
            {
                entity.ToTable("FCS_FcsContractAllocation", "ReferenceInput");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.LearningRatePremiumFactor).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.StopNewStartsFromDate).HasColumnType("datetime");

                entity.HasOne(d => d.EsfEligibilityRule_)
                    .WithMany(p => p.FcsFcsContractAllocations)
                    .HasForeignKey(d => d.EsfEligibilityRule_Id)
                    .HasConstraintName("FK_ReferenceInput.FCS_FcsContractAllocation_ReferenceInput.FCS_EsfEligibilityRule_EsfEligibilityRule_Id");
            });

            modelBuilder.Entity<FcsFcsContractDeliverable>(entity =>
            {
                entity.ToTable("FCS_FcsContractDeliverable", "ReferenceInput");

                entity.Property(e => e.PlannedValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UnitCost).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.FCS_FcsContractAllocation_)
                    .WithMany(p => p.FcsFcsContractDeliverables)
                    .HasForeignKey(d => d.FCS_FcsContractAllocation_Id)
                    .HasConstraintName("FK_ReferenceInput.FCS_FcsContractDeliverable_ReferenceInput.FCS_FcsContractAllocation_FcsContractAllocation_Id");
            });

            modelBuilder.Entity<FrmFrmLearner>(entity =>
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

            modelBuilder.Entity<FrmFrmReferenceData>(entity =>
            {
                entity.ToTable("FRM_FrmReferenceData", "ReferenceInput");
            });

            modelBuilder.Entity<FrmLearningDeliveryFam>(entity =>
            {
                entity.ToTable("FRM_LearningDeliveryFAM", "ReferenceInput");

                entity.Property(e => e.LearnDelFAMDateFrom).HasColumnType("datetime");

                entity.Property(e => e.LearnDelFAMDateTo).HasColumnType("datetime");

                entity.HasOne(d => d.FRM_FrmLearner_)
                    .WithMany(p => p.FrmLearningDeliveryFams)
                    .HasForeignKey(d => d.FRM_FrmLearner_Id)
                    .HasConstraintName("FK_ReferenceInput.FRM_LearningDeliveryFAM_ReferenceInput.FRM_FrmLearner_FrmLearner_Id");
            });

            modelBuilder.Entity<FrmProviderSpecDeliveryMonitoring>(entity =>
            {
                entity.ToTable("FRM_ProviderSpecDeliveryMonitoring", "ReferenceInput");

                entity.HasOne(d => d.FRM_FrmLearner_)
                    .WithMany(p => p.FrmProviderSpecDeliveryMonitorings)
                    .HasForeignKey(d => d.FRM_FrmLearner_Id)
                    .HasConstraintName("FK_ReferenceInput.FRM_ProviderSpecDeliveryMonitoring_ReferenceInput.FRM_FrmLearner_FrmLearner_Id");
            });

            modelBuilder.Entity<FrmProviderSpecLearnerMonitoring>(entity =>
            {
                entity.ToTable("FRM_ProviderSpecLearnerMonitoring", "ReferenceInput");

                entity.HasOne(d => d.FRM_FrmLearner_)
                    .WithMany(p => p.FrmProviderSpecLearnerMonitorings)
                    .HasForeignKey(d => d.FRM_FrmLearner_Id)
                    .HasConstraintName("FK_ReferenceInput.FRM_ProviderSpecLearnerMonitoring_ReferenceInput.FRM_FrmLearner_FrmLearner_Id");
            });

            modelBuilder.Entity<LarsLarsannualValue>(entity =>
            {
                entity.ToTable("LARS_LARSAnnualValue", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.FullLevel2Percent).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FullLevel3Percent).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.LARS_LARSLearningDelivery_)
                    .WithMany(p => p.LarsLarsannualValues)
                    .HasForeignKey(d => d.LARS_LARSLearningDelivery_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSAnnualValue_ReferenceInput.LARS_LARSLearningDelivery_LARSLearningDelivery_Id");
            });

            modelBuilder.Entity<LarsLarsframework>(entity =>
            {
                entity.ToTable("LARS_LARSFramework", "ReferenceInput");

                entity.Property(e => e.EffectiveFromNullable).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.LARSFrameworkAim_)
                    .WithMany(p => p.LarsLarsframeworks)
                    .HasForeignKey(d => d.LARSFrameworkAim_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSFramework_ReferenceInput.LARS_LARSFrameworkAim_LARSFrameworkAim_Id");

                entity.HasOne(d => d.LARS_LARSLearningDelivery_)
                    .WithMany(p => p.LarsLarsframeworks)
                    .HasForeignKey(d => d.LARS_LARSLearningDelivery_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSFramework_ReferenceInput.LARS_LARSLearningDelivery_LARSLearningDelivery_Id");
            });

            modelBuilder.Entity<LarsLarsframeworkAim>(entity =>
            {
                entity.ToTable("LARS_LARSFrameworkAim", "ReferenceInput");

                entity.Property(e => e.Discriminator)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.LearnAimRef).HasMaxLength(2000);
            });

            modelBuilder.Entity<LarsLarsframeworkApprenticeshipFunding>(entity =>
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
                    .WithMany(p => p.LarsLarsframeworkApprenticeshipFundings)
                    .HasForeignKey(d => d.LARS_LARSFrameworkDesktop_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSFrameworkApprenticeshipFunding_ReferenceInput.LARS_LARSFrameworkDesktop_LARSFrameworkDesktop_Id");

                entity.HasOne(d => d.LARS_LARSFramework_)
                    .WithMany(p => p.LarsLarsframeworkApprenticeshipFundings)
                    .HasForeignKey(d => d.LARS_LARSFramework_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSFrameworkApprenticeshipFunding_ReferenceInput.LARS_LARSFramework_LARSFramework_Id");
            });

            modelBuilder.Entity<LarsLarsframeworkCommonComponent>(entity =>
            {
                entity.ToTable("LARS_LARSFrameworkCommonComponent", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.LARS_LARSFrameworkDesktop_)
                    .WithMany(p => p.LarsLarsframeworkCommonComponents)
                    .HasForeignKey(d => d.LARS_LARSFrameworkDesktop_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSFrameworkCommonComponent_ReferenceInput.LARS_LARSFrameworkDesktop_LARSFrameworkDesktop_Id");

                entity.HasOne(d => d.LARS_LARSFramework_)
                    .WithMany(p => p.LarsLarsframeworkCommonComponents)
                    .HasForeignKey(d => d.LARS_LARSFramework_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSFrameworkCommonComponent_ReferenceInput.LARS_LARSFramework_LARSFramework_Id");
            });

            modelBuilder.Entity<LarsLarsframeworkDesktop>(entity =>
            {
                entity.ToTable("LARS_LARSFrameworkDesktop", "ReferenceInput");

                entity.Property(e => e.EffectiveFromNullable).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");
            });

            modelBuilder.Entity<LarsLarsfunding>(entity =>
            {
                entity.ToTable("LARS_LARSFunding", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.RateUnWeighted).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RateWeighted).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.LARS_LARSLearningDelivery_)
                    .WithMany(p => p.LarsLarsfundings)
                    .HasForeignKey(d => d.LARS_LARSLearningDelivery_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSFunding_ReferenceInput.LARS_LARSLearningDelivery_LARSLearningDelivery_Id");
            });

            modelBuilder.Entity<LarsLarslearningDelivery>(entity =>
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

            modelBuilder.Entity<LarsLarslearningDeliveryCategory>(entity =>
            {
                entity.ToTable("LARS_LARSLearningDeliveryCategory", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.LARS_LARSLearningDelivery_)
                    .WithMany(p => p.LarsLarslearningDeliveryCategories)
                    .HasForeignKey(d => d.LARS_LARSLearningDelivery_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSLearningDeliveryCategory_ReferenceInput.LARS_LARSLearningDelivery_LARSLearningDelivery_Id");
            });

            modelBuilder.Entity<LarsLarsstandard>(entity =>
            {
                entity.ToTable("LARS_LARSStandard", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.NotionalEndLevel).HasMaxLength(2000);

                entity.Property(e => e.StandardSectorCode).HasMaxLength(2000);
            });

            modelBuilder.Entity<LarsLarsstandardApprenticeshipFunding>(entity =>
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
                    .WithMany(p => p.LarsLarsstandardApprenticeshipFundings)
                    .HasForeignKey(d => d.LARS_LARSStandard_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSStandardApprenticeshipFunding_ReferenceInput.LARS_LARSStandard_LARSStandard_Id");
            });

            modelBuilder.Entity<LarsLarsstandardCommonComponent>(entity =>
            {
                entity.ToTable("LARS_LARSStandardCommonComponent", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.LARS_LARSStandard_)
                    .WithMany(p => p.LarsLarsstandardCommonComponents)
                    .HasForeignKey(d => d.LARS_LARSStandard_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSStandardCommonComponent_ReferenceInput.LARS_LARSStandard_LARSStandard_Id");
            });

            modelBuilder.Entity<LarsLarsstandardFunding>(entity =>
            {
                entity.ToTable("LARS_LARSStandardFunding", "ReferenceInput");

                entity.Property(e => e.AchievementIncentive).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CoreGovContributionCap).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.SixteenToEighteenIncentive).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SmallBusinessIncentive).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.LARS_LARSStandard_)
                    .WithMany(p => p.LarsLarsstandardFundings)
                    .HasForeignKey(d => d.LARS_LARSStandard_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSStandardFunding_ReferenceInput.LARS_LARSStandard_LARSStandard_Id");
            });

            modelBuilder.Entity<LarsLarsstandardValidity>(entity =>
            {
                entity.ToTable("LARS_LARSStandardValidity", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.LastNewStartDate).HasColumnType("datetime");

                entity.HasOne(d => d.LARS_LARSStandard_)
                    .WithMany(p => p.LarsLarsstandardValidities)
                    .HasForeignKey(d => d.LARS_LARSStandard_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSStandardValidity_ReferenceInput.LARS_LARSStandard_LARSStandard_Id");
            });

            modelBuilder.Entity<LarsLarsvalidity>(entity =>
            {
                entity.ToTable("LARS_LARSValidity", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.LastNewStartDate).HasColumnType("datetime");

                entity.HasOne(d => d.LARS_LARSLearningDelivery_)
                    .WithMany(p => p.LarsLarsvalidities)
                    .HasForeignKey(d => d.LARS_LARSLearningDelivery_Id)
                    .HasConstraintName("FK_ReferenceInput.LARS_LARSValidity_ReferenceInput.LARS_LARSLearningDelivery_LARSLearningDelivery_Id");
            });

            modelBuilder.Entity<LarsLarsversion>(entity =>
            {
                entity.ToTable("LARS_LARSVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<McaContractsMcaDevolvedContract>(entity =>
            {
                entity.ToTable("McaContracts_McaDevolvedContract", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.McaGlaShortCode).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaDataCampusIdentifierVersion>(entity =>
            {
                entity.ToTable("MetaData_CampusIdentifierVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaDataCensusDate>(entity =>
            {
                entity.ToTable("MetaData_CensusDate", "ReferenceInput");

                entity.Property(e => e.End).HasColumnType("datetime");

                entity.Property(e => e.Start).HasColumnType("datetime");
            });

            modelBuilder.Entity<MetaDataCoFversion>(entity =>
            {
                entity.ToTable("MetaData_CoFVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaDataDevolvedPostcodesVersion>(entity =>
            {
                entity.ToTable("MetaData_DevolvedPostcodesVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaDataEasUploadDateTime>(entity =>
            {
                entity.ToTable("MetaData_EasUploadDateTime", "ReferenceInput");

                entity.Property(e => e.UploadDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<MetaDataEmployersVersion>(entity =>
            {
                entity.ToTable("MetaData_EmployersVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaDataHmppPostcodesVersion>(entity =>
            {
                entity.ToTable("MetaData_HmppPostcodesVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaDataIlrCollectionDate>(entity =>
            {
                entity.ToTable("MetaData_IlrCollectionDates", "ReferenceInput");
            });

            modelBuilder.Entity<MetaDataLarsVersion>(entity =>
            {
                entity.ToTable("MetaData_LarsVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaDataLookup>(entity =>
            {
                entity.ToTable("MetaData_Lookup", "ReferenceInput");

                entity.Property(e => e.Code).HasMaxLength(2000);

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaDataLookupSubCategory>(entity =>
            {
                entity.ToTable("MetaData_LookupSubCategory", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.MetaData_Lookup_)
                    .WithMany(p => p.MetaDataLookupSubCategories)
                    .HasForeignKey(d => d.MetaData_Lookup_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_LookupSubCategory_ReferenceInput.MetaData_Lookup_Lookup_Id");
            });

            modelBuilder.Entity<MetaDataMetaData>(entity =>
            {
                entity.ToTable("MetaData_MetaData", "ReferenceInput");

                entity.Property(e => e.DateGenerated).HasColumnType("datetime");

                entity.HasOne(d => d.CollectionDates_)
                    .WithMany(p => p.MetaDataMetaDatas)
                    .HasForeignKey(d => d.CollectionDates_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_MetaData_ReferenceInput.MetaData_IlrCollectionDates_CollectionDates_Id");

                entity.HasOne(d => d.ReferenceDataVersions_)
                    .WithMany(p => p.MetaDataMetaDatas)
                    .HasForeignKey(d => d.ReferenceDataVersions_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_MetaData_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceDataVersions_Id");
            });

            modelBuilder.Entity<MetaDataOrganisationsVersion>(entity =>
            {
                entity.ToTable("MetaData_OrganisationsVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaDataPostcodeFactorsVersion>(entity =>
            {
                entity.ToTable("MetaData_PostcodeFactorsVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaDataPostcodesVersion>(entity =>
            {
                entity.ToTable("MetaData_PostcodesVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaDataReferenceDataVersion>(entity =>
            {
                entity.ToTable("MetaData_ReferenceDataVersion", "ReferenceInput");

                entity.HasOne(d => d.CampusIdentifierVersion_)
                    .WithMany(p => p.MetaDataReferenceDataVersions)
                    .HasForeignKey(d => d.CampusIdentifierVersion_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_CampusIdentifierVersion_CampusIdentifierVersion_Id");

                entity.HasOne(d => d.CoFVersion_)
                    .WithMany(p => p.MetaDataReferenceDataVersions)
                    .HasForeignKey(d => d.CoFVersion_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_CoFVersion_CoFVersion_Id");

                entity.HasOne(d => d.DevolvedPostcodesVersion_)
                    .WithMany(p => p.MetaDataReferenceDataVersions)
                    .HasForeignKey(d => d.DevolvedPostcodesVersion_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_DevolvedPostcodesVersion_DevolvedPostcodesVersion_Id");

                entity.HasOne(d => d.EasUploadDateTime_)
                    .WithMany(p => p.MetaDataReferenceDataVersions)
                    .HasForeignKey(d => d.EasUploadDateTime_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_EasUploadDateTime_EasUploadDateTime_Id");

                entity.HasOne(d => d.Employers_)
                    .WithMany(p => p.MetaDataReferenceDataVersions)
                    .HasForeignKey(d => d.Employers_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_EmployersVersion_Employers_Id");

                entity.HasOne(d => d.HmppPostcodesVersion_)
                    .WithMany(p => p.MetaDataReferenceDataVersions)
                    .HasForeignKey(d => d.HmppPostcodesVersion_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_HmppPostcodesVersion_HmppPostcodesVersion_Id");

                entity.HasOne(d => d.LarsVersion_)
                    .WithMany(p => p.MetaDataReferenceDataVersions)
                    .HasForeignKey(d => d.LarsVersion_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_LarsVersion_LarsVersion_Id");

                entity.HasOne(d => d.OrganisationsVersion_)
                    .WithMany(p => p.MetaDataReferenceDataVersions)
                    .HasForeignKey(d => d.OrganisationsVersion_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_OrganisationsVersion_OrganisationsVersion_Id");

                entity.HasOne(d => d.PostcodeFactorsVersion_)
                    .WithMany(p => p.MetaDataReferenceDataVersions)
                    .HasForeignKey(d => d.PostcodeFactorsVersion_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_PostcodeFactorsVersion_PostcodeFactorsVersion_Id");

                entity.HasOne(d => d.PostcodesVersion_)
                    .WithMany(p => p.MetaDataReferenceDataVersions)
                    .HasForeignKey(d => d.PostcodesVersion_Id)
                    .HasConstraintName("FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_PostcodesVersion_PostcodesVersion_Id");
            });

            modelBuilder.Entity<MetaDataValidationError>(entity =>
            {
                entity.ToTable("MetaData_ValidationError", "ReferenceInput");

                entity.Property(e => e.Message).HasMaxLength(2000);

                entity.Property(e => e.RuleName).HasMaxLength(2000);
            });

            modelBuilder.Entity<MetaDataValidationRule>(entity =>
            {
                entity.ToTable("MetaData_ValidationRule", "ReferenceInput");

                entity.Property(e => e.RuleName).HasMaxLength(2000);
            });

            modelBuilder.Entity<OrganisationsOrganisation>(entity =>
            {
                entity.ToTable("Organisations_Organisation", "ReferenceInput");

                entity.Property(e => e.LegalOrgType).HasMaxLength(2000);

                entity.Property(e => e.Name).HasMaxLength(2000);
            });

            modelBuilder.Entity<OrganisationsOrganisationCampusIdentifier>(entity =>
            {
                entity.ToTable("Organisations_OrganisationCampusIdentifier", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.Organisations_Organisation_)
                    .WithMany(p => p.OrganisationsOrganisationCampusIdentifiers)
                    .HasForeignKey(d => d.Organisations_Organisation_Id)
                    .HasConstraintName("FK_ReferenceInput.Organisations_OrganisationCampusIdentifier_ReferenceInput.Organisations_Organisation_Organisation_Id");
            });

            modelBuilder.Entity<OrganisationsOrganisationCoFremoval>(entity =>
            {
                entity.ToTable("Organisations_OrganisationCoFRemoval", "ReferenceInput");

                entity.Property(e => e.CoFRemoval).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.Organisations_Organisation_)
                    .WithMany(p => p.OrganisationsOrganisationCoFremovals)
                    .HasForeignKey(d => d.Organisations_Organisation_Id)
                    .HasConstraintName("FK_ReferenceInput.Organisations_OrganisationCoFRemoval_ReferenceInput.Organisations_Organisation_Organisation_Id");
            });

            modelBuilder.Entity<OrganisationsOrganisationFunding>(entity =>
            {
                entity.ToTable("Organisations_OrganisationFunding", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.Organisations_Organisation_)
                    .WithMany(p => p.OrganisationsOrganisationFundings)
                    .HasForeignKey(d => d.Organisations_Organisation_Id)
                    .HasConstraintName("FK_ReferenceInput.Organisations_OrganisationFunding_ReferenceInput.Organisations_Organisation_Organisation_Id");
            });

            modelBuilder.Entity<OrganisationsOrganisationVersion>(entity =>
            {
                entity.ToTable("Organisations_OrganisationVersion", "ReferenceInput");

                entity.Property(e => e.Version).HasMaxLength(2000);
            });

            modelBuilder.Entity<OrganisationsSpecialistResource>(entity =>
            {
                entity.ToTable("Organisations_SpecialistResource", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.Organisations_OrganisationCampusIdentifier_)
                    .WithMany(p => p.OrganisationsSpecialistResources)
                    .HasForeignKey(d => d.Organisations_OrganisationCampusIdentifier_Id)
                    .HasConstraintName("FK_ReferenceInput.Organisations_SpecialistResource_ReferenceInput.Organisations_OrganisationCampusIdentifier_Id");
            });

            modelBuilder.Entity<PostcodesDasDisadvantage>(entity =>
            {
                entity.ToTable("Postcodes_DasDisadvantage", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.Uplift).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Postcodes_Postcode_)
                    .WithMany(p => p.PostcodesDasDisadvantages)
                    .HasForeignKey(d => d.Postcodes_Postcode_Id)
                    .HasConstraintName("FK_ReferenceInput.Postcodes_DasDisadvantage_ReferenceInput.Postcodes_Postcode_Postcode_Id");
            });

            modelBuilder.Entity<PostcodesDevolutionDevolvedPostcode>(entity =>
            {
                entity.ToTable("PostcodesDevolution_DevolvedPostcodes", "ReferenceInput");
            });

            modelBuilder.Entity<PostcodesDevolutionMcaGlaSofLookup>(entity =>
            {
                entity.ToTable("PostcodesDevolution_McaGlaSofLookup", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.PostcodesDevolution_DevolvedPostcodes_)
                    .WithMany(p => p.PostcodesDevolutionMcaGlaSofLookups)
                    .HasForeignKey(d => d.PostcodesDevolution_DevolvedPostcodes_Id)
                    .HasConstraintName("FK_ReferenceInput.PostcodesDevolution_McaGlaSofLookup_ReferenceInput.PostcodesDevolution_DevolvedPostcodes_DevolvedPostcodes_Id");
            });

            modelBuilder.Entity<PostcodesDevolutionPostcode>(entity =>
            {
                entity.ToTable("PostcodesDevolution_Postcode", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.PostcodesDevolution_DevolvedPostcodes_)
                    .WithMany(p => p.PostcodesDevolutionPostcodes)
                    .HasForeignKey(d => d.PostcodesDevolution_DevolvedPostcodes_Id)
                    .HasConstraintName("FK_ReferenceInput.PostcodesDevolution_Postcode_ReferenceInput.PostcodesDevolution_DevolvedPostcodes_DevolvedPostcodes_Id");
            });

            modelBuilder.Entity<PostcodesEfaDisadvantage>(entity =>
            {
                entity.ToTable("Postcodes_EfaDisadvantage", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.Uplift).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Postcodes_Postcode_)
                    .WithMany(p => p.PostcodesEfaDisadvantages)
                    .HasForeignKey(d => d.Postcodes_Postcode_Id)
                    .HasConstraintName("FK_ReferenceInput.Postcodes_EfaDisadvantage_ReferenceInput.Postcodes_Postcode_Postcode_Id");
            });

            modelBuilder.Entity<PostcodesMcaglaSof>(entity =>
            {
                entity.ToTable("Postcodes_McaglaSOF", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.Postcodes_Postcode_)
                    .WithMany(p => p.PostcodesMcaglaSofs)
                    .HasForeignKey(d => d.Postcodes_Postcode_Id)
                    .HasConstraintName("FK_ReferenceInput.Postcodes_McaglaSOF_ReferenceInput.Postcodes_Postcode_Postcode_Id");
            });

            modelBuilder.Entity<PostcodesOnsdata>(entity =>
            {
                entity.ToTable("Postcodes_ONSData", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.Termination).HasColumnType("datetime");

                entity.HasOne(d => d.Postcodes_Postcode_)
                    .WithMany(p => p.PostcodesOnsdatas)
                    .HasForeignKey(d => d.Postcodes_Postcode_Id)
                    .HasConstraintName("FK_ReferenceInput.Postcodes_ONSData_ReferenceInput.Postcodes_Postcode_Postcode_Id");
            });

            modelBuilder.Entity<PostcodesPostCodeVersion>(entity =>
            {
                entity.ToTable("Postcodes_PostCodeVersion", "ReferenceInput");

                entity.Property(e => e.PostcodeCurrentVersion).HasMaxLength(2000);
            });

            modelBuilder.Entity<PostcodesPostcode>(entity =>
            {
                entity.ToTable("Postcodes_Postcode", "ReferenceInput");

                entity.Property(e => e.PostCode).HasMaxLength(2000);
            });

            modelBuilder.Entity<PostcodesSfaAreaCost>(entity =>
            {
                entity.ToTable("Postcodes_SfaAreaCost", "ReferenceInput");

                entity.Property(e => e.AreaCostFactor).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.HasOne(d => d.Postcodes_Postcode_)
                    .WithMany(p => p.PostcodesSfaAreaCosts)
                    .HasForeignKey(d => d.Postcodes_Postcode_Id)
                    .HasConstraintName("FK_ReferenceInput.Postcodes_SfaAreaCost_ReferenceInput.Postcodes_Postcode_Postcode_Id");
            });

            modelBuilder.Entity<PostcodesSfaDisadvantage>(entity =>
            {
                entity.ToTable("Postcodes_SfaDisadvantage", "ReferenceInput");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo).HasColumnType("datetime");

                entity.Property(e => e.Uplift).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Postcodes_Postcode_)
                    .WithMany(p => p.PostcodesSfaDisadvantages)
                    .HasForeignKey(d => d.Postcodes_Postcode_Id)
                    .HasConstraintName("FK_ReferenceInput.Postcodes_SfaDisadvantage_ReferenceInput.Postcodes_Postcode_Postcode_Id");
            });

            modelBuilder.Entity<Uln>(entity =>
            {
                entity.ToTable("ULNs", "ReferenceInput");
            });
        }
    }
}
