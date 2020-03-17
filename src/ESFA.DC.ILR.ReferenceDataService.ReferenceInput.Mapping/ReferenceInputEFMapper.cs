using System.Runtime.InteropServices.ComTypes;
using AutoMapper;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions;
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
                cfg.CreateMap<MetaData, LARS_LARSVersion>()
                    .ForMember(m => m.Version, opt => opt.MapFrom(src => src.ReferenceDataVersions.LarsVersion.Version));
                cfg.CreateMap<MetaData, MetaData_PostcodesVersion>()
                    .ForMember(m => m.Version, opt => opt.MapFrom(src => src.ReferenceDataVersions.PostcodesVersion.Version));

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
            });

            return config.CreateMapper();
        }
    }
}
