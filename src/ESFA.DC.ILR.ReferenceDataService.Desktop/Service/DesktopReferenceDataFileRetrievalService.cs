using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service
{
    public class DesktopReferenceDataFileRetrievalService : IDesktopReferenceDataFileRetrievalService
    {
        private readonly IJsonSerializationService _jsonSerializationService;

        public DesktopReferenceDataFileRetrievalService(IJsonSerializationService jsonSerializationService)
        {
            _jsonSerializationService = jsonSerializationService;
        }

        public DesktopReferenceDataRoot Retrieve()
        {
            DesktopReferenceDataRoot desktopReferenceData;

            var resourcePath = Assembly.GetExecutingAssembly().GetManifestResourceNames().FirstOrDefault();

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath))
            {
                using (var gzipStream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    desktopReferenceData = _jsonSerializationService.Deserialize<DesktopReferenceDataRoot>(gzipStream);
                }
            }

            return desktopReferenceData;
        }
    }
}
