using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.McaContracts;
using ESFA.DC.ReferenceData.FCS.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class McaDevolvedContractsRepositoryService : IReferenceDataRetrievalService<int, IReadOnlyCollection<McaDevolvedContract>>
    {
        private readonly IDbContextFactory<IFcsContext> _fcsContextFactory;

        public McaDevolvedContractsRepositoryService(IDbContextFactory<IFcsContext> fcsContextFactory)
        {
            _fcsContextFactory = fcsContextFactory;
        }

        public async Task<IReadOnlyCollection<McaDevolvedContract>> RetrieveAsync(int ukprn, CancellationToken cancellationToken)
        {
            using (var context = _fcsContextFactory.Create())
            {
                return new List<McaDevolvedContract>();
                //return await context.McaDevolved
            }
        }
    }
}
