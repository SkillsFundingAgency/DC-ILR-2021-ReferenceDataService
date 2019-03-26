using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Model;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests
{
    public class MessageMapperServiceTests
    {
        [Fact]
        public void MapFromMessage()
        {
            var message = new TestMessage();

            var empIds = new List<int> { 1, 2, 3 };
            var epaOrgIds = new List<string> { "1", "2", "3" };
            var learnAimRefs = new List<string> { "1", "2", "3" };
            var ukprn = 1;
            var postcodes = new List<string> { "1", "2", "3" };
            var standardCodes = new List<int> { 1, 2, 3 };
            var ukprns = new List<int> { 1, 2, 3 };
            var ulns = new List<long> { 1, 2, 3 };

            var empIMapperdMock = new Mock<IEmpIdMapper>();
            var epaMapperMock = new Mock<IEpaOrgIdMapper>();
            var fm36UlnMapperMock = new Mock<IFM36UlnMapper>();
            var learnAimRefMapperMock = new Mock<ILearnAimRefMapper>();
            var learningProvUkprnMapperMock = new Mock<ILearningProviderUkprnMapper>();
            var postcodesMapperMock = new Mock<IPostcodesMapper>();
            var organisationsMapperMock = new Mock<IStandardCodesMapper>();
            var ukprnsMapperMock = new Mock<IUkprnsMapper>();
            var ulnMapperMock = new Mock<IUlnMapper>();

            empIMapperdMock.Setup(mm => mm.MapEmpIdsFromMessage(message)).Returns(empIds);
            epaMapperMock.Setup(mm => mm.MapEpaOrgIdsFromMessage(message)).Returns(epaOrgIds);
            fm36UlnMapperMock.Setup(mm => mm.MapFM36UlnsFromMessage(message)).Returns(ulns);
            learnAimRefMapperMock.Setup(mm => mm.MapLearnAimRefsFromMessage(message)).Returns(learnAimRefs);
            learningProvUkprnMapperMock.Setup(mm => mm.MapLearningProviderUKPRNFromMessage(message)).Returns(ukprn);
            postcodesMapperMock.Setup(mm => mm.MapPostcodesFromMessage(message)).Returns(postcodes);
            organisationsMapperMock.Setup(mm => mm.MapStandardCodesFromMessage(message)).Returns(standardCodes);
            ukprnsMapperMock.Setup(mm => mm.MapUKPRNsFromMessage(message)).Returns(ukprns);
            ulnMapperMock.Setup(mm => mm.MapUlnsFromMessage(message)).Returns(ulns);

            var mapperData = NewService(
                empIMapperdMock.Object,
                epaMapperMock.Object,
                fm36UlnMapperMock.Object,
                learnAimRefMapperMock.Object,
                learningProvUkprnMapperMock.Object,
                postcodesMapperMock.Object,
                organisationsMapperMock.Object,
                ukprnsMapperMock.Object,
                ulnMapperMock.Object).MapFromMessage(message);

            mapperData.EmployerIds.Should().BeEquivalentTo(empIds);
            mapperData.EpaOrgIds.Should().BeEquivalentTo(epaOrgIds);
            mapperData.FM36Ulns.Should().BeEquivalentTo(ulns);
            mapperData.LearnAimRefs.Should().BeEquivalentTo(learnAimRefs);
            mapperData.LearningProviderUKPRN.Should().Be(ukprn);
            mapperData.Postcodes.Should().BeEquivalentTo(postcodes);
            mapperData.StandardCodes.Should().BeEquivalentTo(standardCodes);
            mapperData.UKPRNs.Should().BeEquivalentTo(ukprns);
            mapperData.ULNs.Should().BeEquivalentTo(ulns);
        }

        private MessageMapperService NewService(
            IEmpIdMapper empIdMapper = null,
            IEpaOrgIdMapper epaOrgIdMapper = null,
            IFM36UlnMapper fM36UlnMapper = null,
            ILearnAimRefMapper learnAimRefMapper = null,
            ILearningProviderUkprnMapper learningProviderUkprnMapper = null,
            IPostcodesMapper postcodesMapper = null,
            IStandardCodesMapper standardCodesMapper = null,
            IUkprnsMapper ukprnsMapper = null,
            IUlnMapper ulnMapper = null)
        {
            return new MessageMapperService(
            empIdMapper,
            epaOrgIdMapper,
            fM36UlnMapper,
            learnAimRefMapper,
            learningProviderUkprnMapper,
            postcodesMapper,
            standardCodesMapper,
            ukprnsMapper,
            ulnMapper);
        }
    }
}
