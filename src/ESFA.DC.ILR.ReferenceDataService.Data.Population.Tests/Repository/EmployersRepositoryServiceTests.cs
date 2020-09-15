using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR.ReferenceDataService.Interfaces.Service.Clients;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.ReferenceData.Employers.Model;
using ESFA.DC.ReferenceData.Employers.Model.Interface;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class EmployersRepositoryServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var empIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 30, 31, 32, 33, 34, 35 };

            IEnumerable<Employer> edrsList = new List<Employer>
            {
                new Employer { Urn = 1 },
                new Employer { Urn = 2 },
                new Employer { Urn = 3 },
                new Employer { Urn = 4 },
                new Employer { Urn = 5 },
                new Employer { Urn = 6 },
                new Employer { Urn = 7 },
                new Employer { Urn = 8 },
                new Employer { Urn = 9 },
                new Employer { Urn = 10 },
                new Employer { Urn = 11 },
                new Employer { Urn = 12 },
                new Employer { Urn = 13 },
                new Employer { Urn = 14 },
                new Employer { Urn = 15 },
                new Employer { Urn = 16 },
                new Employer { Urn = 17 },
                new Employer { Urn = 18 },
                new Employer { Urn = 19 },
                new Employer { Urn = 20 },
                new Employer { Urn = 35 },
            };

            IEnumerable<LargeEmployer> lempList = new List<LargeEmployer>
            {
                new LargeEmployer { Ern = 1, EffectiveFrom = new DateTime(2018, 8, 1) },
                new LargeEmployer { Ern = 2, EffectiveFrom = new DateTime(2018, 8, 1) },
                new LargeEmployer { Ern = 3, EffectiveFrom = new DateTime(2018, 8, 1) },
                new LargeEmployer { Ern = 4, EffectiveFrom = new DateTime(2018, 8, 1) },
                new LargeEmployer { Ern = 5, EffectiveFrom = new DateTime(2018, 8, 1) },
                new LargeEmployer { Ern = 6, EffectiveFrom = new DateTime(2018, 8, 1) },
                new LargeEmployer { Ern = 7, EffectiveFrom = new DateTime(2018, 8, 1) },
                new LargeEmployer { Ern = 8, EffectiveFrom = new DateTime(2018, 8, 1), EffectiveTo = new DateTime(2018, 10, 31) },
                new LargeEmployer { Ern = 8, EffectiveFrom = new DateTime(2018, 11, 1) },
                new LargeEmployer { Ern = 10, EffectiveFrom = new DateTime(2018, 8, 1) },
                new LargeEmployer { Ern = 50, EffectiveFrom = new DateTime(2018, 8, 1) },
                new LargeEmployer { Ern = 51, EffectiveFrom = new DateTime(2018, 8, 1) },
                new LargeEmployer { Ern = 52, EffectiveFrom = new DateTime(2018, 8, 1) },
            };

            var edrsDbMock = edrsList.AsQueryable().BuildMockDbSet();
            var lempDbMock = lempList.AsQueryable().BuildMockDbSet();

            var clientServiceMock = new Mock<IEDRSClientService>();
            clientServiceMock
                .Setup(m => m.ValidateErns(empIds, CancellationToken.None))
                .ReturnsAsync(Enumerable.Empty<int>());

            var employersMock = new Mock<IEmployersContext>();
            employersMock.Setup(e => e.Employers).Returns(edrsDbMock.Object);
            employersMock.Setup(e => e.LargeEmployers).Returns(lempDbMock.Object);

            var employersContextFactoryMock = new Mock<IDbContextFactory<IEmployersContext>>();
            employersContextFactoryMock.Setup(c => c.Create()).Returns(employersMock.Object);

            var serviceResult = await NewService(employersContextFactoryMock.Object, clientServiceMock.Object)
                .RetrieveAsync(empIds, CancellationToken.None);

            serviceResult.Should().HaveCount(21);
            serviceResult.Select(e => e.ERN).Should().BeEquivalentTo(edrsList.Select(u => u.Urn).ToList());
            serviceResult.SelectMany(e => e.LargeEmployerEffectiveDates).Should().HaveCount(10);
            serviceResult.Where(e => e.ERN == 8).SelectMany(e => e.LargeEmployerEffectiveDates).Should().HaveCount(2);
        }

        private EmployersRepositoryService NewService(
            IDbContextFactory<IEmployersContext> employersContextFactory = null,
            IEDRSClientService edrsClientService = null,
            FeatureConfiguration featureConfiguration = null,
            ILogger logger = null)
        {
            return new EmployersRepositoryService(employersContextFactory, edrsClientService, featureConfiguration, logger);
        }
    }
}
