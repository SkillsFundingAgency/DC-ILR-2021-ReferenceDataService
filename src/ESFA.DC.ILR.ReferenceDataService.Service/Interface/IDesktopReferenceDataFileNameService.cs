using System;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Interface
{
    public interface IDesktopReferenceDataFileNameService
    {
        string BuildFileName(string filePath, string fileName, string versionNumber);
    }
}
