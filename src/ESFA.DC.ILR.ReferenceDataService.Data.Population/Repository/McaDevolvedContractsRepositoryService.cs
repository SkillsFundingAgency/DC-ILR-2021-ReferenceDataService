using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model.McaContracts;
using ESFA.DC.ReferenceData.FCS.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class McaDevolvedContractsRepositoryService : IReferenceDataRetrievalService<int, IReadOnlyCollection<McaDevolvedContract>>
    {
        private readonly IDbContextFactory<IFcsContext> _fcsContextFactory;
        private readonly IReferenceDataStatisticsService _referenceDataStatisticsService;

        public McaDevolvedContractsRepositoryService(
            IDbContextFactory<IFcsContext> fcsContextFactory,
            IReferenceDataStatisticsService referenceDataStatisticsService)
        {
            _fcsContextFactory = fcsContextFactory;
            _referenceDataStatisticsService = referenceDataStatisticsService;
        }

        public async Task<IReadOnlyCollection<McaDevolvedContract>> RetrieveAsync(int ukprn, CancellationToken cancellationToken)
        {
            using (var context = _fcsContextFactory.Create())
            {
                var contracts = await context.DevolvedContracts
                    .Where(dc => dc.Ukprn == ukprn)
                    .Select(dc =>
                    new McaDevolvedContract
                    {
                        Ukprn = dc.Ukprn,
                        McaGlaShortCode = dc.McaglashortCode,
                        EffectiveFrom = dc.EffectiveFrom,
                        EffectiveTo = dc.EffectiveTo
                    }).ToListAsync(cancellationToken);

                _referenceDataStatisticsService.AddRecordCount("Devolved Contracts", contracts.Count);

                return contracts;
            }
        }
    }
}
