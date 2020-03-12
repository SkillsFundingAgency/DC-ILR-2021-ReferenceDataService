using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model.Containers.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Mapping.Interface
{
    public interface IReferenceInputPersistence
    {
        void PersistEfModelByType<T>(SqlConnection connection, SqlTransaction sqlTransaction, IEnumerable<T> source);
        void PersistEFModels(IInputReferenceDataContext inputReferenceDataContext, IEFReferenceInputDataRoot efModels);
    }
}
