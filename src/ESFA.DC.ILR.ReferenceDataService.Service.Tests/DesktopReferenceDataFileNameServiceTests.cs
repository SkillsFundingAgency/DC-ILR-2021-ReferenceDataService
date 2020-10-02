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
            var rdsModelVersion = Assembly.GetExecutingAssembly().GetReferencedAssemblies().First(a => a.Name == "ESFA.DC.ILR.ReferenceDataService.Model").Version.ToString(3);

            var expectedFileName = string.Concat(Path.Combine("FilePath", "FileName."), rdsModelVersion, ".202008010900", ".zip");

            NewService().BuildFileName("FilePath", "FileName", new System.DateTime(2020, 8, 1, 9, 00, 00)).Should().BeEquivalentTo(expectedFileName);
        }

        private DesktopReferenceDataFileNameService NewService(ILogger logger = null)
        {
            return new DesktopReferenceDataFileNameService(logger ?? Mock.Of<ILogger>());
        }
    }
}
