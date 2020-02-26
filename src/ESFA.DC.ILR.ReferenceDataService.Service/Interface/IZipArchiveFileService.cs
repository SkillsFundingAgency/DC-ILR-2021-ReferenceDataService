using System;
using System.Collections.Generic;
using System.IO.Compression;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Interface
{
    public interface IZipArchiveFileService
    {
        T RetrieveModel<T>(ZipArchive zipArchive, string fileName);

        IReadOnlyCollection<T> RetrieveModels<T>(ZipArchive zipArchive, string fileName);

        IReadOnlyCollection<T> RetrieveModels<T>(ZipArchive zipArchive, string fileName, Func<T, bool> predicate);
    }
}
