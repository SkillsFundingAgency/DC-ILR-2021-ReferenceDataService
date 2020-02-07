using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.Interface;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class McaDevolvedContractsRepositoryServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var ukprn = 1;
        }

        private FcsRepositoryService NewService(IDbContextFactory<IFcsContext> fcsContextFactory = null)
        {
            return new FcsRepositoryService(fcsContextFactory);
        }
    }
}