using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public class CsvRetrievalService : ICsvRetrievalService
    {
        private readonly IFileService _fileService;

        public CsvRetrievalService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<IEnumerable<TModel>> RetrieveCsvData<TModel, TMapper>(string fileReference, string container, CancellationToken cancellationToken)
            where TModel : class
            where TMapper : CsvHelper.Configuration.ClassMap
        {
            using (var stream = await _fileService.OpenReadStreamAsync(fileReference, container, cancellationToken))
            {
                using (var reader = new StreamReader(stream))
                using (var csv = new CsvReader(reader))
                {
                    csv.Configuration.RegisterClassMap<TMapper>();

                    return csv.GetRecords<TModel>().ToList();
                }
            }
        }
    }
}
