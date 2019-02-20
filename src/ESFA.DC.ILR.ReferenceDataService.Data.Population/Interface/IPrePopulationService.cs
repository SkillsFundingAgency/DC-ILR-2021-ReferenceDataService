using System.Collections.Generic;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface
{
    public interface IPrePopulationService
    {
        IReadOnlyCollection<int> UniqueUKPRNsFromMessage(IMessage message);

        IReadOnlyCollection<string> UniqueEpaOrgIdsFromMessage(IMessage message);

        IReadOnlyCollection<string> UniqueLearnAimRefsFromMessage(IMessage message);

        IReadOnlyCollection<long> UniqueULNsFromMessage(IMessage message);

        IReadOnlyCollection<string> UniquePostcodesFromMessage(IMessage message);

        IReadOnlyCollection<int> UniqueEmployerIdsFromMessage(IMessage message);

        IReadOnlyCollection<int> UniqueSTDCodesFromMessage(IMessage message);

        // Not sure if needed yet
        // IReadOnlyCollection<LARSFrameworkKey> UniqueFrameworksFromMessage(IMessage message);
    }
}
