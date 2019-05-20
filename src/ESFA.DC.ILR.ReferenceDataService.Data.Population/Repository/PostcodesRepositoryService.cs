using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Extensions;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ReferenceData.Postcodes.Model;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class PostcodesRepositoryService : IPostcodesRepositoryService
    {
        private const int BatchSize = 5000;
        private readonly IPostcodesContext _postcodesContext;

        public PostcodesRepositoryService(IPostcodesContext postcodesContext)
        {
            _postcodesContext = postcodesContext;
        }

        public async Task<IReadOnlyCollection<Postcode>> RetrieveAsync(IReadOnlyCollection<string> input, CancellationToken cancellationToken)
        {
            var postcodes = new List<Postcode>();

            foreach (var batch in input.Batch(BatchSize))
            {
                postcodes.AddRange(
                    await _postcodesContext.MasterPostcodes
                        .Where(p => batch.Contains(p.Postcode))
                        .Select(p =>
                            new Postcode()
                            {
                                PostCode = p.Postcode,
                                SfaDisadvantages = p.SfaPostcodeDisadvantages.Select(spd => SfaPostcodeDisadvantagesToEntity(spd)).ToList(),
                                SfaAreaCosts = p.SfaPostcodeAreaCosts.Select(spa => SfaAreaCostsToEntity(spa)).ToList(),
                                DasDisadvantages = p.DasPostcodeDisadvantages.Select(dpd => DasPostcodeDisadvantagesToEntity(dpd)).ToList(),
                                EfaDisadvantages = p.EfaPostcodeDisadvantages.Select(epd => EfaPostcodeDisadvantagesToEntity(epd)).ToList(),
                                CareerLearningPilots = p.CareerLearningPilotPostcodes.Select(cp => CareerLearningPilotsToEntity(cp)).ToList(),
                                ONSData = p.OnsPostcodes.Select(ons => ONSDataToEntity(ons)).ToList(),
                            }).ToListAsync(cancellationToken));
            }

            return postcodes;
        }

        public SfaDisadvantage SfaPostcodeDisadvantagesToEntity(SfaPostcodeDisadvantage sfaPostcodeDisadvantage)
        {
            return new SfaDisadvantage
            {
                Uplift = sfaPostcodeDisadvantage.Uplift,
                EffectiveFrom = sfaPostcodeDisadvantage.EffectiveFrom,
                EffectiveTo = sfaPostcodeDisadvantage.EffectiveTo,
            };
        }

        public SfaAreaCost SfaAreaCostsToEntity(SfaPostcodeAreaCost sfaPostcodeAreaCost)
        {
            return new SfaAreaCost
            {
                AreaCostFactor = sfaPostcodeAreaCost.AreaCostFactor,
                EffectiveFrom = sfaPostcodeAreaCost.EffectiveFrom,
                EffectiveTo = sfaPostcodeAreaCost.EffectiveTo,
            };
        }

        public DasDisadvantage DasPostcodeDisadvantagesToEntity(DasPostcodeDisadvantage dasPostcodeDisadvantage)
        {
            return new DasDisadvantage
            {
                Uplift = dasPostcodeDisadvantage.Uplift,
                EffectiveFrom = dasPostcodeDisadvantage.EffectiveFrom,
                EffectiveTo = dasPostcodeDisadvantage.EffectiveTo,
            };
        }

        public EfaDisadvantage EfaPostcodeDisadvantagesToEntity(EfaPostcodeDisadvantage efaPostcodeDisadvantage)
        {
            return new EfaDisadvantage
            {
                Uplift = efaPostcodeDisadvantage.Uplift,
                EffectiveFrom = efaPostcodeDisadvantage.EffectiveFrom,
                EffectiveTo = efaPostcodeDisadvantage.EffectiveTo,
            };
        }

        public CareerLearningPilot CareerLearningPilotsToEntity(CareerLearningPilotPostcode careerLearningPilot)
        {
            return new CareerLearningPilot
            {
                AreaCode = careerLearningPilot.AreaCode,
                EffectiveFrom = careerLearningPilot.EffectiveFrom,
                EffectiveTo = careerLearningPilot.EffectiveTo,
            };
        }

        public ONSData ONSDataToEntity(OnsPostcode onsPostcode)
        {
            return new ONSData
            {
                EffectiveFrom = onsPostcode.EffectiveFrom,
                EffectiveTo = onsPostcode.EffectiveTo,
                Lep1 = onsPostcode.Lep1,
                Lep2 = onsPostcode.Lep2,
                LocalAuthority = onsPostcode.LocalAuthority,
                Nuts = onsPostcode.Nuts,
                Termination = GetEndOfMonthDateFromYearMonthString(onsPostcode.Termination),
            };
        }

        private DateTime? GetEndOfMonthDateFromYearMonthString(string yearMonth)
        {
            if (yearMonth == null || string.IsNullOrEmpty(yearMonth.Trim()) || yearMonth.Length != 6)
            {
                return null;
            }

            var yearParsed = int.TryParse(yearMonth.Substring(0, 4), out var year);
            var monthParsed = int.TryParse(yearMonth.Substring(4), out var month);

            if (!yearParsed || !monthParsed)
            {
                return null;
            }

            return new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
        }
    }
}
