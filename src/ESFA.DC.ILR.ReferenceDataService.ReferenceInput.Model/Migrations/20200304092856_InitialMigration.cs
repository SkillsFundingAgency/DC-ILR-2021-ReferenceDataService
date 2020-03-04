using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ReferenceInput");

            migrationBuilder.CreateTable(
                name: "AppsEarningHistory_ApprenticeshipEarningsHistory",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppIdentifier = table.Column<string>(maxLength: 2000, nullable: true),
                    AppProgCompletedInTheYearInput = table.Column<bool>(nullable: true),
                    CollectionYear = table.Column<string>(maxLength: 2000, nullable: true),
                    CollectionReturnCode = table.Column<string>(maxLength: 2000, nullable: true),
                    DaysInYear = table.Column<int>(nullable: true),
                    FworkCode = table.Column<int>(nullable: true),
                    HistoricEffectiveTNPStartDateInput = table.Column<DateTime>(type: "datetime", nullable: true),
                    HistoricEmpIdEndWithinYear = table.Column<long>(nullable: true),
                    HistoricEmpIdStartWithinYear = table.Column<long>(nullable: true),
                    HistoricLearner1618StartInput = table.Column<bool>(nullable: true),
                    HistoricPMRAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    HistoricTNP1Input = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    HistoricTNP2Input = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    HistoricTNP3Input = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    HistoricTNP4Input = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    HistoricTotal1618UpliftPaymentsInTheYearInput = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    HistoricVirtualTNP3EndOfTheYearInput = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    HistoricVirtualTNP4EndOfTheYearInput = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    HistoricLearnDelProgEarliestACT2DateInput = table.Column<DateTime>(type: "datetime", nullable: true),
                    LatestInYear = table.Column<bool>(nullable: false),
                    LearnRefNumber = table.Column<string>(maxLength: 2000, nullable: true),
                    ProgrammeStartDateIgnorePathway = table.Column<DateTime>(type: "datetime", nullable: true),
                    ProgrammeStartDateMatchPathway = table.Column<DateTime>(type: "datetime", nullable: true),
                    ProgType = table.Column<int>(nullable: true),
                    PwayCode = table.Column<int>(nullable: true),
                    STDCode = table.Column<int>(nullable: true),
                    TotalProgAimPaymentsInTheYear = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    UptoEndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UKPRN = table.Column<int>(nullable: false),
                    ULN = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppsEarningHistory_ApprenticeshipEarningsHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employers_Employer",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ERN = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employers_Employer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employers_LargeEmployerVersion",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employers_LargeEmployerVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EPAOrganisations_EPAOrganisation",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Standard = table.Column<string>(maxLength: 2000, nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EPAOrganisations_EPAOrganisation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FCS_EsfEligibilityRule",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Benefits = table.Column<bool>(nullable: true),
                    CalcMethod = table.Column<int>(nullable: true),
                    TenderSpecReference = table.Column<string>(maxLength: 2000, nullable: true),
                    LotReference = table.Column<string>(maxLength: 2000, nullable: true),
                    MinAge = table.Column<int>(nullable: true),
                    MaxAge = table.Column<int>(nullable: true),
                    MinLengthOfUnemployment = table.Column<int>(nullable: true),
                    MaxLengthOfUnemployment = table.Column<int>(nullable: true),
                    MinPriorAttainment = table.Column<string>(maxLength: 2000, nullable: true),
                    MaxPriorAttainment = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCS_EsfEligibilityRule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FRM_FrmLearner",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UKPRN = table.Column<long>(nullable: false),
                    ULN = table.Column<long>(nullable: false),
                    AimSeqNumber = table.Column<int>(nullable: false),
                    LearnRefNumber = table.Column<string>(maxLength: 2000, nullable: true),
                    LearnAimRef = table.Column<string>(maxLength: 2000, nullable: true),
                    LearningAimTitle = table.Column<string>(maxLength: 2000, nullable: true),
                    OrgName = table.Column<string>(maxLength: 2000, nullable: true),
                    PartnerOrgName = table.Column<string>(maxLength: 2000, nullable: true),
                    ProgTypeNullable = table.Column<int>(nullable: true),
                    StdCodeNullable = table.Column<int>(nullable: true),
                    FworkCodeNullable = table.Column<int>(nullable: true),
                    PwayCodeNullable = table.Column<int>(nullable: true),
                    LearnStartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    AimType = table.Column<int>(nullable: false),
                    FundModel = table.Column<int>(nullable: false),
                    PrevUKPRN = table.Column<long>(nullable: true),
                    PMUKPRN = table.Column<long>(nullable: true),
                    PartnerUKPRN = table.Column<long>(nullable: true),
                    PrevLearnRefNumber = table.Column<string>(maxLength: 2000, nullable: true),
                    SWSupAimId = table.Column<string>(maxLength: 2000, nullable: true),
                    LearnPlanEndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LearnActEndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PriorLearnFundAdj = table.Column<int>(nullable: true),
                    OtherFundAdj = table.Column<int>(nullable: true),
                    CompStatus = table.Column<int>(nullable: false),
                    Outcome = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRM_FrmLearner", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FRM_FrmReferenceData",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRM_FrmReferenceData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LARS_LARSFrameworkAim",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LearnAimRef = table.Column<string>(maxLength: 2000, nullable: true),
                    FrameworkComponentType = table.Column<int>(nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    FworkCode = table.Column<int>(nullable: true),
                    ProgType = table.Column<int>(nullable: true),
                    PwayCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LARS_LARSFrameworkAim", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LARS_LARSFrameworkDesktop",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FworkCode = table.Column<int>(nullable: false),
                    ProgType = table.Column<int>(nullable: false),
                    PwayCode = table.Column<int>(nullable: false),
                    EffectiveFromNullable = table.Column<DateTime>(type: "datetime", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LARS_LARSFrameworkDesktop", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LARS_LARSLearningDelivery",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LearnAimRef = table.Column<string>(maxLength: 2000, nullable: true),
                    LearnAimRefType = table.Column<string>(maxLength: 2000, nullable: true),
                    LearnAimRefTitle = table.Column<string>(maxLength: 2000, nullable: true),
                    EnglPrscID = table.Column<int>(nullable: true),
                    NotionalNVQLevel = table.Column<string>(maxLength: 2000, nullable: true),
                    NotionalNVQLevelv2 = table.Column<string>(maxLength: 2000, nullable: true),
                    FrameworkCommonComponent = table.Column<int>(nullable: true),
                    LearnDirectClassSystemCode1 = table.Column<string>(maxLength: 2000, nullable: true),
                    LearnDirectClassSystemCode2 = table.Column<string>(maxLength: 2000, nullable: true),
                    LearnDirectClassSystemCode3 = table.Column<string>(maxLength: 2000, nullable: true),
                    SectorSubjectAreaTier1 = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    SectorSubjectAreaTier2 = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    LearningDeliveryGenre = table.Column<string>(maxLength: 2000, nullable: true),
                    RegulatedCreditValue = table.Column<int>(nullable: true),
                    EnglandFEHEStatus = table.Column<string>(maxLength: 2000, nullable: true),
                    AwardOrgCode = table.Column<string>(maxLength: 2000, nullable: true),
                    EFACOFType = table.Column<int>(nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LARS_LARSLearningDelivery", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LARS_LARSStandard",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StandardCode = table.Column<int>(nullable: false),
                    StandardSectorCode = table.Column<string>(maxLength: 2000, nullable: true),
                    NotionalEndLevel = table.Column<string>(maxLength: 2000, nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LARS_LARSStandard", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LARS_LARSVersion",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LARS_LARSVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "McaContracts_McaDevolvedContract",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    McaGlaShortCode = table.Column<string>(maxLength: 2000, nullable: true),
                    Ukprn = table.Column<int>(nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_McaContracts_McaDevolvedContract", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetaData_CampusIdentifierVersion",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData_CampusIdentifierVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetaData_CensusDate",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Period = table.Column<int>(nullable: false),
                    Start = table.Column<DateTime>(type: "datetime", nullable: false),
                    End = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData_CensusDate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetaData_CoFVersion",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData_CoFVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetaData_DevolvedPostcodesVersion",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData_DevolvedPostcodesVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetaData_EasUploadDateTime",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UploadDateTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData_EasUploadDateTime", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetaData_EmployersVersion",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData_EmployersVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetaData_HmppPostcodesVersion",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData_HmppPostcodesVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetaData_IlrCollectionDates",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData_IlrCollectionDates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetaData_LarsVersion",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData_LarsVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetaData_Lookup",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 2000, nullable: true),
                    Code = table.Column<string>(maxLength: 2000, nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData_Lookup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetaData_OrganisationsVersion",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData_OrganisationsVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetaData_PostcodeFactorsVersion",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData_PostcodeFactorsVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetaData_PostcodesVersion",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData_PostcodesVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetaData_ValidationError",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RuleName = table.Column<string>(maxLength: 2000, nullable: true),
                    Severity = table.Column<int>(nullable: false),
                    Message = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData_ValidationError", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetaData_ValidationRule",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RuleName = table.Column<string>(maxLength: 2000, nullable: true),
                    Desktop = table.Column<bool>(nullable: false),
                    Online = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData_ValidationRule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organisations_Organisation",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UKPRN = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 2000, nullable: true),
                    PartnerUKPRN = table.Column<bool>(nullable: true),
                    LegalOrgType = table.Column<string>(maxLength: 2000, nullable: true),
                    LongTermResid = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations_Organisation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organisations_OrganisationVersion",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations_OrganisationVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Postcodes_Postcode",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PostCode = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postcodes_Postcode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Postcodes_PostCodeVersion",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PostcodeCurrentVersion = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postcodes_PostCodeVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostcodesDevolution_DevolvedPostcodes",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostcodesDevolution_DevolvedPostcodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ULNs",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UniqueLearnerNumber = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ULNs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employers_LargeEmployerEffectiveDates",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    Employers_Employer_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employers_LargeEmployerEffectiveDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.Employers_LargeEmployerEffectiveDates_ReferenceInput.Employers_Employer_Employer_Id",
                        column: x => x.Employers_Employer_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "Employers_Employer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FCS_EsfEligibilityRuleEmploymentStatus",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(nullable: false),
                    FCS_EsfEligibilityRule_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCS_EsfEligibilityRuleEmploymentStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.FCS_EsfEligibilityRuleEmploymentStatus_ReferenceInput.FCS_EsfEligibilityRule_EsfEligibilityRule_Id",
                        column: x => x.FCS_EsfEligibilityRule_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "FCS_EsfEligibilityRule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FCS_EsfEligibilityRuleLocalAuthority",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    FCS_EsfEligibilityRule_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCS_EsfEligibilityRuleLocalAuthority", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.FCS_EsfEligibilityRuleLocalAuthority_ReferenceInput.FCS_EsfEligibilityRule_EsfEligibilityRule_Id",
                        column: x => x.FCS_EsfEligibilityRule_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "FCS_EsfEligibilityRule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FCS_EsfEligibilityRuleLocalEnterprisePartnership",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    FCS_EsfEligibilityRule_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCS_EsfEligibilityRuleLocalEnterprisePartnership", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.FCS_EsfEligibilityRuleLocalEnterprisePartnership_ReferenceInput.FCS_EsfEligibilityRule_EsfEligibilityRule_Id",
                        column: x => x.FCS_EsfEligibilityRule_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "FCS_EsfEligibilityRule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FCS_EsfEligibilityRuleSectorSubjectAreaLevel",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SectorSubjectAreaCode = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    MinLevelCode = table.Column<string>(nullable: true),
                    MaxLevelCode = table.Column<string>(nullable: true),
                    FCS_EsfEligibilityRule_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCS_EsfEligibilityRuleSectorSubjectAreaLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.FCS_EsfEligibilityRuleSectorSubjectAreaLevel_ReferenceInput.FCS_EsfEligibilityRule_EsfEligibilityRule_Id",
                        column: x => x.FCS_EsfEligibilityRule_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "FCS_EsfEligibilityRule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FCS_FcsContractAllocation",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContractAllocationNumber = table.Column<string>(nullable: true),
                    DeliveryUKPRN = table.Column<int>(nullable: false),
                    LearningRatePremiumFactor = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    TenderSpecReference = table.Column<string>(nullable: true),
                    LotReference = table.Column<string>(nullable: true),
                    FundingStreamPeriodCode = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    StopNewStartsFromDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EsfEligibilityRule_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCS_FcsContractAllocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.FCS_FcsContractAllocation_ReferenceInput.FCS_EsfEligibilityRule_EsfEligibilityRule_Id",
                        column: x => x.EsfEligibilityRule_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "FCS_EsfEligibilityRule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRM_LearningDeliveryFAM",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LearnDelFAMType = table.Column<string>(nullable: true),
                    LearnDelFAMCode = table.Column<string>(nullable: true),
                    LearnDelFAMDateFrom = table.Column<DateTime>(type: "datetime", nullable: true),
                    LearnDelFAMDateTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    FRM_FrmLearner_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRM_LearningDeliveryFAM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.FRM_LearningDeliveryFAM_ReferenceInput.FRM_FrmLearner_FrmLearner_Id",
                        column: x => x.FRM_FrmLearner_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "FRM_FrmLearner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRM_ProviderSpecDeliveryMonitoring",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProvSpecDelMonOccur = table.Column<string>(nullable: true),
                    ProvSpecDelMon = table.Column<string>(nullable: true),
                    FRM_FrmLearner_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRM_ProviderSpecDeliveryMonitoring", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.FRM_ProviderSpecDeliveryMonitoring_ReferenceInput.FRM_FrmLearner_FrmLearner_Id",
                        column: x => x.FRM_FrmLearner_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "FRM_FrmLearner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FRM_ProviderSpecLearnerMonitoring",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProvSpecLearnMonOccur = table.Column<string>(nullable: true),
                    ProvSpecLearnMon = table.Column<string>(nullable: true),
                    FRM_FrmLearner_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRM_ProviderSpecLearnerMonitoring", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.FRM_ProviderSpecLearnerMonitoring_ReferenceInput.FRM_FrmLearner_FrmLearner_Id",
                        column: x => x.FRM_FrmLearner_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "FRM_FrmLearner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LARS_LARSFrameworkApprenticeshipFunding",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FundingCategory = table.Column<string>(nullable: true),
                    BandNumber = table.Column<int>(nullable: true),
                    CoreGovContributionCap = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    SixteenToEighteenIncentive = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    SixteenToEighteenProviderAdditionalPayment = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    SixteenToEighteenEmployerAdditionalPayment = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    SixteenToEighteenFrameworkUplift = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    CareLeaverAdditionalPayment = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Duration = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    ReservedValue2 = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    ReservedValue3 = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    MaxEmployerLevyCap = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    FundableWithoutEmployer = table.Column<string>(nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    LARS_LARSFrameworkDesktop_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LARS_LARSFrameworkApprenticeshipFunding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.LARS_LARSFrameworkApprenticeshipFunding_ReferenceInput.LARS_LARSFrameworkDesktop_LARSFrameworkDesktop_Id",
                        column: x => x.LARS_LARSFrameworkDesktop_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "LARS_LARSFrameworkDesktop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LARS_LARSFrameworkCommonComponent",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CommonComponent = table.Column<int>(nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    LARS_LARSFrameworkDesktop_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LARS_LARSFrameworkCommonComponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.LARS_LARSFrameworkCommonComponent_ReferenceInput.LARS_LARSFrameworkDesktop_LARSFrameworkDesktop_Id",
                        column: x => x.LARS_LARSFrameworkDesktop_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "LARS_LARSFrameworkDesktop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LARS_LARSAnnualValue",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LearnAimRef = table.Column<string>(nullable: true),
                    BasicSkills = table.Column<int>(nullable: true),
                    BasicSkillsType = table.Column<int>(nullable: true),
                    FullLevel2EntitlementCategory = table.Column<int>(nullable: true),
                    FullLevel3EntitlementCategory = table.Column<int>(nullable: true),
                    FullLevel2Percent = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    FullLevel3Percent = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    LARS_LARSLearningDelivery_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LARS_LARSAnnualValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.LARS_LARSAnnualValue_ReferenceInput.LARS_LARSLearningDelivery_LARSLearningDelivery_Id",
                        column: x => x.LARS_LARSLearningDelivery_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "LARS_LARSLearningDelivery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LARS_LARSFunding",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LearnAimRef = table.Column<string>(nullable: true),
                    FundingCategory = table.Column<string>(nullable: true),
                    RateUnWeighted = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    RateWeighted = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    WeightingFactor = table.Column<string>(nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    LARS_LARSLearningDelivery_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LARS_LARSFunding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.LARS_LARSFunding_ReferenceInput.LARS_LARSLearningDelivery_LARSLearningDelivery_Id",
                        column: x => x.LARS_LARSLearningDelivery_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "LARS_LARSLearningDelivery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LARS_LARSLearningDeliveryCategory",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LearnAimRef = table.Column<string>(nullable: true),
                    CategoryRef = table.Column<int>(nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    LARS_LARSLearningDelivery_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LARS_LARSLearningDeliveryCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.LARS_LARSLearningDeliveryCategory_ReferenceInput.LARS_LARSLearningDelivery_LARSLearningDelivery_Id",
                        column: x => x.LARS_LARSLearningDelivery_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "LARS_LARSLearningDelivery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LARS_LARSValidity",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LearnAimRef = table.Column<string>(nullable: true),
                    ValidityCategory = table.Column<string>(nullable: true),
                    LastNewStartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    LARS_LARSLearningDelivery_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LARS_LARSValidity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.LARS_LARSValidity_ReferenceInput.LARS_LARSLearningDelivery_LARSLearningDelivery_Id",
                        column: x => x.LARS_LARSLearningDelivery_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "LARS_LARSLearningDelivery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LARS_LARSStandardApprenticeshipFunding",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProgType = table.Column<int>(nullable: false),
                    PwayCode = table.Column<int>(nullable: true),
                    FundingCategory = table.Column<string>(nullable: true),
                    BandNumber = table.Column<int>(nullable: true),
                    CoreGovContributionCap = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    SixteenToEighteenIncentive = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    SixteenToEighteenProviderAdditionalPayment = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    SixteenToEighteenEmployerAdditionalPayment = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    SixteenToEighteenFrameworkUplift = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    CareLeaverAdditionalPayment = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Duration = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    ReservedValue2 = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    ReservedValue3 = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    MaxEmployerLevyCap = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    FundableWithoutEmployer = table.Column<string>(nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    LARS_LARSStandard_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LARS_LARSStandardApprenticeshipFunding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.LARS_LARSStandardApprenticeshipFunding_ReferenceInput.LARS_LARSStandard_LARSStandard_Id",
                        column: x => x.LARS_LARSStandard_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "LARS_LARSStandard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LARS_LARSStandardCommonComponent",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CommonComponent = table.Column<int>(nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    LARS_LARSStandard_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LARS_LARSStandardCommonComponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.LARS_LARSStandardCommonComponent_ReferenceInput.LARS_LARSStandard_LARSStandard_Id",
                        column: x => x.LARS_LARSStandard_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "LARS_LARSStandard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LARS_LARSStandardFunding",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FundingCategory = table.Column<string>(nullable: true),
                    BandNumber = table.Column<int>(nullable: true),
                    CoreGovContributionCap = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    SixteenToEighteenIncentive = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    SmallBusinessIncentive = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    AchievementIncentive = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    FundableWithoutEmployer = table.Column<string>(nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    LARS_LARSStandard_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LARS_LARSStandardFunding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.LARS_LARSStandardFunding_ReferenceInput.LARS_LARSStandard_LARSStandard_Id",
                        column: x => x.LARS_LARSStandard_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "LARS_LARSStandard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LARS_LARSStandardValidity",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ValidityCategory = table.Column<string>(nullable: true),
                    LastNewStartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    LARS_LARSStandard_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LARS_LARSStandardValidity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.LARS_LARSStandardValidity_ReferenceInput.LARS_LARSStandard_LARSStandard_Id",
                        column: x => x.LARS_LARSStandard_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "LARS_LARSStandard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MetaData_LookupSubCategory",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    MetaData_Lookup_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData_LookupSubCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.MetaData_LookupSubCategory_ReferenceInput.MetaData_Lookup_Lookup_Id",
                        column: x => x.MetaData_Lookup_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "MetaData_Lookup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MetaData_ReferenceDataVersion",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CampusIdentifierVersion_Id = table.Column<int>(nullable: true),
                    CoFVersion_Id = table.Column<int>(nullable: true),
                    DevolvedPostcodesVersion_Id = table.Column<int>(nullable: true),
                    EasUploadDateTime_Id = table.Column<int>(nullable: true),
                    Employers_Id = table.Column<int>(nullable: true),
                    HmppPostcodesVersion_Id = table.Column<int>(nullable: true),
                    LarsVersion_Id = table.Column<int>(nullable: true),
                    OrganisationsVersion_Id = table.Column<int>(nullable: true),
                    PostcodeFactorsVersion_Id = table.Column<int>(nullable: true),
                    PostcodesVersion_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData_ReferenceDataVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_CampusIdentifierVersion_CampusIdentifierVersion_Id",
                        column: x => x.CampusIdentifierVersion_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "MetaData_CampusIdentifierVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_CoFVersion_CoFVersion_Id",
                        column: x => x.CoFVersion_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "MetaData_CoFVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_DevolvedPostcodesVersion_DevolvedPostcodesVersion_Id",
                        column: x => x.DevolvedPostcodesVersion_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "MetaData_DevolvedPostcodesVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_EasUploadDateTime_EasUploadDateTime_Id",
                        column: x => x.EasUploadDateTime_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "MetaData_EasUploadDateTime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_EmployersVersion_Employers_Id",
                        column: x => x.Employers_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "MetaData_EmployersVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_HmppPostcodesVersion_HmppPostcodesVersion_Id",
                        column: x => x.HmppPostcodesVersion_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "MetaData_HmppPostcodesVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_LarsVersion_LarsVersion_Id",
                        column: x => x.LarsVersion_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "MetaData_LarsVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_OrganisationsVersion_OrganisationsVersion_Id",
                        column: x => x.OrganisationsVersion_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "MetaData_OrganisationsVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_PostcodeFactorsVersion_PostcodeFactorsVersion_Id",
                        column: x => x.PostcodeFactorsVersion_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "MetaData_PostcodeFactorsVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceInput.MetaData_PostcodesVersion_PostcodesVersion_Id",
                        column: x => x.PostcodesVersion_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "MetaData_PostcodesVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Organisations_OrganisationCampusIdentifier",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UKPRN = table.Column<long>(nullable: false),
                    CampusIdentifier = table.Column<string>(nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    Organisations_Organisation_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations_OrganisationCampusIdentifier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.Organisations_OrganisationCampusIdentifier_ReferenceInput.Organisations_Organisation_Organisation_Id",
                        column: x => x.Organisations_Organisation_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "Organisations_Organisation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Organisations_OrganisationCoFRemoval",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CoFRemoval = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    Organisations_Organisation_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations_OrganisationCoFRemoval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.Organisations_OrganisationCoFRemoval_ReferenceInput.Organisations_Organisation_Organisation_Id",
                        column: x => x.Organisations_Organisation_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "Organisations_Organisation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Organisations_OrganisationFunding",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrgFundFactor = table.Column<string>(nullable: true),
                    OrgFundFactType = table.Column<string>(nullable: true),
                    OrgFundFactValue = table.Column<string>(nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    Organisations_Organisation_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations_OrganisationFunding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.Organisations_OrganisationFunding_ReferenceInput.Organisations_Organisation_Organisation_Id",
                        column: x => x.Organisations_Organisation_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "Organisations_Organisation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Postcodes_DasDisadvantage",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Uplift = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    Postcodes_Postcode_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postcodes_DasDisadvantage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.Postcodes_DasDisadvantage_ReferenceInput.Postcodes_Postcode_Postcode_Id",
                        column: x => x.Postcodes_Postcode_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "Postcodes_Postcode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Postcodes_EfaDisadvantage",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Uplift = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    Postcodes_Postcode_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postcodes_EfaDisadvantage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.Postcodes_EfaDisadvantage_ReferenceInput.Postcodes_Postcode_Postcode_Id",
                        column: x => x.Postcodes_Postcode_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "Postcodes_Postcode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Postcodes_McaglaSOF",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SofCode = table.Column<string>(nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    Postcodes_Postcode_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postcodes_McaglaSOF", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.Postcodes_McaglaSOF_ReferenceInput.Postcodes_Postcode_Postcode_Id",
                        column: x => x.Postcodes_Postcode_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "Postcodes_Postcode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Postcodes_ONSData",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Termination = table.Column<DateTime>(type: "datetime", nullable: true),
                    LocalAuthority = table.Column<string>(nullable: true),
                    Lep1 = table.Column<string>(nullable: true),
                    Lep2 = table.Column<string>(nullable: true),
                    Nuts = table.Column<string>(nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    Postcodes_Postcode_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postcodes_ONSData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.Postcodes_ONSData_ReferenceInput.Postcodes_Postcode_Postcode_Id",
                        column: x => x.Postcodes_Postcode_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "Postcodes_Postcode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Postcodes_SfaAreaCost",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AreaCostFactor = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    Postcodes_Postcode_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postcodes_SfaAreaCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.Postcodes_SfaAreaCost_ReferenceInput.Postcodes_Postcode_Postcode_Id",
                        column: x => x.Postcodes_Postcode_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "Postcodes_Postcode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Postcodes_SfaDisadvantage",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Uplift = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    Postcodes_Postcode_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postcodes_SfaDisadvantage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.Postcodes_SfaDisadvantage_ReferenceInput.Postcodes_Postcode_Postcode_Id",
                        column: x => x.Postcodes_Postcode_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "Postcodes_Postcode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostcodesDevolution_McaGlaSofLookup",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SofCode = table.Column<string>(nullable: true),
                    McaGlaShortCode = table.Column<string>(nullable: true),
                    McaGlaFullName = table.Column<string>(nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    PostcodesDevolution_DevolvedPostcodes_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostcodesDevolution_McaGlaSofLookup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.PostcodesDevolution_McaGlaSofLookup_ReferenceInput.PostcodesDevolution_DevolvedPostcodes_DevolvedPostcodes_Id",
                        column: x => x.PostcodesDevolution_DevolvedPostcodes_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "PostcodesDevolution_DevolvedPostcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostcodesDevolution_Postcode",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Postcode = table.Column<string>(nullable: true),
                    Area = table.Column<string>(nullable: true),
                    SourceOfFunding = table.Column<string>(nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    PostcodesDevolution_DevolvedPostcodes_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostcodesDevolution_Postcode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.PostcodesDevolution_Postcode_ReferenceInput.PostcodesDevolution_DevolvedPostcodes_DevolvedPostcodes_Id",
                        column: x => x.PostcodesDevolution_DevolvedPostcodes_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "PostcodesDevolution_DevolvedPostcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FCS_FcsContractDeliverable",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DeliverableCode = table.Column<int>(nullable: true),
                    DeliverableDescription = table.Column<string>(nullable: true),
                    ExternalDeliverableCode = table.Column<string>(nullable: true),
                    UnitCost = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    PlannedVolume = table.Column<int>(nullable: true),
                    PlannedValue = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    FCS_FcsContractAllocation_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCS_FcsContractDeliverable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.FCS_FcsContractDeliverable_ReferenceInput.FCS_FcsContractAllocation_FcsContractAllocation_Id",
                        column: x => x.FCS_FcsContractAllocation_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "FCS_FcsContractAllocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MetaData_MetaData",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateGenerated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CollectionDates_Id = table.Column<int>(nullable: true),
                    ReferenceDataVersions_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData_MetaData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.MetaData_MetaData_ReferenceInput.MetaData_IlrCollectionDates_CollectionDates_Id",
                        column: x => x.CollectionDates_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "MetaData_IlrCollectionDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.MetaData_MetaData_ReferenceInput.MetaData_ReferenceDataVersion_ReferenceDataVersions_Id",
                        column: x => x.ReferenceDataVersions_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "MetaData_ReferenceDataVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Organisations_SpecialistResource",
                schema: "ReferenceInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsSpecialistResource = table.Column<bool>(nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    Organisations_OrganisationCampusIdentifier_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations_SpecialistResource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceInput.Organisations_SpecialistResource_ReferenceInput.Organisations_OrganisationCampusIdentifier_Id",
                        column: x => x.Organisations_OrganisationCampusIdentifier_Id,
                        principalSchema: "ReferenceInput",
                        principalTable: "Organisations_OrganisationCampusIdentifier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employers_LargeEmployerEffectiveDates_Employers_Employer_Id",
                schema: "ReferenceInput",
                table: "Employers_LargeEmployerEffectiveDates",
                column: "Employers_Employer_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FCS_EsfEligibilityRuleEmploymentStatus_FCS_EsfEligibilityRule_Id",
                schema: "ReferenceInput",
                table: "FCS_EsfEligibilityRuleEmploymentStatus",
                column: "FCS_EsfEligibilityRule_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FCS_EsfEligibilityRuleLocalAuthority_FCS_EsfEligibilityRule_Id",
                schema: "ReferenceInput",
                table: "FCS_EsfEligibilityRuleLocalAuthority",
                column: "FCS_EsfEligibilityRule_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FCS_EsfEligibilityRuleLocalEnterprisePartnership_FCS_EsfEligibilityRule_Id",
                schema: "ReferenceInput",
                table: "FCS_EsfEligibilityRuleLocalEnterprisePartnership",
                column: "FCS_EsfEligibilityRule_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FCS_EsfEligibilityRuleSectorSubjectAreaLevel_FCS_EsfEligibilityRule_Id",
                schema: "ReferenceInput",
                table: "FCS_EsfEligibilityRuleSectorSubjectAreaLevel",
                column: "FCS_EsfEligibilityRule_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FCS_FcsContractAllocation_EsfEligibilityRule_Id",
                schema: "ReferenceInput",
                table: "FCS_FcsContractAllocation",
                column: "EsfEligibilityRule_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FCS_FcsContractDeliverable_FCS_FcsContractAllocation_Id",
                schema: "ReferenceInput",
                table: "FCS_FcsContractDeliverable",
                column: "FCS_FcsContractAllocation_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FRM_LearningDeliveryFAM_FRM_FrmLearner_Id",
                schema: "ReferenceInput",
                table: "FRM_LearningDeliveryFAM",
                column: "FRM_FrmLearner_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FRM_ProviderSpecDeliveryMonitoring_FRM_FrmLearner_Id",
                schema: "ReferenceInput",
                table: "FRM_ProviderSpecDeliveryMonitoring",
                column: "FRM_FrmLearner_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FRM_ProviderSpecLearnerMonitoring_FRM_FrmLearner_Id",
                schema: "ReferenceInput",
                table: "FRM_ProviderSpecLearnerMonitoring",
                column: "FRM_FrmLearner_Id");

            migrationBuilder.CreateIndex(
                name: "IX_LARS_LARSAnnualValue_LARS_LARSLearningDelivery_Id",
                schema: "ReferenceInput",
                table: "LARS_LARSAnnualValue",
                column: "LARS_LARSLearningDelivery_Id");

            migrationBuilder.CreateIndex(
                name: "IX_LARS_LARSFrameworkApprenticeshipFunding_LARS_LARSFrameworkDesktop_Id",
                schema: "ReferenceInput",
                table: "LARS_LARSFrameworkApprenticeshipFunding",
                column: "LARS_LARSFrameworkDesktop_Id");

            migrationBuilder.CreateIndex(
                name: "IX_LARS_LARSFrameworkCommonComponent_LARS_LARSFrameworkDesktop_Id",
                schema: "ReferenceInput",
                table: "LARS_LARSFrameworkCommonComponent",
                column: "LARS_LARSFrameworkDesktop_Id");

            migrationBuilder.CreateIndex(
                name: "IX_LARS_LARSFunding_LARS_LARSLearningDelivery_Id",
                schema: "ReferenceInput",
                table: "LARS_LARSFunding",
                column: "LARS_LARSLearningDelivery_Id");

            migrationBuilder.CreateIndex(
                name: "IX_LARS_LARSLearningDeliveryCategory_LARS_LARSLearningDelivery_Id",
                schema: "ReferenceInput",
                table: "LARS_LARSLearningDeliveryCategory",
                column: "LARS_LARSLearningDelivery_Id");

            migrationBuilder.CreateIndex(
                name: "IX_LARS_LARSStandardApprenticeshipFunding_LARS_LARSStandard_Id",
                schema: "ReferenceInput",
                table: "LARS_LARSStandardApprenticeshipFunding",
                column: "LARS_LARSStandard_Id");

            migrationBuilder.CreateIndex(
                name: "IX_LARS_LARSStandardCommonComponent_LARS_LARSStandard_Id",
                schema: "ReferenceInput",
                table: "LARS_LARSStandardCommonComponent",
                column: "LARS_LARSStandard_Id");

            migrationBuilder.CreateIndex(
                name: "IX_LARS_LARSStandardFunding_LARS_LARSStandard_Id",
                schema: "ReferenceInput",
                table: "LARS_LARSStandardFunding",
                column: "LARS_LARSStandard_Id");

            migrationBuilder.CreateIndex(
                name: "IX_LARS_LARSStandardValidity_LARS_LARSStandard_Id",
                schema: "ReferenceInput",
                table: "LARS_LARSStandardValidity",
                column: "LARS_LARSStandard_Id");

            migrationBuilder.CreateIndex(
                name: "IX_LARS_LARSValidity_LARS_LARSLearningDelivery_Id",
                schema: "ReferenceInput",
                table: "LARS_LARSValidity",
                column: "LARS_LARSLearningDelivery_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MetaData_LookupSubCategory_MetaData_Lookup_Id",
                schema: "ReferenceInput",
                table: "MetaData_LookupSubCategory",
                column: "MetaData_Lookup_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MetaData_MetaData_CollectionDates_Id",
                schema: "ReferenceInput",
                table: "MetaData_MetaData",
                column: "CollectionDates_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MetaData_MetaData_ReferenceDataVersions_Id",
                schema: "ReferenceInput",
                table: "MetaData_MetaData",
                column: "ReferenceDataVersions_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MetaData_ReferenceDataVersion_CampusIdentifierVersion_Id",
                schema: "ReferenceInput",
                table: "MetaData_ReferenceDataVersion",
                column: "CampusIdentifierVersion_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MetaData_ReferenceDataVersion_CoFVersion_Id",
                schema: "ReferenceInput",
                table: "MetaData_ReferenceDataVersion",
                column: "CoFVersion_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MetaData_ReferenceDataVersion_DevolvedPostcodesVersion_Id",
                schema: "ReferenceInput",
                table: "MetaData_ReferenceDataVersion",
                column: "DevolvedPostcodesVersion_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MetaData_ReferenceDataVersion_EasUploadDateTime_Id",
                schema: "ReferenceInput",
                table: "MetaData_ReferenceDataVersion",
                column: "EasUploadDateTime_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MetaData_ReferenceDataVersion_Employers_Id",
                schema: "ReferenceInput",
                table: "MetaData_ReferenceDataVersion",
                column: "Employers_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MetaData_ReferenceDataVersion_HmppPostcodesVersion_Id",
                schema: "ReferenceInput",
                table: "MetaData_ReferenceDataVersion",
                column: "HmppPostcodesVersion_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MetaData_ReferenceDataVersion_LarsVersion_Id",
                schema: "ReferenceInput",
                table: "MetaData_ReferenceDataVersion",
                column: "LarsVersion_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MetaData_ReferenceDataVersion_OrganisationsVersion_Id",
                schema: "ReferenceInput",
                table: "MetaData_ReferenceDataVersion",
                column: "OrganisationsVersion_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MetaData_ReferenceDataVersion_PostcodeFactorsVersion_Id",
                schema: "ReferenceInput",
                table: "MetaData_ReferenceDataVersion",
                column: "PostcodeFactorsVersion_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MetaData_ReferenceDataVersion_PostcodesVersion_Id",
                schema: "ReferenceInput",
                table: "MetaData_ReferenceDataVersion",
                column: "PostcodesVersion_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Organisations_OrganisationCampusIdentifier_Organisations_Organisation_Id",
                schema: "ReferenceInput",
                table: "Organisations_OrganisationCampusIdentifier",
                column: "Organisations_Organisation_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Organisations_OrganisationCoFRemoval_Organisations_Organisation_Id",
                schema: "ReferenceInput",
                table: "Organisations_OrganisationCoFRemoval",
                column: "Organisations_Organisation_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Organisations_OrganisationFunding_Organisations_Organisation_Id",
                schema: "ReferenceInput",
                table: "Organisations_OrganisationFunding",
                column: "Organisations_Organisation_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Organisations_SpecialistResource_Organisations_OrganisationCampusIdentifier_Id",
                schema: "ReferenceInput",
                table: "Organisations_SpecialistResource",
                column: "Organisations_OrganisationCampusIdentifier_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Postcodes_DasDisadvantage_Postcodes_Postcode_Id",
                schema: "ReferenceInput",
                table: "Postcodes_DasDisadvantage",
                column: "Postcodes_Postcode_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Postcodes_EfaDisadvantage_Postcodes_Postcode_Id",
                schema: "ReferenceInput",
                table: "Postcodes_EfaDisadvantage",
                column: "Postcodes_Postcode_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Postcodes_McaglaSOF_Postcodes_Postcode_Id",
                schema: "ReferenceInput",
                table: "Postcodes_McaglaSOF",
                column: "Postcodes_Postcode_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Postcodes_ONSData_Postcodes_Postcode_Id",
                schema: "ReferenceInput",
                table: "Postcodes_ONSData",
                column: "Postcodes_Postcode_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Postcodes_SfaAreaCost_Postcodes_Postcode_Id",
                schema: "ReferenceInput",
                table: "Postcodes_SfaAreaCost",
                column: "Postcodes_Postcode_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Postcodes_SfaDisadvantage_Postcodes_Postcode_Id",
                schema: "ReferenceInput",
                table: "Postcodes_SfaDisadvantage",
                column: "Postcodes_Postcode_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PostcodesDevolution_McaGlaSofLookup_PostcodesDevolution_DevolvedPostcodes_Id",
                schema: "ReferenceInput",
                table: "PostcodesDevolution_McaGlaSofLookup",
                column: "PostcodesDevolution_DevolvedPostcodes_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PostcodesDevolution_Postcode_PostcodesDevolution_DevolvedPostcodes_Id",
                schema: "ReferenceInput",
                table: "PostcodesDevolution_Postcode",
                column: "PostcodesDevolution_DevolvedPostcodes_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppsEarningHistory_ApprenticeshipEarningsHistory",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "Employers_LargeEmployerEffectiveDates",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "Employers_LargeEmployerVersion",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "EPAOrganisations_EPAOrganisation",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "FCS_EsfEligibilityRuleEmploymentStatus",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "FCS_EsfEligibilityRuleLocalAuthority",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "FCS_EsfEligibilityRuleLocalEnterprisePartnership",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "FCS_EsfEligibilityRuleSectorSubjectAreaLevel",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "FCS_FcsContractDeliverable",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "FRM_FrmReferenceData",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "FRM_LearningDeliveryFAM",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "FRM_ProviderSpecDeliveryMonitoring",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "FRM_ProviderSpecLearnerMonitoring",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "LARS_LARSAnnualValue",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "LARS_LARSFrameworkAim",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "LARS_LARSFrameworkApprenticeshipFunding",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "LARS_LARSFrameworkCommonComponent",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "LARS_LARSFunding",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "LARS_LARSLearningDeliveryCategory",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "LARS_LARSStandardApprenticeshipFunding",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "LARS_LARSStandardCommonComponent",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "LARS_LARSStandardFunding",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "LARS_LARSStandardValidity",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "LARS_LARSValidity",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "LARS_LARSVersion",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "McaContracts_McaDevolvedContract",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "MetaData_CensusDate",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "MetaData_LookupSubCategory",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "MetaData_MetaData",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "MetaData_ValidationError",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "MetaData_ValidationRule",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "Organisations_OrganisationCoFRemoval",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "Organisations_OrganisationFunding",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "Organisations_OrganisationVersion",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "Organisations_SpecialistResource",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "Postcodes_DasDisadvantage",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "Postcodes_EfaDisadvantage",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "Postcodes_McaglaSOF",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "Postcodes_ONSData",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "Postcodes_PostCodeVersion",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "Postcodes_SfaAreaCost",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "Postcodes_SfaDisadvantage",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "PostcodesDevolution_McaGlaSofLookup",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "PostcodesDevolution_Postcode",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "ULNs",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "Employers_Employer",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "FCS_FcsContractAllocation",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "FRM_FrmLearner",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "LARS_LARSFrameworkDesktop",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "LARS_LARSStandard",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "LARS_LARSLearningDelivery",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "MetaData_Lookup",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "MetaData_IlrCollectionDates",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "MetaData_ReferenceDataVersion",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "Organisations_OrganisationCampusIdentifier",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "Postcodes_Postcode",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "PostcodesDevolution_DevolvedPostcodes",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "FCS_EsfEligibilityRule",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "MetaData_CampusIdentifierVersion",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "MetaData_CoFVersion",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "MetaData_DevolvedPostcodesVersion",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "MetaData_EasUploadDateTime",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "MetaData_EmployersVersion",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "MetaData_HmppPostcodesVersion",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "MetaData_LarsVersion",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "MetaData_OrganisationsVersion",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "MetaData_PostcodeFactorsVersion",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "MetaData_PostcodesVersion",
                schema: "ReferenceInput");

            migrationBuilder.DropTable(
                name: "Organisations_Organisation",
                schema: "ReferenceInput");
        }
    }
}
