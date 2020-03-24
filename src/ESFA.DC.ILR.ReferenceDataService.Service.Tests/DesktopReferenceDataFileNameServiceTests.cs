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
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            dateTimeProviderMock.Setup(dm => dm.GetNowUtc()).Returns(new System.DateTime(2020, 8, 1, 9, 0, 0));
            var rdsModelVersion = Assembly.GetExecutingAssembly().GetReferencedAssemblies().First(a => a.Name == "ESFA.DC.ILR.ReferenceDataService.Model").Version.ToString(3);

            var expectedFileName = string.Concat(Path.Combine("FilePath", "FileName."), rdsModelVersion, ".202008010900", ".zip");

            NewService(dateTimeProviderMock.Object).BuildFileName("FilePath", "FileName").Should().BeEquivalentTo(expectedFileName);
        }

        private DesktopReferenceDataFileNameService NewService(IDateTimeProvider dateTimeProvider = null, ILogger logger = null)
        {
            return new DesktopReferenceDataFileNameService(dateTimeProvider, logger ?? Mock.Of<ILogger>());
        }
    }
}
