using AutoMapper;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model.Containers;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model.Containers.Interface;

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

        public IEFReferenceInputDataRoot Map(DesktopReferenceDataRoot desktopReferenceDataRoot)
        {
            var mapper = GetMapper();

            var result = mapper.Map<EFReferenceInputDataRoot>(desktopReferenceDataRoot);

            return result;
        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // The below mappings can be broken out into injectable resources as and when some customisation is required for that specific classes mapping
                // Until then, just leave then in the list below.

                // Top level DataRoot
                cfg.CreateMap<DesktopReferenceDataRoot, EFReferenceInputDataRoot>()
                    .ForMember(m => m.Lars_LarsVersion, opt => opt.MapFrom(src => src.MetaDatas.ReferenceDataVersions.LarsVersion))
                    .ForMember(m => m.Lars_LarsStandards, opt => opt.MapFrom(src => src.LARSStandards))
                    .ForMember(m => m.Lars_LarsLearningDeliveries,opt => opt.MapFrom(src => src.LARSLearningDeliveries))
                    .ForMember(m => m.Lars_LarsFrameworkDesktops, opt => opt.MapFrom(src => src.LARSFrameworks))
                    .ForMember(m => m.Lars_LarsFrameworkAims, opt => opt.MapFrom((src => src.LARSFrameworkAims)));

                // Metadata
                cfg.CreateMap<MetaData, LARS_LARSVersion>()
                    .ForMember(m => m.Version, opt => opt.MapFrom(src => src.ReferenceDataVersions.LarsVersion.Version));

                // LArsVersion
                cfg.CreateMap<LarsVersion, LARS_LARSVersion>();

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

            });

            return config.CreateMapper();
        }
    }
}
