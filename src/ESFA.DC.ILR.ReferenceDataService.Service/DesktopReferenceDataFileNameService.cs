using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public class DesktopReferenceDataFileNameService : IDesktopReferenceDataFileNameService
    {
        private const string _referenceDataFileExtension = ".zip";

        private readonly ILogger _logger;

        public DesktopReferenceDataFileNameService(ILogger logger)
        {
            _logger = logger;
        }

        public string BuildFileName(string filePath, string fileName, string versionNumber)
        {
            _logger.LogInfo("Builiding Desktop reference data file name.");

            return string.Concat(Path.Combine(filePath, fileName), ".", versionNumber, _referenceDataFileExtension);
        }
    }
}