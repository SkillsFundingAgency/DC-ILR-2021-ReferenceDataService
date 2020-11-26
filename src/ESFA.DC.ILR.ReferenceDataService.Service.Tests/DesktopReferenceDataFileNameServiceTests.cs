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

            NewService().BuildFileName("FilePath", "FileName", "2").Should().BeEquivalentTo(expectedFileName);
        }

        private DesktopReferenceDataFileNameService NewService()
        {
            return new DesktopReferenceDataFileNameService(Mock.Of<ILogger>());
        }
    }
}
