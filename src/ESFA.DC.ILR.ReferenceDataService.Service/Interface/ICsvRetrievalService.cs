using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Interface
{
    public interface ICsvRetrievalService
    {
        Task<IEnumerable<TModel>> RetrieveCsvData<TModel, TMapper>(string fileReference, string container, CancellationToken cancellationToken)
            where TModel : class
            where TMapper : CsvHelper.Configuration.ClassMap;
    }
}
