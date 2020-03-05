using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model.Containers.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface
{
    public interface IReferenceInputPersistence
    {
        Task PersistEFModelsAsync(IInputReferenceDataContext inputReferenceDataContext, IEFReferenceInputDataRoot efModels, CancellationToken cancellationToken);
    }
}
