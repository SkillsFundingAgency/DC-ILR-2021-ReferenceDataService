using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.Logging.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tests
{
    public class DesktopReferenceDataFileNameServiceTests
    {
        [Fact]
        public async Task ProcessAync()
        {
            var versionNumber = "2";

            var expectedFileName = string.Concat(Path.Combine("FilePath", "FileName."), versionNumber, ".zip");

            var dateTimeUtc = new System.DateTime(2020, 8, 1, 10, 0, 0);
            var dateTimeUk = new System.DateTime(2020, 8, 1, 9, 0, 0);

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(x => x.ConvertUtcToUk(dateTimeUtc)).Returns(dateTimeUk);

            NewService(dateTimeProviderMock.Object).BuildFileName("FilePath", "FileName", "2").Should().BeEquivalentTo(expectedFileName);
        }

        private DesktopReferenceDataFileNameService NewService(IDateTimeProvider dateTimeProvider)
        {
            return new DesktopReferenceDataFileNameService(dateTimeProvider, Mock.Of<ILogger>());
        }
    }
}
