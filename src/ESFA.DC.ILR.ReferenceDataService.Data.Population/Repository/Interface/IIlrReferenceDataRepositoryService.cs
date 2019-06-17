using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface
{
    public interface IIlrReferenceDataRepositoryService
    {
        Task<IReadOnlyCollection<ValidationError>> RetrieveValidationErrorsAsync(CancellationToken cancellationToken);

        Task<IReadOnlyCollection<ValidationRule>> RetrieveValdiationRulesAsync(CancellationToken cancellationToken);

        Task<List<Lookup>> RetrieveLookupsAsync(CancellationToken cancellationToken);
    }
}
