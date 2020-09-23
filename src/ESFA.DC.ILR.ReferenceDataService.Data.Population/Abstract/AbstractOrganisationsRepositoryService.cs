using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ReferenceData.Organisations.Model;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Abstract
{
    public abstract class AbstractOrganisationsRepositoryService
    {
        protected const int LongTermResidValue = 1;
        protected readonly List<OrganisationCampusIdentifier> _defaultCampusIdentifiers = new List<OrganisationCampusIdentifier>();
        protected readonly List<OrganisationPostcodeSpecialistResource> _defaultPostcodeSepcialistResources = new List<OrganisationPostcodeSpecialistResource>();
        protected readonly List<OrganisationShortTermFundingInitiative> _defaultShortTermFundingInitiatives = new List<OrganisationShortTermFundingInitiative>();
        protected readonly IDbContextFactory<IOrganisationsContext> _organisationsFactory;

        protected AbstractOrganisationsRepositoryService(IDbContextFactory<IOrganisationsContext> organisationsFactory, IAcademicYearDataService academicYearDataService)
        {
            _organisationsFactory = organisationsFactory;
            AcademicYearStart = academicYearDataService.CurrentYearStart;
            AcademicYearEnd = academicYearDataService.CurrentYearEnd;
        }

        protected DateTime AcademicYearStart { get; }

        protected DateTime AcademicYearEnd { get; }

        public List<OrganisationCampusIdentifier> GetCampusIdentifiers(long ukprn, Dictionary<long, List<OrganisationCampusIdentifier>> campusIdentifiers)
        {
            campusIdentifiers.TryGetValue(ukprn, out var campusIds);

            return campusIds ?? _defaultCampusIdentifiers;
        }

        public List<OrganisationPostcodeSpecialistResource> GetPostcodeSpecResources(long ukprn, Dictionary<long, List<OrganisationPostcodeSpecialistResource>> specResourcesDictionary)
        {
            specResourcesDictionary.TryGetValue(ukprn, out var specRes);

            return specRes ?? _defaultPostcodeSepcialistResources;
        }

        public List<OrganisationShortTermFundingInitiative> GetShortTermFundingInitiatives(long ukprn, Dictionary<long, List<OrganisationShortTermFundingInitiative>> shortTermFundingDictionary)
        {
            shortTermFundingDictionary.TryGetValue(ukprn, out var shortTermFunding);

            return shortTermFunding ?? _defaultShortTermFundingInitiatives;
        }

        protected OrganisationCampusIdentifier BuildCampusIdentifiers(CampusIdentifier campusIdentifier, Dictionary<long, Dictionary<string, List<OrganisationCampusIdSpecialistResource>>> specResourcesForUkprnDictionary)
        {
            return new OrganisationCampusIdentifier
            {
                UKPRN = campusIdentifier.MasterUkprn,
                CampusIdentifier = campusIdentifier.CampusIdentifier1,
                EffectiveFrom = campusIdentifier.EffectiveFrom,
                EffectiveTo = campusIdentifier.EffectiveTo,
                SpecialistResources = GetCampusIdSpecialistResources(campusIdentifier.MasterUkprn, campusIdentifier.CampusIdentifier1, specResourcesForUkprnDictionary).ToList()
            };
        }

        protected IEnumerable<OrganisationCampusIdSpecialistResource> GetCampusIdSpecialistResources(long ukprn, string campusIdentifier, Dictionary<long, Dictionary<string, List<OrganisationCampusIdSpecialistResource>>> specResourcesDictionary)
        {
            specResourcesDictionary.TryGetValue(ukprn, out var campusIdSpecResources);

            return campusIdSpecResources != null ?
                  campusIdSpecResources.TryGetValue(campusIdentifier, out var resources) ? resources : Enumerable.Empty<OrganisationCampusIdSpecialistResource>() : Enumerable.Empty<OrganisationCampusIdSpecialistResource>();
        }

        protected Dictionary<long, List<OrganisationShortTermFundingInitiative>> BuildShortTermFundingInitiativesDictionary(ICollection<ShortTermFundingInitiative> shortTermFundingInitiatives)
        {
            return shortTermFundingInitiatives?
                .GroupBy(s => s.Ukprn)
                .ToDictionary(
                    k1 => k1.Key,
                    v1 => v1.Select(s => new OrganisationShortTermFundingInitiative()
                    {
                        UKPRN = s.Ukprn,
                        LdmCode = s.Ldmcode,
                        Reason = s.Reason,
                        EffectiveFrom = s.EffectiveFrom,
                        EffectiveTo = s.EffectiveTo
                    }).ToList());
        }

        protected Dictionary<long, List<OrganisationPostcodeSpecialistResource>> BuildPostcodeSpecResDictionary(ICollection<ProviderPostcodeSpecialistResource> specResources)
        {
            return specResources?
                .GroupBy(sr => sr.Ukprn)
                .ToDictionary(
                    k1 => k1.Key,
                    v1 => v1.Select(sr => new OrganisationPostcodeSpecialistResource
                    {
                        UKPRN = sr.Ukprn,
                        Postcode = sr.Postcode,
                        SpecialistResources = sr.SpecialistResources,
                        EffectiveFrom = sr.EffectiveFrom,
                        EffectiveTo = sr.EffectiveTo
                    }).ToList());
        }

        protected Dictionary<long, List<OrganisationCampusIdentifier>> BuildCampusIdentifiersDictionary(ICollection<OrganisationCampusIdentifier> campusIdentifiers)
        {
            return
                campusIdentifiers
                    .GroupBy(ci => ci.UKPRN)
                    .ToDictionary(
                        k => k.Key,
                        v => v.Select(c => c).ToList());
        }

        protected Dictionary<long, Dictionary<string, List<OrganisationCampusIdSpecialistResource>>> BuildCampusIdSpecResourceDictionary(ICollection<CampusIdentifierSpecResource> campusIdentifierSpecResources)
        {
            return campusIdentifierSpecResources?
                .GroupBy(sr => sr.MasterUkprn)
                .ToDictionary(
                    k1 => k1.Key,
                    v1 => v1.GroupBy(ci => ci.CampusIdentifier)
                        .ToDictionary(
                            k2 => k2.Key,
                            v2 => v2.Select(sr => new OrganisationCampusIdSpecialistResource
                            {
                                IsSpecialistResource = sr.SpecialistResources,
                                EffectiveFrom = sr.EffectiveFrom,
                                EffectiveTo = sr.EffectiveTo
                            }).ToList()));
        }

        protected IReadOnlyCollection<Organisation> BuildOrganisations(
         IEnumerable<MasterOrganisation> orgs,
         Dictionary<long, Dictionary<string, List<OrganisationCampusIdSpecialistResource>>> specResourcesForUkprnDictionary,
         Dictionary<long, List<OrganisationCampusIdentifier>> campusIdentifiersDictionary,
         Dictionary<long, List<OrganisationPostcodeSpecialistResource>> postcodeSpecResourcesDictionary,
         Dictionary<long, List<OrganisationShortTermFundingInitiative>> shortTermFundingInitiativesDictionary)
            {
                return orgs?.Select(org => new Organisation
                {
                    UKPRN = (int)org.Ukprn,
                    Name = org?.OrgDetail?.Name,
                    LegalOrgType = org?.OrgDetail?.LegalOrgType,
                    PartnerUKPRN = org?.OrgPartnerUkprns?.Any(op => op.Ukprn == org.Ukprn),
                    LongTermResid = org?.OrgDetail?.LongTermResid == LongTermResidValue,
                    CampusIdentifers = GetCampusIdentifiers(org.Ukprn, campusIdentifiersDictionary),
                    PostcodeSpecialistResources = GetPostcodeSpecResources(org.Ukprn, postcodeSpecResourcesDictionary),
                    OrganisationFundings = org?.OrgFundings?.Select(of =>
                        new OrganisationFunding()
                        {
                            OrgFundFactor = of.FundingFactor,
                            OrgFundFactType = of.FundingFactorType,
                            OrgFundFactValue = of.FundingFactorValue,
                            EffectiveFrom = of.EffectiveFrom,
                            EffectiveTo = of.EffectiveTo,
                        }).ToList(),
                    OrganisationCoFRemovals = org?.ConditionOfFundingRemovals?
                        .Where(cf => cf.EffectiveFrom >= AcademicYearStart && cf.EffectiveTo <= AcademicYearEnd)
                        .Select(c =>
                            new OrganisationCoFRemoval
                            {
                                CoFRemoval = c.CoFremoval,
                                EffectiveFrom = c.EffectiveFrom,
                                EffectiveTo = c.EffectiveTo,
                            }).ToList(),
                    OrganisationShortTermFundingInitiatives = GetShortTermFundingInitiatives(org.Ukprn, shortTermFundingInitiativesDictionary),
                }).ToList();
            }
    }
}
