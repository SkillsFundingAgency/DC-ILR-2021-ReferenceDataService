using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Extensions;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ReferenceData.Postcodes.Model;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class PostcodesService : IRetrievalService<IReadOnlyDictionary<string, Postcode>, IReadOnlyCollection<string>>
    {
        private const int BatchSize = 5000;
        private readonly IPostcodesContext _postcodesContext;

        public PostcodesService(IPostcodesContext postcodesContext)
        {
            _postcodesContext = postcodesContext;
        }

        public async Task<IReadOnlyDictionary<string, Postcode>> RetrieveAsync(IReadOnlyCollection<string> input, CancellationToken cancellationToken)
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
                                SfaDisadvantages = p.SfaPostcodeDisadvantages.Select(SfaPostcodeDisadvantagesToEntity).ToList(),
                                SfaAreaCosts = p.SfaPostcodeAreaCosts.Select(SfaAreaCostsToEntity).ToList(),
                                DasDisadvantages = p.DasPostcodeDisadvantages.Select(DasPostcodeDisadvantagesToEntity).ToList(),
                                EfaDisadvantages = p.EfaPostcodeDisadvantages.Select(EfaPostcodeDisadvantagesToEntity).ToList(),
                                CareerLearningPilots = p.CareerLearningPilotPostcodes.Select(CareerLearningPilotsToEntity).ToList(),
                                ONSData = p.OnsPostcodes.Select(ONSDataToEntity).ToList()
                            }).ToListAsync(cancellationToken));
            }

             return postcodes.ToDictionary(k => k.PostCode, v => v, StringComparer.OrdinalIgnoreCase);
        }

        public SfaDisadvantage SfaPostcodeDisadvantagesToEntity(SfaPostcodeDisadvantage sfaPostcodeDisadvantage)
        {
            return new SfaDisadvantage
            {
                Uplift = sfaPostcodeDisadvantage.Uplift,
                EffectiveFrom = sfaPostcodeDisadvantage.EffectiveFrom,
                EffectiveTo = sfaPostcodeDisadvantage.EffectiveTo
            };
        }

        public SfaAreaCost SfaAreaCostsToEntity(SfaPostcodeAreaCost sfaPostcodeAreaCost)
        {
            return new SfaAreaCost
            {
                AreaCostFactor = sfaPostcodeAreaCost.AreaCostFactor,
                EffectiveFrom = sfaPostcodeAreaCost.EffectiveFrom,
                EffectiveTo = sfaPostcodeAreaCost.EffectiveTo
            };
        }

        public DasDisadvantage DasPostcodeDisadvantagesToEntity(DasPostcodeDisadvantage dasPostcodeDisadvantage)
        {
            return new DasDisadvantage
            {
                Uplift = dasPostcodeDisadvantage.Uplift,
                EffectiveFrom = dasPostcodeDisadvantage.EffectiveFrom,
                EffectiveTo = dasPostcodeDisadvantage.EffectiveTo
            };
        }

        public EfaDisadvantage EfaPostcodeDisadvantagesToEntity(EfaPostcodeDisadvantage efaPostcodeDisadvantage)
        {
            return new EfaDisadvantage
            {
                Uplift = efaPostcodeDisadvantage.Uplift,
                EffectiveFrom = efaPostcodeDisadvantage.EffectiveFrom,
                EffectiveTo = efaPostcodeDisadvantage.EffectiveTo
            };
        }

        public CareerLearningPilot CareerLearningPilotsToEntity(CareerLearningPilotPostcode careerLearningPilot)
        {
            return new CareerLearningPilot
            {
                AreaCode = careerLearningPilot.AreaCode,
                EffectiveFrom = careerLearningPilot.EffectiveFrom,
                EffectiveTo = careerLearningPilot.EffectiveTo
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
                Termination = DateTime.Parse(onsPostcode.Termination)
            };
        }
    }
}
