using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface
{
    public interface IValidationMessagesTransaction
    {
        Task WriteValidationMessagesAsync(IEnumerable<Rule> validationMessages, CancellationToken cancellationToken);
    }
}
