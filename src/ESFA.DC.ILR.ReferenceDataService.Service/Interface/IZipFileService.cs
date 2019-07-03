using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public interface IZipFileService
    {
        Task SaveCollectionZipAsync(string zipFileName, string container, IReadOnlyDictionary<string, string> zipContents, CancellationToken cancellationToken);
    }
}
