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
        private const string _dateTimeFormat = "yyyyMMddHHmm";
        private readonly ILogger _logger;

        public DesktopReferenceDataFileNameService(ILogger logger)
        {
            _logger = logger;
        }

        public string BuildFileName(string filePath, string fileName, DateTime submissionDateTimeUtc)
        {
            _logger.LogInfo("Builiding Desktop reference data file name.");
            var referenceDataModelVersion = Assembly.GetExecutingAssembly().GetReferencedAssemblies().First(a => a.Name == "ESFA.DC.ILR.ReferenceDataService.Model").Version.ToString(3);

            return string.Concat(Path.Combine(filePath, fileName), ".", referenceDataModelVersion, ".", submissionDateTimeUtc.ToString(_dateTimeFormat), _referenceDataFileExtension);
        }
    }
}