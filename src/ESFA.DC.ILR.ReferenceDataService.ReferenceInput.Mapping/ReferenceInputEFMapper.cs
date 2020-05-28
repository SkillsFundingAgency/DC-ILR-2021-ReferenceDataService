using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using AutoMapper;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.CollectionDates;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping
{
    public class ReferenceInputEFMapper : IReferenceInputEFMapper
    {
        public TTarget MapByType<TSource, TTarget>(TSource source)
        {
            var mapper = GetMapper();

            var target = mapper.Map<TTarget>(source);

            return target;

        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // The below mappings can be broken out into injectable resources as and when some customisation is required for that specific classes mapping
                // Until then, just leave then in the list below.

                // Metadata
                cfg.CreateMap<IReadOnlyCollection<MetaData>, List<MetaData_ReferenceDataVersion>>()
                    .ConvertUsing((source, target, c) => c.Mapper.Map<List<MetaData_ReferenceDataVersion>>(source.Select(src => src.ReferenceDataVersions).ToList()));
                cfg.CreateMap<ReferenceDataVersion, MetaData_ReferenceDataVersion>();

                cfg.CreateMap<MetaData, ReferenceDataVersion>()
                    .ConvertUsing(source => source.ReferenceDataVersions);
                cfg.CreateMap<ReferenceDataVersion, MetaData_ReferenceDataVersion>()
                    .ForMember(m => m.EmployersVersion_, opt => opt.MapFrom(src => src.Employers));

                cfg.CreateMap<CampusIdentifierVersion, MetaData_CampusIdentifierVersion>();
                cfg.CreateMap<CoFVersion, MetaData_CoFVersion>();
                cfg.CreateMap<EmployersVersion, MetaData_EmployersVersion>();
                cfg.CreateMap<LarsVersion, MetaData_LarsVersion>();
                cfg.CreateMap<OrganisationsVersion, MetaData_OrganisationsVersion>();
                cfg.CreateMap<PostcodesVersion, MetaData_PostcodesVersion>();
                cfg.CreateMap<DevolvedPostcodesVersion, MetaData_DevolvedPostcodesVersion>();
                cfg.CreateMap<HmppPostcodesVersion, MetaData_HmppPostcodesVersion>();
                cfg.CreateMap<PostcodeFactorsVersion, MetaData_PostcodeFactorsVersion>();
                cfg.CreateMap<EasFileDetails, MetaData_EasFileDetails>();

                // Metadata - Collection Dates
                cfg.CreateMap<IReadOnlyCollection<MetaData>, List<MetaData_CensusDate>>()
                    .ConvertUsing((source, target, c) => c.Mapper.Map<List<MetaData_CensusDate>>(source.SelectMany(src => src.CollectionDates.CensusDates).ToList()));
                cfg.CreateMap<CensusDate, MetaData_CensusDate>();
                cfg.CreateMap<IReadOnlyCollection<MetaData>, List<MetaData_ReturnPeriod>>()
                    .ConvertUsing((source, target, c) => c.Mapper.Map<List<MetaData_ReturnPeriod>>(source.SelectMany(src => src.CollectionDates.ReturnPeriods).ToList()));
                cfg.CreateMap<ReturnPeriod, MetaData_ReturnPeriod>();

                // Metadata - Validation Errors / Rules
                cfg.CreateMap<IReadOnlyCollection<MetaData>, List<MetaData_ValidationError>>()
                    .ConvertUsing((source, target, c) => c.Mapper.Map<List<MetaData_ValidationError>>(source.SelectMany(src => src.ValidationErrors).ToList()));
                cfg.CreateMap<ValidationError, MetaData_ValidationError>();
                cfg.CreateMap<IReadOnlyCollection<MetaData>, List<MetaData_ValidationRule>>()
                    .ConvertUsing((source, target, c) => c.Mapper.Map<List<MetaData_ValidationRule>>(source.SelectMany(src => src.ValidationRules).ToList()));
                cfg.CreateMap<ValidationRule, MetaData_ValidationRule>();

                // Metadata - Lookups and Subcategories
                cfg.CreateMap<IReadOnlyCollection<MetaData>, List<MetaData_Lookup>>()
                    .ConvertUsing((source, target, c) => c.Mapper.Map<List<MetaData_Lookup>>(source.SelectMany(src => src.Lookups).ToList()));
                cfg.CreateMap<Lookup, MetaData_Lookup>()
                    .ForMember(m => m.MetaData_LookupSubCategories, opt => opt.MapFrom(src => src.SubCategories));
                cfg.CreateMap<LookupSubCategory, MetaData_LookupSubCategory>();

                // LARSStandards
                cfg.CreateMap<LARSStandard, LARS_LARSStandard>()
                    .ForMember(m => m.LARS_LARSStandardApprenticeshipFundings, opt => opt.MapFrom(src => src.LARSStandardApprenticeshipFundings))
                    .ForMember(m => m.LARS_LARSStandardCommonComponents, opt => opt.MapFrom(src => src.LARSStandardCommonComponents))
                    .ForMember(m => m.LARS_LARSStandardFundings, opt => opt.MapFrom(src => src.LARSStandardFundings))
                    .ForMember(m => m.LARS_LARSStandardValidities, opt => opt.MapFrom(src => src.LARSStandardValidities));

                cfg.CreateMap<LARSStandardApprenticeshipFunding, LARS_LARSStandardApprenticeshipFunding>();
                cfg.CreateMap<LARSStandardCommonComponent, LARS_LARSStandardCommonComponent> ();
                cfg.CreateMap<LARSStandardFunding, LARS_LARSStandardFunding> ();
                cfg.CreateMap<LARSStandardValidity, LARS_LARSStandardValidity> ();

                // LARSLearningDeliveries
                cfg.CreateMap<LARSLearningDelivery, LARS_LARSLearningDelivery>()
                    .ForMember(m => m.LARS_LARSFundings, opt => opt.MapFrom(src => src.LARSFundings))
                    .ForMember(m => m.LARS_LARSAnnualValues, opt => opt.MapFrom(src => src.LARSAnnualValues))
                    .ForMember(m => m.LARS_LARSLearningDeliveryCategories, opt => opt.MapFrom(src => src.LARSLearningDeliveryCategories))
                    .ForMember(m => m.LARS_LARSValidities, opt => opt.MapFrom(src => src.LARSValidities));

                cfg.CreateMap<LARSFunding, LARS_LARSFunding>();
                cfg.CreateMap<LARSAnnualValue, LARS_LARSAnnualValue>();
                cfg.CreateMap<LARSLearningDeliveryCategory, LARS_LARSLearningDeliveryCategory>();
                cfg.CreateMap<LARSValidity, LARS_LARSValidity>();

                // LARSFrameworks
                cfg.CreateMap<LARSFrameworkDesktop, LARS_LARSFrameworkDesktop>()
                    .ForMember(m => m.LARS_LARSFrameworkApprenticeshipFundings, opt => opt.MapFrom(src => src.LARSFrameworkApprenticeshipFundings))
                    .ForMember(m => m.LARS_LARSFrameworkCommonComponents, opt => opt.MapFrom((src => src.LARSFrameworkCommonComponents)));

                cfg.CreateMap<LARSFrameworkApprenticeshipFunding, LARS_LARSFrameworkApprenticeshipFunding>();
                cfg.CreateMap<LARSFrameworkCommonComponent, LARS_LARSFrameworkCommonComponent>();

                // LarsFrameworkAims
                cfg.CreateMap<LARSFrameworkAimDesktop, LARS_LARSFrameworkAim>();

                // Postcodes
                cfg.CreateMap<Postcode, Postcodes_Postcode>()
                    .ForMember(m => m.Postcodes_ONSDatas, opt => opt.MapFrom(src => src.ONSData))
                    .ForMember(m => m.Postcodes_DasDisadvantages, opt => opt.MapFrom(src => src.DasDisadvantages))
                    .ForMember(m => m.Postcodes_EfaDisadvantages, opt => opt.MapFrom(src => src.EfaDisadvantages))
                    .ForMember(m => m.Postcodes_SfaAreaCosts, opt => opt.MapFrom(src => src.SfaAreaCosts))
                    .ForMember(m => m.Postcodes_SfaDisadvantages, opt => opt.MapFrom(src => src.SfaDisadvantages));

                cfg.CreateMap<ONSData, Postcodes_ONSData>();
                cfg.CreateMap<DasDisadvantage, Postcodes_DasDisadvantage>();
                cfg.CreateMap<EfaDisadvantage, Postcodes_EfaDisadvantage>();
                cfg.CreateMap<SfaAreaCost, Postcodes_SfaAreaCost>();
                cfg.CreateMap<SfaDisadvantage, Postcodes_SfaDisadvantage>();

                cfg.CreateMap<McaGlaSofLookup, PostcodesDevolution_McaGlaSofLookup>();
                cfg.CreateMap<DevolvedPostcode, PostcodesDevolution_Postcode>();

                // Organisations
                cfg.CreateMap<Organisation, Organisations_Organisation>()
                    .ForMember(m => m.Organisations_OrganisationCampusIdentifiers, opt => opt.MapFrom(src => src.CampusIdentifers))
                    .ForMember(m => m.Organisations_OrganisationCoFRemovals, opt => opt.MapFrom(src => src.OrganisationCoFRemovals))
                    .ForMember(m => m.Organisations_OrganisationFundings, opt => opt.MapFrom(src => src.OrganisationFundings));
                cfg.CreateMap<OrganisationCampusIdentifier, Organisations_OrganisationCampusIdentifier>()
                    .ForMember(m => m.Organisations_SpecialistResources, opt => opt.MapFrom(src => src.SpecialistResources));
                cfg.CreateMap<OrganisationCoFRemoval, Organisations_OrganisationCoFRemoval>();
                cfg.CreateMap<OrganisationFunding, Organisations_OrganisationFunding>();
                cfg.CreateMap<OrganisationCampusIdSpecialistResource, Organisations_SpecialistResource>();
                cfg.CreateMap<OrganisationPostcodeSpecialistResource, Organisations_PostcodesSpecialistResource>();

                cfg.CreateMap<EPAOrganisation, EPAOrganisations_EPAOrganisation>()
                    .ForMember(m => m.Id, opt => opt.Ignore())
                    .ForMember(m => m.EPAID, opt => opt.MapFrom(src => src.ID));

                // Employers
                cfg.CreateMap<Employer, Employers_Employer>()
                    .ForMember(m => m.Employers_LargeEmployerEffectiveDates, opt => opt.MapFrom(src => src.LargeEmployerEffectiveDates));
                cfg.CreateMap<LargeEmployerEffectiveDates, Employers_LargeEmployerEffectiveDate>();
            });

            return config.CreateMapper();
        }
    }
}
