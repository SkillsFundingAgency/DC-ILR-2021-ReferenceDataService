using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.CollectionDates;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface
{
    public interface IIlrReferenceDataRepositoryService
    {
        Task<IReadOnlyCollection<ValidationError>> RetrieveValidationErrorsAsync(CancellationToken cancellationToken);

        Task<IReadOnlyCollection<ValidationRule>> RetrieveValidationRulesAsync(CancellationToken cancellationToken);

        Task<List<Lookup>> RetrieveLookupsAsync(CancellationToken cancellationToken);

        IlrCollectionDates RetrieveCollectionDates();
    }
}
