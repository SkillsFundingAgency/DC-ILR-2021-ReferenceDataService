using System.IO;
using System.Linq;
using System.Reflection;
using ESFA.DC.Logging.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tests
{
    public class DesktopReferenceDataFileNameServiceTests
    {
        [Fact]
        public void ProcessAync()
        {
            var rdsModelVersion = Assembly.GetExecutingAssembly().GetReferencedAssemblies().First(a => a.Name == "ESFA.DC.ILR.ReferenceDataService.Model").Version.ToString(3);

            var expectedFileName = string.Concat(Path.Combine("FilePath", "FileName."), rdsModelVersion, ".zip");

            NewService().BuildFileName("FilePath", "FileName").Should().BeEquivalentTo(expectedFileName);
        }

        private DesktopReferenceDataFileNameService NewService(ILogger logger = null)
        {
            return new DesktopReferenceDataFileNameService(logger ?? Mock.Of<ILogger>());
        }
    }
}
