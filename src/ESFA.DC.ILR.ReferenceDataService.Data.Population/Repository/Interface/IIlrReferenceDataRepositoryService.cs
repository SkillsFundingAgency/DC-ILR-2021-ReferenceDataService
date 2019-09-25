using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface
{
    public interface IIlrReferenceDataRepositoryService
    {
        Task<MetaData> RetrieveIlrReferenceDataAsync(CancellationToken cancellationToken);

        Task ClearValidationRules(SqlConnection sqlConnection, SqlTransaction sqlTransaction, CancellationToken cancellationToken);

        Task WriteValidationRules(IEnumerable<Rule> rules, SqlConnection sqlConnection, SqlTransaction sqlTransaction, CancellationToken cancellationToken);
    }
}
